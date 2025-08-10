using BusinessInterfaceLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModelLayer;
using System.Net;

namespace WepAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class ExpensesManagementController : Controller
    {
        private readonly IExpenseIncomeService _iIncomeExpenseRepository;
        private readonly ILogger<ExpensesManagementController> _logger;

        public ExpensesManagementController(IExpenseIncomeService iIncomeExpenseService, ILogger<ExpensesManagementController> logger)
        {
            _iIncomeExpenseRepository = iIncomeExpenseService;
            _logger = logger;
        }

        [HttpGet("RetrieveIncomesExpenses")]
        public async Task<ActionResult<List<DataGridModel>>> RetrieveIncomesExpenses()
        {
            try
            {
                return await _iIncomeExpenseRepository.RetrieveIncomeExpenseTable();
            }
            catch (Exception ex) 
            {

                _logger.LogError(ex, "Error retrieving incomes and expenses data.");
                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while retrieving data.");
            }
        }

        [HttpPost("AddOrUpdateExpensesIncomes")]
        public async Task<IActionResult> AddOrUpdateExpensesIncomes(AddOrUpdateModel addOrUpdateModel)
        {
            if (addOrUpdateModel == null)
            {
                _logger.LogError("Input data is required.");
                return StatusCode((int)HttpStatusCode.BadRequest, "Input data is required.");
            }
            try
            {
                var response = await _iIncomeExpenseRepository.AddorUpdateIncomesExpenses(addOrUpdateModel);
                return new JsonResult(new ResultModel
                {
                    status = (int)HttpStatusCode.OK
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding or updating expenses/incomes.");
                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while processing your request.");
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
                    status = (int)HttpStatusCode.OK
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting incomes/expenses for id {Id}.", id);
                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while deleting the record.");
            }
        }
    }
}
