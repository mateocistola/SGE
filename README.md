# Sistema de Gestión de Expedientes (SGE)

## Requisitos

- .NET 10
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

1. Iniciar sesión con el administrador (`POST /login`).
2. Listar usuarios (`GET /usuarios`).
3. Crear un expediente (`POST /expedientes`).
4. Listar expedientes (`GET /expedientes`).
5. Obtener un expediente por Id (`GET /expedientes/{id}`).
6. Modificar la carátula (`PUT /expedientes/{id}/caratula`).
7. Cambiar el estado (`PUT /expedientes/{id}/estado`).
8. Agregar un trámite (`POST /tramites`).
9. Listar los trámites del expediente (`GET /expedientes/{id}/tramites`).
10. Modificar un trámite (`PUT /tramites/{id}`).
11. Eliminar un trámite (`DELETE /tramites/{id}`).
12. Eliminar un expediente (`DELETE /expedientes/{id}`).

## Notas

- Todos los endpoints protegidos requieren un JWT obtenido mediante el login.
- El UserId se obtiene del token JWT y no se envía en el cuerpo de las solicitudes.