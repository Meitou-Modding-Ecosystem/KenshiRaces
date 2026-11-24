<a id="permanent-branch-policy-and-repository-onboarding"></a>
# Política de filial permanente e integração de repositório

Todos os repositórios Meitou -Modding-Ecosystem usam a [política central permanente](https://github.com/TheIvoryDisaster/GutenStructure/blob/main/docs/branch-policy.md) :

| Filial | Exigência |
| --- | --- |
| Filiais comuns | Desprotegido |
| beta | PR + passando CI ; super-mantenedores podem ignorar a revisão, incluindo auto-mescagens |
| principal / lançamento | PR + aprovação em CI + aprovação de um supermantenedor independente; sem desvio de revisão |

GitHub Actions não tem desvio de revisão. A exceção da equipe Beta se aplica apenas a revisões: a segurança obrigatória PR / CI não tem bypass. CODEOWNERS não adiciona um requisito extra de aprovação.

Os modelos do GitHub copiam a origem, não conjuntos de regras de ramificação, permissões de equipe, ambientes ou segredos.  Configurar proteções de ramificação de lançamento do GutenStructure descobre novos repositórios públicos da organização a cada hora no minuto 17, atribui ao repositório da equipe acesso de gravação, se estiver faltando, e aplica a política. Para configurar um novo repositório imediatamente, peça a um mantenedor autorizado do GutenStructure para visualizar e aplicar esse fluxo de trabalho na organização principal de destino. Faça isso antes de confiar em mesclagens ou publicações protegidas. Mantenha Release readiness no chamador CI e verifique as execuções de passagem reais; verificações esperadas sem fluxo de trabalho correspondente aguardarão para sempre.

Crie main/release a partir da fonte estável revisada quando estiver pronto; a política os visa antes mesmo de existirem. Mantenha os pins de fluxo de trabalho central revisados ​​e imutáveis ​​deste modelo, atualizando o portão de lançamento e o editor do pacote juntos por meio de PRs. Os repositórios privados exigem validação de plano/proteção separada; nenhum acesso ao Azure é concedido por descoberta. Os segredos da plataforma externa permanecem em ambientes de propriedade do autor e nunca são copiados deste modelo.
