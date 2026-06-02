# Mejora SIGEM: permisos por rol y signos vitales por formato clinico

## Objetivo

Mejorar SIGEM a partir del analisis proporcionado, priorizando permisos reales por rol y el orden clinico de los signos vitales. Por ahora se omite ampliar datos sensibles de identidad del paciente. `Direccion` y `Telefono` ya existen en el modelo, pero no se agregaran nuevos campos administrativos como contacto de emergencia, persona responsable, ocupacion o escolaridad en esta etapa.

## Alcance de esta etapa

Esta etapa incluye:

- Centralizar permisos para Medico, Enfermera, Recepcionista y Administrador.
- Ajustar el menu principal y acciones visibles segun permisos.
- Mantener los datos actuales del paciente sin ampliar identidad sensible.
- Ordenar los signos vitales segun los formatos clinicos revisados.
- Permitir visualizar signos vitales en una vista compatible con Nota de Evolucion.
- Permitir visualizar signos vitales en una vista resumida compatible con Historia Clinica General.
- Mantener estado pendiente o validado en registros de signos vitales.
- Agregar alertas simples cuando signos vitales esten fuera de rango.

Esta etapa no incluye persistencia profunda de diagnosticos, tratamientos, recetas ni informes medicos. El formato de receta se reconoce como formato separado de prescripcion; no se usara como vista principal de signos vitales.

## Formatos revisados

### HISTORIA_CLINICA_GENERAL AM.xls

Este formato incluye una seccion resumida de signos dentro de la historia clinica:

- Fecha de atencion.
- Numero de expediente.
- Edad y sexo.
- Peso.
- Temperatura.
- Talla/estatura.
- Diagnosticos.

SIGEM debe usar este formato como vista clinica resumida. Los signos vitales se mostraran de manera compacta y ordenada para que el medico pueda consultarlos dentro del contexto de historia clinica.

### NOTA_DE_EVOLUCION AM.xls

Este formato contiene el bloque mas completo para signos vitales y somatometria:

- Fecha.
- Hora.
- Peso.
- Estatura/talla.
- Temperatura.
- Pulso/F.C.
- Frecuencia respiratoria.
- Presion arterial.
- Saturacion O2.
- IMC.
- PAM.
- Nota medica.
- Estado de validacion en SIGEM: pendiente o validado.
- Usuario que capturo y medico que valido, aunque estos no necesariamente esten impresos como campos del formato original.

SIGEM debe usar este formato como vista principal de signos vitales. El historial debe ordenarse por fecha y hora descendente para consulta, y poder mostrarse en orden cronologico cuando se prepare una salida tipo documento.

### RECETA FORMATO.xlsx

Este formato corresponde a receta medica. Incluye paciente, CURP, edad, sexo, expediente, diagnostico, servicio, alergias, medicamento, presentacion, concentracion e indicaciones.

No se usara para signos vitales en esta etapa. Queda documentado para una etapa posterior de recetas y tratamientos del rol Medico.

## Roles y permisos

| Funcion | Medico | Enfermera | Recepcionista | Administrador |
|---|---:|---:|---:|---:|
| Ver panel principal | Si | Si, limitado | Si, administrativo | Si, tecnico |
| Alta de paciente | Si | Si | Si | No clinico |
| Editar datos personales existentes | Si | Si | Si | No clinico |
| Ver signos vitales | Si | Si | No | No clinico |
| Capturar signos vitales | Si | Si | No | No |
| Validar signos vitales | Si | No | No | No |
| Ver vista Historia Clinica | Si | Parcial | No | No clinico |
| Ver vista Nota de Evolucion | Si | Si | No | No clinico |
| Diagnosticos | Si | No | No | No |
| Tratamientos | Si | No | No | No |
| Recetas | Si | No | No | No |
| Informes medicos | Si | No | No | No |
| Administrar usuarios | No | No | No | Si |
| Respaldos y restauracion | No | No | No | Si |

## Arquitectura propuesta

### Servicio de permisos

Crear una clase de dominio simple, por ejemplo `PermisosRol`, que reciba `RolUsuario` y exponga propiedades booleanas para cada permiso relevante:

- `PuedeVerPacientes`
- `PuedeAltaPaciente`
- `PuedeEditarDatosPersonales`
- `PuedeVerSignosVitales`
- `PuedeCapturarSignosVitales`
- `PuedeValidarSignosVitales`
- `PuedeVerHistoriaClinica`
- `PuedeVerNotaEvolucion`
- `PuedeAdministrarSistema`

Esto evita seguir repartiendo comparaciones directas como `rol == RolUsuario.Enfermera` por formularios y presentadores. Las vistas seguiran decidiendo que mostrar, pero basadas en permisos centralizados.

### Menu principal

`MenuPrincipalVista` debe usar `PermisosRol` para configurar botones, tarjetas y acciones:

- Medico: panel, pacientes, signos vitales, validacion, historia clinica, nota de evolucion y secciones clinicas futuras.
- Enfermera: panel limitado, captura de signos vitales y vista de nota de evolucion sin validacion.
- Recepcionista: panel administrativo y gestion basica de pacientes sin acceso a signos vitales ni historial clinico.
- Administrador: administracion tecnica, usuarios y respaldos; sin capturar ni validar informacion clinica.

Cuando un rol no tenga permiso, se deben ocultar los accesos y bloquear tambien el metodo que abre la pantalla. No basta con ocultar el boton.

### Modelo de paciente

No se ampliara el modelo de paciente con nuevos campos de identidad en esta etapa. Se mantienen los campos actuales para no introducir problemas de identidad ni cambios innecesarios:

