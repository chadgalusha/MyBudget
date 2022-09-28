using MyBudget.DataAccess;
using MyBudget.Models;
using Serilog;

namespace MyBudget.Services
{
	public class IncomeService
	{
		private readonly IncomeDataAccess _incomeDataAccess;

		public IncomeService(IncomeDataAccess incomeDataAccess)
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
				return new Incomes();
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
				Log.Error($"Error creating new Income: {e.Message}");
				return new Incomes();
			}
        }

        public async Task<Incomes> UpdateRecord(Incomes income)
        {
			if (IsUpdatedIncomeNameModified(income) == false)
			{
                if (IsIncomeNameAlreadyUsed(income.IncomeName) == true)
                {
                    return new Incomes();
                }
            }

			try
			{
                return await _incomeDataAccess.UpdateRecordAsync(income);
            }
			catch (Exception e)
			{
				Log.Error($"Error updating Income: {e.Message}");
				return new Incomes();
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
				Log.Error($"Error deleting Income: {e.Message}");
				return new Incomes();
			}
        }

		// private methods

		private bool IsIncomeNameAlreadyUsed(string incomeName)
		{
			return _incomeDataAccess.DoesIncomeNameExist(incomeName);
		}

		private bool IsUpdatedIncomeNameModified(Incomes income)
		{
			var currentIncomeName = _incomeDataAccess.GetNameOfIncomeById(income.IncomeId);
			return currentIncomeName.Equals(income.IncomeName);
		}
    }
}
