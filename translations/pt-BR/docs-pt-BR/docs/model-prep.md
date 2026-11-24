<a id="preparing-a-model"></a>
# Preparando um modelo

Como obter um `.glb` gerado ( Meshy , Hi3D , Blender ) para um polycount, tamanho de textura e tamanho do mundo real que o jogo controla, antes de ir para `<Mod>/Structures/` (edifícios) ou `<Mod>/ItemModels/` (itens). A importação em si: [estruturas.md](structures.md) .

Os comandos são executados a partir da raiz do repositório no PowerShell. Precisa de Python 3; `prepare-model.py` também precisa de `pip install pillow numpy` .

<a id="1-inspect"></a>
## 1. Inspecione

```powershell
python tools\model-info.py <Mod>\Structures\my-hut.glb
```

Ele imprime o tamanho em metros (X, Y para cima, Z), o lado mais longo, triângulos, materiais e o tamanho do pixel de cada textura e MB . Uma observação sobre a escala ou rotação do nó significa que o `prepare-model.py` não pode redimensioná-lo: exporte-o novamente do Blender com as transformações aplicadas (Objeto > Aplicar > Todas as transformações).

<a id="2-cut-the-polycount"></a>
## 2. Corte o polycount

| Tipo | Triângulos | Texturas |
| --- | --- | --- |
| Construção ou decoração | 5.000 a 20.000 | 2048 |
| Item (retido, deixado cair, em uma loja) | menos de 5.000 | 1024 |

Os geradores geralmente dão 100.000 ou mais; a importação alerta acima de 150.000. Para cortar:

- ** Meshy :** Faça novamente a malha do modelo com um polycount alvo no orçamento acima (topologia quádrupla ou triangular ambas funcionam), mantendo as texturas, e baixe-o como GLB novamente.
- ** Blender :** Arquivo > Importar > glTF, selecione a malha, adicione um modificador Decimate (Recolher) e diminua a Proporção até que a contagem de faces na barra de status esteja no orçamento, aplique-a e, em seguida, Arquivo > Exportar > glTF Binário ( `.glb` ) com Aplicar Modificadores ativado.

A colisão é simplificada separadamente ( `collisionTriangles` , padrão 2000), portanto, o corte da malha visível depende da taxa de quadros e do tamanho do arquivo.

<a id="3-choose-the-size"></a>
## 3. Escolha o tamanho

A importação trata o modelo como metros; o tamanho de um gerador é arbitrário. Escolha o tamanho real e defina-o no arquivo de configurações do modelo ([estruturas.md "Settings"](structures.md#settings) ), não no modelo:

- Prédio: `"height"` em metros em `<model>.json` .  O pessoal do Kenshi tem cerca de 1,8 m de altura e é pequeno em comparação com os tamanhos do mundo real, então pegue o tamanho que você descreveria e adicione 20% (uma postagem de 2,2 m se torna `2.64` ).
- Item: `"size"` , seu lado mais longo em metros, em `<model>.json` ao lado em `ItemModels/` .

<a id="4-shrink-the-textures"></a>
## 4. Reduza as texturas

```powershell
python tools\prepare-model.py <Mod>\Structures\my-hut.glb 2048
```

Isso reescreve o arquivo no lugar com cada textura com no máximo aquela quantidade de pixels por lado (2.048 para edifícios, 1.024 para itens), geralmente cortando de dezenas de MB para alguns. Um terceiro argumento opcional também redimensiona o modelo para o lado mais longo em metros, para quando um arquivo de configurações não for desejado. A importação também reduz pela metade as texturas maiores ( `textureSize` ), mas a confirmação do arquivo reduzido mantém o repositório pequeno.

Execute `model-info.py` novamente para verificar o resultado.

<a id="5-import-and-check"></a>
## 5. Importe e verifique

Crie e implante com o Play (ou execute a importação sozinho, [structures.md "Quick start"](structures.md#quick-start) ). O log possui uma linha por modelo:

```
Structure my-hut.glb -> "My Hut": 2 part(s), 12480 triangles, 3.1 m tall, collision mesh, unlocked by start
Item beast-capsule.glb -> mesh mymod_beast_capsule.mesh, material 950000-MyMod.mod, 4082 triangles, 0.11 m long
```

Verifique a altura ou comprimento e a contagem de triângulos, depois observe no jogo: coloque o prédio ou solte e pegue o item.

<a id="6-commit"></a>
## 6. Comprometa-se

Confirme o `.glb` , seu `.json` e o `ids.json` da pasta juntos. `ids.json` mantém fixos os ids de registro de cada modelo e salva os nomes dos edifícios colocados por esses ids.
