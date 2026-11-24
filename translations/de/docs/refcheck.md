<a id="engine-references-refcheck"></a>
# Kompatibilität mit neuen Meitou-Versionen prüfen

Ein Mod wird für eine Engine (das `engine` -Submodul dieses Repos) erstellt und auf jeder Meitou -Version gespielt, die der Spieler hat. Wenn eine Veröffentlichung die Signatur von etwas ändert, das der Mod aufruft, wird der Quellcode des Mods möglicherweise noch kompiliert, aber der bereits erstellte DLL fragt das Spiel nach dem alten Mitglied und schlägt zur Laufzeit mit `MissingMethodException` fehl.

Dies ist nicht hypothetisch. Meitou 0.21 gab `Inventory.Add(ItemInstance)` einen zweiten, optionalen Parameter (`Add(ItemInstance, bool mainShut = false)`). Eine Quelle, die `bag.Add(item)` aufruft, wird mit beiden Versionen kompiliert, aber ein DLL, der mit 0.20 oder älter erstellt wurde, ruft die Ein-Parameter-Methode auf, die 0.21 nicht mehr hat. In „The Wasteland Remembers“ wurden Andenken im Spiel kaputt gemacht und die Ausnahme stoppte ein ganzes System für den Rest der Sitzung. Kein Test hat es erfasst: Tests werden für eine Engine kompiliert.

<a id="what-it-checks"></a>
## Was es überprüft

`tools/RefCheck` liest die Metadaten eines erstellten Mods von DLL und listet alle Engine-Mitglieder auf, auf die es verweist (Methoden, Konstruktoren, Felder für Typen in `Meitou*.dll`), nach Namen und Parametertypen, die eine bestimmte Version nicht hat. Nichts von der Mod oder dem Spiel läuft.

`tools/refcheck.sh` erstellt jeden Mod im Repo (jeder Ordner der obersten Ebene mit `meitou.json` und `<Folder>/<Folder>.csproj`), lädt die Win-x64-ZIP-Datei einer Version einmal herunter (behält nur `Meitou*.dll` und `0Harmony.dll` bei) und überprüft jede Assembly auf die `meitou.json` -Listen des Mods.

```
tools/refcheck.sh                   # the newest full Meitou release
tools/refcheck.sh v0.21.0 v0.20.1   # given releases, e.g. the oldest your meitou.json allows
CACHE=<dir> tools/refcheck.sh       # where downloaded release DLLs are kept
```

Ausgabe, eine Zeile pro Release und Assembly:

```
v0.21.0 ExampleMod/ExampleMod.dll: 1 engine references checked, 0 missing
```

oder, für eine Pause:

```
MISSING Meitou.Simulation.Items.Inventory::Add(Meitou.Simulation.Items.ItemInstance)
```

<a id="in-ci"></a>
## In CI

Der `Engine references` -Job führt `tools/refcheck.sh` bei jedem PR und Push mit der neuesten Version aus, und `Release readiness` wartet darauf. Ein Mod, der einen DLL liefert, der für eine ältere Engine gebaut wurde, sollte auch prüfen, ob der DLL mit jeder Version verglichen wird, die sein `meitou.json` zulässt.

<a id="when-it-finds-something"></a>
## Wenn es etwas findet

Rufen Sie das Mitglied auf eine Weise auf, die bei jeder von Ihnen unterstützten Version funktioniert: Erstellen Sie mit der neueren Engine neu, wenn Sie die älteren löschen, oder suchen Sie das Mitglied durch Reflektion und füllen Sie die Parameter, die Sie nicht übergeben, mit ihren Standardwerten aus. Letzteres erledigt `InventoryCompat` von The Wasteland Remembers für `Inventory.Add`.

Reflection-Aufrufe in die Engine werden nicht geprüft: Sie scheitern sanft an der Stelle, an die Sie sie geschrieben haben.

[English](../../../docs/refcheck.md) · [Kenshi Races](../README.md)
