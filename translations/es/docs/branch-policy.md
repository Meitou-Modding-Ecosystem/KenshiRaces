<a id="permanent-branch-policy-and-repository-onboarding"></a>
# Política de sucursal permanente e incorporación del repositorio

Todos los repositorios de Meitou -Modding-Ecosystem utilizan la [política central permanente](https://github.com/TheIvoryDisaster/GutenStructure/blob/main/docs/branch-policy.md) :

| Rama | Requisito |
| --- | --- |
| Ramas ordinarias | Desprotegido |
| beta | PR + pasando por CI ; Los supermantenedores pueden eludir la revisión, incluidas las autofusiones. |
| principal / lanzamiento | PR + aprobación de CI + una aprobación de supermantenedor independiente; sin omisión de revisión |

GitHub Actions no tiene omisión de revisión. La excepción del equipo Beta se aplica solo a las revisiones: la seguridad obligatoria de PR / CI no tiene derivación. CODEOWNERS no agrega ningún requisito de aprobación adicional.

Las plantillas de GitHub copian el código fuente, no ramifican conjuntos de reglas, permisos de equipo, entornos o secretos.  Configurar protecciones de rama de lanzamiento de GutenStructure descubre nuevos repositorios de organizaciones públicas cada hora en el minuto 17, asigna acceso de escritura al repositorio del equipo si falta y aplica la política. Para configurar un nuevo repositorio de inmediato, solicite a un mantenedor autorizado de GutenStructure una vista previa y luego aplique ese flujo de trabajo en la organización principal de destino. Haga esto antes de confiar en fusiones o publicaciones protegidas. Mantenga Release readiness en el llamante CI y verifique las ejecuciones de pases reales; Las comprobaciones esperadas sin un flujo de trabajo coincidente esperarán para siempre.

Cree principal/lanzamiento a partir de una fuente estable revisada cuando esté listo; la política se dirige a ellos incluso antes de que existan. Mantenga los pines de flujo de trabajo centrales revisados ​​e inmutables de esta plantilla, actualizando tanto la puerta de lanzamiento como el editor del paquete juntos a través de relaciones públicas. Los repositorios privados requieren una validación de plan/protección por separado; el descubrimiento no otorga acceso a Azure. Los secretos de la plataforma externa permanecen en entornos propiedad del autor y nunca se copian de esta plantilla.
