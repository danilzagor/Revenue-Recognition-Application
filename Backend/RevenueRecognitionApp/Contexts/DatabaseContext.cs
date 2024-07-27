using Microsoft.EntityFrameworkCore;
using RevenueRecognition.Models;

namespace RevenueRecognition.Contexts;

public class DatabaseContext : DbContext
{

    public DbSet<Client> Clients { get; set; }
    public DbSet<PhysicalClient> PhysicalClients { get; set; }
    public DbSet<CompanyClient> CompanyClients { get; set; }
    public DbSet<Software> Software { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<SoftwareCategories> SoftwareCategories { get; set; }
    public DbSet<SoftwareSales> SoftwareSales { get; set; }
    public DbSet<Sale> Sales { get; set; }
    public DbSet<SoftwareVersion> SoftwareVersions { get; set; }
    public DbSet<Contract> Contracts { get; set; }
    public DbSet<ContractStatus> ContractStatuses { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<User> Users { get; set; }

    
    protected DatabaseContext()
    {
    }

    public DatabaseContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ContractStatus>().HasData(new ContractStatus
        {
            Id=1,
            Name = "Waiting for a payment"
        });
        modelBuilder.Entity<ContractStatus>().HasData(new ContractStatus
        {
            Id=2,
            Name = "Signed"
        });
        modelBuilder.Entity<ContractStatus>().HasData(new ContractStatus
        {
            Id=3,
            Name = "Outdated"
        });
        
    }
}