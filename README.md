# Sistema de Gestión de Expedientes (SGE)

## Requisitos

- .NET 10 SDK
- SQLite

## Usuarios semilla

### Administrador

- Correo: admin@sge.com
- Contraseña: admin123

### Usuario 1

- Correo: usuario1@sge.com
- Contraseña: 123456

Permisos:
- ExpedienteAlta
- TramiteAlta

### Usuario 2

- Correo: usuario2@sge.com
- Contraseña: 123456

Permisos:
- ExpedienteModificacion
- TramiteModificacion

## Orden recomendado de prueba

1. Registrar un usuario (`POST /auth/registro`) *(opcional)*.
2. Iniciar sesión como administrador (`POST /auth/login`).
3. Listar usuarios (`GET /usuarios`).
4. Modificar permisos de un usuario (`PUT /usuarios/{id}/permisos`).
5. Iniciar sesión con el usuario al que se le asignaron permisos (`POST /auth/login`).
6. Crear un expediente (`POST /expedientes`).
7. Listar expedientes (`GET /expedientes`).
8. Obtener un expediente por Id (`GET /expedientes/{id}`).
9. Modificar la carátula (`PUT /expedientes/{id}/caratula`).
10. Cambiar el estado (`PUT /expedientes/{id}/estado`).
11. Agregar un trámite (`POST /tramites`).
12. Listar los trámites del expediente (`GET /expedientes/{id}/tramites`).
13. Modificar un trámite (`PUT /tramites/{id}`).
14. Eliminar un trámite (`DELETE /tramites/{id}`).
15. Eliminar un expediente (`DELETE /expedientes/{id}`).

## Notas

- Todos los endpoints protegidos requieren un token JWT obtenido mediante `POST /auth/login`.
- El UserId se obtiene del token JWT y nunca se envía en el cuerpo de las solicitudes.