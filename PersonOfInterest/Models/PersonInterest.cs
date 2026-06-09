namespace PersonOfInterest.Models
{
    // Join entity for many-to-many Person <-> Interest
    public class PersonInterest
    {
        // Foreign key for person object, which it can't be null
        public int PersonId { get; set; }
        public Person Person { get; set; } = null!;

        // Foreign key for interet object
        public int InterestId { get; set; }
        public Interest Interest { get; set; } = null!;
    }
}