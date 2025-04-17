namespace ModelLayer
{
    public class ExpensesIncomesModel
    {
        public int id { get; set; }
        public required string name { get; set; }
        public decimal total_incomes { get; set; }
        public decimal total_expenses { get; set; }
    }
}
