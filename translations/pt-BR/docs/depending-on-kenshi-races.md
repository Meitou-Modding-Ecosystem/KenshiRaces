<a id="depending-on-kenshi-races"></a>
# Usar o framework em outro mod e adicionar sub-raças

Como um mod de código usa corridas e animações de Kenshi Races, adiciona sub-raças a elas ou adiciona suas próprias corridas (`kenshi.races`). Regras do carregador: mecanismo [modding-dependencies.md](https://github.com/Meitou-Modding-Ecosystem/MeitouClient/blob/master/docs/modding-dependencies.md). Uma habilidade do agente segue as mesmas etapas: [.claude/skills/kenshi-races-dependent](../../../.claude/skills/kenshi-races-dependent/SKILL.md).

<a id="1-reference-it-at-build-time-never-ship-it"></a>
## 1. Faça referência no momento da construção, nunca envie

Em um repositório feito a partir de [MeitouModTemplate](https://github.com/Meitou-Modding-Ecosystem/MeitouModTemplate), adicione Kenshi Races ' release zip a `meitou.lock.json` ; a compilação baixa, verifica e faz referência a ela, e o Play a implanta. Cada valor vem das notas de lançamento (`kenshi-races-v<version>`):

```json
{
  "id": "kenshi.races",
  "repo": "Meitou-Modding-Ecosystem/KenshiRaces",
  "tag": "kenshi-races-v0.4.0",
  "asset": "KenshiRaces-0.4.0.zip",
  "sha256": "<Zip SHA-256 from the notes>",
  "folder": "KenshiRaces",
  "engine": "<Submodule engine commit from the notes>"
}
```

- Seu pino `engine/` se move para esse commit do mecanismo (GutenStructure [framework lock](https://github.com/TheIvoryDisaster/GutenStructure/blob/beta/docs/framework-lock.md)).
- Nunca liste `KenshiRaces.dll` em seu próprio `assemblies` ; o `kenshi.races` carregado atende aos tipos.

<a id="2-declare-it-in-meitoujson"></a>
## 2. Declare em `meitou.json`

```json
"apiVersion": 2,
"gameVersion": ">=0.22.2",
"dependencies": [{ "id": "kenshi.races", "version": ">=0.4.0 <0.5.0" }]
```

- Antes de 1.0, uma versão secundária pode quebrar o API, então limite o intervalo na próxima versão secundária.
- Kenshi Races deve ficar acima do seu mod em `data/mods.cfg` (o Arrange do inicializador faz isso), para que seus registros existam quando sua fase de dados for executada.

<a id="3-use-its-races"></a>
## 3. Use suas corridas

Consulte os ids em `Races` de seus registros, por exemplo. na sua fase de dados:

```csharp
public void EditData(ModDataContext c)
{
    var guard = c.Add("20-MyMod.mod", FcsRecordType.Character, "leomaguard");
    // ... fields ...
    guard.References["race"] = [new FcsReference(Races.LeomaRace, 100, 0, 0)];
    guard.Ints["female chance"] = 50;                                   // leoma and crevan have both genders
}
```

- RACEs (sub-raças): `Races.LeomaRace`, `CrevanRace`, `CrabmanRace`, `CrabQueenRace`.
- PERSONAGENS (sem kit, recrutáveis): `LeomaMaleCharacter`, `LeomaFemaleCharacter`, `CrevanMaleCharacter`, `CrevanFemaleCharacter`, `CrabmanCharacter`, `CrabQueenCharacter`.
- RACE_GROUPs (as corridas do editor): `LeomaGroup`, `CrevanGroup`, `CrabfolkGroup`. `Races.Recruit` é o "Junte-se a nós". falar.
- `Races.Leoma`, `Crevan`, `Crabman`, `CrabQueen` (`Subrace`) e `Races.Groups` os descrevem. Tabela: [kenshi-races.md "Corridas"](kenshi-races.md#races).

<a id="4-add-a-subrace-or-a-race"></a>
## 4. Adicione uma sub-raça ou corrida

1. Coloque cada corpo com o [race import](race-import.md) de um clone KenshiRaces, no `items/races` do seu mod com sua própria haste (`mymod_<character>` ; `female` como equipamento para um corpo feminino) e envie os arquivos. Adicione `!/<Mod>/items/races/*.mesh` e `!/<Mod>/items/races/*.dds` a `.gitignore`.
2. Descreva-o e adicione seus registros na fase de dados do seu mod (a entrada implementa `IModDataPhase`):

```csharp
static readonly Subrace GhostCrab = new("ghostcrab", "30-MyMod.mod",
    Male: new RaceBody("MyMod", "mymod_ghostcrab", NormalMap: true), Female: null,
    Characters: [new RaceCharacter("ghostcrab", "31-MyMod.mod", Female: false)]);

public void EditData(ModDataContext c)
{
    RaceRecords.Add(c, GhostCrab);                                       // RACE + CHARACTERs, the recruit talk by default
    RaceRecords.JoinGroup(c, Races.CrabfolkGroup, GhostCrab.RaceId);    // the editor lists it under Crabfolk
}
```

- Uma sub-raça com corpo `Male` e `Female` recebe a seta de gênero do editor; com um é do mesmo gênero e todos os personagens o usam.
- `RaceRecords.Add(c, subrace, talk)` : `talk` é um ID DIALOGUE para os personagens, `null` para nenhum.
- Uma corrida própria: `RaceRecords.AddGroup(c, new RaceGroup("Naga", "32-MyMod.mod", [subraces]))` adiciona o RACE_GROUP e todas as sub-raças.
- As sub-raças são `playable` com limites do editor Kenshi Races (`RaceRecords.EditorLimits`); `FillRace` / `FillCharacter` preenche os registros que você mesmo adiciona (`c.Add`) quando deseja alterar mais campos posteriormente.
- Os ids são seus: registros do seu próprio nome de arquivo `.mod`.

<a id="6-play-its-animations"></a>
## 6. Reproduza suas animações

Cada clipe de [animations.md](animations.md) está vinculado a todas as raças dos esqueletos humanos do jogo (uma sub-raça adicionada com `RaceRecords` também), então um mod apenas o reproduz. Um movimento de combate mod é apenas uma animação, solicitado sempre que o jogo não executa nada próprio (engine docs/modding.md, "Movimentos de combate"):

```csharp
CombatTechnique? _wave;

public void Initialize(ModContext c)
{
    _wave = AnimationRecords.Technique(c.Data, RaceAnimations.WaveForHelp3);
    c.AddCombatMove((world, id, tick) => Waving(world, id, tick) is { } progress && _wave is { } wave ? new CombatMove(wave, progress) : null);
}
```

- `Waving` é seu: uma resposta barata, segura para threads e somente leitura do seu próprio estado, o progresso do clipe de 0 a 1 (segundos / `clip.Length`).
- Os clipes são reproduzidos no lugar e voltados para frente; um clipe mais curto que o seu movimento mantém seu último quadro, a menos que você finalize o progresso.
- As animações precisam apenas da dependência (seções 1 e 2), sem registros próprios.

<a id="5-check-it"></a>
## 5. Verifique

`dotnet build`, seus testes, então no jogo F10 **Kenshi Races : races**lista sua raça com sua malha e `spawn <name>` a gera. Nomeie sub-raças e PERSONAGENS com uma palavra minúscula, por ex. `ghostcrab`, então o console os aceita sem aspas; o editor de um novo jogo lista sua sub-raça.

[English](../../../docs/depending-on-kenshi-races.md) · [Kenshi Races](../README.md)
