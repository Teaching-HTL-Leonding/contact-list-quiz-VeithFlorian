using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using ContactList.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ContactList.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContactsController : ControllerBase
    {
        private readonly IContactRepository repository;

        public ContactsController(IContactRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Contact>))]
        public IActionResult GetAll() => Ok(repository.GetAll());

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Contact))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Add([Required] [FromBody] Contact contact)
        {
            if (contact.Id < 1 || contact.Firstname == String.Empty || contact.Lastname == String.Empty || contact.Email == String.Empty)
            {
                return BadRequest("Invalid contact");
            }

            repository.Add(contact);
            return CreatedAtAction(nameof(FindByName), new {nameFilter = contact.Lastname}, contact);
        }

        [HttpDelete]
        [Route("{personId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Delete(int personId)
        {
            if (personId < 1) return BadRequest("Invalid Id");
            try
            {
                repository.Delete(personId);
            }
            catch (ArgumentException)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpGet("findByName")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Contact>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult FindByName([Required] [FromQuery] string nameFilter)
        {
            IEnumerable<Contact> contacts;
            try
            {
                contacts = repository.FindByName(nameFilter);
            }
            catch (ArgumentException)
            {
                return BadRequest("Invalid or missing name");
            }

            return Ok(contacts);
        }
    }
}