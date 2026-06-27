using Microsoft.EntityFrameworkCore;
using SGE.Dominio.Expedientes;
using SGE.Dominio.Tramites;
using SGE.Dominio.Usuarios;

namespace SGE.Infraestructura.Persistencia;

public class SgeContext : DbContext
{
    public SgeContext(DbContextOptions<SgeContext> options)
        : base(options)
    {
    }

    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<Expediente> Expedientes => Set<Expediente>();
    public DbSet<Tramite> Tramites => Set<Tramite>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Expediente>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.ComplexProperty(x => x.Caratula, caratula =>
            {
                caratula.Property(c => c.Valor)
                    .HasColumnName("Caratula")
                    .IsRequired();
            });
        });

        modelBuilder.Entity<Tramite>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.ComplexProperty(x => x.Contenido, contenido =>
            {
                contenido.Property(c => c.Valor)
                    .HasColumnName("Contenido")
                    .IsRequired();
            });

            entity.Property(x => x.ExpedienteId)
                .IsRequired();
        });

        modelBuilder.Entity<PermisoUsuario>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Permiso)
                .HasConversion<int>();
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.HasIndex(x => x.CorreoElectronico)
                .IsUnique();
                
            entity.Property(x => x.Nombre)
                .IsRequired();

            entity.Property(x => x.CorreoElectronico)
                .IsRequired();

            entity.Property(x => x.ContrasenaHash)
                .IsRequired();

            entity.HasMany(x => x.Permisos)
                .WithOne()
                .HasForeignKey("UsuarioId")
                .OnDelete(DeleteBehavior.Cascade);

            entity.Property(x => x.EsAdministrador)
                .IsRequired();
        });
    }
}