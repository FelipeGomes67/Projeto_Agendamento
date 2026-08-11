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

    public virtual DbSet<Profissional> Profissionais { get; set; }

    public virtual DbSet<Servico> Servicos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
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

            entity.HasIndex(e => e.Email, "UQ_Cliente_Email").IsUnique();

            entity.Property(e => e.IdCliente).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Email)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasDefaultValue("", "DF_Cliente_Email");
            entity.Property(e => e.Nome)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Senha)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasDefaultValue("", "DF_Cliente_Senha");
            entity.Property(e => e.Telefone)
                .HasMaxLength(15)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Profissional>(entity =>
        {
            entity.HasKey(e => e.IdProfissional).HasName("PK__Profissi__B9503FBC500B6D9C");

            entity.ToTable("Profissional");

            entity.HasIndex(e => e.Email, "UQ_Profissional_Email").IsUnique();

            entity.Property(e => e.IdProfissional).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Disponivel).HasDefaultValue(true);
            entity.Property(e => e.Email)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasDefaultValue("", "DF_Profissional_Email");
            entity.Property(e => e.Nome)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Senha)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasDefaultValue("", "DF_Profissional_Senha");
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
            entity.Property(e => e.Preco).HasColumnType("decimal(10, 2)");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
