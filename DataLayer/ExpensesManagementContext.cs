using Microsoft.EntityFrameworkCore;
using ModelLayer;
using System.Threading.Tasks;

namespace DataLayer
{
    public class ExpensesManagementContext : DbContext 
    {
        private const string MYCONN = "server=127.0.0.1; port=3333; uid=root; pwd=root; database=ExpensesManagement;";

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var mySQVersion = new MySqlServerVersion(new Version(10, 4, 17));
            optionsBuilder.UseMySql(MYCONN, mySQVersion); 
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