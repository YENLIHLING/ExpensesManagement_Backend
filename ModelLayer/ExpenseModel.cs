using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelLayer
{
    public class ExpenseModel
    {
        [Key]
        public int id { get; set; }

        [ForeignKey("OrderId")]
        public int personalId { get; set; }
        public decimal expense { get; set; }
        public PersonalModel? personal { get; set; }
    }
}
