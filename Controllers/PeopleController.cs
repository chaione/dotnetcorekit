namespace DotNetCoreKit.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using AutoMapper;
    using DotNetCoreKit.Dto;
    using DotNetCoreKit.FluentValidations;
    using DotNetCoreKit.Models;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/[controller]")]
    public class PersonController : Controller
    {
        private readonly PeopleContext _context;

        public PeopleContext Context { get; }

        public PersonController(PeopleContext context)
        {
            _context = context;

            if (_context.People.Count() == 0)
            {
                this.Context = context;
                _context.People.Add(entity: new People { Name = "Item1" });
                _context.SaveChanges();
            }
        }

        [HttpGet]
        public IEnumerable<People> GetAll() => Context.People.ToList();

        [HttpGet("{id}", Name = "GetTodo")]
        public IActionResult GetById(long id)
        {
            var person = Context.People.FirstOrDefault(t => t.Id == id);
            if (person == null)
            {
                return NotFound();
            }
            return new ObjectResult(person);
        }

        [HttpPost]
        public IActionResult Create([FromBody] Person person)
        {
            if (person == null)
            {
                return BadRequest();
            }

            var peopleDto = Mapper.Map<PeopleDto>(person);
            var people = Mapper.Map<People>(peopleDto);

            Context.People.Add(people);
            Context.SaveChanges();

            return CreatedAtRoute("GetNewPerson", new { id = person.Id }, person);
        }

        [HttpPut("{id}")]
        public IActionResult Update(long id, [FromBody] Person person)
        {
            if (person == null || person.Id != id)
            {
                return BadRequest();
            }

            var people = Context.People.FirstOrDefault(t => t.Id == id);
            if (people == null)
            {
                return NotFound();
            }

            var peopleDto = Mapper.Map<PeopleDto>(person);
            people = Mapper.Map<People>(peopleDto);

            Context.People.Update(people);
            Context.SaveChanges();
            return new NoContentResult();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            var people = Context.People.FirstOrDefault(t => t.Id == id);
            if (people == null)
            {
                return NotFound();
            }

            Context.People.Remove(people);
            Context.SaveChanges();
            return new NoContentResult();
        }
    }
}