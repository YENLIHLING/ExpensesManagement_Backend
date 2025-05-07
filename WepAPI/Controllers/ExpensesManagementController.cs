using BusinessInterfaceLayer;
using Microsoft.AspNetCore.Mvc;
using ModelLayer;

namespace WepAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ExpensesManagementController : Controller
    {
        private readonly IExpenseIncomeService _iIncomeExpenseRepository;

        public ExpensesManagementController(IExpenseIncomeService iIncomeExpenseService)
        {
            _iIncomeExpenseRepository = iIncomeExpenseService;
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

        [HttpPost("AddOrUpdateExpensesIncomes")]
        public async Task<IActionResult> AddOrUpdateExpensesIncomes(ExpensesIncomesModel expensesIncomes)
        {
            if (expensesIncomes == null)
            {
                return BadRequest();
            }
            try
            {
                var response = await _iIncomeExpenseRepository.AddorUpdateIncomesExpenses(expensesIncomes);
                return new JsonResult(new ResultModel
                {
                    status = response
                });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpDelete("DeleteIncomesExpenses")]
        public async Task<IActionResult> DeleteIncomesExpenses(int id)
        {
            try
            {
                var response = await _iIncomeExpenseRepository.DeleteIncomesExpenses(id);
                return new JsonResult(new ResultModel
                {
                    status = response
                });
            }
            catch (Exception ex)
            { 
                throw new Exception(ex.Message);
            }
        }
    }
}
