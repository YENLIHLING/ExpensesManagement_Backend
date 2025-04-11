using ModelLayer;

namespace BusinessInterfaceLayer
{
    public interface IExpenseIncomeRepository
    {
        Task<int> AddorUpdateIncomesExpenses(ExpensesIncomesModel expensesIncomes);
        Task<List<DataGridModel>> RetrieveIncomeExpenseTable();
    }
}
