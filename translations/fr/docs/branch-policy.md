<a id="permanent-branch-policy-and-repository-onboarding"></a>
# Politique de branche permanente et de stockage à bord

Tous les archives Meitou-Modding-Ecosystem utilisent le [Politique permanente centrale](https://github.com/TheIvoryDisaster/GutenStructure/blob/main/docs/branch-policy.md):

| Branche | Exigences |
| --- | --- |
| Les branches ordinaires | Non protégé |
| Bébé | PR + passer CI; les super-maîtres peuvent passer la révision, y compris les auto-métrages |
| Principal / libération | PR + passant CIA+une approbation de super-maîtres indépendants; pas de bypass d'examen |

GitHub Actions n’a pas de bypass de révision. L'exception de l'équipe de Beta s'applique uniquement aux avis: obligatoire PR/CI La sécurité n’a pas de bypass. CODEOWNERS Il n'y a pas de demande d'approbation supplémentaire.

Les modèles de GitHub copient la source, pas les règles de branche, les permis d'équipe, les environnements ou les secrets. Les protections de Configuration de GutenStructure découvrent de nouvelles répercussions d'organes par heure, et ils attribuent l'accès à l'équipe en cas de disparition et appliquent la politique. Pour configurer un nouveau repo immédiatement, demandez à un maintien autorisé de la Structure de prévoir puis appliquez ce flux de travail sur l'organisation principale. Faites cela avant de compter sur des mélanges protégées ou de la publication. Gardez la préparation dans le caller CI et vérifiez les courses réelles; les vérifications attendues avec aucun flux de travail seront à jamais.

Créer main/release de la source stable révisée quand elle est prête ; la politique les cible même avant qu'elles existent. Gardez les pins de flux de travail immobiles de ce modèle, mettez à jour la porte de sortie et l'éditeur de paquets à travers les PR. Le repos privé nécessite une séparation plan/protection validation; aucun accès Azure n’est accordé par la découverte. Les secrets extérieurs de la plate-forme restent dans des environnements propriétaires et ne sont jamais copiés de ce modèle.
