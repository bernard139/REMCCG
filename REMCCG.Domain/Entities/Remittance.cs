namespace REMCCG.Domain.Entities
{
    public class Remittance
    {
        public int ID { get; set; }
        public string RemittanceType { get; set; } // Enum for different types of offerings
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public int MemberID { get; set; }
        public Member Member { get; set; }
    }
}
