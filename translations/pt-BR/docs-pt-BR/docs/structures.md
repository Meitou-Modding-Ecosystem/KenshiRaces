<a id="structures-from-3d-models"></a>
# Estruturas de modelos 3D

Coloque um modelo `.glb` ( Meshy , Hi3D , uma exportação Blender ) na pasta `Structures` do seu mod e a construção do jogo o transforma em uma construção que você pode colocar no menu de construção: malha Ogre, texturas compactadas, colisão e os registros em `<Mod>.mod` . Não FCS , não Blender , nenhuma instalação de jogo necessária para a importação em si.

<a id="quick-start"></a>
## Início rápido

1. Coloque o modelo em `<Mod>/Structures/` , por ex. `MyMod/Structures/watch-tower.glb` .
2. Opcional: ao lado, `watch-tower.json` com qualquer [configurações](#settings) , por ex.

   ```json
   { "name": "Watch Tower", "height": 6, "materials": 20, "description": "A lookout of scrap and rope." }
   ```

3. Crie e implante normalmente ([play.md](play.md) ): Visual Studio F5, VS Code F5 ou `dotnet run --project Play` . O log de construção mostra uma linha `Structure ...` por modelo.
4. No jogo: menu de construção, categoria `EXTERIOR` (ou aquela que você definir). Com o `"research": "start"` padrão ele é conhecido desde o início do jogo, e também nos salvamentos existentes ( Meitou desbloqueia a partir da pesquisa finalizada, que sempre inclui `_Default Start` ).
5. Confirme o modelo, seu `.json` e `Structures/ids.json` (escrito na primeira importação). Os arquivos gerados são saída de compilação e não são confirmados.

Sem jogo: `dotnet run --project tools/StructureImport -- <Mod>/Structures <Kenshi>/mods/<Mod>` .

<a id="settings"></a>
## Configurações

Todos opcionais ( `<model>.json` , comentários permitidos; um nome desconhecido é um erro para que erros de digitação não passem despercebidos).

| Contexto | Padrão | Significado |
| --- | --- | --- |
| `name` | nome do arquivo, `-` / `_` como espaços | nome no menu de construção |
| `description` | `""` | construir texto do menu |
| `height` | tamanho próprio do modelo | altura final em metros; o modelo é dimensionado uniformemente para ele.  Os modelos AI vêm em tamanhos arbitrários, então defina isso |
| `sink` | `0` | metros afundados no solo (evita que flutue nas encostas) |
| `yaw` | `0` | graus girados em torno do eixo vertical |
| `category` | `EXTERIOR` | categoria do menu de construção ( `building category` ): o do jogo ( `BUILDINGS` , `CAMPING` , `FARMING` , ...) ou um novo nome |
| `group` | `""` | `building group` : edifícios que compartilham um são variantes de uma entrada |
| `materials` | `5` | Materiais de construção que custa |
| `buildSpeed` | `2` | `build speed mult` (tendas do jogo 5, casas 1 a 2) |
| `research` | `start` | `start` : conhecido desde o início; `none` : nada o desbloqueia (seus próprios registros o fazem); ou um ID de string RESEARCH , por exemplo `96564-rebirth.mod` |
| `placement` | `outdoor` | `outdoor` , `indoor` ou `both` |
| `createsTown` | `true` | colocá-lo fora de uma cidade inicia um posto avançado |
| `collision` | `mesh` | `mesh` (uma cópia simplificada do modelo), `box` (sua caixa delimitadora) ou `none` (percorra-o) |
| `collisionTriangles` | `2000` | a maioria dos triângulos que a colisão `mesh` mantém (12 a 50.000) |
| `textureSize` | `2048` | maior lado de textura em pixels (uma potência de dois, no máximo 8192) |

<a id="item-models"></a>
## Modelos de itens

Em vez disso, coloque o modelo de um item em `<Mod>/ItemModels/` ( `Structures` o tornaria um edifício). A importação grava sua malha e texturas ao lado das estruturas e um registro MATERIAL_SPECS_CLOTHING ; o registro ITEM do seu mod aponta para eles. O log de construção mostra uma linha `Item ...` por modelo com os nomes a serem usados.

1. `MyMod/ItemModels/lantern.glb` , opcionalmente `lantern.json` : `{ "size": 0.3 }` .
2. No seu registro de item (gerador de dados ou fase de dados), defina:

| Campo | Valor |
   | --- | --- |
| `mesh` e `ground mesh` (nomes de arquivos) | `<mod>_<model>.mesh` , por ex. `mymod_lantern.mesh` | | `material` (referência) | `<950000 + 100 × block>-<Mod>.mod` , bloquear de `ItemModels/ids.json` , por ex. `950000-MyMod.mod` |

Procure o material primeiro ( `GameDatabase.Find` ) e mantenha a aparência do substituto quando estiver faltando: um zip liberado e testes que leem o jogo sem o mod implantado, não o possuem.
3. Confirme o modelo, seu `.json` e `ItemModels/ids.json` .

| Contexto | Padrão | Significado |
| --- | --- | --- |
| `name` | nome do arquivo, `-` / `_` como espaços | o nome do registro do material |
| `size` | tamanho próprio do modelo | lado mais longo em metros; o modelo é dimensionado uniformemente para ele |
| `yaw` | `0` | graus girados em torno do eixo vertical |
| `textureSize` | `1024` | maior lado de textura em pixels (uma potência de dois, no máximo 8192) |

- Um modelo de material por item (o ITEM nomeia um `material` ); o modelo fica no ponto mais baixo, centralizado, como fazem as malhas de itens do jogo.
- O jogo desenha um item no chão, em uma mão e em um ícone automático com seu `mesh` ( `ground mesh` primeiro no chão) e o MATERIAL_SPECS_CLOTHING seus nomes `material` (motor `docs/formats/runtime-materials.md` "Callers", `WorldObjectLayout.ItemMesh` ).
- O ícone do inventário é extraído do modelo, a menos que o registro defina `auto icon image` como falso e seu próprio `icon` .

<a id="getting-a-good-model"></a>
## Conseguir um bom modelo

- Peça ao Meshy ou Hi3D um modelo com textura PBR e baixe-o como GLB . Algumas dezenas de milhares de triângulos são suficientes; o log sinaliza um modelo acima de 150.000. Cortando o polycount, escolhendo o tamanho e diminuindo as texturas: [model-prep.md](model-prep.md) .
- A frente do modelo está voltada para +Z no arquivo (convenção do glTF); use `yaw` para girá-lo.
- Cada material se torna sua própria parte. Os geradores geralmente fornecem um; no máximo 49.
- O modelo fica no ponto mais baixo, centralizado.

<a id="what-it-writes"></a>
## O que escreve

Na pasta mod implantada ( `<Kenshi>/mods/<Mod>/` ):

- `structures/<mod>_<model>.mesh` (um por material, `_1` , `_2` , ... quando vários): Ogre `[MeshSerializer_v1.8]` , posição, normal, UV e tangente com lateralidade em w, o layout das malhas de construção do próprio jogo.
- `structures/<mod>_<model>_d.dds` , `_n.dds` , `_m.dds` : difuso (BC3), normal (BC1, BC3 quando cortado), metalizado (BC1), com mips.
- `structures/<mod>_<model>_col.xml` : a colisão.
- Para um modelo de item: `structures/<mod>_<model>.mesh` e suas texturas, sem colisão.
- Registros mesclados em `<Mod>.mod` ao lado de qualquer outra coisa nele (os registros de um gerador de dados são mantidos; uma importação anterior é substituída): um BUILDING , um BUILDING_PART e um MATERIAL_SPEC por material, e o desbloqueio (o `enable buildings` da pesquisa ganha o edifício); um MATERIAL_SPECS_CLOTHING por modelo de item.

Os nomes dos arquivos começam com o nome do mod porque o jogo procura recursos pelo nome do arquivo em cada mod.

Os IDs de registro são `<n>-<Mod>.mod` com n de 900.000 em diante, um bloco de 100 por modelo (construção +0, peças +1, materiais +50); modelos de itens contam a partir de 950.000 (material +0) em `ItemModels/ids.json` . `Structures/ids.json` mantém o bloco de cada modelo para que os IDs nunca mudem: um salvamento nomeia um edifício colocado pelo ID de seu registro. Nunca edite ou remova um bloco; remover um modelo remove sua construção dos jogos que o colocaram.

<a id="facts-the-import-relies-on"></a>
## Fatos nos quais a importação depende

Verificado com os arquivos do jogo (dados Kenshi 1.0.68) e com os leitores do motor:

- **Unidades**: a unidade mundial é 0,1 m, Y para cima; glTF tem metros, Y para cima, então a escala é 10 (os arquivos de colisão do jogo usam gravidade 98,1).
- **Materiais**: a aparência de uma peça de construção é seu MATERIAL_SPEC ( `texture map` , `normal map` , `metalness map` , `material type` ), não o nome do material da malha (mecanismo `docs/formats/runtime-materials.md` ). O shader de construção do jogo ( `data/materials/deferred/objects.hlsl` ) lê RGB difuso como cor e A como brilho (vezes `specular mult` ), RGB normal como o espaço tangente normal e A como a máscara de recorte ( `material type` 1, `alpha threshold` ), metalidade de R. A importação grava gloss = 1 - rugosidade e metalidade do glTF do canal azul do glTF; glTF MASK e BLEND tornam-se um recorte.
- **Tangentes**: o binormal do shader é cross(tangente, normal) × w, e nas malhas do jogo ele aponta ao longo de V crescente (**Verificado**: `prisoner_pole.mesh` 86 de 86 triângulos, `Camp_tent1.mesh` 1858 de 1865). Os mapas normais do glTF apontam para verde na imagem (diminuindo V), então a importação vira o canal verde e define o caminho do jogo.
- **Colisão**: o `xml collision` de uma peça é um arquivo PhysX NxuStream2; um arquivo mínimo escrito à mão é carregado (o `Camp_tent1_COL.xml` do jogo é uma caixa). PhysX está Z up e o jogo mapeia (x, y, z) para (x, z, -y), então a importação escreve os pontos da parte como (x, -z, y). A malha triangular aparece como pontos simples e triângulos; o jogo cozinha durante a carga, Meitou lê como está (motor `docs/formats/collision.md` ).
- **Campos**: BUILDING e BUILDING_PART carregam o conjunto completo de campos da Barraca de Acampamento 1 do jogo ( `18026-small_changes_otto.mod` ), então nada cai no padrão; não há `functionality` , então é um edifício simples.
- **Desbloquear**: um edifício está no menu de construção assim que uma pesquisa finalizada o lista em `enable buildings` ; todo jogo começa com `_Default Start` ( `5359-gamedata.base` ) finalizado. **Verificado** com o motor `GameDatabase` , `ResearchGate` e `BuildingBuildData` no jogo base mais um mod importado: as estruturas são desbloqueadas e custam materiais de construção conforme definido.

<a id="not-covered-yet"></a>
## Ainda não coberto

Interiores, portas, funções (armazenamento, camas, artesanato) e peças giratórias.
