using BusinessInterfaceLayer;
using DataLayer;
using Microsoft.EntityFrameworkCore;
using ModelLayer;

namespace BusinessLogicLayer
{
    public class ExpenseIncomeService : IExpenseIncomeService
    {

        private readonly ExpensesManagementContext _expensesManagementContext;

        public ExpenseIncomeService(ExpensesManagementContext expensesManagementContext)
        {
            _expensesManagementContext = expensesManagementContext;
        }

        private async Task<List<PersonalIncomeExpenseDto>> RetrieveIncomeExpense()
        {
            // Retrieve all Personal 
            var result = await (from p in _expensesManagementContext.personal
                                join i in _expensesManagementContext.income on p.id equals i.personalId into incomes
                                from i in incomes.DefaultIfEmpty()
                                join e in _expensesManagementContext.expense on p.id equals e.personalId into expenses
                                from e in expenses.DefaultIfEmpty()
                                select new PersonalIncomeExpenseDto
                                {
                                    personalId = p.id,
                                    name = p.name,
                                    totalIncome = i != null ? i.income : 0,
                                    totalExpense = e != null ? e.expense : 0
                                })
                                .Where(x => x.totalIncome > 0 && x.totalExpense > 0)
                                .ToListAsync();

            return result;
        }

        public async Task<List<DataGridModel>> RetrieveIncomeExpenseTable()
        {
            var personalIncomeExpense = await RetrieveIncomeExpense();

            var expensesDataGridModels = new List<DataGridModel>();

            personalIncomeExpense.ForEach(x =>
            {
                var savingOfPctg = (x.totalIncome > 0 && x.totalExpense > 0) ? (x.totalIncome - x.totalExpense) / x.totalIncome * 100 : 0;

                expensesDataGridModels.Add(
                new DataGridModel
                {
                    personalId = x.personalId,
                    name = x.name, 
                    totalIncome = x.totalIncome,
                    totalExpense = x.totalExpense,
                    pctgOfSaving = savingOfPctg,
                });
            });

            return expensesDataGridModels;
        }

        public async Task<int> AddorUpdateIncomesExpenses(AddOrUpdateModel addOrUpdateModel)
        {
            //Find the existing PersonalModel by name 
            var personalModel = await _expensesManagementContext.personal
                               .FirstOrDefaultAsync(p => p.name == addOrUpdateModel.name);

            if (personalModel == null)
            {
                throw new InvalidOperationException("Personal model not found. Please ensure the personal name is correct.");
            }

            // CREATE LOGIC:
            // Note: Creating a PersonalModel here might be risky if it's missing
            // required fields like a password hash. This assumes it's acceptable.
            var incomeModel = new IncomeModel
            {
                personalId = personalModel.id,
                income = addOrUpdateModel.total_incomes
            };

            var expenseModel = new ExpenseModel
            {
                personalId = personalModel.id,
                expense = addOrUpdateModel.total_expenses
            };

            _expensesManagementContext.expense.Add(expenseModel);
            _expensesManagementContext.income.Add(incomeModel);
       
            return await _expensesManagementContext.SaveChangesAsync(); 
        }

        public async Task<int> DeleteIncomesExpenses(int personalId)
        {
            var personalExpense = await _expensesManagementContext.expense
                .Where(e => e.personalId == personalId)
                .ToListAsync();

            if (personalExpense != null)
            {
                _expensesManagementContext.expense.RemoveRange(personalExpense);
            }

            var personalIncome = await _expensesManagementContext.income
                .Where(e => e.personalId == personalId)
                .ToListAsync(); 

            if (personalIncome != null)
            {
                _expensesManagementContext.income.RemoveRange(personalIncome);
            }

            return await _expensesManagementContext.SaveChangesAsync();
        }
    }
}
