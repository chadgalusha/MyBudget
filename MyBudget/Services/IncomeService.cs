using MyBudget.DataAccess;
using MyBudget.Helpers;
using MyBudget.Models;

namespace MyBudget.Services
{
	public class IncomeService : IService<Incomes>
	{
		private readonly IDataAccess<Incomes> _incomeDataAccess;

		public IncomeService(IDataAccess<Incomes> incomeDataAccess)
		{
			_incomeDataAccess = incomeDataAccess;
		}

		public async Task<Incomes> GetById(int id)
		{
			return await _incomeDataAccess.GetRecordByIdAsync(id);
		}

		public async Task<List<Incomes>> GetList()
		{
			return await _incomeDataAccess.GetListAsync();
		}

		public async Task<Incomes> CreateRecord(Incomes newIncome)
		{
			if (IsIncomeNameAlreadyUsed(newIncome.IncomeName) == true)
			{
				return new Incomes() { IncomeId = -1 };
			}

			try
			{
				Incomes income = new()
				{
					IncomeName = newIncome.IncomeName,
					IncomeTypeId = newIncome.IncomeTypeId,
					PaymentFrequencyTypeId = newIncome.PaymentFrequencyTypeId,
					IncomeAmount = newIncome.IncomeAmount,
					InitialIncomeDate = newIncome.InitialIncomeDate
				};

				return await _incomeDataAccess.CreateRecord(income);
			}
			catch (Exception e)
			{
				MyBudgetLogger.ErrorCreating(newIncome, e);
				return new Incomes() { IncomeId = 0 };
			}
		}

		public async Task<Incomes> UpdateRecord(Incomes income)
		{
			if (IsUpdatedIncomeNameModified(income) == true)
			{
				if (IsIncomeNameAlreadyUsed(income.IncomeName) == true)
				{
					return new Incomes() { IncomeId = -1 };
				}
			}

			try
			{
				return await _incomeDataAccess.UpdateRecordAsync(income);
			}
			catch (Exception e)
			{
				MyBudgetLogger.ErrorUpdating(income, e);
                return new Incomes() { IncomeId = 0 };
            }
		}

		public async Task<Incomes> DeleteRecord(Incomes income)
		{
			try
			{
				return await _incomeDataAccess.DeleteRecordAsync(income);
			}
			catch (Exception e)
			{
				MyBudgetLogger.ErrorDeleting(income, e);
                return new Incomes() { IncomeId = 0 };
            }
		}

        // PRIVATE METHODS

        private bool IsIncomeNameAlreadyUsed(string incomeName)
		{
			return _incomeDataAccess.DoesNameExist(incomeName);
		}

		// return false if NOT modified
		private bool IsUpdatedIncomeNameModified(Incomes income)
		{
			string currentIncomeName = _incomeDataAccess.GetNameById(income.IncomeId);
			return !currentIncomeName.Equals(income.IncomeName);
		}
    }
}
