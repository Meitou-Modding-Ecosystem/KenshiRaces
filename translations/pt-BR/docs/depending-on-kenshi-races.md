<a id="depending-on-kenshi-races"></a>
# Dependendo da Kenshi raças

Como um código mod usa Kenshi As raças e animações de Races, adiciona subraças a elas ou adiciona suas próprias raças (`kenshi.races`). Regras de carregamento: motor [modding-dependencies.md](https://github.com/Meitou-Modding-Ecosystem/MeitouClient/blob/master/docs/modding-dependencies.md). Uma habilidade de agente passa pelos mesmos passos: [.claude/skills/kenshi-races-dependent](../../../.claude/skills/kenshi-races-dependent/SKILL.md).

<a id="1-reference-it-at-build-time-never-ship-it"></a>
## 1. Referir-se ao tempo de construção, nunca enviá-lo

Em um repo feito de [MeitouModTemplado](https://github.com/Meitou-Modding-Ecosystem/MeitouModTemplate)e adicionar Kenshi Races' release Zip para `meitou.lock.json`O edifício descarga, verifica e refere-se a ele, e o jogo o desloca. Todos os valores são derivados das notas da publicação (`kenshi-races-v<version>`):

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

- O seu `engine/` Pin mover para aquele motor composta (GutenStructure
[Figura do framework lock](https://github.com/TheIvoryDisaster/GutenStructure/blob/beta/docs/framework-lock.md)).
- Nunca lista `KenshiRaces.dll` Em seu próprio `assemblies`O carregado `kenshi.races` servem os tipos.

<a id="2-declare-it-in-meitoujson"></a>
## 2. Declaração em `meitou.json`

```json
"apiVersion": 2,
"gameVersion": ">=0.22.2",
"dependencies": [{ "id": "kenshi.races", "version": ">=0.4.0 <0.5.0" }]
```

- Antes de 1.0 uma versão menor pode quebrar o APIEm seguida, coloque a faixa no próximo menor.
- Kenshi As raças devem estar acima do seu modo de `data/mods.cfg` (O arranjo do lançador faz isso), então seus registros existem quando a sua fase de dados está em andamento.

<a id="3-use-its-races"></a>
## 3. Use suas corridas

Conheça os IDs em `Races` dos seus registros, por exemplo, na sua fase de dados:

```csharp
public void EditData(ModDataContext c)
{
    var guard = c.Add("20-MyMod.mod", FcsRecordType.Character, "leomaguard");
    // ... fields ...
    guard.References["race"] = [new FcsReference(Races.LeomaRace, 100, 0, 0)];
    guard.Ints["female chance"] = 50;                                   // leoma and crevan have both genders
}
```

- RACEs ( em submissão ) `Races.LeomaRace`, `CrevanRace`, `CrabmanRace`, `CrabQueenRace`.
- CHARACTERs (Não é possível recrutá-lo) `LeomaMaleCharacter`, `LeomaFemaleCharacter`, `CrevanMaleCharacter`, `CrevanFemaleCharacter`,
`CrabmanCharacter`, `CrabQueenCharacter`.
- RACE_GROUPs (a corrida do editor) `LeomaGroup`, `CrevanGroup`, `CrabfolkGroup`. `Races.Recruit` É a palavra “junto a nós”.
- `Races.Leoma`, `Crevan`, `Crabman`, `CrabQueen` (`Subrace`e) e `Races.Groups` Descreva os A mesa é:
[Kenshi-races.md “Razes”](kenshi-races.md#races).

<a id="4-add-a-subrace-or-a-race"></a>
## 4. Adicione uma subraça ou uma corrida

1. Cada um dos seus corpos tem a [A corrida importação](race-import.md) de um clone KenshiRaces, para o seu mod `items/races` Com sua própria voz
(`mymod_<character>`; `female` como a rig para um corpo feminino), e comete os arquivos. Adicionar `!/<Mod>/items/races/*.mesh` e `!/<Mod>/items/races/*.dds` Para `.gitignore`.
2. Descreva-o e adicione seus registros na fase de dados do seu mod (as implementações de entrada `IModDataPhase`):

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

- Uma subraça com a `Male` e a `Female` O corpo recebe a armadura de gênero do editor; com um é gênero único e cada personagem a usa.
- `RaceRecords.Add(c, subrace, talk)`: `talk` É a DIALOGUE ID para os personagens, `null` Para ninguém.
- Uma corrida do seu: `RaceRecords.AddGroup(c, new RaceGroup("Naga", "32-MyMod.mod", [subraces]))` Adicione o RACE_GROUP e todas as subrasas.
- Substâncias são `playable` com Kenshi Apresentação de limites de editora (`RaceRecords.EditorLimits`); `FillRace` / `FillCharacter` Preencha os registros que você adiciona
Você mesmo (`c.Add`Quando você quiser mudar mais campos depois.
- Os IDs são seus: registros de seus próprios `.mod` Nome do arquivo.

<a id="6-play-its-animations"></a>
## 6. Jogue suas animações

Cada clipe de [animations.md](animations.md) está ligado a cada corrida sobre os esqueletos humanos do jogo (uma subraça adicionada com `RaceRecords` Também), então um mod só o joga. Um movimento de combate de modo simples é apenas animação, perguntada sempre que o jogo não joga nada de si próprio (motor) docs/modding.md“O combate se move” :

```csharp
CombatTechnique? _wave;

public void Initialize(ModContext c)
{
    _wave = AnimationRecords.Technique(c.Data, RaceAnimations.WaveForHelp3);
    c.AddCombatMove((world, id, tick) => Waving(world, id, tick) is { } progress && _wave is { } wave ? new CombatMove(wave, progress) : null);
}
```

- `Waving` É seu: um barato, thread-safe, leitura-somente resposta de seu próprio estado, o progresso do clipe de 0 a 1 (secundos / `clip.Length`).
- Os clipes estão no local e estão em frente; um clipe mais curto do que o seu movimento mantém o seu último quadro, a menos que você abraça o progresso.
- As animações precisam apenas da dependência (segundos 1 e 2), sem registros próprios.

<a id="5-check-it"></a>
## 5. Verifique isso

`dotnet build`O jogo, em seguida, F10 **Kenshi Race: A Race** A sua corrida é com a sua messa, e `spawn <name>` espalhou isso. Nome de subrases e CHARACTERs Uma palavra inferior, por exemplo. `ghostcrab`Então a consola os leva sem citações; o editor de um novo jogo lista sua subraça.
