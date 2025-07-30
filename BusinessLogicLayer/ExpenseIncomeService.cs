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

        private async Task<List<PersonalExpensesModel>> RetrieveIncomeExpense()
        {
            return await _expensesManagementContext.personalExpenses
                .Include(x => x.personal)
                .Include(x => x.incomes)
                .Include(x => x.expenses)
                .ToListAsync();
        }

        public async Task<List<DataGridModel>> RetrieveIncomeExpenseTable()
        {
            var personals = await RetrieveIncomeExpense();

            var expensesDataGridModels = new List<DataGridModel>();

            personals.ForEach(x =>
            {

                var totalExpenses = x.expenses.Sum(y => y.expense);
                var totalIncome = x.incomes.Sum(y => y.income);
                var savingOfPctg = (totalIncome > 0 && totalExpenses > 0) ? (totalIncome - totalExpenses) / totalIncome * 100 : 0;

                expensesDataGridModels.Add(
                new DataGridModel
                {
                    id = x.personal.id,
                    name = x.personal.name,

                    total_incomes = totalIncome,
                    total_expenses = totalExpenses,
                    pctg_of_saving = savingOfPctg,
                });

            });

            return expensesDataGridModels;
        }

        public async Task<int> AddorUpdateIncomesExpenses(ExpensesIncomesModel expensesIncomes)
        {
            var searchResult = await (from e in _expensesManagementContext.personalExpenses
                                      where e.personal.name == expensesIncomes.name
                                      select e)
                                .ToListAsync();

            if (searchResult.Any())
            {
                var values = await _expensesManagementContext
                    .personalExpenses
                    .Include(x => x.expenses)
                    .Include(x => x.incomes)
                    .Where(i => i.personal.id == searchResult.First().personal.id)
                    .FirstOrDefaultAsync();

                if (values?.expenses != null)
                {
                    foreach (var value in values.expenses)
                    {
                        value.expense = expensesIncomes.total_expenses;
                    }
                }

                if (values?.incomes != null)
                {
                    foreach (var value in values.incomes)
                    {
                        value.income = expensesIncomes.total_incomes;
                    }
                }
            }
            else
            {
                var personal = new PersonalExpensesModel()
                { 
                    //name = expensesIncomes.name,
                    personal = new PersonalModel
                    {
                        name = expensesIncomes.name
                    },

                    expenses = new List<ExpenseModel>()
                    {
                        new ExpenseModel
                        {
                            expense = expensesIncomes.total_expenses
                        }
                    }, 

                    incomes = new List<IncomeModel>()
                    {
                        new IncomeModel
                        {
                            income = expensesIncomes.total_incomes
                        }
                    }
                };
                _expensesManagementContext.personalExpenses.Add(personal);

            }

            return await _expensesManagementContext.SaveChangesAsync(); 
        }

        public async Task<int> DeleteIncomesExpenses(int personalId)
        {
            var personal = await _expensesManagementContext.personalExpenses.SingleAsync(p => p.personal.id == personalId);
            _expensesManagementContext.Remove(_expensesManagementContext.personalExpenses.Single(p => p.personal.id == personalId));

            return await _expensesManagementContext.SaveChangesAsync();
        }
    }
}
