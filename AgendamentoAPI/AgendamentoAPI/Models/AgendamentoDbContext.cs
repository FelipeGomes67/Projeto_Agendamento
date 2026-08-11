using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace AgendamentoAPI.Models;

public partial class AgendamentoDbContext : DbContext
{
    public AgendamentoDbContext()
    {
    }

    public AgendamentoDbContext(DbContextOptions<AgendamentoDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Agendamento> Agendamentos { get; set; }

    public virtual DbSet<Cliente> Clientes { get; set; }

    public virtual DbSet<Profissional> Profissionals { get; set; }

    public virtual DbSet<Servico> Servicos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=AgendamentoDB;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Agendamento>(entity =>
        {
            entity.HasKey(e => e.IdAgendamento).HasName("PK__Agendame__DC0823C9707C09CB");

            entity.ToTable("Agendamento");

            entity.Property(e => e.IdAgendamento).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("Agendado");

            entity.HasOne(d => d.IdClienteNavigation).WithMany(p => p.Agendamentos)
                .HasForeignKey(d => d.IdCliente)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Agendamento_Cliente");

            entity.HasOne(d => d.IdProfissionalNavigation).WithMany(p => p.Agendamentos)
                .HasForeignKey(d => d.IdProfissional)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Agendamento_Profissional");

            entity.HasOne(d => d.IdServicoNavigation).WithMany(p => p.Agendamentos)
                .HasForeignKey(d => d.IdServico)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Agendamento_Servico");
        });

        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.HasKey(e => e.IdCliente).HasName("PK__Cliente__D594664237A5A952");

            entity.ToTable("Cliente");

            entity.Property(e => e.IdCliente).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Nome)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Telefone)
                .HasMaxLength(15)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Profissional>(entity =>
        {
            entity.HasKey(e => e.IdProfissional).HasName("PK__Profissi__B9503FBC500B6D9C");

            entity.ToTable("Profissional");

            entity.Property(e => e.IdProfissional).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Disponivel).HasDefaultValue(true);
            entity.Property(e => e.Nome)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Preco).HasColumnType("decimal(10, 2)");
        });

        modelBuilder.Entity<Servico>(entity =>
        {
            entity.HasKey(e => e.IdServico).HasName("PK__Servico__474DDE3AEA3ACFDF");

            entity.ToTable("Servico");

            entity.Property(e => e.IdServico).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Descricao).IsUnicode(false);
            entity.Property(e => e.Nome)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
