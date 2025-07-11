using Org.BouncyCastle.Asn1.Pkcs;
using TASKHIVE.Models;
using System.Security.Cryptography;

namespace TASKHIVE.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {

        }
        //Add Your DbContext here Ex: public DbSet<Budget> Budgets { get; set; }
        public DbSet<DeveloperRate> DeveloperRates { get; set; }
        public DbSet<Budget> Budgets { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Transaction> Transactions { get; set; }


        public DbSet<Models.Task> Tasks { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Developer> Developers { get; set; }

        public DbSet<DeveloperProject> DeveloperProjects { get; set; }
        public DbSet<FileResource> FileResources { get; set; }
        public DbSet<ProjectManager> ProjectManagers { get; set; }

        public DbSet<User> Users { get; set; }
        public DbSet<UserCategory> UsersCategories { get; set; }
        public DbSet<JobRole> JobRoles { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<RefreshTokenClient> RefreshTokenClients { get; set; }


        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<ViewInvoice> ViewInvoices { get; set; }

        public DbSet<ViewReport> ViewReports { get; set; }
        public DbSet<ViewResource> ViewResources { get; set; }
        public DbSet<DeveloperFinancialRecipt> DeveloperFinancialRecipts { get; set; }

        public DbSet<TaskTime> TaskTimes { get; set; }
        public DbSet<Payment> Payments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure Project relationships to prevent multiple cascade paths
            modelBuilder.Entity<Project>()
                .HasOne(p => p.Admin)
                .WithMany(a => a.Projects)
                .HasForeignKey(p => p.AdminId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Project>()
                .HasOne(p => p.Client)
                .WithMany(c => c.Projects)
                .HasForeignKey(p => p.ClientId)
                .OnDelete(DeleteBehavior.Cascade); // Keep cascade for Client

            modelBuilder.Entity<Project>()
                .HasOne(p => p.ProjectManager)
                .WithMany(pm => pm.Projects)
                .HasForeignKey(p => p.ProjectManagerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TaskTime>()
              .HasOne(tt => tt.Task)
              .WithMany(t => t.TaskTimes)
              .HasForeignKey(tt => tt.TaskId)
              .OnDelete(DeleteBehavior.Cascade); // Keep cascade for Tasks

            modelBuilder.Entity<TaskTime>()
                .HasOne(tt => tt.Developer)
                .WithMany(d => d.TaskTimes)
                .HasForeignKey(tt => tt.DeveloperId)
                .OnDelete(DeleteBehavior.NoAction); // Remove cascade for Developer

            base.OnModelCreating(modelBuilder);
        }

    }


}