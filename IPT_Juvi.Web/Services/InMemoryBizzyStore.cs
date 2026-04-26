using System.Security.Cryptography;
using System.Text;
using IPT_Juvi.Web.Models.Bizzy;
using IPT_Juvi.Web.Models.ViewModels;

namespace IPT_Juvi.Web.Services;

public sealed class InMemoryBizzyStore
{
    private readonly object _gate = new();

    private readonly EnterpriseProfile _enterprise = new()
    {
        EnterpriseName = "Magic Powder",
        EnterpriseType = "Food",
        Phone = "09561234567",
        Email = "PabloEscubz@gmail.com",
        Role = "Innovation Strategist",
        PhotoPath = "/img/avatar-default.svg",
        PaymentQrPath = null,
    };

    private readonly ManagerProfile _manager = new()
    {
        Name = "Pablo Escobar",
        Section = "SBENT-3D",
        StudentId = "24-6769",
        ContactNo = "09561234567",
        Email = "PabloEscubz@gmail.com",
    };

    private readonly List<OrderRecord> _orders = new();
    private readonly List<TransactionRecord> _transactions = new();
    private readonly List<SalesTransactionRow> _salesTransactions = new();

    private decimal _walletBalance = 50_000m;
    private int _nextTxId = 1;

    private string _passwordSaltB64;
    private string _passwordHashB64;

    public InMemoryBizzyStore()
    {
        _orders.AddRange(
            Enumerable.Range(1, 120).Select(i => new OrderRecord
            {
                Id = i,
                CustomerName = $"Customer {i}",
                OrderedAt = DateTimeOffset.UtcNow.AddDays(-i),
                Total = 120 + i,
                Status = "Completed",
            }));

        _transactions.Add(new TransactionRecord
        {
            Id = _nextTxId++,
            CreatedAt = DateTimeOffset.UtcNow.AddDays(-2),
            Type = "Sale",
            Description = "Total sales payout",
            Amount = 50_000m,
        });

        _salesTransactions.AddRange(new[]
        {
            new SalesTransactionRow
            {
                Id = 1,
                UserName = "Rick Grimes",
                ProductSummary = "2x Pork Sisig Rice, 1x Mountain Dew",
                Date = new DateOnly(2026, 4, 25),
                Total = 700m,
            },
            new SalesTransactionRow
            {
                Id = 2,
                UserName = "Boy Abunda",
                ProductSummary = "1x Caramel Macchiato, 1x Tuna Sandwich",
                Date = new DateOnly(2026, 4, 22),
                Total = 1000m,
            },
            new SalesTransactionRow
            {
                Id = 3,
                UserName = "Carl Poppa",
                ProductSummary = "4pcs Pork Siomai, 1x Gulaman",
                Date = new DateOnly(2026, 4, 15),
                Total = 300m,
            },
            new SalesTransactionRow
            {
                Id = 4,
                UserName = "Raja Kulambu",
                ProductSummary = "1x Beef Pares w/ Rice",
                Date = new DateOnly(2026, 3, 29),
                Total = 500m,
            },
        });

        (_passwordSaltB64, _passwordHashB64) = HashPassword("password");
    }

    public ProfileDashboardViewModel GetDashboard()
    {
        lock (_gate)
        {
            return new ProfileDashboardViewModel
            {
                EnterpriseName = _enterprise.EnterpriseName,
                EnterpriseType = _enterprise.EnterpriseType,
                Phone = _enterprise.Phone,
                Role = _enterprise.Role,
                ManagerName = _manager.Name,
                ManagerSection = _manager.Section,
                ManagerStudentId = _manager.StudentId,
                OrdersCompleted = _orders.Count(o => string.Equals(o.Status, "Completed", StringComparison.OrdinalIgnoreCase)),
                ProductsListed = 35,
                TotalSales = 50_000m,
                WalletBalance = _walletBalance,
                PhotoPath = _enterprise.PhotoPath,
                PaymentQrPath = _enterprise.PaymentQrPath,
            };
        }
    }

