using BusinessInterfaceLayer;
using DataLayer;
using Microsoft.EntityFrameworkCore;
using ModelLayer;

namespace BusinessLogicLayer
{
    public class ExpenseIncomeRepository : IExpenseIncomeRepository
    {

        private readonly ExpensesManagementContext _expensesManagementContext;

        public ExpenseIncomeRepository(ExpensesManagementContext expensesManagementContext)
        {
            _expensesManagementContext = expensesManagementContext;
        }

        private List<PersonalModel> RetrieveIncomeExpense()
        {
            return _expensesManagementContext.personal
                .Include(x => x.incomes)
                .Include(x => x.expenses)
                .ToList();
        }

        public Task<List<DataGridModel>> RetrieveIncomeExpenseTable()
        {
            var personals = RetrieveIncomeExpense();

            var expensesDataGridModels = new List<DataGridModel>();

            personals.ForEach(x =>
            {

                var totalExpenses = x.expenses.Sum(y => y.expense);
                var totalIncome = x.incomes.Sum(y => y.income);
                var savingOfPctg = (totalIncome > 0 && totalExpenses > 0) ? (totalIncome - totalExpenses) / totalIncome * 100 : 0;

                expensesDataGridModels.Add(
                new DataGridModel
                {
                    id = x.id,
                    name = x.name,

                    total_incomes = totalIncome,
                    total_expenses = totalExpenses,
                    pctg_of_saving = savingOfPctg,
                });

            });

            return Task.FromResult(expensesDataGridModels); 
        }

        public async Task<int> AddorUpdateIncomesExpenses(ExpensesIncomesModel expensesIncomes)
        {
            var searchResult = (from e in _expensesManagementContext.personal
                                where e.name == expensesIncomes.name
                                select e)
                                .ToList();

            if (searchResult.Any())
            {
                var values = _expensesManagementContext
                    .personal
                    .Include(x => x.expenses)
                    .Include(x => x.incomes)
                    .Where(i => i.id == searchResult.First().id)
                    .FirstOrDefault();

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
                var personal = new PersonalModel()
                { 
                    name = expensesIncomes.name,

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
                _expensesManagementContext.personal.Add(personal);

            }

            var result = await Task.Run(() => _expensesManagementContext.SaveChanges()); 

            return result; 
        }
    }
}
