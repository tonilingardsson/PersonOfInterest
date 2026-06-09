using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PersonOfInterest.Data;
using PersonOfInterest.Models;

namespace PersonOfInterest.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PersonsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PersonsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/persons
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Person>>> GetAllPersons()
        {
            var persons = await _context.Persons.ToListAsync();
            return Ok(persons);
        }

        // GET: api/persons/1/interests
        [HttpGet("{personId}/interests")]
        public async Task<ActionResult> GetPersonInterests(int personId)
        {
            var person = await _context.Persons
                .Include(p => p.PersonInterests)
                .ThenInclude(pi => pi.Interest)
                .FirstOrDefaultAsync(p => p.Id == personId);

            if (person == null)
                return NotFound("Person not found.");

            var interests = person.PersonInterests
                .Select(pi => new
                {
                    pi.Interest.Id,
                    pi.Interest.Title,
                    pi.Interest.Description
                })
                .ToList();

            return Ok(interests);
        }

        // GET: api/persons/1/links
        [HttpGet("{personId}/links")]
        public async Task<ActionResult<IEnumerable<Link>>> GetPersonLinks(int personId)
        {
            var personExists = await _context.Persons.AnyAsync(p => p.Id == personId);

            if (!personExists)
                return NotFound("Person not found.");

            var links = await _context.Links
                .Where(l => l.PersonId == personId)
                .ToListAsync();

            return Ok(links);
        }

        // POST: api/persons/1/interests
        [HttpPost("{personId}/interests")]
        public async Task<IActionResult> AddInterestToPerson(int personId, AddPersonInterestDto dto)
        {
            var person = await _context.Persons.FindAsync(personId);
            if (person == null)
                return NotFound("Person not found.");

            var interest = await _context.Interests.FindAsync(dto.InterestId);
            if (interest == null)
                return NotFound("Interest not found.");

            var alreadyExists = await _context.PersonInterests
                .AnyAsync(pi => pi.PersonId == personId && pi.InterestId == dto.InterestId);

            if (alreadyExists)
                return BadRequest("This interest is already connected to the person.");

            var personInterest = new PersonInterest
            {
                PersonId = personId,
                InterestId = dto.InterestId
            };

            _context.PersonInterests.Add(personInterest);
            await _context.SaveChangesAsync();

            return Ok("Interest added to person.");
        }

        // POST: api/persons/1/links
        [HttpPost("{personId}/links")]
        public async Task<IActionResult> AddLinkToPerson(int personId, AddLinkDto dto)
        {
            var person = await _context.Persons.FindAsync(personId);
            if (person == null)
                return NotFound("Person not found.");

            var interest = await _context.Interests.FindAsync(dto.InterestId);
            if (interest == null)
                return NotFound("Interest not found.");

            var personHasInterest = await _context.PersonInterests
                .AnyAsync(pi => pi.PersonId == personId && pi.InterestId == dto.InterestId);

            if (!personHasInterest)
                return BadRequest("This person is not connected to that interest.");

            var link = new Link
            {
                Url = dto.Url,
                PersonId = personId,
                InterestId = dto.InterestId
            };

            _context.Links.Add(link);
            await _context.SaveChangesAsync();

            return Ok("Link added.");
        }
    }

    public class AddPersonInterestDto
    {
        public int InterestId { get; set; }
    }

    public class AddLinkDto
    {
        public string Url { get; set; } = string.Empty;
        public int InterestId { get; set; }
    }
}