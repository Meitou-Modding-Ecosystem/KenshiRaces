<a id="race-import-fitting-a-body-onto-the-games-skeleton"></a>
# Race Import: Ajuste um corpo no esqueleto do jogo

`tools/RaceImport` Tornando um rigido `.glb` O corpo (Meshy, Hi3D3ds Max Biped, os joints `Bip001 ...`) em um corpo o jogo se anima com suas próprias animações humanas: uma mesha de Ogre coberta ligada a `male_skeleton.skeleton` ou `female_skeleton.skeleton`A sua DDS Texto e parte do mapa. Ele lê o esqueleto, corpo humano e parte do jogo de instalação, então ele corre em uma máquina com KenshiNunca em CI; a sua produção é comprometida.

<a id="run-it"></a>
## Corra isso

```powershell
dotnet run --project tools/RaceImport -c Release -- <model.glb> <Kenshi folder> KenshiRaces/items/races kenshiraces_<character> [male|female] [slim]
```

- `<Kenshi folder>`Apresentação do jogo (`data/` dentro de si, por exemplo. `"C:\Program Files (x86)\Steam\steamapps\common\Kenshi"`.
- Os símbolos nomeam os arquivos e devem ser únicos em mods (o jogo encontra arquivos por só nome): `<modfolder lower case>_<race>`.
- `male` / `female`O RIG, O MULHER DEFINENTE. Um corpo feminino está preparado para `female_skeleton` Medido contra `human_female.mesh` e
`FemaleMask_Default.png` (animações femininas e voz no jogo, o editor de pedaços); um homem para `male_skeleton`, `human_male.mesh` e `MaleMask_Default.png`.
- `slim`0 a 1 (default 1 a 1)[O Slimme](#slimming)( ); reduzi-lo para uma corrida que significava ser mais bulk do que suas roupas.
- Escritos `<stem>.mesh`, `<stem>_d.dds`, `<stem>_n.dds` (só quando o modelo tem um mapa normal) e `<stem>_parts.png`A linha de log dá a
Número de vertices, escala, limites e quantas vertices caem em partes escondidas.
- Mantenha o `.glb` fora do repo (tens de MB); tome as quatro saídas. `.gitignore` Deixe `*/items/races/*.mesh` e `*.dds` através do
- Um modo dependente, que é executado a partir de um clone KenshiRaces com sua própria pasta de saída (`<Mod>/items/races`e a votação.

<a id="what-the-model-needs"></a>
## O que o modelo precisa

- Uma camada cuja pele tem as articulações Biped `Bip001 Pelvis`, `Spine`, `Spine1`, `Spine2`, `Neck`, `Head`Os dois `Clavicle`, `UpperArm`,
`Forearm`, `Hand`, `Thigh`, `Calf`, `Foot`, `Toe0`e `mixamorig:HeadTop_End` acima da cabeça. As articulações extras (dentes, ombros, ajudantes) seguem o seu ancestral mais próximo. **Verificado** em Leoma, CrevanCobra e a rainha Cobra (Meshy das exportações).
- Uma textura de cor básica; uma secura metálica e uma textura normal são usados quando presentes.
- Qualquer posição de descanso: T-posição e A-posição ambos se encaixam (a posição de liga do jogo tem os braços para baixo).

<a id="how-the-fit-works"></a>
## Como funciona o Fit

1. Cada vertex é pintado para o resto da posição dos arquivos (inversamente ligando tempos de repouso joint transform).
2. Escala: a altura do osso do jogo acima da articulação do modelo acima do seu vertex mais baixo.
3. Cada espinha, braço e perna é voltada para que ele ponha de sua articulação para o seu filho como o jogo faz, e estendido para o comprimento do jogo.
de 6 a 1,6 vezes).
4. Pelvis, espinhas, pescoço e clavicles são equipados como um pedaço rígido (mais quadrados sobre o pescoço, ombro e articulações do jogo), então o peito
mantém a sua forma; per-bone se encaixa em lá onde a espina e o clavicle pesam misturando. As articulações da cintura são deixadas fora da mesma forma: a cintura do jogo é de nível com as suas cabeças e os lugares de exportação de uma cintura, que caiu no peito em torno de uma unidade.
5. A cabeça caiu para o meio de onde o torso o leva para a articulação da cabeça, assim que sua camada fica abaixo do seu ossos. O pescoço do jogo sobe
A partir de então, a cabeça de um estê-lo se estende a cerca de três vezes o seu comprimento (**Observado** No Homem LeomaCrabman e a Rainha Crab: equipados para o pescoço 2.4 unidades, os modelos possuem 0.8, o jogo 1.4).
6. As pernas estão bem escuras e são esfregadas para que os solos se põem no chão (as pernas e os pés de crab são mais altos do que os pés do jogo).
7. Cada vertente é a soma pesada das suas articulações; os seus pesos vão para os ossos de jogo, os quatro mais pesados são mantidos.

**Verificado**As mesas da ferramenta C# correspondem ao protótipo original de Python (posições dentro de 1e-5 unidades, pesos ósseos idênticos) em Crabman.

<a id="slimming"></a>
## O Slimme

A roupa do jogo é modelada em torno do corpo humano da riga, de modo que o corpo é mais bulkier do que ele mostra. Ao redor de cada torso e osso limbico (pélvicos, espinhas, clavículas, braços e pernas superiores e inferiores) a ferramenta mede a extensão mais estreita do corpo humano por um sexto dos ossos e por doze de um giro ao seu redor; um vertex fixado além desse extensão plus 0.15 unidades (1.5 cm de costura) mantém um quinto do seu excesso, sua correção misturada pelos seus pesos ósseos. A correção é entre 1,5 e 2,5 vezes o limite (um apêndice como um cego, só), é zero para uma vertente fora das camadas (uma coluna pendurada abaixo da pelvis), e é suave sobre a superfície (12 rodadas de vizinho, UV seados soldados) para que as vertices vizinhas se movam de forma semelhante. **Observado** ( o homem Leoma• 39% das vertices movem-se, em média, a 1,3 cm e a 9,5 cm no máximo.

<a id="bust"></a>
## ônibus

O corpo de uma mulher é elevado para o homem feminino (**Observado**• O Leoma e Crevan As mulheres colocaram seus 2.0 e 1.75 unidades abaixo, debaixo do peito da roupa e da parte do mapa). O busto é o meio do veto de uma unidade quadrado, entre `Spine1` e os ombros e 0.3 a 1.8 unidades ao lado da linha média, cujo vertex mais frontal é o mais próximo. Cada vertente aumenta pela diferença de tempo seu peso torso-bônus, vezes exp(-(dy / 1.5)2) de sua altura do colo, vezes um passo suave da profundidade da espina (0) para o colo (1), para que a parte de trás, os braços e as pernas permanecem. Depois, a Lula corre.

<a id="human-only-bones"></a>
## Os ossos humanos

O esqueleto feminino tem `L Boob` e `R Boob` (O tamanho do peito do editor, altura e escala de espaço e mover-os, motor docs/animation.mdque falta um Biped rig. Após a perda de peso, cada vertex cujo osso mais pesado é `Spine1` ou `Spine2` Toma os pesos humanos do vertex humano mais próximo que tem alguns, dentro de 1 unidade, escalado por 1 - distância; seus próprios pesos se reduzem para fazer espaço. **Observado** (`human_female.mesh`): as vertices do estômago são carregadas pelos dois ossos (892 das 994 vertices front-chest). Os ossos (`L Boob` 28, `R Boob` 29) e as outras mãos da rig femininas diferem da do masculino (`R Clavicle` 23 contra 25; o homem tem `Bip01 Jaw`Assim, as mesas são sempre escritas contra as mãos de seu próprio rigo.

<a id="part-map"></a>
## Parte do mapa

A corrida da `part map male`/`female` PNG Marcas que corpo parte de cada vertex da Terra pertence; objetos usados' `hide parts` Não deixe de desenhar essas peças (motor docs/characters.md “As partes do corpo escondidas”. A ferramenta dá a parte da vertente mais próxima do corpo humano, levada pelo mesmo osso, lida do mapa da parte do ramo no UV desse vertente, e escreve-a no próprio UV (1024 × 1024), outros tecidos transparentes. Cada parte humana é cultivada com 0,8 unidades (uma parte que toma a parte mais próxima), uma vez que um corpo de bullying clipe sobre uma faixa mais ampla.

**Observado** (`MaleMask_Default.png`2562, mostrado em `human_male.mesh` UVs como o jogo mostra: 642 de 15 vertices têm uma parte: vermelho o peito e o ombro, bandas brancas em torno dos braços superiores, verde a frente do estômago. `hide parts` em dados de base (rebirth.mod): Drifter e Padded Leather Jacket 1, Sleeveless Longcoat e Dustcoat 3, Ninja Gi e Rags 4, roupas 5, juntas de pele e Crab Armour 7; Santo Servente Rags, Monk Pants e Hoods 0. Os itens com 0 não escondem nada; a perda de peso cobre aqueles. **Observado** (`FemaleMask_Default.png` Em `human_female.mesh` UVs: vermelho o busto (1,102 vertices), branco as bandas do braço superior (560), verde o estômago (250).

<a id="textures"></a>
## Texturas

- `_d`: cor de base, alfa = gloss = (255 - rigidez) / 2 ( rigidez 200 sem um mapa); 20482, BC3 com mips.
- `_n`: layout do personagem do jogo, X em alfa e Y em verde, Y flipped de glTF+ Y em cima; vermelho e azul 0.
