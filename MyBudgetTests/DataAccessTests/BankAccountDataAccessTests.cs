using Moq;
using MyBudget.DataAccess;
using MyBudget.Models;
using MyBudget.Services;
using Xunit.Abstractions;

namespace MyBudgetTests.DataAccessTests
{
	public class BankAccountDataAccessTests
	{
		private readonly ITestOutputHelper output;

		public BankAccountDataAccessTests(ITestOutputHelper output)
		{
			this.output = output;
		}

		[Fact]
		public async void GetRecordByIdAsync_Test()
		{
			int testId = 1;
			var bankAccountList = GetFakeList();
			var bankAccountDataAccess = new Mock<IDataAccess<BankAccounts>>();
			bankAccountDataAccess.Setup(x => x.GetRecordByIdAsync(testId))
				.ReturnsAsync(bankAccountList.First(b => b.BankAccountId == testId));
			var dataAccess = bankAccountDataAccess.Object;

			var result = await dataAccess.GetRecordByIdAsync(testId);
			var expectedResult = bankAccountList.First(b => b.BankAccountId == testId);

			Assert.Equal(expectedResult, result);
		}

		[Fact]
		public async void GetListAsync_Test()
		{
			var bankAccountList = GetFakeList();
			var bankAccountDataAccess = new Mock<IDataAccess<BankAccounts>>();
			bankAccountDataAccess.Setup(x => x.GetListAsync()).ReturnsAsync(bankAccountList);
			var dataAccess = bankAccountDataAccess.Object;

			var result = await dataAccess.GetListAsync();
			var expectedResult = bankAccountList;

			Assert.Equal(expectedResult, result);
		}

		[Fact]
		public async void CreateRecord_Test()
		{
			var newBankAccount = GetNewFakeBankAccount();
            var bankAccountDataAccess = new Mock<IDataAccess<BankAccounts>>();
			bankAccountDataAccess.Setup(b => b.CreateRecord(newBankAccount)).ReturnsAsync(newBankAccount);
			var dataAccess = bankAccountDataAccess.Object;

			var result = await dataAccess.CreateRecord(newBankAccount);
			var expectedResult = newBankAccount;

			Assert.Equal(expectedResult, result);
        }

		[Fact]
		public async void UpdateRecordAsync_Test()
		{
			var updatedBankAccount = GetFakeList().First(b => b.BankAccountId == 1);
			updatedBankAccount.BankAccountName = "Modified";

            var bankAccountDataAccess = new Mock<IDataAccess<BankAccounts>>();
			bankAccountDataAccess.Setup(b => b.UpdateRecordAsync(updatedBankAccount)).ReturnsAsync(updatedBankAccount);
			var dataAccess = bankAccountDataAccess.Object;

			var result = await dataAccess.UpdateRecordAsync(updatedBankAccount);
			var expectedResult = updatedBankAccount;

			Assert.Equal(expectedResult, result);
        }

		[Fact]
		public async void DeleteRecordAsync_Test()
		{
			var bankAccountToDelete = GetFakeList().Where(b => b.BankAccountId == 1).First();
            var bankAccountDataAccess = new Mock<IDataAccess<BankAccounts>>();
			bankAccountDataAccess.Setup(b => b.DeleteRecordAsync(bankAccountToDelete)).ReturnsAsync(bankAccountToDelete);
			var dataAccess = bankAccountDataAccess.Object;
			
			var result = await dataAccess.DeleteRecordAsync(bankAccountToDelete);
			var expectedResult = bankAccountToDelete;

			Assert.Equal(expectedResult, result);
        }

		// PRIVATE METHODS

		private List<BankAccounts> GetFakeList()
		{
			var bankAccounts = new List<BankAccounts>()
			{
                new BankAccounts() { BankAccountId = 1, BankAccountName = "Test1", BankAccountTypeId = 1, Balance = 100 },
                new BankAccounts() { BankAccountId = 2, BankAccountName = "Test2", BankAccountTypeId = 2, Balance = 200 }
            };

			return bankAccounts;
        }

		private BankAccounts GetNewFakeBankAccount()
		{
			return new BankAccounts() { BankAccountId = 3, BankAccountName = "Test3", BankAccountTypeId = 1, Balance = 300 };
		}
	}
}
