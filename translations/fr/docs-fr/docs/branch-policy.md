<a id="permanent-branch-policy-and-repository-onboarding"></a>
# Politique de branche permanente et intégration du référentiel

Tous les référentiels Meitou -Modding-Ecosystem utilisent la [politique permanente centrale](https://github.com/TheIvoryDisaster/GutenStructure/blob/main/docs/branch-policy.md) :

| Bifurquer | Exigence |
| --- | --- |
| Branches ordinaires | Non protégé |
| bêta | PR + dépassement de CI ; les super-responsables peuvent contourner la révision, y compris les auto-fusions |
| principal / version | PR + réussite à CI + une approbation de super-mainteneur indépendant ; pas de contournement des avis |

GitHub Actions n’a pas de contournement de révision. L'exception de l'équipe Beta s'applique uniquement aux avis : la sécurité obligatoire des PR / CI n'a pas de contournement. CODEOWNERS n’ajoute pas d’exigence d’approbation supplémentaire.

Les modèles GitHub copient la source, et non les ensembles de règles de branche, les autorisations d'équipe, les environnements ou les secrets.  L'outil Configurer les protections des branches de version de GutenStructure découvre de nouveaux référentiels d'organisation publics toutes les heures à la minute 17, attribue au référentiel d'équipe un accès en écriture en cas d'absence et applique la stratégie. Pour configurer un nouveau dépôt immédiatement, demandez à un responsable GutenStructure autorisé de prévisualiser puis d'appliquer ce flux de travail sur l'organisation cible principale. Faites-le avant de compter sur des fusions ou des publications protégées. Conservez Release readiness dans l'appelant CI et vérifiez les passages réels ; les contrôles attendus sans flux de travail correspondant attendront indéfiniment.

Créez la version principale/version à partir de la source stable révisée lorsque vous êtes prêt ; la politique les cible avant même qu’ils existent. Conservez les broches de flux de travail centrales révisées et immuables de ce modèle, en mettant à jour ensemble la porte de publication et l'éditeur de packages via les PR. Les dépôts privés nécessitent une validation de plan/protection distincte ; aucun accès Azure n’est accordé par découverte. Les secrets de la plateforme externe restent dans les environnements appartenant à l'auteur et ne sont jamais copiés à partir de ce modèle.
