<a id="structures-from-3d-models"></a>
# Estruturas de modelos 3D

Ponta a `.glb` O modelo (Meshy, Hi3D, um blender exportador) em seu mod `Structures` A pasta e a construção do jogo tornam-se um edifício que você pode colocar no menu: Ogre mesh, texturas comprimidas, colisão e registros em `<Mod>.mod`. Não FCSNão há Blender, não há Instalação para a importação.

<a id="quick-start"></a>
## Início rápido

1. Deixe o modelo em `<Mod>/Structures/`e o g. `MyMod/Structures/watch-tower.glb`.
2. Opcional: Além disso, `watch-tower.json` Com qualquer [SISTEMAS](#settings)e o g.

   ```json
   { "name": "Watch Tower", "height": 6, "materials": 20, "description": "A lookout of scrap and rope." }
   ```

3. Aplicar e executar como é habitual ([play.md](play.md)• Visual Studio F5, VS Código F5, ou `dotnet run --project Play`. O log mostra um
`Structure ...` Linha por modelo.
4. No jogo: construindo o menu, categoria `EXTERIOR` (Ou o que você fez) Com o default `"research": "start"` É conhecido desde o início da
O jogo, e também em poupanças existentes (Meitou trabalha para abrir mão da pesquisa concluída, que sempre inclui `_Default Start`).
5. Comprometendo o modelo, a sua `.json` e `Structures/ids.json` (Não se sabe se a primeira introdução foi feita). Os arquivos gerados são criados e não são
Compromisso →

Sem jogar: `dotnet run --project tools/StructureImport -- <Mod>/Structures <Kenshi>/mods/<Mod>`.

<a id="settings"></a>
## A configuração

Tudo o que é opcional (`<model>.json`, comentários permitidos; um nome desconhecido é um erro para que os tipos não irão não notados).

| Estabelecimento | Definição | Significado |
| --- | --- | --- |
| `name` | Nome do arquivo, `-`/`_` Como espaços | Nome no menu de construção |
| `description` | `""` | Criar texto menu |
| `height` | O tamanho do modelo | altura terminada em metros; o modelo é escalado em equidade com ele. Os modelos de AI vêm em tamanhos arbitrários, então coloque isso |
| `sink` | `0` | metros caindo no chão (mantenha-o de flutuar em pedaços) |
| `yaw` | `0` | Os graus giram em torno do eixo vertical |
| `category` | `EXTERIOR` | Apresentação do menu (`building category`O jogo (`BUILDINGS`, `CAMPING`, `FARMING`... ou um novo nome |
| `group` | `""` | `building group`: edifícios compartilhando um são variantes de uma entrada |
| `materials` | `5` | Materiais que custam |
| `buildSpeed` | `2` | `build speed mult` (Tendas do jogo 5, casas de 1 a 2) |
| `research` | `start` | `start`conhecido desde o início; `none`Não há nada que possa desbloquear (o seu próprio registro faz); ou um RESEARCH Avaliação do ID, e.g. `96564-rebirth.mod` |
| `placement` | `outdoor` | `outdoor`, `indoor` ou `both` |
| `createsTown` | `true` | colocando-o fora de uma cidade começa um outpost |
| `collision` | `mesh` | `mesh` (Uma cópia simplificada do modelo) `box` (a partir da caixa) ou `none` (Caminhar pelo seu caminho) |
| `collisionTriangles` | `2000` | A maioria dos triângulos `mesh` A colisão mantém (12 a 50 mil) |
| `textureSize` | `2048` | maior lado de textura em pixels (uma potência de dois, no máximo 8192) |

<a id="item-models"></a>
## Modelos de item

Faça um modelo de item em `<Mod>/ItemModels/` Em vez disso (`Structures` que o tornaria um edifício). A importação escreve a sua mesha e as suas texturas ao lado das estruturas e um MATERIAL_SPECS_CLOTHING O que você faz; o seu mod ITEM O registro, então, aponta para eles. O log mostra um `Item ...` Linha por modelo com os nomes a usar.

1. `MyMod/ItemModels/lantern.glb`Opcionalmente `lantern.json`: `{ "size": 0.3 }`.
2. No seu registo de itens (generador de dados ou fase de dados), definir:

Avaliação do valor do campo
   | --- | --- |
| `mesh` e `ground mesh` (Filão) em São Paulo `<mod>_<model>.mesh`e o g. `mymod_lantern.mesh` | | `material` (Refiguração) Apresentação `<950000 + 100 × block>-<Mod>.mod`O bloco de `ItemModels/ids.json`e o g. `950000-MyMod.mod` |

Veja o material primeiro (`GameDatabase.Find`E mantenha a aparência quando está faltando: um zip liberado, e os testes que lêem o jogo sem o modo implementado, não tem.
3. Comprometendo o modelo, a sua `.json` e `ItemModels/ids.json`.

| Estabelecimento | Definição | Significado |
| --- | --- | --- |
| `name` | Nome do arquivo, `-`/`_` Como espaços | Nome do registro do material |
| `size` | O tamanho do modelo | o lado mais longo em metros; o modelo é escalado em equidade com ele |
| `yaw` | `0` | Os graus giram em torno do eixo vertical |
| `textureSize` | `1024` | maior lado de textura em pixels (uma potência de dois, no máximo 8192) |

- Um material por item modelo (o ITEM Nomes um `material`); o modelo está no seu ponto mais baixo, centrado, como o jogo item mesas fazem.
- O jogo tira um item no chão, em uma mão e em um ícone automático com sua `mesh` (`ground mesh` Primeiro no chão e depois no
MATERIAL_SPECS_CLOTHING A sua `material` Nome do motor `docs/formats/runtime-materials.md` Os chamadores, `WorldObjectLayout.ItemMesh`).
- O ícone de inventário é retirado do modelo, a menos que os registros `auto icon image` Falso e seu próprio `icon`.

<a id="getting-a-good-model"></a>
## Obter um bom modelo

- Pergunte Meshy ou Hi3D Para a PBR- Modelo de texto e descarregá-lo GLB. Algumas dezenas de milhares de triângulos são abundantes; a bandeira de logs é um modelo
Mais de 150 mil. Coloque o rebanho, escolha o tamanho e reduza as texturas: [model-prep.md](model-prep.md).
- O modelo de frente da cara + Z no arquivo (glTFA Convenção; utilização `yaw` para virá-lo
- Cada material torna-se sua parte. Geradores geralmente dão um; no máximo 49.
- O modelo está no seu ponto mais baixo, centrado.

<a id="what-it-writes"></a>
## O que escreve

Em um mapa de modos (`<Kenshi>/mods/<Mod>/`):

- `structures/<mod>_<model>.mesh` 1 por material, `_1`, `_2`, ... quando vários): Ogre `[MeshSerializer_v1.8]`A posição é normal, UV e
tangent com manutenção em w, o layout do jogo próprio edifício mesas.
- `structures/<mod>_<model>_d.dds`, `_n.dds`, `_m.dds`: difusa (BC3), normal (BC1, BC3 quando cortado), metalidade (BC1), com mips.
- `structures/<mod>_<model>_col.xml`E a colisão.
- Para um modelo de item: `structures/<mod>_<model>.mesh` e suas texturas, sem colisão.
- Os registros se misturam `<Mod>.mod` Além de qualquer outra coisa que seja colocada nele (os registros de um gerador de dados são mantidos; as importações anteriores são substituídas): a
BUILDINGE um BUILDING_PART e MATERIAL_SPEC O material, e o desbloqueio (a pesquisa `enable buildings` O edifício é construído; um MATERIAL_SPECS_CLOTHING por modelo de item.

Os nomes de arquivo começam com o nome do modo porque o jogo mostra recursos por nome de arquivo em cada modo.

Os registros ids são `<n>-<Mod>.mod` com n de 900000 para mais, um bloco de 100 por modelo (construção +0, peças +1, materiais +50); modelos de itens contam de 950000 (material +0) em `ItemModels/ids.json`. `Structures/ids.json` Mantenha o bloco de cada modelo para que os IDs nunca mudem: um saldo chama um edifício localizado pelo ID do seu registro. Nunca edite ou remova um bloco; remover um modelo remove seu edifício dos jogos que o colocaram.

<a id="facts-the-import-relies-on"></a>
## A importação depende de

Confira os arquivos do jogo (Kenshi 1.0.68 dados) e com os leitores do motor:

- **Unidades**: a unidade mundial é 0,1 m, Y em cima; glTF é metros, Y para cima, então a escala é 10 (os arquivos de colisão do jogo usam gravidade 98.1).
- **Materiais**O aspecto de uma parte do edifício é o seu MATERIAL_SPEC (`texture map`, `normal map`, `metalness map`, `material type`E não o Messias
Nome do material (Motor `docs/formats/runtime-materials.md`). Apresentação do jogo (`data/materials/deferred/objects.hlsl`Leia a Difusão RGB como cor e A como brilho (tempos `specular mult`• Normalidade RGB A, a, e a, a, a e a, e o que é a mesma (`material type` 1, `alpha threshold`A metalização de R. A importação escreve gloss = 1 - glTF O metal e o metal de glTFO canal azul; glTF MASK e BLEND Seja um cut-out.
- **Tango**O sistema binário do shader é cruzado (tangente, normal) × w, e nas meshas do jogo ele indica ao longo de aumento V (**Verificado**:
`prisoner_pole.mesh` 86 de 86 triângulos, `Camp_tent1.mesh` em 1858 e em 1865. glTF Os mapas normais apontam para a imagem (a redução de V), então a importação volta o canal verde e coloca o caminho do jogo.
- **Colisão**• Uma parte do `xml collision` é um arquivo PhysX NxuStream2; um arquivo escrito à mão mínimo cargas (o jogo `Camp_tent1_COL.xml` É
uma caixa) PhysX é Z up e os mapas do jogo (x, y, z) para (x, z, -y), portanto, a importação escreve os pontos da parte como (x, -z, y). O triângulo entra como pontos e triângulos; o jogo coza como é, Meitou lê como é. `docs/formats/collision.md`).
- **campos**: BUILDING e BUILDING_PART Confira a lista completa de jogos do Campeão de Campeões (`18026-small_changes_otto.mod`E assim nada
É uma falha; não `functionality`É um edifício plano.
- **Desbloqueio**Um edifício está no menu de construção uma vez que uma pesquisa concluída o enumera em `enable buildings`Cada jogo começa com
`_Default Start` (`5359-gamedata.base`e terminou. **Verificado** Com o motor `GameDatabase`, `ResearchGate` e `BuildingBuildData` sobre o jogo de base e um modo importado: as estruturas são desligadas e custam os materiais de construção como definido.

<a id="not-covered-yet"></a>
## Ainda não coberto

Interiores, portas, funções (servimento, camas, desenho) e peças de conversão.
