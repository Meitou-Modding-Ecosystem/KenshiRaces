<a id="permanent-branch-policy-and-repository-onboarding"></a>
# Permanente Branch-Richtlinie und Repository-Onboarding

Alle Meitou -Modding-Ecosystem-Repositories verwenden die [zentrale permanente Richtlinie](https://github.com/TheIvoryDisaster/GutenStructure/blob/main/docs/branch-policy.md) :

| Zweig | Erfordernis |
| --- | --- |
| Gewöhnliche Zweige | Ungeschützt |
| Beta | PR + vorbei an CI ; Super-Maintainer können die Überprüfung umgehen, einschließlich Selbstzusammenführungen |
| Haupt-/Veröffentlichung | PR + Bestehen von CI + eine unabhängige Super-Maintainer-Genehmigung; Kein Review-Bypass |

GitHub Actions verfügt über keine Überprüfungsumgehung. Die Beta-Team-Ausnahme gilt nur für Bewertungen: Die obligatorische PR -/ CI -Sicherheit hat keinen Bypass. CODEOWNERS fügt keine zusätzliche Genehmigungspflicht hinzu.

GitHub-Vorlagen kopieren die Quelle, nicht Zweigregelsätze, Teamberechtigungen, Umgebungen oder Geheimnisse. GutenStructure s „Release-Branch-Schutz konfigurieren“ erkennt stündlich in Minute 17 neue öffentliche Organisations-Repositorys, weist dem Team-Repository Schreibzugriff zu, falls dieser fehlt, und wendet die Richtlinie an. Um sofort ein neues Repo zu konfigurieren, bitten Sie einen autorisierten GutenStructure -Betreuer, eine Vorschau anzuzeigen und diesen Workflow dann auf die Haupt- und Zielorganisation anzuwenden. Tun Sie dies, bevor Sie sich auf geschützte Zusammenführungen oder Veröffentlichungen verlassen. Behalten Sie Release readiness im Anrufer CI bei und überprüfen Sie die tatsächlichen Passläufe. erwartete Prüfungen ohne passenden Workflow warten ewig.

Erstellen Sie die Haupt-/Releaseversion aus der überprüften stabilen Quelle, wenn Sie fertig sind. Die Politik nimmt sie ins Visier, noch bevor sie existieren. Behalten Sie die unveränderlichen, überprüften zentralen Workflow-Pins dieser Vorlage bei und aktualisieren Sie sowohl das Release-Gate als auch den Paketherausgeber gemeinsam durch PRs. Private Repos erfordern eine separate Plan-/Schutzvalidierung; Durch die Erkennung wird kein Azure-Zugriff gewährt. Externe Plattformgeheimnisse verbleiben in Umgebungen, die dem Autor gehören, und werden niemals aus dieser Vorlage kopiert.
