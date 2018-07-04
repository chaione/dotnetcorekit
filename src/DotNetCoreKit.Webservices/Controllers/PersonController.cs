// -----------------------------------------------------------------------
// <copyright file="PersonController.cs" company="ChaiOne">
// Copyright (c) ChaiOne. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace DotNetCoreKit.Webservices.Controllers
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;
    using DotNetCoreKit.EntityFramework;
    using DotNetCoreKit.Models.Domain;
    using DotNetCoreKit.Models.Dto;
    using DotNetCoreKit.Utilities.Extensions;
    using DotNetCoreKit.Webservices.FluentValidations;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    /// <inheritdoc />
    /// <summary>
    /// The person controller.
    /// </summary>
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PersonController"/> class.
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        public PersonController(PeopleContext context)
        {
            Context = context;

            if (Context.People.Any())
            {
                return;
            }

            Context.People.Add(entity: new People { Name = "Item1", Email = SystemClock.Now.ToString() });
            Context.SaveChanges();
        }

        /// <summary>
        /// Gets or sets the context.
        /// </summary>
        private PeopleContext Context { get; set; }

        private MachineClockDateTime SystemClock => new MachineClockDateTime();

        /// <summary>
        /// The get all people.
        /// </summary>
        /// <returns>
        /// The <see cref="IEnumerable"/> collection of people.
        /// </returns>
        [HttpGet]
        public IEnumerable<People> GetAll() => Context.People.ToList();

        /// <summary>
        /// Gets the person by id.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="IActionResult"/>.
        /// </returns>
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

        /// <summary>
        /// Creates the person that is specified.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /Person
        ///     {
        ///        "id": 1,
        ///        "name": "Tom Bombadil",
        ///        "email": tom.bombadil@lotr.com,
        ///        "age": 62,
        ///        "pets": []
        ///     }
        ///
        /// </remarks>
        /// <param name="person">
        /// The person.
        /// </param>
        /// <returns>
        /// The <see cref="IActionResult"/>.
        /// </returns>
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

        /// <summary>
        /// Updates the person that is specified.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <param name="person">
        /// The person.
        /// </param>
        /// <returns>
        /// The <see cref="IActionResult"/>.
        /// </returns>
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

        /// <summary>
        /// Deletes the person that is specified.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="IActionResult"/>.
        /// </returns>
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