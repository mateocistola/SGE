using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using SGE.Aplicacion.Autorizacion;
using SGE.Aplicacion.Expedientes;
using SGE.Aplicacion.Tramites;
using SGE.Aplicacion.Usuarios;
using SGE.Dominio.Expedientes;
using SGE.Dominio.Tramites;
using SGE.Dominio.Usuarios;
using SGE.Infraestructura.Expedientes;
using SGE.Infraestructura.Persistencia;
using SGE.Infraestructura.Seguridad;
using SGE.Infraestructura.Tramites;
using SGE.Infraestructura.Usuarios;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IAutorizacionService, AutorizacionService>();
builder.Services.AddScoped<AgregarExpedienteUseCase>();
builder.Services.AddScoped<AgregarTramiteUseCase>();
builder.Services.AddScoped<ActualizacionEstadoExpedienteService>();
builder.Services.AddScoped<ITokenService, JwtTokenService>();
builder.Services.AddScoped<LoginUseCase>();
builder.Services.AddScoped<RegistrarUsuarioUseCase>();

var claveJwt = "clave-super-secreta-sge-tp2-2026-minimo-32-caracteres";

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(claveJwt))
        };
    });

builder.Services.AddAuthorization();


builder.Services.AddOpenApi();

builder.Services.AddDbContext<SgeContext>(options =>
{
    options.UseSqlite("Data Source=SGE.sqlite");
});

builder.Services.AddScoped<IHashService, HashService>();
builder.Services.AddScoped<IRepositorioUsuarios, UsuarioRepository>();
builder.Services.AddScoped<IExpedienteRepository, ExpedienteRepository>();
builder.Services.AddScoped<ITramiteRepository, TramiteRepository>();
builder.Services.AddScoped<IUnidadDeTrabajo, UnidadDeTrabajo>();
builder.Services.AddScoped<AgregarExpedienteUseCase>();
builder.Services.AddScoped<IAutorizacionService, AutorizacionService>();
builder.Services.AddScoped<ModificarCaratulaExpedienteUseCase>();
builder.Services.AddScoped<ListarExpedientesUseCase>();
builder.Services.AddScoped<EliminarExpedienteUseCase>();
builder.Services.AddScoped<CambiarEstadoExpedienteUseCase>();
builder.Services.AddScoped<ListarTramitesPorExpedienteUseCase>();
builder.Services.AddScoped<ModificarTramiteUseCase>();
builder.Services.AddScoped<EliminarTramiteUseCase>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.MapPost("/expedientes", [Authorize] (
    AgregarExpedienteUseCase useCase,
    CrearExpedienteRequest request,
    ClaimsPrincipal usuario) =>
{
    var usuarioIdTexto = usuario.FindFirstValue(ClaimTypes.NameIdentifier);

    if (!Guid.TryParse(usuarioIdTexto, out var usuarioId))
    {
        return Results.Unauthorized();
    }

    var response = useCase.Ejecutar(
        new AgregarExpedienteRequest(
            request.Caratula,
            usuarioId));

        return Results.Created(
            $"/expedientes/{response.Id}",
            new { id = response.Id });
    })
.WithTags("Expedientes");

app.MapPut("/expedientes/{id:guid}/estado", [Authorize] (
    Guid id,
    CambiarEstadoExpedienteUseCase useCase,
    CambiarEstadoRequest request,
    ClaimsPrincipal usuario) =>
{
    var usuarioIdTexto = usuario.FindFirstValue(ClaimTypes.NameIdentifier);

    if (!Guid.TryParse(usuarioIdTexto, out var usuarioId))
    {
        return Results.Unauthorized();
    }

    var response = useCase.Ejecutar(
        new CambiarEstadoExpedienteRequest(
            id,
            request.NuevoEstado,
            usuarioId));

    return Results.Ok(response);
})
.WithTags("Expedientes");

app.MapGet("/expedientes", [Authorize] (
    ListarExpedientesUseCase useCase) =>
{
    var response = useCase.Ejecutar();

    return Results.Ok(response);
})
.WithTags("Expedientes");

app.MapDelete("/expedientes/{id:guid}", [Authorize] (
    Guid id,
    EliminarExpedienteUseCase useCase,
    ClaimsPrincipal usuario) =>
{
    var usuarioIdTexto = usuario.FindFirstValue(ClaimTypes.NameIdentifier);

    if (!Guid.TryParse(usuarioIdTexto, out var usuarioId))
    {
        return Results.Unauthorized();
    }

    var response = useCase.Ejecutar(
        new EliminarExpedienteRequest(
            id,
            usuarioId));

    return Results.Ok(response);
})
.WithTags("Expedientes");

app.MapGet("/auth/yo", [Authorize] (ClaimsPrincipal usuario) =>
{
    var id = usuario.FindFirstValue(ClaimTypes.NameIdentifier);

    return Results.Ok(new { usuarioId = id });
})
.WithTags("Autenticación");

app.MapPost("/auth/registro", (
    RegistrarUsuarioUseCase useCase,
    RegistrarUsuarioRequest request) =>
{
    var response = useCase.Ejecutar(request);

    return Results.Created(
        $"/usuarios/{response.UsuarioId}",
        new { id = response.UsuarioId });
})
.WithTags("Autenticación");


