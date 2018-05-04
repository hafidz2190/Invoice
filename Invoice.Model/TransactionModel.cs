namespace Invoice.Model
{
    public class TransactionModel : BaseModel
    {
        public virtual string InvoiceContent { get; set; }
        public virtual float? Total { get; set; }
    }
}
