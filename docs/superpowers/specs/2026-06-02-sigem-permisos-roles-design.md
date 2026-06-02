# Mejora SIGEM: permisos por rol, paciente completo y signos vitales

## Objetivo

Mejorar SIGEM a partir del analisis proporcionado, priorizando permisos reales por rol. La primera etapa debe dejar claro que cada usuario ve y ejecuta solo las funciones que corresponden a su rol, mientras se completa la informacion administrativa del paciente y se refuerza el modulo de signos vitales.

## Alcance de esta etapa

Esta etapa incluye:

- Centralizar permisos para Medico, Enfermera, Recepcionista y Administrador.
- Ajustar el menu principal y acciones visibles segun permisos.
- Completar el modelo de paciente con datos administrativos adicionales.
- Permitir alta y edicion de datos personales segun rol.
- Mantener signos vitales como modulo central para medico y enfermera.
- Mostrar estado pendiente o validado en registros de signos vitales.
- Agregar alertas simples cuando signos vitales esten fuera de rango.

Esta etapa no incluye persistencia profunda de diagnosticos, tratamientos, recetas ni informes medicos. Esas funciones pueden quedar visibles solo para el rol Medico como secciones preparadas, pero se implementaran en una etapa posterior.

## Roles y permisos

| Funcion | Medico | Enfermera | Recepcionista | Administrador |
|---|---:|---:|---:|---:|
| Ver panel principal | Si | Si, limitado | Si, administrativo | Si, tecnico |
| Alta de paciente | Si | Si | Si | No clinico |
| Editar datos personales | Si | Si | Si | No clinico |
| Ver signos vitales | Si | Si | No | No clinico |
| Capturar signos vitales | Si | Si | No | No |
| Validar signos vitales | Si | No | No | No |
| Ver expediente completo | Si | Parcial | No | Tecnico |
| Diagnosticos | Si | No | No | No |
| Tratamientos | Si | No | No | No |
| Recetas | Si | No | No | No |
| Informes medicos | Si | No | No | No |
| Administrar usuarios | No | No | No | Si |
| Respaldos y restauracion | No | No | No | Si |

## Arquitectura propuesta

### Servicio de permisos

Crear una clase de dominio simple, por ejemplo `PermisosRol`, que reciba `RolUsuario` y exponga metodos o propiedades booleanas para cada permiso relevante:

- `PuedeVerPacientes`
- `PuedeAltaPaciente`
- `PuedeEditarDatosPersonales`
- `PuedeVerSignosVitales`
- `PuedeCapturarSignosVitales`
- `PuedeValidarSignosVitales`
- `PuedeVerClinicoCompleto`
- `PuedeAdministrarSistema`

Esto evita seguir repartiendo comparaciones directas como `rol == RolUsuario.Enfermera` por formularios y presentadores. Las vistas seguiran decidiendo que mostrar, pero basadas en permisos centralizados.

### Menu principal

`MenuPrincipalVista` debe usar `PermisosRol` para configurar botones, tarjetas y acciones:

- Medico: panel, pacientes, consulta medica, validacion de signos y secciones clinicas.
- Enfermera: panel limitado, consulta/signos vitales, alta o edicion administrativa permitida.
- Recepcionista: panel administrativo y gestion de pacientes sin acceso clinico.
- Administrador: administracion tecnica, usuarios y respaldos; sin capturar ni validar informacion clinica.

Cuando un rol no tenga permiso, se deben ocultar los accesos y bloquear tambien el metodo que abre la pantalla. No basta con ocultar el boton.

### Modelo de paciente

Ampliar `Paciente` con datos administrativos del analisis:

- `ContactoEmergencia`
- `PersonaResponsable`
- `Ocupacion`
- `Escolaridad`
- `Alergias`

`Direccion` y `Telefono` ya existen y deben mantenerse. La propiedad `Direccion` puede seguir representando domicilio para evitar cambios innecesarios.

### Repositorios

`SigemRepositorioJson` debe serializar los nuevos campos de manera natural a JSON.

`SigemRepositorioPostgres` debe leer y guardar estos campos cuando existan columnas disponibles. Si el esquema real de IMSS todavia no contiene todas las columnas, la implementacion debe preparar el modelo y evitar romper la conexion existente. El script SQL de inicializacion debe documentar/agregar las columnas para bases nuevas o migraciones locales.

### Formulario de paciente

La alta y edicion de paciente deben incluir los nuevos campos administrativos. El acceso se controla por permisos:

- Medico, Enfermera y Recepcionista pueden capturar datos personales.
- Recepcionista no puede abrir signos vitales ni historial clinico.
- Administrador no debe usar el formulario como usuario clinico.

### Signos vitales

El modulo mantiene:

- Captura de peso, talla, temperatura, pulso, frecuencia respiratoria, presion arterial, CC y saturacion.
- IMC y PAM calculados.
- `RegistradoPor`, `Validado`, `ValidadoPor` e `IdDoctor`.
- Enfermera guarda registros pendientes.
- Medico guarda validado o valida registros pendientes.

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
5. En alta/edicion de paciente se guardan datos administrativos extendidos.
6. En signos vitales se guardan registros con usuario capturador y estado pendiente/validado.
7. El medico puede validar registros pendientes.

## Manejo de errores

- Si un usuario intenta abrir una seccion sin permiso desde codigo, mostrar mensaje claro y no ejecutar la accion.
- Si faltan columnas nuevas en PostgreSQL, evitar caidas inesperadas durante lectura cuando sea posible y documentar la migracion en `SIGEM/sql/init.sql`.
- Si los signos vitales estan fuera de rango, mostrar advertencia visible, pero permitir guardar si los campos obligatorios son validos.

## Pruebas y verificacion

Verificar con pruebas o ejecucion manual:

- El medico ve pacientes, consulta, signos vitales, validacion y secciones clinicas.
- La enfermera no ve administracion ni diagnosticos/tratamientos/recetas, pero puede capturar signos vitales.
- La recepcionista puede alta/editar paciente y no puede ver/capturar signos vitales.
- El administrador ve administracion y no puede capturar datos clinicos.
- Los nuevos campos del paciente se guardan en JSON.
- La inicializacion SQL incluye los nuevos campos administrativos.
- Las alertas de signos vitales aparecen cuando hay valores fuera de rango.

## Criterios de aceptacion

- Las reglas de rol estan centralizadas en una clase reutilizable.
- El menu y las acciones internas usan permisos centralizados.
- Recepcionista queda integrada como rol real.
- Administrador queda separado de funciones clinicas.
- Paciente contiene los nuevos datos administrativos solicitados.
- Signos vitales conserva historial, pendiente/validado y agrega advertencias simples.
- El proyecto compila despues de los cambios.
