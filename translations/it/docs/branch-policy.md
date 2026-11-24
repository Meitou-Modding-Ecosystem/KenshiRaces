# Protezione dei branch e configurazione dei repository

Tutti i repository dell’organizzazione seguono la politica permanente comune. [GutenStructure](https://github.com/TheIvoryDisaster/GutenStructure/blob/main/docs/branch-policy.md)

| Branch | Requisiti |
| --- | --- |
| Branch di lavoro | Senza protezione. |
| `beta` | PR e CI superata. I super-manutentori possono saltare la revisione, anche per unire le proprie PR. |
| `main` / `release` | PR, CI superata e approvazione indipendente di un super-manutentore. La revisione è sempre obbligatoria. |

GitHub Actions non dispone di deroghe. Su beta la deroga riguarda soltanto la revisione. La PR e i controlli obbligatori rimangono necessari. CODEOWNERS non richiede un’approvazione aggiuntiva.

I modelli GitHub copiano il codice sorgente. Regole, permessi dei team, ambienti e segreti si configurano separatamente. Al minuto 17 di ogni ora il workflow centrale rileva i nuovi repository pubblici, concede al team l’accesso in scrittura se necessario e applica la politica. Per procedere subito, un manutentore autorizzato di GutenStructure (`Configure release branch protections`) deve eseguire da main l’anteprima e poi l’applicazione del workflow, indicando l’organizzazione. Farlo prima di pubblicare o fare affidamento sulle protezioni.

Mantenere Release readiness nella CI chiamante e verificare che le esecuzioni riescano. Un controllo atteso senza il relativo workflow resta in sospeso. Creare main e release da sorgenti stabili e revisionati. Le regole si applicano anche prima della creazione.

Conservare i riferimenti immutabili e revisionati ai workflow centrali. Aggiornare insieme tramite PR il controllo di rilascio e la pubblicazione dei pacchetti. Per i repository privati verificare separatamente piano e protezioni. Il rilevamento non concede accesso ad Azure. I segreti delle piattaforme rimangono negli ambienti dei rispettivi autori.

[English](../../../docs/branch-policy.md) · [Kenshi Races](../README.md)
