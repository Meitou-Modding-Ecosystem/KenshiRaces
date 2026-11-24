<a id="kenshi-races"></a>
# Kenshi raças

Três novas corridas para o editor de personagens, cada uma com subrases, no esqueleto humano do jogo (`kenshi.races`, sem dependência): cada registro adicionado à carga, os corpos equipados pelo [A corrida importação](race-import.md). Também vai embarcar em [Biblioteca de Animação](animations.md) 59 clipes para cada corrida sobre esses esqueletos. Os modos usam as corridas e clipes, adicionam subrases ou adicionam suas próprias corridas: [depending-on-kenshi-races.md](depending-on-kenshi-races.md).

<a id="races"></a>
## raças

O editor apresenta cada RACE_GROUP Como uma corrida e sua RACEs como subrases (Motor docs/characters.md “Editor de caráter”.

| A corrida (RACE_GROUP) | Apresentação (RACE) | Gênero | CHARACTERs |
| --- | --- | --- | --- |
| Leoma `30-` | Leão `10-` | Homem e Mulher | Leomão `11-`Leia mais `17-` |
| Crevan `31-` | A equipe `18-` | Homem e Mulher | Crédito `19-`Criança `21-` |
| Crabfolk `32-` | Crabão `12-` | Solteira | Crabão `13-` |
| | Crabqueão `14-` | Solteira | Crabqueão `15-` |

Fim do `KenshiRaces.mod`. Subraça e CHARACTER Nomes são uma palavra inferior, portanto, a consola `spawn` Faça isso sem citações. Os arquivos de cada corpo são `KenshiRaces/items/races/kenshiraces_<character>*` (Mapas normais em Leoma e Crevan). Os corpos masculinos estão preparados para `male_skeleton`As mulheres em `female_skeleton`Os corpos de crab são masculinos, então uma rainha de crab é de gênero único com as animações masculinas e a voz. O modelo de origem (Meshy exportações, limpo 1): leomal "Leonid Bare - 3ds Max Biped.glb", crabman e crabqueen "... - 3ds max Biped,glb", os outros `leonidfemale.glb`, `corvidmale.glb`, `corvidfemale.glb`.

<a id="records"></a>
## Recordes

A partir da data de lançamento do jogo (`RaceRecords`) assim `KenshiRaces.mod` Naves vazias e CI não precisa de jogo:

- RACE_GROUPO nome da raça e suas subraças RACEsEm ordem do editor.
- RACE (subraça): uma cópia de Greenlander (`17-gamedata.quack`: estatísticas, animações, vozes) com cada gênero, textura de cores, mapa normal
e um mapa parcial (uma subraça de gênero jate usa um corpo para ambos); `single gender` a não ser que tenha ambos os corpos; nenhum cabelo ou cabelo (uma raça que não lista nenhum cabelo tira nenhum; o corpo tem o seu próprio); `morph num` 0; `no shirts` (T-shirts e pantalões pintam o layout UV do corpo humano); máscaras do corpo e cartas normais de força limpos; `playable` com `editor limits` [`kenshiraces_editor.xml`](#character-editor).
- CHARACTERUma cópia de Drifter (`2205-gamedata.base`(a) da subraça e do gênero (`female chance` 100 ou 0), nenhum kit, dinheiro ou bônus, com o
Conversação de recrutamento.
- Motivo: A CHARACTERO seu próprio `mesh` O campo supera a mesha corporal da sua corrida.

<a id="character-editor"></a>
## Caracterização da Editora

As corridas são oferecidas na criação de personagens como o próprio jogo. `KenshiRaces/items/races/kenshiraces_editor.xml` mantém apenas os esmagadores que movem os ossos, para que trabalhem nos corpos equipados como em um humano (motor) docs/animation.md, as escalas ósseas): o rosto tem o tamanho da cabeça, a forma da cabeça e as esguilas do pescoço (não há posições do rosto: as cabeças fixadas não têm nenhuma); o corpo tem as esguilhas do corpo humano e a tonalidade da pele, com o tamanho do peito, a altura e o espaço para as mulheres, que move o esqueleto feminino `L Boob` / `R Boob` As mulheres são consideradas ([A corrida importação](race-import.md#bust)). Os homens são os homens (`data/editor/editor_data_human.xml`).

<a id="recruiting"></a>
## Recrutamento

DIALOGUE `1-KenshiRaces.mod`O personagem diz: "Você pode usar um par de mãos", e a ação 18 (a equipe) adiciona o personagem à equipe.

<a id="testing-in-game"></a>
## Teste em jogo

1. Possibilidade de Kenshi Race no lançador (ou `data/mods.cfg`).
2. Consolação : `spawn leomafemale` (ou qualquer CHARACTER na tabela) esponja no cursor; falar para recrutamento, ou F10 recrutamento de borracha.
3. F10 **Kenshi Race: A Race** listas de todas as corridas emplacadas (aqueles modos e dependentes).
4. Novo jogo: as armadilhas da corrida do editor chegam Leoma, Crevan e Crabfolk.

<a id="clothing"></a>
## Roupas

Armour partilha o esqueleto do corpo, então ele se move com esses corpos, mas é modelado em torno do corpo humano. A parte do mapa esconde a pele sob itens com `hide parts` (Caixas, roupas, jantares); a perda de peso atrapalha no que é mais bulkiro do que um humano noutro lugar ([A corrida importação](race-import.md#slimming)). As formas longe dos humanos (caixas, cravas) ainda se apodrecem através de armário estreito.
