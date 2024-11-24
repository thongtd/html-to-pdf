namespace html_to_pdf.Models
{
    public class OrderModel
    {
        public int OrderId { get; set; }

        public string CustomerName { get; set; }

        public double TotalPrice { get; set; }

        public DateTime OrderDate { get; set; }
    }
}