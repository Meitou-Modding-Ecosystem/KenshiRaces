# Proteção de branches e configuração de repositórios

Todos os repositórios da organização seguem a política permanente central. [GutenStructure](https://github.com/TheIvoryDisaster/GutenStructure/blob/main/docs/branch-policy.md)

| Branch | Requisitos |
| --- | --- |
| Branches de trabalho | Sem proteção. |
| `beta` | PR e CI aprovada. Supermantenedores podem dispensar a revisão, inclusive para mesclar as próprias PRs. |
| `main` / `release` | PR, CI aprovada e uma aprovação independente de um supermantenedor. A revisão é obrigatória, sem exceção. |

O GitHub Actions não pode dispensar a revisão. Em beta, a exceção da equipe se limita à revisão. A PR e as verificações obrigatórias continuam sendo exigidas. O CODEOWNERS não acrescenta outra aprovação.

Os modelos do GitHub copiam o código-fonte. Regras, permissões da equipe, ambientes e segredos são configurados separadamente. No minuto 17 de cada hora, o fluxo central detecta novos repositórios públicos, concede acesso de escrita à equipe quando necessário e aplica a política. Para configurar imediatamente, um mantenedor autorizado do GutenStructure (`Configure release branch protections`) deve executar a prévia e depois aplicar o fluxo em main, tendo a organização como alvo. Faça isso antes de publicar ou depender das proteções.

Mantenha Release readiness na CI do repositório e confira se as execuções passaram. Uma verificação esperada sem um fluxo correspondente ficará pendente. Crie main e release a partir de código estável e revisado. As regras já visam essas branches antes da criação.

Mantenha as referências imutáveis e revisadas dos fluxos centrais. Atualize juntos, por PR, o controle de liberação e o publicador de pacotes. Repositórios privados exigem validação separada do plano e das proteções. A descoberta não concede acesso ao Azure. Os segredos das plataformas permanecem nos ambientes dos autores.

[English](../../../docs/branch-policy.md) · [Kenshi Races](../README.md)
