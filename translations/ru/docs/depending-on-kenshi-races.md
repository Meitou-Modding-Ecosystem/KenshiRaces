<a id="depending-on-kenshi-races"></a>
# В зависимости от Kenshi расы

Как используется код Kenshi Расы и анимации, добавляет к ним субрасы или добавляет свои собственные расы (`kenshi.races`). Правила зарядки: двигатель [modding-dependencies.md](https://github.com/Meitou-Modding-Ecosystem/MeitouClient/blob/master/docs/modding-dependencies.md). Агентская умение проходит через те же шаги: [.claude/skills/kenshi-races-dependent](../../../.claude/skills/kenshi-races-dependent/SKILL.md).

<a id="1-reference-it-at-build-time-never-ship-it"></a>
## 1. Реферат на строительство времени, никогда не бросать

В репетиции, сделанной из [Модель Модель](https://github.com/Meitou-Modding-Ecosystem/MeitouModTemplate)Добавить Kenshi Распущенный ZIP для `meitou.lock.json`• постройка загружает, проверяет и ссылается на нее, а Play размещает ее. Вся стоимость поступает из записей издания (`kenshi-races-v<version>`):

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

- Ваш `engine/` PIN перемещается к этому двигателю (GutenStructure
[Фреймворк](https://github.com/TheIvoryDisaster/GutenStructure/blob/beta/docs/framework-lock.md)).
- Никогда не перечислюсь `KenshiRaces.dll` В своем собственном `assemblies`• Загруженный `kenshi.races` Служит типу.

<a id="2-declare-it-in-meitoujson"></a>
## 2. Декларируйте в `meitou.json`

```json
"apiVersion": 2,
"gameVersion": ">=0.22.2",
"dependencies": [{ "id": "kenshi.races", "version": ">=0.4.0 <0.5.0" }]
```

- До 1.0 небольшая версия может нарушать APIИтак, поймите диапазон на следующий миниатюр.
- Kenshi Расы должны сидеть выше вашего режима `data/mods.cfg` (Организация запускателя делает это), поэтому его записи существуют, когда идет фаза данных.

<a id="3-use-its-races"></a>
## 3. Используйте свои гонки

Показания к IDS в `Races` из ваших записей, например, в фазе ваших данных:

```csharp
public void EditData(ModDataContext c)
{
    var guard = c.Add("20-MyMod.mod", FcsRecordType.Character, "leomaguard");
    // ... fields ...
    guard.References["race"] = [new FcsReference(Races.LeomaRace, 100, 0, 0)];
    guard.Ints["female chance"] = 50;                                   // leoma and crevan have both genders
}
```

- RACEs (в том числе и субстанции ) `Races.LeomaRace`, `CrevanRace`, `CrabmanRace`, `CrabQueenRace`.
- CHARACTERs (Одна из них – рекрутинговая и рекрутинговая) `LeomaMaleCharacter`, `LeomaFemaleCharacter`, `CrevanMaleCharacter`, `CrevanFemaleCharacter`,
`CrabmanCharacter`, `CrabQueenCharacter`.
- RACE_GROUPs (Подробнее о издательской гонке): `LeomaGroup`, `CrevanGroup`, `CrabfolkGroup`. `Races.Recruit` Это слово «присоединяйтесь».
- `Races.Leoma`, `Crevan`, `Crabman`, `CrabQueen` (`Subrace`) и `Races.Groups` Опишите их Таблица →
[Kenshi-races.md «Разы»](kenshi-races.md#races).

<a id="4-add-a-subrace-or-a-race"></a>
## 4. Добавить подрасы или расы

1. Каждый из них подходит для [Ввоз расы](race-import.md) от клона KenshiRaces, в ваш режим `items/races` С собственным голосом
(`mymod_<character>`; `female` Как риг для женского тела), и совершать файлы. Добавить `!/<Mod>/items/races/*.mesh` и `!/<Mod>/items/races/*.dds` Для `.gitignore`.
2. Опишите его и добавьте его записи в фазу данных вашего режима (входные имплементации `IModDataPhase`):

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

- Субарация с а `Male` И А `Female` Тело получает половую стрелу редактора; с одним оно однополые, и каждый персонаж носит его.
- `RaceRecords.Add(c, subrace, talk)`: `talk` Это A DIALOGUE Идентификация персонажей, `null` Для никого.
- Выберите свою гонку: `RaceRecords.AddGroup(c, new RaceGroup("Naga", "32-MyMod.mod", [subraces]))` Добавить в RACE_GROUP И все подрасы.
- Субъекты `playable` С Kenshi Ограничения редактора (`RaceRecords.EditorLimits`); `FillRace` / `FillCharacter` Заполните записи, которые вы добавили
Собственность (`c.Add`Когда вы хотите изменить больше поля после.
- Идентификаторы являются вашими: запись собственной `.mod` Название файла .

<a id="6-play-its-animations"></a>
## 6. Играть в его анимации

Каждый клип [animations.md](animations.md) связывается с каждым гонком на человеческих скелетах игры (субраса добавляется с `RaceRecords` Так что, так что мод играет только на него. Мод боевого движения - это только анимация, просьба, когда игра не играет ничего собственного (мотор) docs/modding.md«Коббат движется»):

```csharp
CombatTechnique? _wave;

public void Initialize(ModContext c)
{
    _wave = AnimationRecords.Technique(c.Data, RaceAnimations.WaveForHelp3);
    c.AddCombatMove((world, id, tick) => Waving(world, id, tick) is { } progress && _wave is { } wave ? new CombatMove(wave, progress) : null);
}
```

- `Waving` Ваш: дешевый, безопасный, читающий только ответ из вашего состояния, прогресс клипа 0 до 1 (секунд / `clip.Length`).
- Клипы играют на месте и передвигаются вперед; клип, короткий по отношению к вашему движению, держит свою последнюю рамку, если вы не запутаете прогресс.
- Анимации требуют только зависимости (секции 1 и 2), никакой собственной записи.

<a id="5-check-it"></a>
## 5. Проверьте это

`dotnet build`Итак, в игре F10 **Kenshi Расы: расы** Своей расой он перечисляет, и `spawn <name>` И шпинат его. Название подрастая и CHARACTERs В одном из слов ниже, например. `ghostcrab`Так что консоль берет их без цитат; редактор новой игры перечисляет вашу субрасу.
