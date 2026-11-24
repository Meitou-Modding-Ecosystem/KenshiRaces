<a id="race-import-fitting-a-body-onto-the-games-skeleton"></a>
# Importação de corrida: encaixando um corpo no esqueleto do jogo

`tools/RaceImport` transforma um corpo `.glb` manipulado ( Meshy , Hi3D : o equipamento "3ds Max Biped", juntas `Bip001 ...` ) em um corpo que o jogo anima com suas próprias animações humanas: uma malha de Ogro esfolada vinculada a `male_skeleton.skeleton` ou `female_skeleton.skeleton` , suas texturas DDS e um mapa parcial. Ele lê o esqueleto do equipamento, o corpo humano e o mapa de partes da instalação do jogo, então roda em uma máquina com Kenshi , nunca em CI ; sua saída está comprometida.

<a id="run-it"></a>
## Execute-o

```powershell
dotnet run --project tools/RaceImport -c Release -- <model.glb> <Kenshi folder> KenshiRaces/items/races kenshiraces_<character> [male|female] [slim]
```

- `<Kenshi folder>` : a pasta do jogo ( `data/` dentro dela), por exemplo. `"C:\Program Files (x86)\Steam\steamapps\common\Kenshi"` .
- O radical nomeia os arquivos e deve ser exclusivo entre os mods (o jogo encontra os arquivos pelo nome): `<modfolder lower case>_<race>` .
- `male` / `female` : o equipamento, macho padrão. Um corpo feminino é ajustado ao `female_skeleton` e medido em relação ao `human_female.mesh` e `FemaleMask_Default.png` (animações e vozes femininas no jogo, os controles deslizantes do busto do editor); um masculino para `male_skeleton` , `human_male.mesh` e `MaleMask_Default.png` .
- `slim` : 0 a 1, padrão 1 ([Emagrecimento](#slimming) ); abaixe-o para uma corrida que deve ser mais volumosa do que suas roupas.
- Escreve `<stem>.mesh` , `<stem>_d.dds` , `<stem>_n.dds` (somente quando o modelo possui mapa normal) e `<stem>_parts.png` ; a linha de log fornece a contagem de vértices, escala, limites e quantos vértices caem em partes ocultáveis.
- Mantenha o `.glb` fora do repositório (dezenas de MB ); comprometer as quatro saídas. `.gitignore` permite a passagem de `*/items/races/*.mesh` e `*.dds` .
- Um mod dependente o executa a partir de um clone KenshiRaces com sua própria pasta de saída ( `<Mod>/items/races` ) e stem.

<a id="what-the-model-needs"></a>
## O que o modelo precisa

- Uma malha esfolada cuja pele possui as articulações bípedes `Bip001 Pelvis` , `Spine` , `Spine1` , `Spine2` , `Neck` , `Head` , ambas `Clavicle` , `UpperArm` , `Forearm` , `Hand` , `Thigh` , `Calf` , `Foot` , `Toe0` e `mixamorig:HeadTop_End` acima da cabeça. Articulações extras (dedos, caudas, ajudantes) seguem seu ancestral bípede mais próximo. **Verificado** em Leoma , Crevan , Crabman and the Crab Queen (exportações Meshy ).
- Uma textura de cor base; uma rugosidade metálica e uma textura normal são usadas quando presentes.
- Qualquer pose de descanso: pose T e pose A se encaixam (a pose de amarração do jogo tem os braços para baixo).

<a id="how-the-fit-works"></a>
## Como funciona o ajuste

1. Cada vértice é revestido na pose de repouso que o arquivo mostra (tempos de ligação inversos, transformação de repouso da junta).
2. Escala: a altura do osso da cabeça do jogo sobre a articulação da cabeça do modelo, acima do vértice mais baixo.
3. Cada osso da coluna, braço e perna é girado de modo que aponte de sua articulação para o filho, como faz o jogo, e esticado até o comprimento do jogo (0,6 a 1,6 vezes).
4. A pélvis, a coluna, o pescoço e as clavículas são encaixados como uma peça rígida (mínimos quadrados nas articulações do pescoço, ombros e quadril do jogo), para que o peito mantenha sua forma; os ajustes por osso cortam-no onde os pesos da coluna e da clavícula se misturam. As articulações da pélvis e da coluna ficam de fora desse ajuste: a pélvis do jogo está no nível dos quadris e um bípede exportado fica acima deles, o que deixa o peito cair cerca de uma unidade.
5. A cabeça pousa a meio caminho de onde o torso a leva até a articulação da cabeça do jogo, de modo que sua malha fica abaixo do osso. O pescoço do jogo sobe cerca de uma unidade acima de seus ombros e o desses modelos quase não o faz: colocar a cabeça na articulação estica seus pescoços cerca de três vezes seu comprimento (**Observado** no Leoma masculino, Crabman e a Rainha Caranguejo: pescoço ajustado 2,4 unidades, o próprio modelo 0,8, o jogo 1,4).
6. Os pés ficam em pé e são comprimidos de forma que as solas dos pés pousem no chão (as patas e os pés do caranguejo são mais altos que o pé do jogo).
7. Cada vértice é a soma ponderada dos ajustes de suas juntas; seus pesos vão para os ossos do jogo, suas articulações seguem, mantendo as quatro mais pesadas.

**Verificado**: as malhas da ferramenta C# correspondem às do protótipo original do Python (posições entre 1e-5 unidades, pesos de ossos idênticos) no Crabman.

<a id="slimming"></a>
## Emagrecimento

As roupas do jogo são modeladas em torno do corpo humano do equipamento, portanto, um corpo mais volumoso do que aparece. Em torno de cada osso do tronco e dos membros (pelve, coluna vertebral, clavículas, braços e pernas superiores e inferiores), a ferramenta mede a extensão máxima do corpo humano por sexto do osso e por doze avos de volta em torno dele; um vértice ajustado além dessa extensão mais 0,15 unidades (1,5 cm de tecido) mantém um quinto de seu excesso, sua correção mesclada por seus pesos ósseos. A correção desaparece entre 1,5 e 2,5 vezes o limite (além de: um apêndice como uma garra, deixado sozinho), é zero para um vértice fora das fatias do osso (uma cauda pendurada abaixo da pélvis) e é suavizada sobre a superfície (12 rodadas de média vizinha, costuras UV soldadas) para que os vértices vizinhos se movam de maneira semelhante. **Observado** (masculino Leoma ): 39% dos vértices se movem, 1,3 cm em média e 9,5 cm no máximo.

<a id="bust"></a>
## Busto

O busto do corpo feminino é elevado ao do humano feminino (**Observado**: as mulheres Leoma e Crevan sentaram os seus 2,0 e 1,75 unidades mais abaixo, sob o peito da roupa e o busto do mapa parcial). O busto está no meio da fatia de um quarto de unidade, entre `Spine1` e os ombros e 0,3 a 1,8 unidades ao lado da linha média, cujo vértice frontal está mais à frente. Cada vértice sobe pela diferença vezes o peso do tronco, vezes exp(-(dy / 1,5)²) de sua altura do busto, vezes um passo suave da profundidade da coluna (0) até a do busto (1), de modo que as costas, braços e pernas ficam. Depois o emagrecimento corre.

<a id="human-only-bones"></a>
## Ossos apenas humanos

O esqueleto feminino tem `L Boob` e `R Boob` (o tamanho do peito do editor, altura e escala de espaçamento e movê-los, motor docs/animation.md), que falta em um equipamento Biped. Após o emagrecimento, cada vértice cujo osso mais pesado é `Spine1` ou `Spine2` assume os pesos humanos do vértice humano mais próximo que possui alguns, dentro de 1 unidade, dimensionados em 1 distância; seus próprios pesos encolhem para abrir espaço. **Observado** ( `human_female.mesh` ): os vértices do busto são carregados pelos dois ossos (892 dos 994 vértices frontais do tórax). Os ossos ( `L Boob` 28, `R Boob` 29) e as outras alças do equipamento feminino diferem dos masculinos ( `R Clavicle` 23 contra 25; o macho tem `Bip01 Jaw` ), então as malhas são sempre escritas contra as alças de seu próprio equipamento.

<a id="part-map"></a>
## Mapa de peças

O `part map male` / `female`  PNG da corrida marca a qual parte do corpo cada vértice pertence; O `hide parts` dos itens desgastados para de desenhar essas peças (engine docs/characters.md "Partes ocultas do corpo"). A ferramenta fornece a cada vértice ajustado a parte do vértice do corpo humano mais próximo transportado pelo mesmo osso, lida no mapa de peças da plataforma no uv desse vértice e escreve no próprio uv do vértice ajustado (1024 × 1024, outros texels transparentes). Cada parte humana é primeiro aumentada em 0,8 unidades (um vértice sem uma parte ocupa o vértice marcado mais próximo dentro dessa distância), uma vez que um corpo mais volumoso se estende sobre uma faixa mais larga.

**Observado** ( `MaleMask_Default.png` , 256², amostrado em `human_male.mesh` uvs conforme o jogo os amostra): 642 dos 15.504 vértices têm uma parte: vermelho na parte superior do peito e ombros, faixas brancas ao redor dos braços, verde na frente da barriga. `hide parts` nos dados base (rebirth.mod): Drifter's e jaqueta de couro acolchoada 1, Longcoat e Dustcoat sem mangas 2, Longcoat e Dustcoat 3, Ninja Gi e Rags 4, vestes 5, jaquetas de placas, ternos de pele e Armadura de Caranguejo 7; Trapos de Servo Sagrado, Calças de Monge e capuzes 0. Itens com 0 não escondem nada; o emagrecimento cobre isso. **Observado** ( `FemaleMask_Default.png` a `human_female.mesh` uvs): vermelho o busto (1.102 vértices), branco as faixas dos braços (560), verde a barriga (250).

<a id="textures"></a>
## Texturas

- `_d` : cor base, alfa = brilho = (255 - rugosidade) / 2 (rugosidade 200 sem mapa); 2048², BC3 com mips.
- `_n` : o layout dos personagens do jogo, X em alfa e Y em verde, Y invertido de +Y do glTF para cima; vermelho e azul 0.
