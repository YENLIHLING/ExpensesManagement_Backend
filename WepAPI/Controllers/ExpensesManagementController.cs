using BusinessInterfaceLayer;
using BusinessLogicLayer;
using Microsoft.AspNetCore.Mvc;
using ModelLayer;

namespace WepAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ExpensesManagementController : Controller
    {
        private readonly IExpenseIncomeRepository _iIncomeExpenseRepository;

        public ExpensesManagementController(IExpenseIncomeRepository iIncomeExpenseRepository)
        {
            _iIncomeExpenseRepository = iIncomeExpenseRepository;
        }

        [HttpGet("RetrieveIncomesExpenses")]
        public async Task<List<DataGridModel>> RetrieveIncomesExpenses()
        {
            try
            {
                return await _iIncomeExpenseRepository.RetrieveIncomeExpenseTable();
            }
            catch (Exception ex) 
            {

                throw new Exception(ex.Message);
            }
        }

        [HttpPost("AddOrExpensesIncomes")]
        public async Task<IActionResult> AddOrUpdateExpensesIncomes(ExpensesIncomesModel expensesIncomes)
        {
            if (expensesIncomes == null)
            {
                return BadRequest();
            }
            try
            {
                var response = await _iIncomeExpenseRepository.AddorUpdateIncomesExpenses(expensesIncomes);
                return Ok(response);
            }
            catch
            {
                throw;
            }
        }
    }
}
