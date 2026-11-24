<a id="engine-references-refcheck"></a>
# Motorenreferenzen (RefCheck)

Ein Mod wird gegen einen Motor gebaut (dieses Repo `engine` Untermodule) und spielt auf dem, was Meitou veröffentlicht hat. Wenn eine Release die Unterschrift eines Mods ändert, kann die Quelle des Mods noch zusammengefasst werden. DLL die bereits gebaut ist, und das Spiel für den alten Mitglied verlangt und fehlt an der Zeit mit `MissingMethodException`.

Das ist nicht hypothetisch. Meitou 0.21 Geschenke `Inventory.Add(ItemInstance)` Die zweite Option ist (`Add(ItemInstance, bool mainShut = false)`). Quelle, die aufruft `bag.Add(item)` Die Versionen sind gegen die Versionen, aber eine DLL Das gegen 0,20 oder älter gebildet wird die einparameter-methode, die 0,21 nicht mehr hat. In The Wasteland erinnern Sie sich, dass die Mannschaft im Spiel ist, und die Ausnahme hat ein ganzes System für den Rest der Sitzung gestoppt. Kein Test hat es aufgenommen: Die Tests werden gegen einen Motor zusammengefasst.

<a id="what-it-checks"></a>
## Was es überprüft

`tools/RefCheck` Lernen Sie ein eingebautes Mod DLLDie Metadaten und die Liste jedes Motormitglieds, die sie beziehen (Methoden, Konstrukteure, Felder auf Typen in `Meitou*.dll`, nach Namen und Parameterarten, dass eine bestimmte Release nicht hat. Nichts von der Mod oder des Spiels läuft.

`tools/refcheck.sh` erstellt jedes Mod in der Repo (ein Top-Level-Folder mit `meitou.json` und `<Folder>/<Folder>.csproj`, herunterladen Sie eine release win-x64 zip einmal (behalten Sie nur `Meitou*.dll` und `0Harmony.dll`und überprüft jede Versammlung die Mod's `meitou.json` Die Listen .

```
tools/refcheck.sh                   # the newest full Meitou release
tools/refcheck.sh v0.21.0 v0.20.1   # given releases, e.g. the oldest your meitou.json allows
CACHE=<dir> tools/refcheck.sh       # where downloaded release DLLs are kept
```

Ausgang, eine Linie pro Release und Assembly:

```
v0.21.0 ExampleMod/ExampleMod.dll: 1 engine references checked, 0 missing
```

Oder für eine Pause:

```
MISSING Meitou.Simulation.Items.Inventory::Add(Meitou.Simulation.Items.ItemInstance)
```

<a id="in-ci"></a>
## In den

Die `Engine references` Arbeit Runs `tools/refcheck.sh` gegen die neueste Release auf jeder PR und Druck, und `Release readiness` Warten Sie darauf. Ein Schiff, der einen DLL Ein älter Motor sollte auch überprüfen. DLL Für jede Freilassung `meitou.json` Erlaubt ist.

<a id="when-it-finds-something"></a>
## Wenn er etwas findet

Rufen Sie den Mitglied auf eine Art und Weise, die auf jeden Release unterstützt wird: Wiederherstellen Sie den neuen, wenn Sie die älteren, oder finden Sie den Mitglied durch Reflexion und füllen Sie die Parameter, die Sie nicht mit ihren Anforderungen. Die Wüste erinnert sich » `InventoryCompat` Der letzte für `Inventory.Add`.

Reflexionsanrufe in den Motor werden nicht überprüft: Sie fehlen weich, wo Sie sie geschrieben haben.
