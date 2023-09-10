using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FrontendExercise
{
    public class TasksController : Controller
    {
        private static List<Contact> _contacts = new List<Contact>
        {
            new Contact { Name = "ofir", SerialNumber = "12345", Tasks = GenerateFunnyNonsenseTasks() },
            new Contact { Name = "ginat", SerialNumber = "54321", Tasks = GenerateRealTasks() },
        };

        private static List<string> GenerateRealTasks()
        {
            return new List<string>
            {
                "Clean the house",
                "Buy groceries",
                "Attend a meeting",
                "Go to the gym",
                "Complete project report",
                "Cook dinner",
                "Read a book",
                "Visit the doctor",
                "Pay bills",
                "Call a friend"
            };
        }

        private static List<string> GenerateFunnyNonsenseTasks()
        {
            return new List<string>
            {
                "Fly to the moon using bubble gum",
                "Teach a cat to sing opera",
                "Compete in a pancake flipping Olympics",
                "Invent a language only understood by squirrels",
                "Paint a portrait of a rainbow-colored potato",
                "Run a marathon while wearing clown shoes",
                "Solve a Rubik's cube blindfolded underwater",
                "Become a professional unicorn wrangler",
                "Participate in a pogo stick world championship",
                "Organize a snail racing tournament"
            };
        }

        [HttpGet("{name}")]
        public ActionResult<Dictionary<string, List<string>>> Get(string name)
        {
            var contact = _contacts.FirstOrDefault(c => c.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

            if (contact == null)
            {
                return NotFound();
            }

            var result = new Dictionary<string, List<string>>
            {
                { contact.SerialNumber, contact.Tasks }
            };

            return Ok(result);
        }
    }

    public class Contact
    {
        public string Name { get; set; }
        public string SerialNumber { get; set; }
        public List<string> Tasks { get; set; }
    }
}