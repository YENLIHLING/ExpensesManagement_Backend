using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ModelLayer;

namespace DataLayer
{
    public class ExpensesManagementContext : DbContext 
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

            var connStr = configuration.GetConnectionString("DefaultConnection"); 
            optionsBuilder.UseMySql(connStr, ServerVersion.AutoDetect(connStr)); 
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PersonalModel>() 
                .HasMany(p => p.incomes) 
                .WithOne(i => i. personal)    
                .HasForeignKey(i => i.personalId);

            modelBuilder.Entity<PersonalModel>()
                .HasMany(p => p.expenses)
                .WithOne(e => e.personal)
                .HasForeignKey(e => e.personalId);
        }

        public DbSet<PersonalModel> personal { get; set; }
        public DbSet<IncomeModel> income { get; set; }
        public DbSet<ExpenseModel> expense { get; set; }

    }
}