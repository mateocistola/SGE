namespace SGE.Aplicacion.Autorizacion;

// Esto solo me dice que tiene que existir alguien que sepa responder si un ID tiene o no permisos.
public interface IAutorizacionService
{
    // Indica si el usuario posee permiso true / false.
    bool PoseeElPermiso(Guid idUsuario, Permiso permiso);
}