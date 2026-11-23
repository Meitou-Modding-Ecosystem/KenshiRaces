import argparse
import os
import posixpath
import re
from pathlib import Path

import torch
from transformers import M2M100ForConditionalGeneration, M2M100Tokenizer

parser = argparse.ArgumentParser()
parser.add_argument("language")
args = parser.parse_args()
locale = args.language
language = {"pt-BR": "pt", "zh-CN": "zh"}.get(locale, locale)
root = Path("translations") / locale
files = [Path("README.md"), *sorted(Path("docs").glob("*.md"))]
texts = {}
tokens = {}
protected = re.compile(r"\b[A-Za-z0-9]+(?:_[A-Za-z0-9]+)+\b|`[^`\n]+`|!?\[[^\]\n]*\]\([^()\n]*(?:\([^()\n]*\)[^()\n]*)*\)|https?://[^\s)]+|\b(?:[A-Za-z0-9_]+/)+[A-Za-z0-9_.-]+|\b(?:[A-Z_]{3,}|[a-zA-Z]+[A-Z][a-zA-Z]*[A-Z][a-zA-Z]*|Leoma|Crevan|Crabfolk|Kenshi|MeitouClient|Greenlander|Meshy|Hi3D)\b|\*\*[^*]+\*\*")
def register(s):
    if re.search("[A-Za-z]", s):
        texts.setdefault(s, s)

def units(s):
    return [v for v in re.split(r"(?<=[.!?])\s+(?=[A-Z])", s) if v]

def visit(s, path, render):
    def prose(t):
        if not t.strip(): return t
        first = t[:len(t)-len(t.lstrip())]
        last = t[len(t.rstrip()):]
        words = units(t.strip())
        for part in words: register(part)
        return first + " ".join(texts.get(part, part) for part in words) + last
    result = []
    off = 0
    for m in protected.finditer(s):
        result.append(prose(s[off:m.start()]))
        item = m.group()
        if item.startswith("[") or item.startswith("!["):
            start = item.index("[")+1
            label, href = item[start:-1].split("](", 1)
            dest, sep, anchor = href.partition("#")
            if not re.match(r"\w+:", href) and not href.startswith("/"):
                resolved = Path(posixpath.normpath((path.parent / dest).as_posix())) if dest else path
                if resolved not in files:
                    dest = posixpath.relpath(resolved.as_posix(), (root/path.parent).as_posix()) if dest else ""
                href = dest + sep + anchor
            if not re.search(r"[/`]|\.(md|xml|py|ps1)$", label):
                label = prose(label)
            item = item[:start]+label+"]("+href+")"
        elif item.startswith("**"):
            item = "**"+visit(item[2:-2],path,render)+"**"
        result.append(item)
        off=m.end()
    result.append(prose(s[off:]))
    return "".join(result)

def parse(path, render):
    out=[]
    fenced=False
    para=[]
    def flush():
        if para:
            out.append(visit(" ".join(x.strip() for x in para),path,render))
            para.clear()
    for line in path.read_text().splitlines():
        if re.match(r"^\s*(```|~~~)",line):
            flush()
            fenced=not fenced
            out.append(line)
        elif fenced or not line.strip() or re.fullmatch(r"[| :\-]+",line):
            flush()
            out.append(line)
        elif line.startswith("|"):
            flush()
            out.append("|".join(visit(cell,path,render) for cell in re.split(r"(?<!\\)\|",line)))
        else:
            marker=re.match(r"^(\s*(?:#{1,6}|[-*+] |\d+[.)] |> )\s*)(.*)",line)
            if marker:
                flush()
                if line.startswith("#"):
                    slug=re.sub(r"[^\w -]","",marker[2].lower().replace("`","")).replace(" ","-")
                    out.append('<a id="'+slug+'"></a>')
                out.append(marker[1]+visit(marker[2],path,render))
            else:
                para.append(line)
    flush()
    return "\n".join(out)+"\n"

for file in files: parse(file,False)
torch.set_num_threads(min(4,len(os.sched_getaffinity(0))))
model_id="facebook/m2m100_418M"
tokenizer=M2M100Tokenizer.from_pretrained(model_id,src_lang="en")
model=M2M100ForConditionalGeneration.from_pretrained(model_id).eval()
todo=sorted(texts,key=len)
with torch.inference_mode():
    for start in range(0,len(todo),16):
        batch=todo[start:start+16]
        encoded=tokenizer(batch,return_tensors="pt",padding=True)
        output=model.generate(**encoded,forced_bos_token_id=tokenizer.get_lang_id(language),max_new_tokens=min(450,40+2*encoded.input_ids.shape[1]),num_beams=2, no_repeat_ngram_size=4)
        for key,value in zip(batch,tokenizer.batch_decode(output,skip_special_tokens=True)):
            texts[key]=value.strip() or key
for file in files:
    target=root/file
    target.parent.mkdir(parents=True,exist_ok=True)
    target.write_text(parse(file,True))
print(f"{locale}: {len(files)} Markdown files, {len(todo)} translation segments")
