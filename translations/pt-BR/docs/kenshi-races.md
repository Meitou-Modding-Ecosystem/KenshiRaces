<a id="kenshi-races"></a>
# Kenshi Races

Três novas raças para o editor de personagens, cada uma com sub-raças, nos esqueletos humanos do jogo ( `kenshi.races` , sem dependências ): cada registro adicionado no carregamento, os corpos ajustados pela [race import](race-import.md) . Ele também vem com uma [biblioteca de animação](animations.md) de 59 clipes para cada corrida nesses esqueletos. Os mods usam as corridas e clipes, adicionam sub-raças ou adicionam suas próprias corridas: [depending-on-kenshi-races.md](depending-on-kenshi-races.md) .

<a id="races"></a>
## Corridas

O editor lista cada RACE_GROUP como uma corrida e suas RACEs como sub-raças (engine docs/characters.md "Editor de personagens").

| Corrida ( RACE_GROUP ) | Sub-raça ( RACE ) | Gêneros | PERSONAGENS |
| --- | --- | --- | --- |
| Leoma `30-` | leoma `10-` | masculino, feminino | leomamacho `11-` , leomamacho `17-` |
| Crevan `31-` | crevan `18-` | masculino, feminino | crevanmacho `19-` , crevanmacho `21-` |
| Crabfolk `32-` | homem caranguejo `12-` | solteiro | homem caranguejo `13-` |
|  | rainha do caranguejo `14-` | solteiro | rainha do caranguejo `15-` |

Os IDs terminam em `KenshiRaces.mod` . Os nomes Subrace e CHARACTER são uma palavra minúscula, então o `spawn` do console os leva sem aspas. Os arquivos de cada corpo são `KenshiRaces/items/races/kenshiraces_<character>*` (mapas normais em Leoma e Crevan ). Os corpos masculinos são montados no `male_skeleton` e os femininos no `female_skeleton` ; os corpos dos caranguejos são masculinos, então uma rainha caranguejo é do mesmo gênero com animações e voz masculinas. Modelos de origem (exportações Meshy , slim 1): leomamale "Leonid Bare - 3ds Max Biped.glb", crabman e crabqueen "... - 3ds Max Biped.glb", os outros `leonidfemale.glb` , `corvidmale.glb` , `corvidfemale.glb` .

<a id="records"></a>
## Registros

Adicionado na fase de dados dos próprios registros do jogo carregado ( `RaceRecords` ), então `KenshiRaces.mod` é enviado vazio e CI não precisa de jogo:

- RACE_GROUP : nome da raça e RACEs de suas sub-raças, na ordem do editor.
- RACE (sub-raça): uma cópia do Groenlandês ( `17-gamedata.quack` : estatísticas, animações, vozes) com malha de cada gênero, textura de cor, mapa normal e mapa parcial (uma sub-raça de gênero único usa seu corpo único para ambos); `single gender` a menos que tenha ambos os corpos; sem cabeça ou cabelo (uma raça que não lista nenhuma cabeça não desenha nenhuma; o corpo tem a sua própria); `morph num` 0; `no shirts` (camisas e calças pintam o layout UV do corpo humano); máscara corporal e mapas normais de força apagados; `playable` com `editor limits` [ `kenshiraces_editor.xml` ](#character-editor) .
- CHARACTER : uma cópia do Drifter ( `2205-gamedata.base` ) da sub-raça e gênero ( `female chance` 100 ou 0), sem kit, dinheiro ou recompensa, com conversa de recruta.
- Motor: o campo `mesh` do próprio CHARACTER substitui a malha do corpo de sua corrida.

<a id="character-editor"></a>
## Editor de personagens

As corridas são oferecidas na criação do personagem como as do próprio jogo. `KenshiRaces/items/races/kenshiraces_editor.xml` mantém apenas os controles deslizantes que movem os ossos, então eles funcionam nos corpos ajustados como em um humano (engine docs/animation.md, as escalas ósseas): O rosto mantém o tamanho da cabeça, o formato da cabeça e os controles deslizantes do pescoço (sem poses de rosto: as cabeças ajustadas não têm nenhuma); O corpo contém os controles deslizantes do corpo humano e o tom de pele, com tamanho dos seios, altura e espaçamento para mulheres, que movem o `L Boob` / `R Boob` do esqueleto feminino para o qual os corpos femininos são ponderados ([importação de corrida](race-import.md#bust) ). Os intervalos são os humanos ( `data/editor/editor_data_human.xml` ).

<a id="recruiting"></a>
## Recrutamento

DIALOGUE  `1-KenshiRaces.mod` : o personagem diz "Parece que você precisa de outro par de mãos.", o jogador responde "Junte-se a nós." e a ação 18 (juntar-se ao time) adiciona o personagem ao time do jogador.

<a id="testing-in-game"></a>
## Testando no jogo

1. Habilite Kenshi Races no inicializador (ou `data/mods.cfg` ).
2. Console: `spawn leomafemale` (ou qualquer CHARACTER na tabela) surge no cursor; fale com o recruta ou F10 depure o recruta.
3. F10 ** Kenshi Races : corridas ** lista todas as corridas equipadas carregadas (deste mod e dependentes).
4. Novo jogo: as setas de corrida do editor chegam a Leoma , Crevan e Crabfolk .

<a id="clothing"></a>
## Roupas

A armadura compartilha o esqueleto do corpo, por isso se move com esses corpos, mas é modelada em torno do corpo humano. O mapa de peças esconde a pele sob itens com `hide parts` (casacos, robes, jaquetas); o emagrecimento atrai o que é mais volumoso do que um ser humano em outro lugar ([race import](race-import.md#slimming) ). Formas distantes dos humanos (conchas, garras) ainda atravessam armaduras apertadas.
