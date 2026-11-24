# Protection des branches et configuration des dépôts

Tous les dépôts de l’organisation suivent la politique permanente commune. [GutenStructure](https://github.com/TheIvoryDisaster/GutenStructure/blob/main/docs/branch-policy.md)

| Branche | Conditions |
| --- | --- |
| Branches de travail | Sans protection. |
| `beta` | PR et CI réussie. Les super-mainteneurs peuvent se dispenser de la revue, y compris pour fusionner leur propre PR. |
| `main` / `release` | PR, CI réussie et approbation indépendante d’un super-mainteneur. Aucune dérogation à la revue. |

GitHub Actions ne bénéficie pas de la dérogation. Sur beta, elle porte uniquement sur la revue. La PR et les contrôles obligatoires restent exigés. CODEOWNERS n’ajoute pas d’approbation supplémentaire.

Les modèles GitHub copient le code source. Les règles, droits d’équipe, environnements et secrets se configurent séparément. Chaque heure à la minute 17, le workflow central détecte les nouveaux dépôts publics, accorde si nécessaire l’accès en écriture à l’équipe et applique la politique. Pour une mise en service immédiate, un mainteneur autorisé de GutenStructure (`Configure release branch protections`) doit prévisualiser puis appliquer le workflow depuis main en ciblant l’organisation. Faites-le avant de publier ou de compter sur les protections.

Conservez le contrôle Release readiness dans la CI du dépôt et vérifiez qu’il s’exécute avec succès. Un contrôle attendu sans workflow correspondant reste en attente. Créez main et release à partir d’une source stable déjà revue. Les règles les ciblent avant même leur création.

Conservez les références immuables et revues des workflows centraux. Mettez à jour ensemble le contrôle de publication et l’outil de publication par PR. Pour les dépôts privés, vérifiez séparément le forfait et les protections. La découverte ne donne aucun accès Azure. Les secrets des plateformes restent dans les environnements des auteurs.

[English](../../../docs/branch-policy.md) · [Kenshi Races](../README.md)