app.MapPost("/auth/login", (
    LoginUseCase useCase,
    LoginRequest request) =>
{
    var token = useCase.Ejecutar(
        request.Correo,
        request.Contrasena);

    return Results.Ok(new { token });
})
.WithTags("Autenticación");
app.MapPut("/expedientes/{id:guid}/caratula", [Authorize] (
    Guid id,
    ModificarCaratulaExpedienteUseCase useCase,
    ModificarCaratulaRequest request,
    ClaimsPrincipal usuario) =>
{
    var usuarioIdTexto = usuario.FindFirstValue(ClaimTypes.NameIdentifier);

    if (!Guid.TryParse(usuarioIdTexto, out var usuarioId))
    {
        return Results.Unauthorized();
    }

    var response = useCase.Ejecutar(
        new ModificarCaratulaExpedienteRequest(
            id,
            request.NuevaCaratula,
            usuarioId));

    return Results.Ok(response);
})
.WithTags("Expedientes");

app.MapDelete("/tramites/{id:guid}", [Authorize] (
    Guid id,
    EliminarTramiteUseCase useCase,
    ClaimsPrincipal usuario) =>
{
    var usuarioIdTexto = usuario.FindFirstValue(ClaimTypes.NameIdentifier);

    if (!Guid.TryParse(usuarioIdTexto, out var usuarioId))
    {
        return Results.Unauthorized();
    }

    var response = useCase.Ejecutar(
        new EliminarTramiteRequest
        {
            TramiteId = id,
            UsuarioId = usuarioId
        });

    return Results.Ok(response);
})
.WithTags("Trámites");

app.MapPut("/tramites/{id:guid}", [Authorize] (
    Guid id,
    ModificarTramiteUseCase useCase,
    ModificarTramiteWebRequest request,
    ClaimsPrincipal usuario) =>
{
    var usuarioIdTexto = usuario.FindFirstValue(ClaimTypes.NameIdentifier);

    if (!Guid.TryParse(usuarioIdTexto, out var usuarioId))
    {
        return Results.Unauthorized();
    }

    var response = useCase.Ejecutar(
        new ModificarTramiteRequest
        {
            TramiteId = id,
            Contenido = request.Contenido,
            UsuarioId = usuarioId
        });

    return Results.Ok(response);
})
.WithTags("Trámites");

app.MapGet("/expedientes/{id:guid}/tramites", [Authorize] (
    Guid id,
    ListarTramitesPorExpedienteUseCase useCase,
    ClaimsPrincipal usuario) =>
{
    var usuarioIdTexto = usuario.FindFirstValue(ClaimTypes.NameIdentifier);

    if (!Guid.TryParse(usuarioIdTexto, out var usuarioId))
    {
        return Results.Unauthorized();
    }

    var response = useCase.Ejecutar(
        new ListarTramitesRequest
        {
            ExpedienteId = id,
            UsuarioId = usuarioId
        });

    return Results.Ok(response);
})
.WithTags("Trámites");

app.MapPost("/tramites", [Authorize] (
    AgregarTramiteUseCase useCase,
    CrearTramiteRequest request,
    ClaimsPrincipal usuario) =>
{
    var usuarioIdTexto = usuario.FindFirstValue(ClaimTypes.NameIdentifier);

    if (!Guid.TryParse(usuarioIdTexto, out var usuarioId))
    {
        return Results.Unauthorized();
    }

    var response = useCase.Ejecutar(
        new AgregarTramiteRequest
        {
            ExpedienteId = request.ExpedienteId,
            Etiqueta = request.Etiqueta,
            Contenido = request.Contenido,
            UsuarioId = usuarioId
        });

    return Results.Created(
        $"/tramites/{response.TramiteId}",
        response);
})
.WithTags("Trámites");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

try
{
    using var scope = app.Services.CreateScope();

    var context = scope.ServiceProvider.GetRequiredService<SgeContext>();
    context.Database.EnsureCreated();

    var repositorioUsuarios =
        scope.ServiceProvider.GetRequiredService<IRepositorioUsuarios>();

    var hashService =
        scope.ServiceProvider.GetRequiredService<IHashService>();

    if (repositorioUsuarios.ObtenerPorCorreo("admin@sge.com") == null)
    {
        var admin = new Usuario(
            "Administrador",
            "admin@sge.com",
            hashService.GenerarHash("admin123")
        );

        admin.ConvertirEnAdministrador();

        repositorioUsuarios.Agregar(admin);
        context.SaveChanges();

        Console.WriteLine("Administrador semilla creado.");
    }
}
catch (Exception ex)
{
    Console.WriteLine("ERROR AL INICIAR: " + ex);
    throw;
}

app.Run();
public record LoginRequest(
    string Correo,
    string Contrasena);

public record CrearExpedienteRequest(string Caratula);
public record ModificarCaratulaRequest(string NuevaCaratula);
public record CambiarEstadoRequest(EstadoExpediente NuevoEstado);
public record CrearTramiteRequest(
    Guid ExpedienteId,
    EtiquetaTramite Etiqueta,
    string? Contenido);
public record ModificarTramiteWebRequest(string Contenido);