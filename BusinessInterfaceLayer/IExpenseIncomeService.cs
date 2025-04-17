using ModelLayer;

namespace BusinessInterfaceLayer
{
    public interface IExpenseIncomeService
    {
        Task<int> AddorUpdateIncomesExpenses(ExpensesIncomesModel expensesIncomes);
        Task<List<DataGridModel>> RetrieveIncomeExpenseTable();
    }
}
