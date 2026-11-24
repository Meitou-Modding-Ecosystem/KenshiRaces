# Protección de ramas y alta de repositorios

Todos los repositorios de la organización siguen la política permanente común. [GutenStructure](https://github.com/TheIvoryDisaster/GutenStructure/blob/main/docs/branch-policy.md)

| Rama | Requisitos |
| --- | --- |
| Ramas de trabajo | Sin protección. |
| `beta` | PR y CI satisfactoria. Los supermantenedores pueden omitir la revisión, incluso para fusionar sus propias PR. |
| `main` / `release` | PR, CI satisfactoria y una aprobación independiente de un supermantenedor. La revisión es obligatoria sin excepciones. |

GitHub Actions carece de excepción de revisión. En beta, la excepción del equipo se limita a la revisión. La PR y los controles obligatorios siguen siendo imprescindibles. CODEOWNERS no exige otra aprobación.

Las plantillas copian el código fuente. Las reglas, permisos, entornos y secretos se configuran aparte. Cada hora, en el minuto 17, el flujo central descubre los repositorios públicos nuevos, concede al equipo permiso de escritura si hace falta y aplica la política. Para una configuración inmediata, un mantenedor autorizado de GutenStructure (`Configure release branch protections`) debe previsualizar y aplicar el flujo desde main para la organización. Hazlo antes de publicar o confiar en las protecciones.

Mantén Release readiness en la CI del repositorio y verifica que se ejecute con éxito. Un control esperado sin flujo asociado quedará pendiente. Crea main y release a partir de código estable y revisado. Las reglas ya se dirigen a ellas antes de crearlas.

Conserva las referencias inmutables revisadas de los flujos centrales. Actualiza juntos por PR el control de publicación y el publicador de paquetes. Los repositorios privados requieren verificar el plan y las protecciones por separado. La detección no concede acceso a Azure. Los secretos de plataformas externas permanecen en los entornos de sus propietarios.

[English](../../../docs/branch-policy.md) · [Kenshi Races](../README.md)