- `Expediente`
- `Curp`
- `Nombre`
- `Apellido`
- `FechaNacimiento`
- `Sexo`
- `Telefono`
- `Direccion`

`Telefono` y `Direccion` siguen existiendo porque ya estan en el sistema, pero no son el foco de esta mejora.

### Signos vitales

El modulo debe ordenar y mostrar los signos vitales con nombres alineados a los formatos:

| SIGEM | Nota de Evolucion | Historia Clinica General |
|---|---|---|
| `FechaHora.Date` | Fecha | Fecha de atencion |
| `FechaHora.TimeOfDay` | Hora | No principal |
| `Peso` | Peso | Peso |
| `Estatura` | Estatura/talla | Talla/estatura |
| `Temperatura` | Temperatura | Temperatura |
| `Pulso` | Pulso / F.C. | No principal |
| `FrecuenciaRespiratoria` | Frec. Resp. | Respiratorio, como referencia clinica |
| `PresionSistolica` + `PresionDiastolica` | Presion arterial | No principal |
| `SaturacionO2` | Saturacion O2 | No principal |
| `IMC` | IMC | Calculo disponible |
| `PAM` | PAM | Calculo disponible |
| `RegistradoPor` | Capturado por SIGEM | Auditoria interna |
| `ValidadoPor` | Validado por SIGEM | Auditoria interna |

La vista principal de signos vitales debe seguir el orden de Nota de Evolucion:

1. Fecha.
2. Hora.
3. Peso.
4. Estatura/talla.
5. Temperatura.
6. Pulso/F.C.
7. Frecuencia respiratoria.
8. Presion arterial.
9. Saturacion O2.
10. IMC.
11. PAM.
12. Estado: pendiente o validado.
13. Capturado por.
14. Validado por.

### Visualizacion por formato

Agregar una forma clara de visualizar los signos en dos modos:

- **Vista Nota de Evolucion:** completa, pensada para medico y enfermera. Muestra todos los signos vitales y somatometria en el orden del formato `NOTA_DE_EVOLUCION AM.xls`.
- **Vista Historia Clinica:** resumida, pensada principalmente para medico. Muestra peso, temperatura, talla/estatura, fecha de atencion y datos clinicos relacionados, sin convertirla todavia en expediente completo.

La visualizacion puede iniciar como panel o tabla dentro de SIGEM. La exportacion o llenado automatico de los Excel queda para una etapa posterior si se requiere.

### Repositorios

`SigemRepositorioJson` no requiere cambios estructurales para identidad. Debe mantener signos vitales y los campos existentes.

`SigemRepositorioPostgres` debe mantener la lectura/escritura actual de notas de evolucion. No se agregaran columnas nuevas de identidad en esta etapa.

### Alertas de signos vitales

Agregar alertas de rango simples en la capa de presentacion para mostrar advertencias sin impedir guardar necesariamente. Rangos iniciales sugeridos:

- Temperatura menor a 36.0 o mayor a 37.5.
- Pulso menor a 60 o mayor a 100.
- Frecuencia respiratoria menor a 12 o mayor a 20.
- Presion sistolica menor a 90 o mayor a 140.
- Presion diastolica menor a 60 o mayor a 90.
- Saturacion O2 menor a 95.

## Flujo de datos

1. El usuario inicia sesion y se obtiene su `RolUsuario`.
2. La vista principal crea permisos desde el rol.
3. El menu muestra solo pantallas permitidas.
4. Las acciones internas validan permisos antes de abrir formularios o ejecutar cambios.
5. En signos vitales se guardan registros con usuario capturador y estado pendiente/validado.
6. El medico puede validar registros pendientes.
7. El usuario autorizado puede alternar entre vista Nota de Evolucion y vista Historia Clinica para revisar los signos segun el formato correspondiente.

## Manejo de errores

- Si un usuario intenta abrir una seccion sin permiso desde codigo, mostrar mensaje claro y no ejecutar la accion.
- Si los signos vitales estan fuera de rango, mostrar advertencia visible, pero permitir guardar si los campos obligatorios son validos.
- Si no hay registros de signos vitales para un paciente, la vista debe mostrar un mensaje vacio claro.
- Si un formato Excel no esta disponible, SIGEM no debe fallar porque esta etapa solo define visualizacion interna, no exportacion obligatoria.

## Pruebas y verificacion

Verificar con pruebas o ejecucion manual:

- El medico ve pacientes, signos vitales, validacion, Nota de Evolucion e Historia Clinica.
- La enfermera no ve administracion ni diagnosticos/tratamientos/recetas, pero puede capturar signos vitales y ver Nota de Evolucion.
- La recepcionista puede alta/editar paciente basico y no puede ver/capturar signos vitales.
- El administrador ve administracion y no puede capturar datos clinicos.
- Los signos vitales aparecen en el orden de Nota de Evolucion.
- La vista Historia Clinica muestra el resumen de signos esperado.
- Las alertas de signos vitales aparecen cuando hay valores fuera de rango.
- El proyecto compila despues de los cambios.

## Criterios de aceptacion

- Las reglas de rol estan centralizadas en una clase reutilizable.
- El menu y las acciones internas usan permisos centralizados.
- Recepcionista queda integrada como rol real sin acceso clinico.
- Administrador queda separado de funciones clinicas.
- No se agregan nuevos campos sensibles de identidad del paciente en esta etapa.
- Signos vitales se ordenan segun Nota de Evolucion.
- Signos vitales se pueden visualizar como Nota de Evolucion y como resumen de Historia Clinica.
- Signos vitales conserva historial, pendiente/validado y agrega advertencias simples.
- El proyecto compila despues de los cambios.
