namespace KeeperComparer.Models
{
    public class KeeperRecord
    {
        public int Id { get; set; }
        public string? FullName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? EmailAddress { get; set; }
        public string? MobileNumber { get; set; }
        public string? LandlineNumber { get; set; }
        public Address? Address { get; set; }
    }
}