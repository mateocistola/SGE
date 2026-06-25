using SGE.Aplicacion.Autorizacion;
using SGE.Aplicacion.Comun;
using SGE.Aplicacion.Expedientes;
using SGE.Aplicacion.Tramites;
using SGE.Dominio.Comun;
using SGE.Dominio.Expedientes;
using SGE.Dominio.Tramites;
using SGE.Infraestructura.Autorizacion;
using SGE.Infraestructura.Expedientes;
using SGE.Infraestructura.Tramites;
using SGE.Aplicacion.Usuarios;
using SGE.Infraestructura.Usuarios;
using SGE.Infraestructura.Seguridad;


builder.Services.AddScoped<IHashService, HashService>();

IRepositorioUsuarios usuarioRepository =
    new RepositorioUsuariosTxt("Datos/usuarios.txt");

IExpedienteRepository expedienteRepository =
    new ExpedienteTxtRepository("Datos/expedientes.txt");

ITramiteRepository tramiteRepository =
    new TramiteTxtRepository("Datos/tramites.txt");

IAutorizacionService autorizacionService =
    new AutorizacionProvisionalService();

var registrarUsuarioUseCase =
    new RegistrarUsuarioUseCase(usuarioRepository);

var actualizacionEstadoExpedienteService = new ActualizacionEstadoExpedienteService(
        expedienteRepository,
        tramiteRepository
    );

var agregarExpedienteUseCase = new AgregarExpedienteUseCase(
    expedienteRepository,
    autorizacionService
);

var listarExpedientesUseCase = new ListarExpedientesUseCase(
    expedienteRepository
);

var modificarCaratulaExpedienteUseCase = new ModificarCaratulaExpedienteUseCase(
    expedienteRepository,
    autorizacionService
);

var cambiarEstadoExpedienteUseCase = new CambiarEstadoExpedienteUseCase(
    expedienteRepository,
    autorizacionService
);

var eliminarExpedienteUseCase = new EliminarExpedienteUseCase(
    expedienteRepository,
    tramiteRepository,
    autorizacionService
);

var agregarTramiteUseCase = new AgregarTramiteUseCase(
    tramiteRepository,
    expedienteRepository,
    autorizacionService,
    actualizacionEstadoExpedienteService
);

var listarTramitesPorExpedienteUseCase = new ListarTramitesPorExpedienteUseCase(
    tramiteRepository,
    autorizacionService
);

var modificarTramiteUseCase = new ModificarTramiteUseCase(
    tramiteRepository,
    expedienteRepository,
    autorizacionService,
    actualizacionEstadoExpedienteService
);

var eliminarTramiteUseCase = new EliminarTramiteUseCase(
    tramiteRepository,
    expedienteRepository,
    autorizacionService,
    actualizacionEstadoExpedienteService
);
var usuario = registrarUsuarioUseCase.Ejecutar(
    new RegistrarUsuarioRequest
    {
        Nombre = "Mate",
        CorreoElectronico = "mate@test.com",
        Contrasena = "1234"
    });

Console.WriteLine($"Usuario creado: {usuario.UsuarioId}");

Guid idUsuario = Guid.NewGuid();

const int IntentosMaximos = 5;

bool salir = false;

while (!salir)
{
    MostrarMenu();

    Console.Write("Seleccione una opción: ");
    string? opcion = Console.ReadLine();

    Console.WriteLine();

    try
    {
        switch (opcion)
        {
            case "1":
                CrearExpediente();
                break;

            case "2":
                ListarExpedientes();
                break;

            case "3":
                ModificarCaratula();
                break;

            case "4":
                CambiarEstado();
                break;

            case "5":
                EliminarExpediente();
                break;

            case "6":
                AgregarTramite();
                break;

            case "7":
                ListarTramitesPorExpediente();
                break;

            case "8":
                ModificarTramite();
                break;

            case "9":
                EliminarTramite();
                break;

            case "10":
                ProbarErrorCaratulaVacia();
                break;

            case "0":
                salir = true;
                Console.WriteLine("Saliendo del sistema...");
                break;

            default:
                Console.WriteLine("Opción inválida.");
                break;
        }
    }
    catch (AutorizacionException ex)
    {
        Console.WriteLine("Error de autorización: " + ex.Message);
    }
    catch (DominioException ex)
    {
        Console.WriteLine("Error de dominio: " + ex.Message);
    }
    catch (EntidadNoEncontradaException ex)
    {
        Console.WriteLine("Entidad no encontrada: " + ex.Message);
    }
    catch (SGE.Aplicacion.Comun.RepositorioException ex)
    {
        Console.WriteLine("Error de repositorio: " + ex.Message);
    }
    catch (SGE.Infraestructura.Comun.RepositorioException ex)
    {
        Console.WriteLine("Error de repositorio: " + ex.Message);
    }
    catch (Exception ex)
    {
        Console.WriteLine("Error inesperado: " + ex.Message);
    }

    Console.WriteLine();

    if (!salir)
    {
        Console.WriteLine("Presione ENTER para continuar...");
        Console.ReadLine();
        Console.Clear();
    }
}

