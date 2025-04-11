using System.ComponentModel.DataAnnotations;

namespace ModelLayer
{
    public class PersonalModel
    {
        [Key]
        public int id { get; set; }
        public required string name { get; set; }
        public List<IncomeModel> incomes { set;  get; } = new List<IncomeModel>();
        public List<ExpenseModel> expenses { set;  get; } = new List<ExpenseModel>();
    }
}
