using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace waGimnasio.Models;

public partial class GimnasioContext : DbContext
{
    public GimnasioContext()
    {
    }

    public GimnasioContext(DbContextOptions<GimnasioContext> options)
        : base(options)
    {
    }
    public virtual DbSet<Cliente> Clientes { get; set; }

    public virtual DbSet<Departamento> Departamentos { get; set; }

    public virtual DbSet<Gimnasio> Gimnasios { get; set; }

    public virtual DbSet<Persona> Personas { get; set; }

    public virtual DbSet<PlanOfertado> PlanOfertados { get; set; }

    public virtual DbSet<Promocion> Promocions { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
//        => optionsBuilder.UseSqlServer("Server=LAPTOP-4ATTP8GE;Database=gimnasio;Trusted_Connection=True; TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__cliente__3213E83F153EC89A");

            entity.ToTable("cliente");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.CodigoPlan).HasColumnName("codigo_plan");
            entity.Property(e => e.CodigoPromocion).HasColumnName("codigo_promocion");
            entity.Property(e => e.FechaFin)
                .HasColumnType("date")
                .HasColumnName("fecha_fin");
            entity.Property(e => e.FechaIni)
                .HasColumnType("date")
                .HasColumnName("fecha_ini");
            entity.Property(e => e.NumBoleta).HasColumnName("num_boleta");
            entity.Property(e => e.Ocupacion)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ocupacion");
            entity.Property(e => e.TelefonoEmergencia).HasColumnName("telefono_emergencia");

            entity.HasOne(d => d.CodigoPlanNavigation).WithMany(p => p.Clientes)
                .HasForeignKey(d => d.CodigoPlan)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__cliente__codigo___45F365D3");

            entity.HasOne(d => d.CodigoPromocionNavigation).WithMany(p => p.Clientes)
                .HasForeignKey(d => d.CodigoPromocion)
                .HasConstraintName("FK__cliente__codigo___46E78A0C");

            entity.HasOne(d => d.IdNavigation).WithOne(p => p.Cliente)
                .HasForeignKey<Cliente>(d => d.Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__cliente__id__44FF419A");
        });

        modelBuilder.Entity<Departamento>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__departam__3213E83F0CDD780F");

            entity.ToTable("departamento");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<Gimnasio>(entity =>
        {
            entity.HasKey(e => e.Codigo).HasName("PK__gimnasio__40F9A20729456D94");

            entity.ToTable("gimnasio");

            entity.Property(e => e.Codigo).HasColumnName("codigo");
            entity.Property(e => e.Direccion)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("direccion");
            entity.Property(e => e.Estado)
                .IsRequired()
                .HasDefaultValueSql("((1))")
                .HasColumnName("estado");
            entity.Property(e => e.IdDepartamento).HasColumnName("id_departamento");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.Telefono).HasColumnName("telefono");

            entity.HasOne(d => d.IdDepartamentoNavigation).WithMany(p => p.Gimnasios)
                .HasForeignKey(d => d.IdDepartamento)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__gimnasio__id_dep__3A81B327");
        });

        modelBuilder.Entity<Persona>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__persona__3213E83F1AA72EA1");

            entity.ToTable("persona");

            entity.HasIndex(e => e.Ci, "UQ__persona__32136662F9F871E7").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Ci)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("ci");
            entity.Property(e => e.CodigoGimnasio).HasColumnName("codigo_gimnasio");
            entity.Property(e => e.Direccion)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("direccion");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.FechaNacimiento)
                .HasColumnType("date")
                .HasColumnName("fecha_nacimiento");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.Sexo)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("sexo");
            entity.Property(e => e.Telefono).HasColumnName("telefono");

            entity.HasOne(d => d.CodigoGimnasioNavigation).WithMany(p => p.Personas)
                .HasForeignKey(d => d.CodigoGimnasio)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__persona__codigo___4222D4EF");
        });

        modelBuilder.Entity<PlanOfertado>(entity =>
        {
            entity.HasKey(e => e.Codigo).HasName("PK__plan_ofe__40F9A2075FE89F93");

            entity.ToTable("plan_ofertado");

            entity.Property(e => e.Codigo).HasColumnName("codigo");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("descripcion");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.Precio).HasColumnName("precio");
        });

        modelBuilder.Entity<Promocion>(entity =>
        {
            entity.HasKey(e => e.Codigo).HasName("PK__promocio__40F9A2073E1EA6BF");

            entity.ToTable("promocion");

            entity.Property(e => e.Codigo).HasColumnName("codigo");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("descripcion");
            entity.Property(e => e.Descuento).HasColumnName("descuento");
            entity.Property(e => e.FechaFin)
                .HasColumnType("date")
                .HasColumnName("fecha_fin");
            entity.Property(e => e.FechaIni)
                .HasColumnType("date")
                .HasColumnName("fecha_ini");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__usuario__3213E83FDC5F278D");

            entity.ToTable("usuario");

            entity.HasIndex(e => e.Email, "UQ__usuario__AB6E616437E9FBD4").IsUnique();

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Email)
                .HasMaxLength(70)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.Estado)
                .IsRequired()
                .HasDefaultValueSql("((1))")
                .HasColumnName("estado");
            entity.Property(e => e.Password)
                .HasMaxLength(32)
                .HasColumnName("password");

            entity.HasOne(d => d.IdNavigation).WithOne(p => p.Usuario)
                .HasForeignKey<Usuario>(d => d.Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__usuario__id__4BAC3F29");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
