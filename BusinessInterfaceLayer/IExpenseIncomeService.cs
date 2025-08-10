using ModelLayer;

namespace BusinessInterfaceLayer
{
    public interface IExpenseIncomeService
    {
        Task<int> AddorUpdateIncomesExpenses(AddOrUpdateModel addOrUpdateModel);
        Task<List<DataGridModel>> RetrieveIncomeExpenseTable();
        Task<int> DeleteIncomesExpenses(int personalId);
    }
}