    public ProfileSettingsViewModel GetSettings()
    {
        lock (_gate)
        {
            return new ProfileSettingsViewModel
            {
                PhotoPath = _enterprise.PhotoPath,
                PaymentQrPath = _enterprise.PaymentQrPath,
                EnterpriseName = _enterprise.EnterpriseName,
                EnterpriseType = _enterprise.EnterpriseType,
                Contact = _enterprise.Phone,
                Email = _enterprise.Email,
                ManagerName = _manager.Name,
                StudentId = _manager.StudentId,
                Section = _manager.Section,
                ManagerContact = _manager.ContactNo,
                ManagerEmail = _manager.Email,
            };
        }
    }

    public void SaveSettings(ProfileSettingsViewModel model)
    {
        lock (_gate)
        {
            _enterprise.EnterpriseName = model.EnterpriseName.Trim();
            _enterprise.EnterpriseType = model.EnterpriseType.Trim();
            _enterprise.Phone = model.Contact.Trim();
            _enterprise.Email = model.Email.Trim();

            _manager.Name = model.ManagerName.Trim();
            _manager.StudentId = model.StudentId.Trim();
            _manager.Section = model.Section.Trim();
            _manager.ContactNo = model.ManagerContact.Trim();
            _manager.Email = model.ManagerEmail.Trim();
        }
    }

    public void UpdatePhotoPath(string photoPath)
    {
        lock (_gate)
        {
            _enterprise.PhotoPath = photoPath;
        }
    }

    public void UpdatePaymentQrPath(string qrPath)
    {
        lock (_gate)
        {
            _enterprise.PaymentQrPath = qrPath;
        }
    }

    public WalletViewModel GetWallet()
    {
        lock (_gate)
        {
            return new WalletViewModel { WalletBalance = _walletBalance };
        }
    }

    public TransactionsViewModel GetTransactions(string? query)
    {
        lock (_gate)
        {
            var q = query?.Trim();
            IEnumerable<SalesTransactionRow> rows = _salesTransactions;
            if (!string.IsNullOrWhiteSpace(q))
            {
                rows = rows.Where(r =>
                    r.UserName.Contains(q, StringComparison.OrdinalIgnoreCase) ||
                    r.ProductSummary.Contains(q, StringComparison.OrdinalIgnoreCase));
            }

            return new TransactionsViewModel
            {
                Query = q,
                Rows = rows
                    .OrderByDescending(r => r.Date)
                    .ToArray(),
            };
        }
    }

    public OrdersViewModel GetOrders()
    {
        lock (_gate)
        {
            return new OrdersViewModel { Orders = _orders.OrderByDescending(o => o.OrderedAt).Take(25).ToArray() };
        }
    }

    public void AddFunds(decimal amount)
    {
        if (amount <= 0) return;

        lock (_gate)
        {
            _walletBalance += amount;
            _transactions.Add(new TransactionRecord
            {
                Id = _nextTxId++,
                CreatedAt = DateTimeOffset.UtcNow,
                Type = "Add Funds",
                Description = "Wallet top-up",
                Amount = amount,
            });
        }
    }

    public bool ChangePassword(string currentPassword, string newPassword)
    {
        lock (_gate)
        {
            if (!VerifyPassword(currentPassword, _passwordSaltB64, _passwordHashB64))
            {
                return false;
            }

            (_passwordSaltB64, _passwordHashB64) = HashPassword(newPassword);
            _transactions.Add(new TransactionRecord
            {
                Id = _nextTxId++,
                CreatedAt = DateTimeOffset.UtcNow,
                Type = "Security",
                Description = "Password changed",
                Amount = 0,
            });

            return true;
        }
    }

    private static (string saltB64, string hashB64) HashPassword(string password)
    {
        var salt = RandomNumberGenerator.GetBytes(16);
        var hash = Pbkdf2(password, salt);
        return (Convert.ToBase64String(salt), Convert.ToBase64String(hash));
    }

    private static bool VerifyPassword(string password, string saltB64, string hashB64)
    {
        var salt = Convert.FromBase64String(saltB64);
        var expected = Convert.FromBase64String(hashB64);
        var actual = Pbkdf2(password, salt);
        return CryptographicOperations.FixedTimeEquals(actual, expected);
    }

    private static byte[] Pbkdf2(string password, byte[] salt)
    {
        return Rfc2898DeriveBytes.Pbkdf2(
            password: Encoding.UTF8.GetBytes(password),
            salt: salt,
            iterations: 100_000,
            hashAlgorithm: HashAlgorithmName.SHA256,
            outputLength: 32);
    }
}
