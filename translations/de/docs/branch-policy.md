# Branchschutz und Einrichtung neuer Repositories

Für alle Repositories der Organisation gilt die gemeinsame dauerhafte Richtlinie. [GutenStructure](https://github.com/TheIvoryDisaster/GutenStructure/blob/main/docs/branch-policy.md)

| Branch | Voraussetzungen |
| --- | --- |
| Arbeitsbranches | Ungeschützt. |
| `beta` | PR und erfolgreiche CI. Super-Maintainer dürfen die Reviewpflicht umgehen, auch beim Zusammenführen eigener PRs. |
| `main` / `release` | PR, erfolgreiche CI und eine unabhängige Freigabe durch einen Super-Maintainer. Die Reviewpflicht gilt ausnahmslos. |

GitHub Actions hat keine Review-Ausnahme. Die Ausnahme auf beta gilt ausschließlich für Reviews. PR und Pflichtprüfungen bleiben verbindlich. CODEOWNERS verlangt keine zusätzliche Freigabe.

GitHub-Vorlagen kopieren den Quelltext. Regelsätze, Teamrechte, Umgebungen und Geheimnisse werden separat eingerichtet. Stündlich zur Minute 17 erkennt der zentrale Workflow neue öffentliche Repositories, gewährt dem Team bei Bedarf Schreibzugriff und wendet die Richtlinie an. Für die sofortige Einrichtung lässt ein berechtigter GutenStructure (`Configure release branch protections`)-Maintainer den Workflow auf main für die Organisation erst als Vorschau und dann zur Anwendung laufen. Vor Veröffentlichung und Nutzung geschützter Merges abschließen.

Release readiness muss in der aufrufenden CI bleiben. Erfolgreiche Läufe prüfen. Fehlt einem erwarteten Check der zugehörige Workflow, wartet er dauerhaft. main und release bei Bedarf aus geprüftem stabilem Quellstand erstellen. Die Regeln gelten schon vor der Erstellung.

Die geprüften unveränderlichen Workflow-Referenzen der Vorlage beibehalten. Freigabekontrolle und Paketveröffentlichung gemeinsam per PR aktualisieren. Tarif und Schutzregeln privater Repositories separat prüfen. Die Erkennung gewährt keinen Azure-Zugriff. Plattformgeheimnisse bleiben in den Umgebungen der Autoren.

[English](../../../docs/branch-policy.md) · [Kenshi Races](../README.md)