void MostrarMenu()
{
    Console.WriteLine("=== SGE - Sistema de Gestión de Expedientes ===");
    Console.WriteLine();
    Console.WriteLine("Usuario de prueba: " + idUsuario.ToString());
    Console.WriteLine();
    Console.WriteLine("1. Crear expediente");
    Console.WriteLine("2. Listar expedientes");
    Console.WriteLine("3. Modificar carátula de expediente");
    Console.WriteLine("4. Cambiar estado de expediente");
    Console.WriteLine("5. Eliminar expediente");
    Console.WriteLine("6. Agregar trámite");
    Console.WriteLine("7. Listar trámites por expediente");
    Console.WriteLine("8. Modificar trámite");
    Console.WriteLine("9. Eliminar trámite");
    Console.WriteLine("10. Probar error con carátula vacía");
    Console.WriteLine("0. Salir");
    Console.WriteLine();
}

void CrearExpediente()
{
    Console.Write("Ingrese carátula: ");
    string caratula = Console.ReadLine() ?? "";

    var response = agregarExpedienteUseCase.Ejecutar(
        new AgregarExpedienteRequest(
            caratula,
            idUsuario
        )
    );

    Console.WriteLine("Expediente creado correctamente.");
    Console.WriteLine("Id: " + response.Id);
}

void ListarExpedientes()
{
    var response = listarExpedientesUseCase.Ejecutar();

    Console.WriteLine("=== Expedientes ===");
    Console.WriteLine();

    var expedientes = response.Expedientes.ToList();

    if (!expedientes.Any())
    {
        Console.WriteLine("No hay expedientes cargados.");
        return;
    }

    foreach (var expediente in expedientes)
    {
        Console.WriteLine("Id: " + expediente.Id);
        Console.WriteLine("Carátula: " + expediente.Caratula);
        Console.WriteLine("Estado: " + expediente.Estado);
        Console.WriteLine("Fecha creación: " + expediente.FechaCreacion);
        Console.WriteLine("Última modificación: " + expediente.FechaUltimaModificacion);
        Console.WriteLine("Usuario último cambio: " + expediente.UsuarioUltimoCambio);
        Console.WriteLine("-------------------------------------");
    }
}

void ModificarCaratula()
{
    Guid? expedienteId = SeleccionarExpediente();

    if (expedienteId == null)
    {
        return;
    }

    Console.Write("Ingrese nueva carátula: ");
    string nuevaCaratula = Console.ReadLine() ?? "";

    var response = modificarCaratulaExpedienteUseCase.Ejecutar(
        new ModificarCaratulaExpedienteRequest(
            expedienteId.Value,
            nuevaCaratula,
            idUsuario
        )
    );

    Console.WriteLine("Carátula modificada correctamente.");
    Console.WriteLine("Id expediente: " + response.Id);
}

void CambiarEstado()
{
    Guid? expedienteId = SeleccionarExpediente();

    if (expedienteId == null)
    {
        return;
    }

    Console.WriteLine("Estados disponibles:");

    foreach (var estado in Enum.GetValues<EstadoExpediente>())
    {
        Console.WriteLine("- " + estado);
    }

    EstadoExpediente? nuevoEstado = LeerEstadoExpediente();

    if (nuevoEstado == null)
    {
        return;
    }

    var response = cambiarEstadoExpedienteUseCase.Ejecutar(
        new CambiarEstadoExpedienteRequest(
            expedienteId.Value,
            nuevoEstado.Value,
            idUsuario
        )
    );

    Console.WriteLine("Estado modificado correctamente.");
    Console.WriteLine("Id expediente: " + response.Id);
}

void EliminarExpediente()
{
    Guid? expedienteId = SeleccionarExpediente();

    if (expedienteId == null)
    {
        return;
    }

    Console.Write("¿Seguro que desea eliminar el expediente? S/N: ");
    string? confirmacion = Console.ReadLine();

    if (confirmacion == null || confirmacion.ToUpper() != "S")
    {
        Console.WriteLine("Operación cancelada.");
        return;
    }

    var response = eliminarExpedienteUseCase.Ejecutar(
        new EliminarExpedienteRequest(
            expedienteId.Value,
            idUsuario
        )
    );

    Console.WriteLine("Expediente eliminado correctamente.");
    Console.WriteLine("Id eliminado: " + response.Id);
}

