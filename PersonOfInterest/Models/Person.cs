namespace PersonOfInterest.Models
{
    public class Person
    {
        // The personal properties requested
        public int Id { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; } = string.Empty;

        // List object for PersonInterest
        public List<PersonInterest> PersonInterests { get; set; } = new();
        // List object of Links 
        public List<Link> Links { get; set; } = new();
    }
}
