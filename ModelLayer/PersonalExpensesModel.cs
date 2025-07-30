using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ModelLayer
{
    public class PersonalExpensesModel
    {
        [Key]
        public int id { get; set; }
        public int personalId { get; set; }
        //public required string name { get; set; }
        public required PersonalModel personal { set; get; }
        public List<IncomeModel> incomes { set;  get; } = new List<IncomeModel>();
        public List<ExpenseModel> expenses { set;  get; } = new List<ExpenseModel>();
    }
}
