using System.ComponentModel.DataAnnotations;

namespace ModelLayer
{
    public class PersonalModel
    {
        [Key]
        public int id { get; set; }
        public required string name { get; set; }
        public string passwordHash { get; set; }
        public PersonalExpensesModel? PersonalExpenses { get; set; }

    }
}
