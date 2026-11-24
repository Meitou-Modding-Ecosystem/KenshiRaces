import json
import re
import sys
import time
from pathlib import Path
from urllib.parse import urlsplit
import posixpath
import requests

locale=sys.argv[1]
root=Path('translations')/locale
files=[Path('README.md'),*sorted(Path('docs').glob('*.md'))]
cache={}
session=requests.Session()
def translate(text):
    if not re.search('[A-Za-z]',text): return text
    if text in cache: return cache[text]
    target={'zh-CN':'zh-CN','pt-BR':'pt'}.get(locale,locale)
    for attempt in range(4):
        try:
            response=session.get('https://translate.googleapis.com/translate_a/single',params={'client':'gtx','sl':'en','tl':target,'dt':'t','q':text},timeout=25)
            response.raise_for_status()
            out=''.join(row[0] for row in response.json()[0] if row[0])
            if not out.strip(): raise ValueError('empty')
            cache[text]=out
            return out
        except (requests.RequestException,ValueError):
            if attempt==3: raise
            time.sleep(3*(attempt+1))

protect=re.compile(r'`[^`]*`|\]\([^\n()]+(?:\([^\n()]*\)[^\n()]*)*\)|https?://[^\s)]+|\b(?:[A-Za-z0-9]+_)+[A-Za-z0-9_]+\b|\b(?:Visual Studio|VS Code|MeitouClient|Meitou|Kenshi Races|Kenshi|Meshy|RaceImport|Leoma|Crevan|Crabfolk|Motion Lab|Hi3D|Blender|Release readiness|RefCheck|GutenStructure|[A-Z_]{2,})\b')
def render_unit(s,path):
    values=[]
    def hold(m):
        v=m[0]
        if v.startswith(']('):
            href=v[2:-1]
            dest,sep,anchor=href.partition('#')
            if dest and not urlsplit(href).scheme and not dest.startswith('/'):
                resolved=Path(posixpath.normpath((path.parent/dest).as_posix()))
                if resolved not in files: dest=posixpath.relpath(resolved,root/path.parent)
            v=']('+dest+sep+anchor+')'
        values.append(v)
        return f' ZXQ{len(values)}XZ '
    masked=protect.sub(hold,s)
    if not re.search(r'[A-Za-z]',re.sub(r'ZXQ\d+XZ','',masked)):
        return s if not any(x.startswith('](') for x in values) else restore(masked,values)
    result=translate(masked)
    try: return restore(result,values)
    except ValueError:
        chunks=re.split(r'\s*(ZXQ\d+XZ)\s*',masked)
        return ' '.join(restore(t,values,False) if re.fullmatch(r'ZXQ\d+XZ',t) else translate(t) for t in chunks if t)
def restore(text,vals,strict=True):
    pattern=r'\s*Z\s*X\s*Q\s*(\d+)\s*X\s*Z\s*'
    nums=[int(x) for x in re.findall(pattern,text,re.I)]
    if strict and sorted(nums)!=list(range(1,len(vals)+1)): raise ValueError('lost placeholder')
    return re.sub(pattern,lambda m:' '+vals[int(m[1])-1]+' ',text,flags=re.I).replace(' ](','](').replace('](',' ](').replace(' ](','](').strip()

def page(path):
    out=[]; para=[]; lead=''; fence=False
    def flush():
        nonlocal lead
        if para:
            body=' '.join(x.strip() for x in para)
            if lead.startswith('#'):
                slug=re.sub(r'[^\w -]','',body.lower().replace('`','')).replace(' ','-')
                out.append('<a id="'+slug+'"></a>')
            out.append(lead+render_unit(body,path))
            para.clear(); lead=''
    for line in path.read_text().splitlines():
        if re.match(r'^\s*(```|~~~)',line):
            flush(); fence=not fence;out.append(line)
        elif fence or not line.strip() or re.fullmatch(r'[| :\-]+',line):
            flush();out.append(line)
        elif line.startswith('|'):
            flush()
            if path.name=='animations.md' and line.startswith('| `kr_'):out.append(line)
            else:out.append('| '+' | '.join(render_unit(x.strip(),path) for x in line.strip('|').split('|'))+' |')
        else:
            m=re.match(r'^(\s*(?:#{1,6}\s+|[-*+]\s+|\d+[.)]\s+|>\s+))(.*)',line)
            if m:
                flush();lead=m[1];para.append(m[2])
                if lead.startswith('#'):flush()
            else:para.append(line)
    flush()
    return '\n'.join(out)+'\n'
for file in files:
    target=root/file;target.parent.mkdir(parents=True,exist_ok=True)
    target.write_text(page(file))
    print(file,flush=True)
