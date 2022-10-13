using Moq;
using MyBudget.Helpers;
using MyBudget.Models;
using MyBudget.Services;
using SQLite;

namespace MyBudgetTests.ServiceTests
{
    public class BankAccountServiceTests
    {
        [Fact]
        public async void GetById_TestAsync()
        {
            BankAccounts fakeBankAccount = GetBankAccount();

            var bankAccountService = new Mock<IService<BankAccounts>>();
            bankAccountService.Setup(b => b.GetById(fakeBankAccount.BankAccountId)).ReturnsAsync(fakeBankAccount);
            var service = bankAccountService.Object;

            var result = await service.GetById(fakeBankAccount.BankAccountId);

            Assert.NotNull(service);
            Assert.Equal(fakeBankAccount, result);
        }

        [Fact]
        public async void GetList_Test()
        {
            List<BankAccounts> listBankAccounts = GetListFakeBankAccounts();

            var bankAccountService = new Mock<IService<BankAccounts>>();
            bankAccountService.Setup(b => b.GetList()).ReturnsAsync(listBankAccounts);
            var service = bankAccountService.Object;

            var result = await service.GetList();

            Assert.Equal(listBankAccounts, result);
        }

        [Fact]
        public async void CreateRecord_Test()
        {
            BankAccounts newBankAccount = GetNewFakeAccount();

            var bankAccountService = new Mock<IService<BankAccounts>>();
            bankAccountService.Setup(b => b.CreateRecord(newBankAccount)).ReturnsAsync(newBankAccount);
            var service = bankAccountService.Object;

            var result = await service.CreateRecord(newBankAccount);

            Assert.Equal(newBankAccount, result);
        }

        [Fact]
        public async void CreateRecord_DuplicateName_Test()
        {
            BankAccounts newBankAccount = new() { BankAccountId = 5, BankAccountName = "Test1", BankAccountTypeId = 1, Balance = 125 };

            var bankAccountService = new Mock<IService<BankAccounts>>();
            bankAccountService.Setup(b => b.CreateRecord(newBankAccount)).ReturnsAsync(() =>
                {
                    using var db = new SQLiteConnection("Filename=:memory:");
                    
                    db.CreateTable<BankAccounts>();
                    var bankAccountList = GetListFakeBankAccounts();
                    bankAccountList.ForEach(b => db.Insert(b));

                    BankAccounts bankAccount = db.Table<BankAccounts>().First(b => b.BankAccountName == newBankAccount.BankAccountName);
                    bankAccount ??= new BankAccounts();
                    db.Close();

                    return bankAccount.BankAccountName == newBankAccount.BankAccountName ? new BankAccounts() { BankAccountId = -1 } : newBankAccount;
                }
            );
            var service = bankAccountService.Object;

            var result = await service.CreateRecord(newBankAccount);

            Assert.True(result.BankAccountId == -1);
        }

        [Fact]
        public async void UpdateRecord_Test()
        {
            var updateBankAccount = GetBankAccount();
            updateBankAccount.Balance = 250;

            var bankAccountService = new Mock<IService<BankAccounts>>();
            bankAccountService.Setup(b => b.UpdateRecord(updateBankAccount)).ReturnsAsync(updateBankAccount);
            var service = bankAccountService.Object;

            var result = await service.UpdateRecord(updateBankAccount);

            Assert.Equal(updateBankAccount, result);
        }

        [Fact]
        public async void DeleteRecord()
        {
            BankAccounts deleteBankAccount = GetBankAccount();

            var bankAccountService = new Mock<IService<BankAccounts>>();
            bankAccountService.Setup(b => b.DeleteRecord(deleteBankAccount)).ReturnsAsync(deleteBankAccount);
            var service = bankAccountService.Object;

            var result = await service.DeleteRecord(deleteBankAccount);

            Assert.Equal(deleteBankAccount, result);
        }

        // PRIVATE METHODS

        private BankAccounts GetBankAccount()
        {
            return GetListFakeBankAccounts().First(b => b.BankAccountId == 1);
        }

        private List<BankAccounts> GetListFakeBankAccounts()
        {
            List<BankAccounts> bankAccounts = new()
            {
                new BankAccounts() { BankAccountId = 1, BankAccountName = "Test1", BankAccountTypeId = 1, Balance = 100 },
                new BankAccounts() { BankAccountId = 2, BankAccountName = "Test2", BankAccountTypeId = 2, Balance = 200 },
                new BankAccounts() { BankAccountId = 3, BankAccountName = "Test3", BankAccountTypeId = 1, Balance = 300 }
            };

            return bankAccounts;
        }

        private BankAccounts GetNewFakeAccount()
        {
            return new BankAccounts() { BankAccountId = 4, BankAccountName = "Test4", BankAccountTypeId = 2, Balance = 400 };
        }
    }
}
