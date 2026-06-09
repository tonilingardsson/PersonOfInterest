using PersonOfInterest.Models;

namespace PeopleInterests.Models
{
    public class Link
    {
        public int Id { get; set; }
        public string Url { get; set; } = string.Empty;

        public int PersonId { get; set; }
        public Person Person { get; set; } = null!;

        public int InterestId { get; set; }
        public Interest Interest { get; set; } = null!;
    }
}