using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using GestoreAbbonamenti.Model;
using GestoreAbbonamenti.Common.Bo;
using System.IO;
using GestiSoGestoreAbbonamentift.Common.Enum;
using GestoreAbbonamenti.Common.Enum;

namespace GestoreAbbonamenti.Data.Database
{
    public class DbEntities : DbContext
    {
        IConfiguration? _config;
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            try
            {
                _config ??= new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .Build();

                //var connectionString = _config.GetConnectionString("DefaultConnection");
                var connectionString = _config.GetRequiredSection("Configurations").Get<ConfigurationsBo>()!;

                optionsBuilder.UseSqlServer(connectionString.ConnectionString, sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 5,
                        maxRetryDelay: TimeSpan.FromSeconds(2),
                        errorNumbersToAdd: null
                    );
                });
            }
            catch (Exception ex)
            {
                throw new Exception("OnConfiguring: " + ex.Message, ex);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Genitori>()
                .HasIndex(b => new { b.CodiceFiscale })
                .IsUnique();

            modelBuilder.Entity<Genitori>()
                .HasMany(g => g.Figli)
                .WithOne(f => f.Genitore)
                .HasForeignKey(f => f.GenitoreId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Figlii>()
                .HasMany(f => f.CedoleMensili)
                .WithOne(c => c.Figlio)
                .HasForeignKey(c => c.FiglioId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Figlii>()
                .HasOne(f => f.Scuola)
                .WithMany(s => s.Figli)
                .HasForeignKey(f => f.IdScuola)
                .OnDelete(DeleteBehavior.Cascade);

        }

        #region Models
        public DbSet<Genitori> Genitori { get; set; }
        public DbSet<Figlii> Figli { get; set; }
        public DbSet<CedoleMensili> CedoleMensili { get; set; }
        public DbSet<Scuole> Scuole { get; set; }
        public DbSet<Comuni> Comuni { get; set; }
        public DbSet<Istituti> Istituti { get; set; }
        public DbSet<ComuniItaliani> ComuniItaliani { get; set; }
        #endregion
    }
}
