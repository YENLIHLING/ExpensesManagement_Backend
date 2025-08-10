using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelLayer
{
    public class PersonalIncomeExpenseDto
    {
        public int personalId { get; set; }
        public required string name { get; set; }
        public decimal totalIncome { get; set; }
        public decimal totalExpense { get; set; }
    }
}
