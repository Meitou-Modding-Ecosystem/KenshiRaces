<a id="permanent-branch-policy-and-repository-onboarding"></a>
# Politica della filiale permanente e onboarding del repository

Tutti i repository Meitou -Modding-Ecosystem utilizzano la [politica permanente centrale](https://github.com/TheIvoryDisaster/GutenStructure/blob/main/docs/branch-policy.md) :

| Ramo | Requisito |
| --- | --- |
| Filiali ordinarie | Non protetto |
| beta | PR + passante CI ; i super-manutentori possono aggirare la revisione, comprese le auto-fusioni |
| principale/rilascio | PR + superamento CI + approvazione di un supermanutentore indipendente; nessun bypass di revisione |

GitHub Actions non ha bypass di revisione. L'eccezione del team Beta si applica solo alle revisioni: la sicurezza obbligatoria PR / CI non ha bypass. CODEOWNERS non aggiunge ulteriori requisiti di approvazione.

I modelli GitHub copiano l'origine, non le regole del ramo, le autorizzazioni del team, gli ambienti o i segreti.  La funzione Configura protezioni dei rami di rilascio di GutenStructure rileva nuovi repository dell'organizzazione pubblica ogni ora al minuto 17, assegna l'accesso in scrittura al repository del team se mancante e applica la policy. Per configurare immediatamente un nuovo repository, chiedi a un manutentore GutenStructure autorizzato di visualizzare in anteprima e quindi applicare il flusso di lavoro all'organizzazione principale di destinazione. Fatelo prima di affidarvi a unioni o pubblicazioni protette. Mantenere Release readiness nel chiamante CI e verificare gli effettivi passaggi di passaggio; i controlli previsti senza flusso di lavoro corrispondente attenderanno per sempre.

Crea principale/release dalla fonte stabile rivista quando è pronto; la politica li prende di mira ancor prima che esistano. Mantieni i pin del flusso di lavoro centrale revisionati e immutabili di questo modello, aggiornando insieme sia il gate di rilascio che l'editore del pacchetto tramite PR. I pronti contro termine privati ​​richiedono una convalida separata del piano/protezione; nessun accesso ad Azure viene concesso tramite l'individuazione. I segreti della piattaforma esterna rimangono negli ambienti di proprietà dell'autore e non vengono mai copiati da questo modello.
