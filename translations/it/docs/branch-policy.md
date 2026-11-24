<a id="permanent-branch-policy-and-repository-onboarding"></a>
# Politica di filiale permanente e di deposito a bordo

Tutti i repositori di Meitou-Modding-Ecosystem utilizzano il [La politica centrale permanente](https://github.com/TheIvoryDisaster/GutenStructure/blob/main/docs/branch-policy.md):

| Il ramo | Requisiti |
| --- | --- |
| I rami ordinari | Non protetto |
| Il beta | PR + passare CI; i super-mantenitori possono passare la revisione, compresi gli automargli |
| Principale / Rilascio | PR + passare CIPi + approvazione di uno super-mantenitori indipendente; nessun bypass di revisione |

GitHub non ha un bypass di recensione. L'eccezione di squadra di Beta si applica solo alle recensioni: obbligatorio PR/CI La sicurezza non ha bypass. CODEOWNERS Non aggiunge alcun requisito di approvazione.

I modelli di GitHub copiano la fonte, non i set di regole, le autorizzazioni di squadra, gli ambienti o i segreti. Le protezioni del rilascio di GutenStructure scoprono nuovi depositi di organi all'ora, assegnando al deposito di squadra accesso se mancano, e applicando la politica. Per configurare immediatamente un nuovo repo, chiediamo a un gestore di GutenStructure di prevedere e applicare il flusso di lavoro sulla principale organizzazione mirata. Fai questo prima di affidarti alle fusioni protette o pubblicare. Mantenere la preparazione del rilascio nel caller CI e verificare i passaggi effettivi; i controlli attesi con il flusso di lavoro non corrispondono a sempre.

Creazione main/release da fonti stabili riviste quando sono pronti; la politica li mira anche prima che esistano. Mantenere i pini del flusso di lavoro immutati di questo modello, aggiornando sia la porta di rilascio che l'editore di pacchetti attraverso i pubblici. Il riposo privato richiede una separazione plan/protection validazione; nessun accesso Azure è concesso dalla scoperta. I segreti esterni rimangono in ambienti di proprietà dell'autore e non vengono mai copiati da questo modello.