void AgregarTramite()
{
    Guid? expedienteId = SeleccionarExpediente();

    if (expedienteId == null)
    {
        return;
    }

    Console.WriteLine("Etiquetas disponibles:");

    foreach (var etiqueta in Enum.GetValues<EtiquetaTramite>())
    {
        Console.WriteLine("- " + etiqueta);
    }

    EtiquetaTramite? etiquetaElegida = LeerEtiquetaTramite();

    if (etiquetaElegida == null)
    {
        return;
    }

    Console.Write("Ingrese contenido del trámite: ");
    string contenido = Console.ReadLine() ?? "";

    var response = agregarTramiteUseCase.Ejecutar(
        new AgregarTramiteRequest
        {
            ExpedienteId = expedienteId.Value,
            Etiqueta = etiquetaElegida.Value,
            Contenido = contenido,
            UsuarioId = idUsuario
        }
    );

    Console.WriteLine("Trámite creado correctamente.");
    Console.WriteLine("Id trámite: " + response.TramiteId);
}

void ListarTramitesPorExpediente()
{
    Guid? expedienteId = SeleccionarExpediente();

    if (expedienteId == null)
    {
        return;
    }

    var response = listarTramitesPorExpedienteUseCase.Ejecutar(
        new ListarTramitesRequest
        {
            ExpedienteId = expedienteId.Value,
            UsuarioId = idUsuario
        }
    );

    Console.WriteLine("=== Trámites del expediente ===");
    Console.WriteLine();

    var tramites = response.Tramites.ToList();

    if (!tramites.Any())
    {
        Console.WriteLine("El expediente no tiene trámites cargados.");
        return;
    }

    foreach (var tramite in tramites)
    {
        Console.WriteLine("Id: " + tramite.Id);
        Console.WriteLine("Etiqueta: " + tramite.Etiqueta);
        Console.WriteLine("Contenido: " + tramite.Contenido);
        Console.WriteLine("Fecha creación: " + tramite.FechaCreacion);
        Console.WriteLine("-------------------------------------");
    }
}

void ModificarTramite()
{
    Guid? tramiteId = SeleccionarTramite();

    if (tramiteId == null)
    {
        return;
    }

    Console.Write("Ingrese nuevo contenido: ");
    string contenido = Console.ReadLine() ?? "";

    var response = modificarTramiteUseCase.Ejecutar(
        new ModificarTramiteRequest
        {
            TramiteId = tramiteId.Value,
            Contenido = contenido,
            UsuarioId = idUsuario
        }
    );

    if (response.Modificado)
    {
        Console.WriteLine("Trámite modificado correctamente.");
    }
}

void EliminarTramite()
{
    Guid? tramiteId = SeleccionarTramite();

    if (tramiteId == null)
    {
        return;
    }

    Console.Write("¿Seguro que desea eliminar el trámite? S/N: ");
    string? confirmacion = Console.ReadLine();

    if (confirmacion == null || confirmacion.ToUpper() != "S")
    {
        Console.WriteLine("Operación cancelada.");
        return;
    }

    var response = eliminarTramiteUseCase.Ejecutar(
        new EliminarTramiteRequest
        {
            TramiteId = tramiteId.Value,
            UsuarioId = idUsuario
        }
    );

    if (response.Eliminado)
    {
        Console.WriteLine("Trámite eliminado correctamente.");
    }
}


void ProbarErrorCaratulaVacia()
{
    Console.WriteLine("Se intenta crear un expediente con carátula vacía...");

    agregarExpedienteUseCase.Ejecutar(
        new AgregarExpedienteRequest(
            "",
            idUsuario
        )
    );

    Console.WriteLine("Si aparece este mensaje, algo salió mal porque debería tirar excepción.");
}

Guid? SeleccionarExpediente()
{
    var response = listarExpedientesUseCase.Ejecutar();
    var expedientes = response.Expedientes.ToList();

    if (!expedientes.Any())
    {
        Console.WriteLine("No hay expedientes cargados.");
        return null;
    }

    Console.WriteLine("Expedientes disponibles:");
    Console.WriteLine();

    for (int i = 0; i < expedientes.Count; i++)
    {
        Console.WriteLine($"{i + 1}. {expedientes[i].Caratula} - {expedientes[i].Estado}");
        Console.WriteLine("   Id: " + expedientes[i].Id);
        Console.WriteLine();
    }

    Console.WriteLine("Puede ingresar el número, el Id o la carátula del expediente.");

    for (int intento = 1; intento <= IntentosMaximos; intento++)
    {
        Console.Write("Seleccione expediente: ");
        string texto = Console.ReadLine() ?? "";

        var expediente = BuscarExpediente(expedientes, texto);

        if (expediente != null)
        {
            return expediente.Id;
        }

        Console.WriteLine("No se encontró el expediente.");
        Console.WriteLine("Intento " + intento + " de " + IntentosMaximos + ".");
        Console.WriteLine();
    }

    Console.WriteLine("Se superó la cantidad de intentos. Se vuelve al menú principal.");
    return null;
}

