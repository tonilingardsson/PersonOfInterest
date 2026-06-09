using PersonOfInterest.Models;

namespace PersonOfInterest.Models
{
    public class Interest
    {
        // Basic properties for an Interest
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        // Lists for Interests
        public List<PersonInterest> PersonInterests { get; set; } = new();
        public List<Link> Links { get; set; } = new();
    }
}
