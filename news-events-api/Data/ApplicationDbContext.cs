using Microsoft.EntityFrameworkCore;
using ProjectEvaluationApi.Models;

namespace ProjectEvaluationApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        
        public DbSet<Pessoa> Pessoas { get; set; }
        public DbSet<TipoUsuario> TiposUsuario { get; set; }
        public DbSet<PessoaTipoUsuario> PessoaTipoUsuarios { get; set; }
        public DbSet<Evento> Eventos { get; set; }
        public DbSet<Projeto> Projetos { get; set; }
        public DbSet<InformacoesComplementares> InformacoesComplementares { get; set; }
        public DbSet<Avaliacao> Avaliacoes { get; set; }
        public DbSet<CriterioAvaliacao> CriteriosAvaliacao { get; set; }
        public DbSet<Nota> Notas { get; set; }
        public DbSet<EventoProjeto> EventoProjetos { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // Configure composite keys
            modelBuilder.Entity<PessoaTipoUsuario>()
                .HasKey(ptu => new { ptu.Id, ptu.IdPessoa, ptu.IdTipoUsuario });
                
            modelBuilder.Entity<Nota>()
                .HasKey(n => new { n.Id, n.IdCriterio, n.IdAvaliacao });
                
            modelBuilder.Entity<EventoProjeto>()
                .HasKey(ep => new { ep.Id, ep.IdEvento, ep.IdProjeto });
            
            // Configure relationships
            modelBuilder.Entity<PessoaTipoUsuario>()
                .HasOne(ptu => ptu.Pessoa)
                .WithMany(p => p.PessoaTipoUsuarios)
                .HasForeignKey(ptu => ptu.IdPessoa);
                
            modelBuilder.Entity<PessoaTipoUsuario>()
                .HasOne(ptu => ptu.TipoUsuario)
                .WithMany(tu => tu.PessoaTipoUsuarios)
                .HasForeignKey(ptu => ptu.IdTipoUsuario);
                
            modelBuilder.Entity<InformacoesComplementares>()
                .HasOne(ic => ic.Projeto)
                .WithMany(p => p.InformacoesComplementares)
                .HasForeignKey(ic => ic.IdProjeto);
                
            modelBuilder.Entity<CriterioAvaliacao>()
                .HasOne(ca => ca.Evento)
                .WithMany(e => e.CriteriosAvaliacao)
                .HasForeignKey(ca => ca.IdEvento);
                
            modelBuilder.Entity<Avaliacao>()
                .HasOne(a => a.Pessoa)
                .WithMany(p => p.Avaliacoes)
                .HasForeignKey(a => a.IdPessoa);
                
            modelBuilder.Entity<Avaliacao>()
                .HasOne(a => a.Projeto)
                .WithMany(p => p.Avaliacoes)
                .HasForeignKey(a => a.IdProjeto);
                
            modelBuilder.Entity<Nota>()
                .HasOne(n => n.CriterioAvaliacao)
                .WithMany(ca => ca.Notas)
                .HasForeignKey(n => n.IdCriterio);
                
            modelBuilder.Entity<Nota>()
                .HasOne(n => n.Avaliacao)
                .WithMany(a => a.Notas)
                .HasForeignKey(n => n.IdAvaliacao);
                
            modelBuilder.Entity<EventoProjeto>()
                .HasOne(ep => ep.Evento)
                .WithMany(e => e.EventoProjetos)
                .HasForeignKey(ep => ep.IdEvento);
                
            modelBuilder.Entity<EventoProjeto>()
                .HasOne(ep => ep.Projeto)
                .WithMany(p => p.EventoProjetos)
                .HasForeignKey(ep => ep.IdProjeto);
        }
    }
}