dynamic? BuscarExpediente(dynamic expedientes, string texto)
{
    if (string.IsNullOrWhiteSpace(texto))
    {
        return null;
    }

    texto = texto.Trim();

    if (int.TryParse(texto, out int numero))
    {
        if (numero >= 1 && numero <= expedientes.Count)
        {
            return expedientes[numero - 1];
        }
    }

    if (Guid.TryParse(texto, out Guid id))
    {
        foreach (var expediente in expedientes)
        {
            if (expediente.Id == id)
            {
                return expediente;
            }
        }
    }

    var coincidencias = new List<dynamic>();

    foreach (var expediente in expedientes)
    {
        string caratula = expediente.Caratula.ToString();

        if (caratula.Equals(texto, StringComparison.OrdinalIgnoreCase))
        {
            coincidencias.Add(expediente);
        }
    }

    if (coincidencias.Count == 1)
    {
        return coincidencias[0];
    }

    if (coincidencias.Count > 1)
    {
        Console.WriteLine("Hay más de un expediente con esa carátula. Elegilo por número o por Id.");
        return null;
    }

    return null;
}

Guid? SeleccionarTramite()
{
    Guid? expedienteId = SeleccionarExpediente();

    if (expedienteId == null)
    {
        return null;
    }

    var response = listarTramitesPorExpedienteUseCase.Ejecutar(
        new ListarTramitesRequest
        {
            ExpedienteId = expedienteId.Value,
            UsuarioId = idUsuario
        }
    );

    var tramites = response.Tramites.ToList();

    if (!tramites.Any())
    {
        Console.WriteLine("El expediente seleccionado no tiene trámites cargados.");
        return null;
    }

    Console.WriteLine("Trámites disponibles:");
    Console.WriteLine();

    for (int i = 0; i < tramites.Count; i++)
    {
        Console.WriteLine($"{i + 1}. {tramites[i].Etiqueta}");
        Console.WriteLine("   Contenido: " + tramites[i].Contenido);
        Console.WriteLine("   Id: " + tramites[i].Id);
        Console.WriteLine();
    }

    Console.WriteLine("Puede ingresar el número o el Id del trámite.");

    for (int intento = 1; intento <= IntentosMaximos; intento++)
    {
        Console.Write("Seleccione trámite: ");
        string texto = Console.ReadLine() ?? "";

        Guid? tramiteId = BuscarTramite(tramites, texto);

        if (tramiteId != null)
        {
            return tramiteId;
        }

        Console.WriteLine("No se encontró el trámite.");
        Console.WriteLine("Intento " + intento + " de " + IntentosMaximos + ".");
        Console.WriteLine();
    }

    Console.WriteLine("Se superó la cantidad de intentos. Se vuelve al menú principal.");
    return null;
}

Guid? BuscarTramite(dynamic tramites, string texto)
{
    if (string.IsNullOrWhiteSpace(texto))
    {
        return null;
    }

    texto = texto.Trim();

    if (int.TryParse(texto, out int numero))
    {
        if (numero >= 1 && numero <= tramites.Count)
        {
            return tramites[numero - 1].Id;
        }
    }

    if (Guid.TryParse(texto, out Guid id))
    {
        foreach (var tramite in tramites)
        {
            if (tramite.Id == id)
            {
                return tramite.Id;
            }
        }
    }

    return null;
}

EstadoExpediente? LeerEstadoExpediente()
{
    for (int intento = 1; intento <= IntentosMaximos; intento++)
    {
        Console.Write("Ingrese nuevo estado: ");
        string? estadoTexto = Console.ReadLine();

        if (Enum.TryParse<EstadoExpediente>(estadoTexto, true, out var estado))
        {
            return estado;
        }

        Console.WriteLine("Estado inválido.");
        Console.WriteLine("Intento " + intento + " de " + IntentosMaximos + ".");
        Console.WriteLine();
    }

    Console.WriteLine("Se superó la cantidad de intentos. Se vuelve al menú principal.");
    return null;
}

EtiquetaTramite? LeerEtiquetaTramite()
{
    for (int intento = 1; intento <= IntentosMaximos; intento++)
    {
        Console.Write("Ingrese etiqueta: ");
        string? etiquetaTexto = Console.ReadLine();

        if (Enum.TryParse<EtiquetaTramite>(etiquetaTexto, true, out var etiqueta))
        {
            return etiqueta;
        }

        Console.WriteLine("Etiqueta inválida.");
        Console.WriteLine("Intento " + intento + " de " + IntentosMaximos + ".");
        Console.WriteLine();
    }

    Console.WriteLine("Se superó la cantidad de intentos. Se vuelve al menú principal.");
    return null;
}