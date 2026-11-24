<a id="preparing-a-model"></a>
# Preparando um modelo

Como obter um gerado `.glb` (Meshy, Hi3D, Blender) para um polícone, tamanho da textura e tamanho do mundo real do jogo, antes de entrar em `<Mod>/Structures/` (a partir de edifícios) ou `<Mod>/ItemModels/` e os itens. A importação em si: [structures.md](structures.md).

Os comandos são executados na raiz repo no PowerShell. Necessidade de Python 3 `prepare-model.py` Também precisa `pip install pillow numpy`.

<a id="1-inspect"></a>
## 1. Inspecção

```powershell
python tools\model-info.py <Mod>\Structures\my-hut.glb
```

Ele imprime o tamanho em metros (X, Y up, Z), o lado mais longo, os triângulos, e o tamanho do pixel de cada textura e MB. Uma nota sobre a escala de nódulos ou a rotação `prepare-model.py` Não pode ser rastreado: exportá-lo novamente do Blender com transformações aplicadas (Objeto > Aplicação > Todas as transformações).

<a id="2-cut-the-polycount"></a>
## 2. cortar o poligono

| Boa | Triângulos | Texturas |
| --- | --- | --- |
| Construção ou decoração | de 5 a 20 mil | 2048 |
| Produto (mantenho, caído, em uma loja) | Menos de 5 mil | 1024 |

Os geradores geralmente dão 100,000 ou mais; as importações alertam sobre 150,000. Para cortá-lo em:

- **Meshy:** Remova o modelo com um polimérico no orçamento acima (também trabalha o topólogo), mantendo as texturas, e
Baixe como GLB Mais uma vez
- **O Blender:** Arquivo > Importação glTF, selecione a mesha, adicione um Modificador Decimado (Collapse) e baixe a Ratio até que o rosto conte no status
A barra está no orçamento, aplique-a e, em seguida, Faça > Exportação > glTF Avaliações (`.glb`Com a aplicação de Modificadores.

A colisão é simplificada separadamente (`collisionTriangles`, por padrão 2000), então cortar a camada visível é sobre a taxa de quadro e o tamanho do arquivo.

<a id="3-choose-the-size"></a>
## 3. Escolha o tamanho

A importação trata o modelo como metros; o tamanho de um gerador é arbitrário. Vê a faixa e coloque-a no arquivo de configurações do modelo ([Avaliação.md “Settings”](structures.md#settings)Não no modelo:

- Edifício em: `"height"` Em metros de `<model>.json`. KenshiAs pessoas são de cerca de 1,8 metros de altura e ler pequenas além dos tamanhos do mundo real, então tome o
tamanho que você descreveria e adicionou 20% (uma postagem de 2,2 m torna-se `2.64`).
- O item: `"size"`O seu lado mais longo em metros, em `<model>.json` Além disso, em `ItemModels/`.

<a id="4-shrink-the-textures"></a>
## 4. Esmagando as texturas

```powershell
python tools\prepare-model.py <Mod>\Structures\my-hut.glb 2048
```

Isso reescreve o arquivo com cada textura no máximo que muitos pixels em um lado (2048 para edifícios, 1024 para itens), geralmente cortado de dezenas de MB. Um terceiro argumento também rescata o modelo para o lado mais longo em metros, quando um arquivo de configuração não é desejado. A produção também é de uma forma muito maior (`textureSize`Mas comando o arquivo reduzido mantém o repo pequeno.

Correndo `model-info.py` Mais uma vez para verificar o resultado.

<a id="5-import-and-check"></a>
## 5. Importar e verificar

Construa e desenvolva com o jogo (ou execute a importação sozinho, [Avaliações.md “Comece rápido”](structures.md#quick-start)). O log tem uma linha por modelo:

```
Structure my-hut.glb -> "My Hut": 2 part(s), 12480 triangles, 3.1 m tall, collision mesh, unlocked by start
Item beast-capsule.glb -> mesh mymod_beast_capsule.mesh, material 950000-MyMod.mod, 4082 triangles, 0.11 m long
```

Verifique a altura ou a duração do triângulo, então olhe para ele no jogo: colocar o edifício, ou descer e pegar o item.

<a id="6-commit"></a>
## 6. Compromisso

Compromete-se com `.glb`A sua `.json` E o Folheto `ids.json` Juntos `ids.json` Mantenha os IDs de registro de cada modelo fixos e salva os nomes dos edifícios colocados por esses IDs.
