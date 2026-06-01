using Microsoft.EntityFrameworkCore;
using EcoCredit.API.Models;

namespace EcoCredit.API.Data;

public class AppDbContext : DbContext {
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Company> Companies { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Facility> Facilities { get; set; }
    public DbSet<EmissionRecord> EmissionRecords { get; set; }
    public DbSet<CarbonCredit> CarbonCredits { get; set; }
    public DbSet<Alert> Alerts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        modelBuilder.Entity<Company>(e => {
            e.HasKey(x => x.CompanyId);
            e.HasMany(x => x.Users).WithOne(x => x.Company).HasForeignKey(x => x.CompanyId);
            e.HasMany(x => x.Facilities).WithOne(x => x.Company).HasForeignKey(x => x.CompanyId);
            e.HasMany(x => x.Credits).WithOne(x => x.Company).HasForeignKey(x => x.CompanyId);
        });

        modelBuilder.Entity<User>(e => {
            e.HasKey(x => x.UserId);
            e.HasIndex(x => x.Email).IsUnique();
        });

        modelBuilder.Entity<Facility>(e => {
            e.HasKey(x => x.FacilityId);
            e.HasMany(x => x.Emissions).WithOne(x => x.Facility).HasForeignKey(x => x.FacilityId);
            e.HasMany(x => x.Alerts).WithOne(x => x.Facility).HasForeignKey(x => x.FacilityId);
        });

        modelBuilder.Entity<EmissionRecord>(e => {
            e.HasKey(x => x.RecordId);
        });

        modelBuilder.Entity<CarbonCredit>(e => {
            e.HasKey(x => x.CreditId);
        });

        modelBuilder.Entity<Alert>(e => {
            e.HasKey(x => x.AlertId);
        });

        // Seed data for demo
        var companyId = "comp-001";
        var company2Id = "comp-002";
        var facilityId = "fac-001";
        var facility2Id = "fac-002";

        modelBuilder.Entity<Company>().HasData(
            new Company { CompanyId = companyId, Name = "Petro Energia S.A.", Cnpj = "12.345.678/0001-90", Sector = "OIL_GAS" },
            new Company { CompanyId = company2Id, Name = "AgroVerde Ltda.", Cnpj = "98.765.432/0001-10", Sector = "AGRO" }
        );

        modelBuilder.Entity<User>().HasData(
            new User {
                UserId = "user-001",
                CompanyId = companyId,
                Email = "admin@petro.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@2026", workFactor: 12),
                Role = "ADMIN"
            },
            new User {
                UserId = "user-002",
                CompanyId = companyId,
                Email = "analista@petro.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Analyst@2026", workFactor: 12),
                Role = "ANALYST"
            }
        );

        modelBuilder.Entity<Facility>().HasData(
            new Facility { FacilityId = facilityId, CompanyId = companyId, Name = "Refinaria RJ", FacilityType = "REFINERY", Latitude = -22.9068m, Longitude = -43.1729m, EmissionLimitTco2 = 5000m },
            new Facility { FacilityId = facility2Id, CompanyId = companyId, Name = "Plataforma P-51", FacilityType = "RIG", Latitude = -23.5505m, Longitude = -43.1729m, EmissionLimitTco2 = 2000m }
        );

        modelBuilder.Entity<CarbonCredit>().HasData(
            new CarbonCredit { CreditId = "credit-001", CompanyId = companyId, QuantityTco2 = 2000m, CreditType = "CBIO", PriceUsd = 15.50m, ExpiryDate = new DateTime(2026, 12, 31), RegistryCode = "CBIO-2024-001234" },
            new CarbonCredit { CreditId = "credit-002", CompanyId = companyId, QuantityTco2 = 1500m, CreditType = "REDD", PriceUsd = 22.00m, ExpiryDate = new DateTime(2027, 6, 30), RegistryCode = "REDD+-BRA-0089" }
        );
    }
}
