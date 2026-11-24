<a id="depending-on-kenshi-races"></a>
# В зависимости от Kenshi Races

Как мод кода использует расы и анимацию Kenshi Races , добавляет к ним подрасы или добавляет свои собственные расы ( `kenshi.races` ). Правила загрузчика: engine [modding-dependents.md](https://github.com/Meitou-Modding-Ecosystem/MeitouClient/blob/master/docs/modding-dependencies.md) . Навык агента проходит те же этапы: [.claude/skills/kenshi-races-dependent](../../../.claude/skills/kenshi-races-dependent/SKILL.md) .

<a id="1-reference-it-at-build-time-never-ship-it"></a>
## 1. Ссылайтесь на него во время сборки, никогда не отправляйте его.

В репозиторий, созданный на основе [MeitouModTemplate](https://github.com/Meitou-Modding-Ecosystem/MeitouModTemplate) , добавьте zip-архив выпуска Kenshi Races в `meitou.lock.json` ; сборка загружает, проверяет и ссылается на нее, а Play развертывает ее. Каждое значение взято из примечаний к выпуску ( `kenshi-races-v<version>` ):

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

- Ваш вывод `engine/` перемещается в этот коммит движка ( GutenStructure [блокировка фреймворка](https://github.com/TheIvoryDisaster/GutenStructure/blob/beta/docs/framework-lock.md) ).
- Никогда не указывайте `KenshiRaces.dll` в своем `assemblies` ; загруженный `kenshi.races` обслуживает типы.

<a id="2-declare-it-in-meitoujson"></a>
## 2. Объявите это в `meitou.json` .

```json
"apiVersion": 2,
"gameVersion": ">=0.22.2",
"dependencies": [{ "id": "kenshi.races", "version": ">=0.4.0 <0.5.0" }]
```

- До версии 1.0 минорная версия может сломать API , поэтому ограничьте диапазон следующей минорной версией.
- Kenshi Races должен находиться над вашим модом в `data/mods.cfg` (это делает Arrange программы запуска), поэтому его записи существуют, когда выполняется ваша фаза данных.

<a id="3-use-its-races"></a>
## 3. Используйте свои расы

Обратитесь к идентификаторам в `Races` из ваших записей, например. на этапе данных:

```csharp
public void EditData(ModDataContext c)
{
    var guard = c.Add("20-MyMod.mod", FcsRecordType.Character, "leomaguard");
    // ... fields ...
    guard.References["race"] = [new FcsReference(Races.LeomaRace, 100, 0, 0)];
    guard.Ints["female chance"] = 50;                                   // leoma and crevan have both genders
}
```

- РАСЫ (подрасы): `Races.LeomaRace` , `CrevanRace` , `CrabmanRace` , `CrabQueenRace` .
- ПЕРСОНАЖИ (без комплекта, набираемые): `LeomaMaleCharacter` , `LeomaFemaleCharacter` , `CrevanMaleCharacter` , `CrevanFemaleCharacter` , `CrabmanCharacter` , `CrabQueenCharacter` .
- RACE_GROUPs (расы редактора): `LeomaGroup` , `CrevanGroup` , `CrabfolkGroup` . `Races.Recruit` — «Присоединяйтесь к нам». разговаривать.
- Их описывают `Races.Leoma` , `Crevan` , `Crabman` , `CrabQueen` ( `Subrace` ) и `Races.Groups` . Таблица: [kenshi-races.md "Гонки"](kenshi-races.md#races) .

<a id="4-add-a-subrace-or-a-race"></a>
## 4. Добавьте подрасу или расу.

1. Вставьте каждое тело с помощью [race import](race-import.md) из клона KenshiRaces] в `items/races` вашего мода с собственным выносом ( `mymod_<character>` ; `female` в качестве установки для женского тела) и зафиксируйте файлы. Добавьте `!/<Mod>/items/races/*.mesh` и `!/<Mod>/items/races/*.dds` к `.gitignore` .
2. Опишите его и добавьте его записи в фазу данных вашего мода (запись реализует `IModDataPhase` ):

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

- Подраса с телом `Male` и `Female` получает стрелку пола редактора; один из них однополый, и его носит каждый персонаж.
- `RaceRecords.Add(c, subrace, talk)` : `talk` — это идентификатор DIALOGUE для символов, `null` — нет.
- Ваша собственная раса: `RaceRecords.AddGroup(c, new RaceGroup("Naga", "32-MyMod.mod", [subraces]))` добавляет RACE_GROUP и все подрасы.
- Подрасы — это `playable` с ограничениями редактора Kenshi Races ( `RaceRecords.EditorLimits` ); `FillRace` / `FillCharacter` заполняет записи, которые вы добавляете самостоятельно ( `c.Add` ), когда впоследствии вы захотите изменить дополнительные поля.
- Идентификаторы принадлежат вам: записи вашего собственного имени файла `.mod` .

<a id="6-play-its-animations"></a>
## 6. Воспроизведите анимацию

Каждый клип [animations.md](animations.md) связан с каждой расой человеческих скелетов в игре (подраса также добавлена ​​в `RaceRecords` ), поэтому мод воспроизводит только его. Боевой ход мода — это только анимация, он запрашивается всякий раз, когда игра не воспроизводит ничего собственного (движок docs/modding.md, «Боевые ходы»):

```csharp
CombatTechnique? _wave;

public void Initialize(ModContext c)
{
    _wave = AnimationRecords.Technique(c.Data, RaceAnimations.WaveForHelp3);
    c.AddCombatMove((world, id, tick) => Waving(world, id, tick) is { } progress && _wave is { } wave ? new CombatMove(wave, progress) : null);
}
```

- `Waving` ваш: дешевый, потокобезопасный, доступный только для чтения ответ из вашего собственного состояния, прогресс клипа от 0 до 1 (секунды/ `clip.Length` ).
- Клипы воспроизводятся на месте и лицом вперед; клип короче вашего хода сохраняет свой последний кадр, пока вы не завершите прогресс.
- Анимациям нужны только зависимости (разделы 1 и 2), никаких собственных записей.

<a id="5-check-it"></a>
## 5. Проверьте это

`dotnet build` , ваши тесты, затем в игре F10 ** Kenshi Races : Races** перечисляет вашу расу с ее сеткой, а `spawn <name>` порождает ее. Назовите подрасы и СИМВОЛОВ одним словом в нижнем регистре, например. `ghostcrab` , поэтому консоль принимает их без кавычек; в редакторе новой игры указана ваша подраса.
