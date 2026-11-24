<a id="permanent-branch-policy-and-repository-onboarding"></a>
# Política de ramo permanente y repositorio en la embarcación

Todos los repositorios de Meitou-Modding-Ecosystem utilizan el [La política central permanente](https://github.com/TheIvoryDisaster/GutenStructure/blob/main/docs/branch-policy.md):

| La rama | Requisitos |
| --- | --- |
| Las ramas ordinarias | No protegido |
| La beta | PR + pasar CI; los super-mantenedores pueden pasar la revisión, incluidos los auto-mergidos |
| Principal / Liberación | PR + pasando CIA+ una aprobación de super-mantenedores independientes; no hay bypass de revisión |

GitHub no tiene un bypass de revisión. La excepción de equipo de Beta se aplica sólo a las revisións: obligatoria PR/CI La seguridad no tiene bypass. CODEOWNERS No añade un requisito de aprobación adicional.

GitHub muestra la fuente, no las reglas de rango, los permisos de equipo, los entornos o los secretos. Las protecciones de Configuración de GutenStructure descubren nuevos repositorios de órganos en el minuto 17, asignando al repositorio de escritura si falta, y aplicando la política. Para configurar un nuevo repo, póngase en contacto con un mantenedor autorizado para la previsión y aplicar el flujo de trabajo en la organización principal. Haga esto antes de confiar en la mezcla protegida o la publicación. Mantenga la preparación de la liberación en el llamador y verifica las corrientes reales; las verificaciones esperadas con ningún flujo de trabajo se esperan para siempre.

Creación main/release Desde la fuente estable revisada cuando está listo; la política los mira incluso antes de que existan. Mantenga los picos de flujo central revisados de este modelo, actualizando tanto el portal de lanzamiento como el editor de paquetes a través de los PR. El descanso privado requiere separado plan/protection Validación; no se otorga acceso al Azure por la descoberta. Los secretos de la plataforma se mantienen en los entornos de propiedad de los autores y nunca se copian de este modelo.
