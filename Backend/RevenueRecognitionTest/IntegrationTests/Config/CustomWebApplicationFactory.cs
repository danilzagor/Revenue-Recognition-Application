using System.Data.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RevenueRecognition.Contexts;
using RevenueRecognition.Models;

namespace RevenueRecognitionTest.IntegrationTests.Config;

public class CustomWebApplicationFactory<TProgram>
    : WebApplicationFactory<TProgram> where TProgram : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Test");
        builder.ConfigureServices(services =>
        {

            var dbContextDescriptor = services.SingleOrDefault(
                d => d.ServiceType ==
                     typeof(DbContextOptions<DatabaseContext>));

            services.Remove(dbContextDescriptor);

            var dbConnectionDescriptor = services.SingleOrDefault(
                d => d.ServiceType ==
                     typeof(DbConnection));

            services.Remove(dbConnectionDescriptor);

            services.AddDbContext<DatabaseContext>(options =>
            {
                options.UseInMemoryDatabase("InMemoryDbForTesting");
            });


            var serviceProvider = services.BuildServiceProvider();
            using (var scope = serviceProvider.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<DatabaseContext>();
                var logger = scopedServices.GetRequiredService<ILogger<CustomWebApplicationFactory<Program>>>();
                
                db.Database.EnsureCreated();
                // InitializeDbForTests(db);
                try
                {
                    InitializeDbForTests(db);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred seeding the database with test data. Error: {Message}", ex.Message);
                }
            }
            
        });
    }
    public DatabaseContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<DatabaseContext>()
            .UseInMemoryDatabase("InMemoryDbForTesting")
            .Options;

        return new DatabaseContext(options);
    }
    
    public void ResetDatabase()
    {
        using (var scope = Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            InitializeDbForTests(context);
        }
    }
    
    private void InitializeDbForTests(DatabaseContext db)
    {
        
        var software1 = new Software
        {
            Id = 1,
            Name = "Accounting Software",
            Description = "Software for managing accounts",
            Price = 299.99M
        };

        var software2 = new Software
        {
            Id = 2,
            Name = "Inventory Software",
            Description = "Software for managing inventory",
            Price = 199.99M
        };

        // // Creating instances of Categories
        var category1 = new Category
        {
            Id = 1,
            Name = "Financial"
        };
        
        var category2 = new Category
        {
            Id = 2,
            Name = "Management"
        };
        
        // // Creating instances of Sales
        var sale1 = new Sale
        {
            Id = 1,
            Name = "Spring Sale",
            StartAt = new DateOnly(2024, 3, 1),
            EndAt = new DateOnly(2024, 3, 31)
        };
        
        var sale2 = new Sale
        {
            Id = 2,
            Name = "Summer Sale",
            StartAt = new DateOnly(2024, 6, 1),
            EndAt = new DateOnly(2024, 6, 30)
        };
        
        // // Creating instances of Clients
        var client1 = new Client
        {
            Id = 1,
            Address = "123 Main St",
            Email = "client1@example.com",
            PhoneNumber = "123-456-7890",
            DeletedAt = null
        };
        
        var client2 = new Client
        {
            Id = 2,
            Address = "456 Elm St",
            Email = "client2@example.com",
            PhoneNumber = "987-654-3210",
            DeletedAt = null
        };
        var client3 = new Client
        {
            Id = 3,
            Address = "34534",
            Email = "c234.com",
            PhoneNumber = "9841340",
            DeletedAt = DateTime.Now
        };
        
        // Creating instances of CompanyClients
        var companyClient1 = new CompanyClient
        {
            Id = 1,
            Name = "ABC Corp",
            KRS = "1234567890"
        };
        
        
        var physicalClient2 = new PhysicalClient
        {
            Id = 2,
            FirstName = "Jane",
            LastName = "Smith",
            PESEL = "10987654321"
        };
        var physicalClient3 = new PhysicalClient
        {
            Id = 3,
            FirstName = "asd",
            LastName = "ewrtert",
            PESEL = "124343"
        };
        
        var contract1 = new Contract
        {
            Id = 1,
            BeginningDate = new DateOnly(2024, 1, 1),
            EndingDate = new DateOnly(2024, 1, 25),
            StatusId = 2,
            Price = 999.99M,
            ActualisationPeriod = 2,
            SoftwareAndVersionId = 1,
            ClientId = 1,
        };
        
        var contract2 = new Contract
        {
            Id = 2,
            BeginningDate = new DateOnly(2024, 2, 1),
            EndingDate = new DateOnly(2024, 11, 30),
            StatusId = 1,
            Price = 799.99M,
            ActualisationPeriod = 2,
            SoftwareAndVersionId = 1,
            ClientId = 2
        };
        
        
        
        
        // // Creating instances of SoftwareVersions
        var version1 = new SoftwareVersion
        {
            Id = 1,
            Name = "v1.0",
            SoftwareId = 1
        };
        
        var version2 = new SoftwareVersion
        {
            Id = 2,
            Name = "v2.0",
            SoftwareId = 2
        };
        
        // // Creating instances of Software_Sales
        var softwareSale1 = new SoftwareSales
        {
            SoftwareId = 1,
            SaleId = 1,
            Value = 249.99M
        };
        
        var softwareSale2 = new SoftwareSales
        {
            SoftwareId = 2,
            SaleId = 2,
            Value = 179.99M
        };
        
        // // Creating instances of Software_Categories
        var softwareCategory1 = new SoftwareCategories
        {
            SoftwareId = 1,
            CategoryId = 1
        };
        
        var softwareCategory2 = new SoftwareCategories
        {
            SoftwareId = 2,
            CategoryId = 2
        };

        var user1 = new User
        {
            Id = 1,
            Login = "123",
            Password = "123",
            RefreshToken = "123",
            RefreshTokenExp = DateTime.UtcNow.AddDays(14),
            Roles = ["user"],
            Salt = "123"
        };
        db.Sales.AddRange(sale1, sale2);
        db.Clients.AddRange(client1, client2, client3);
        db.PhysicalClients.AddRange(physicalClient2, physicalClient3);
        db.CompanyClients.Add(companyClient1);
        db.Contracts.AddRange(contract1, contract2);
        db.Software.AddRange(software1, software2);
        db.Categories.AddRange(category1, category2);
        db.SoftwareCategories.AddRange(softwareCategory1, softwareCategory2);
        db.SoftwareSales.AddRange(softwareSale1, softwareSale2);
        db.SoftwareVersions.AddRange(version1, version2);
        db.Users.Add(user1);
        
        
        db.SaveChanges();
    }
    
}