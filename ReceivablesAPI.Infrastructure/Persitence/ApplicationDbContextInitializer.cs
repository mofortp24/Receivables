using ReceivablesAPI.Domain.Entities;
using ReceivablesAPI.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ReceivablesAPI.Domain.Enums;

namespace ReceivablesAPI.Infrastructure.Persistence;

public class ApplicationDbContextInitializer
{
    private readonly ILogger<ApplicationDbContextInitializer> _logger;
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public ApplicationDbContextInitializer(ILogger<ApplicationDbContextInitializer> logger, ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _logger = logger;
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task InitialiseAsync()
    {
        try
        {
            if (_context.Database.IsSqlServer())
            {
                await _context.Database.MigrateAsync();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while initialising the database.");
            throw;
        }
    }

    public async Task SeedAsync()
    {
        try
        {
            await TrySeedAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    public async Task TrySeedAsync()
    {
        // Default roles
        var administratorRole = new IdentityRole("Administrator");

        if (_roleManager.Roles.All(r => r.Name != administratorRole.Name))
        {
            await _roleManager.CreateAsync(administratorRole);
        }

        // Default users
        var administrator = new ApplicationUser { UserName = "administrator@localhost", Email = "administrator@localhost" };

        if (_userManager.Users.All(u => u.UserName != administrator.UserName))
        {
            await _userManager.CreateAsync(administrator, "Administrator1!");
            if (!string.IsNullOrWhiteSpace(administratorRole.Name))
            {
                await _userManager.AddToRolesAsync(administrator, new [] { administratorRole.Name });
            }
        }

        // Default data
        // Seed, if necessary
        if (!_context.ReceivableBatches.Any())
        {
            _context.ReceivableBatches.Add(new ReceivableBatch()
            {
                BatchReference = "Receivable_20231123_184009_1a28",
                Receivables =
                {
                    new Receivable()
                    {
                        Reference = "RCVBL/01/2023",
                        CurrencyCode = CurrencyCode.PLN,

                        Debtor = new ReceivableDebtor()
                        {
                            DebtorName = "John Kowalsky", 
                            DebtorReference = "DBT_Ref_01",
                        },

                        DebtorAddress = new ReceivableDebtorAddress() { 
                            DebtorAddress1 = "Address 1/23", 
                            DebtorAddress2 = "Address 2/48",
                            DebtorCountryCode = CountryCode.PL, 
                            DebtorRegistrationNumber = "PL 87654", 
                            DebtorState = "Mazowieckie", 
                            DebtorTown = "Warsaw",
                            DebtorZip = "02-333"
                        },
                        
                        OpeningValue = 654.32m,
                        PaidValue = 123.45m,
                        IssueDate = DateTime.Now.AddDays(-2),
                        DueDate = DateTime.Now.AddMonths(1),
                        ClosedDate = null,

                        Created = DateTime.Now,
                        CreatedBy = "Init",
                        LastModified = null,
                        LastModifiedBy = null,
                    },
                    new Receivable()
                    {
                        Reference = "RCVBL/15/2023",
                        CurrencyCode = CurrencyCode.PLN,
                        Debtor = new ReceivableDebtor()
                        {
                            DebtorName = "Eva Kowalsky", 
                            DebtorReference = "DBT_Ref_02",
                            
                        },

                        DebtorAddress = new ReceivableDebtorAddress() { 
                            DebtorAddress1 = "Address 1/23", 
                            DebtorAddress2 = "Address 2/48",
                            DebtorCountryCode = CountryCode.PL, 
                            DebtorRegistrationNumber = null, 
                            DebtorState = "Mazowieckie", 
                            DebtorTown = "Warsaw",
                            DebtorZip = "02-333"
                        },

                        OpeningValue = 333.44m,
                        PaidValue = 111.22m,
                        IssueDate = DateTime.Now.AddDays(-4),
                        DueDate = DateTime.Now.AddMonths(2),
                        ClosedDate = null,

                        Created = DateTime.Now,
                        
                        CreatedBy = "Init",
                        LastModified = null,
                        LastModifiedBy = null,
                    }
                }
            });

            await _context.SaveChangesAsync();
        }
    }
}
