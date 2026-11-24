<a id="permanent-branch-policy-and-repository-onboarding"></a>
# Política de ramo permanente e repositório em bordo

Todos os repositórios Meitou-Modding-Ecosystem utilizam o [A política central permanente](https://github.com/TheIvoryDisaster/GutenStructure/blob/main/docs/branch-policy.md):

| Branco | Requisitos |
| --- | --- |
| Faixas comuns | Não protegido |
| A Beta | PR + passando CI; super-mantenheiros podem ultrapassar a revisão, incluindo auto-merges |
| Principal / Liberação | PR + passando a CIA+ uma aprovação de super-mantenheiros independente; não há bypass de revisão |

O GitHub Actions não tem um bypass de revisão. A exceção da equipe da Beta aplica-se apenas às avaliações: obrigatório PR/CI A segurança não tem bypass. CODEOWNERS Não adiciona um requisito de aprovação.

Os modelos do GitHub copiam a fonte, não as regras de ramo, permissões de equipe, ambientes ou segredos. As proteções da Configuração da GutenStructure descrevem novos registros de órgãos por hora, atribuindo aos registros de equipe acesso, se não for, e aplicando a política. Para configurar um novo repo imediatamente, peça um gestor autorizado para a previsão e, em seguida, aplicar o fluxo de trabalho à organização principal. Faça isso antes de confiar em fusões protegidas ou publicar. Mantenha a preparação para o lançamento no CI e verifica as corridas reais; verificações esperadas com nenhum fluxo de trabalho serão esperadas para sempre.

Criando main/release da fonte estável revisada quando preparada; a política os visa até antes de existir. Mantenha os pins de fluxo de trabalho inalterados deste modelo, atuando tanto o portal de lançamento como o editor de pacotes através de PRs. O repouso precisa de separado plan/protection Validação; não é concedido acesso ao Azure por descoberta. Os segredos da plataforma permanecem em ambientes de propriedade e nunca são copiados deste modelo.
