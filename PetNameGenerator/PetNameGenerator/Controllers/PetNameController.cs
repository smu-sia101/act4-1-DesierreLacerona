using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace PetNameGenerator.Controllers
{
    [ApiController]
    [Route("api/petnames")]
    public class PetNameController : ControllerBase
    {
        private static readonly Random rnd = new Random();

        private static readonly string[] dogNames = { "Buddy", "Max", "Charlie", "Rocky", "Rex" };
        private static readonly string[] catNames = { "Whiskers", "Mittens", "Luna", "Simba", "Tiger" };
        private static readonly string[] birdNames = { "Tweety", "Sky", "Chirpy", "Raven", "Sunny" };

        [HttpPost("generate")]
        public IActionResult GenerateName([FromQuery] PetNameRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.AnimalType))
            {
                return BadRequest(new { error = "The 'animalType' field is required." });
            }

            string animalType = request.AnimalType.ToLower();
            string[] nameList;

            if (animalType == "dog")
            {
                nameList = dogNames;
            }
            else if (animalType == "cat")
            {
                nameList = catNames;
            }
            else if (animalType == "bird")
            {
                nameList = birdNames;
            }
            else
            {
                return BadRequest(new { error = "Invalid animal type. Allowed values: dog, cat, bird." });
            }

            if (request.TwoPart.HasValue && !(request.TwoPart is bool))
            {
                return BadRequest(new { error = "The 'twoPart' field must be a boolean (true or false)." });
            }

            string petName;

            if (request.TwoPart == true)
            {
                petName = nameList[rnd.Next(nameList.Length)] + nameList[rnd.Next(nameList.Length)];
            }
            else
            {
                petName = nameList[rnd.Next(nameList.Length)];
            }

            return Ok(new { name = petName });
        }
        public class PetNameRequest
        {
            public string AnimalType { get; set; }
            public bool? TwoPart { get; set; }
        }
    }
}