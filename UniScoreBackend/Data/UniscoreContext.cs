using Microsoft.EntityFrameworkCore;
using UniScore.API.Models;

namespace UniScore.API.Data
{
    public class UniscoreContext : DbContext
    {
        public UniscoreContext(DbContextOptions<UniscoreContext> options) : base(options) { }

        // DbSets - Tabelas do banco
        public DbSet<Pessoa> Pessoas { get; set; }
        public DbSet<TipoUsuario> TiposUsuario { get; set; }
        public DbSet<PessoaTipoUsuario> PessoasTiposUsuario { get; set; }
        public DbSet<Evento> Eventos { get; set; }
        public DbSet<CriterioAvaliacao> CriteriosAvaliacao { get; set; }
        public DbSet<Projeto> Projetos { get; set; }
        public DbSet<InformacoesComplementares> InformacoesComplementares { get; set; }
        public DbSet<EventoProjeto> EventosProjetos { get; set; }
        public DbSet<Avaliacao> Avaliacoes { get; set; }
        public DbSet<Nota> Notas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuração da entidade Pessoa
            modelBuilder.Entity<Pessoa>(entity =>
            {
                entity.HasKey(e => e.IdPessoa);
                entity.Property(e => e.NomePessoa).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Usuario).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Senha).IsRequired().HasMaxLength(255);
                entity.HasIndex(e => e.Usuario).IsUnique();
            });

            // Configuração da entidade TipoUsuario
            modelBuilder.Entity<TipoUsuario>(entity =>
            {
                entity.HasKey(e => e.IdTipoUsuario);
                entity.Property(e => e.PapelUsuario).IsRequired().HasMaxLength(50);
            });

            // Configuração da entidade PessoaTipoUsuario
            modelBuilder.Entity<PessoaTipoUsuario>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.Pessoa)
                      .WithMany(p => p.PessoasTiposUsuario)
                      .HasForeignKey(e => e.IdPessoa);
                entity.HasOne(e => e.TipoUsuario)
                      .WithMany(t => t.PessoasTiposUsuario)
                      .HasForeignKey(e => e.IdTipoUsuario);
            });

            // Configuração da entidade Evento
            modelBuilder.Entity<Evento>(entity =>
            {
                entity.HasKey(e => e.IdEvento);
                entity.Property(e => e.NomeEvento).IsRequired().HasMaxLength(100);
                entity.Property(e => e.DescricaoEvento).HasMaxLength(500);
                entity.Property(e => e.Local).HasMaxLength(100);
            });

            // Configuração da entidade CriterioAvaliacao
            modelBuilder.Entity<CriterioAvaliacao>(entity =>
            {
                entity.HasKey(e => e.IdCriterio);
                entity.Property(e => e.NomeCriterio).IsRequired().HasMaxLength(100);
                entity.HasOne(e => e.Evento)
                      .WithMany(ev => ev.CriteriosAvaliacao)
                      .HasForeignKey(e => e.IdEvento);
            });

            // Configuração da entidade Projeto
            modelBuilder.Entity<Projeto>(entity =>
            {
                entity.HasKey(e => e.IdProjeto);
                entity.Property(e => e.NomeProjeto).IsRequired().HasMaxLength(100);
                entity.Property(e => e.DescricaoProjeto).HasMaxLength(500);
                entity.Property(e => e.Integrantes).HasMaxLength(200);
                entity.Property(e => e.Turma).HasMaxLength(50);
            });

            // Configuração da entidade InformacoesComplementares
            modelBuilder.Entity<InformacoesComplementares>(entity =>
            {
                entity.HasKey(e => e.IdInformacao); // CORRIGIDO: IdInformacao em vez de IdComplemento
                entity.Property(e => e.InformacaoComplementar).IsRequired().HasMaxLength(200);
                entity.HasOne(e => e.Projeto)
                      .WithMany(p => p.InformacoesComplementares)
                      .HasForeignKey(e => e.IdProjeto);
            });

            // Configuração da entidade EventoProjeto
            modelBuilder.Entity<EventoProjeto>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.Evento)
                      .WithMany(ev => ev.EventosProjetos)
                      .HasForeignKey(e => e.IdEvento);
                entity.HasOne(e => e.Projeto)
                      .WithMany(p => p.EventosProjetos)
                      .HasForeignKey(e => e.IdProjeto);
            });

            // Configuração da entidade Avaliacao
            modelBuilder.Entity<Avaliacao>(entity =>
            {
                entity.HasKey(e => e.IdAvaliacao);
                entity.HasOne(e => e.Pessoa)
                      .WithMany(p => p.Avaliacoes)
                      .HasForeignKey(e => e.IdPessoa);
                entity.HasOne(e => e.Projeto)
                      .WithMany(p => p.Avaliacoes)
                      .HasForeignKey(e => e.IdProjeto);
            });

            // Configuração da entidade Nota
            modelBuilder.Entity<Nota>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.CriterioAvaliacao)
                      .WithMany(c => c.Notas)
                      .HasForeignKey(e => e.IdCriterio);
                entity.HasOne(e => e.Avaliacao)
                      .WithMany(a => a.Notas)
                      .HasForeignKey(e => e.IdAvaliacao);
            });

            // Dados iniciais
            modelBuilder.Entity<TipoUsuario>().HasData(
                new TipoUsuario { IdTipoUsuario = 1, PapelUsuario = "Coordenador" },
                new TipoUsuario { IdTipoUsuario = 2, PapelUsuario = "Avaliador" },
                new TipoUsuario { IdTipoUsuario = 3, PapelUsuario = "ProfessorAdmin" }
            );
        }
    }
}