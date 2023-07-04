using ES.DataAccess;
using ES.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ElevatorPersonSystem.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class HomeController : Controller
    {
        public readonly PersonContext personDbContext;

        public HomeController(PersonContext personDbContext)
        {
            this.personDbContext = personDbContext;
        }

        [HttpGet]
        public async Task<IActionResult> getPersonDetails()
        {
            return Ok(await personDbContext.PersonDbDetails.ToListAsync());
        }
        [HttpPost]
        public async Task<IActionResult> addPersonDetails(InsertPersonRequest ipr)
        {
            if(ipr == null)
            {
                return BadRequest("Enter with some value");
            }
            else
            {
                PersonDbDetails pd = new PersonDbDetails();
                pd.personId = new Guid();
                pd.weight = ipr.weight;
                pd.fromFloor = ipr.fromFloor;
                pd.toFloor = ipr.toFloor;

                var result = await personDbContext.PersonDbDetails.AddAsync(pd);
                await personDbContext.SaveChangesAsync();

                return Ok(result);
            }
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> updatePersonDetails([FromRoute] Guid id,UpdatePersonRequest upr)
        {
            if (upr == null)
            {
                return BadRequest("Enter some details");
            }
            else
            {
                var result = personDbContext.PersonDbDetails.FirstOrDefault(x=> x.personId.Equals(id));
                if(result == null)
                {
                    return BadRequest("The person not found");
                }
                result.weight = upr.weight == 0 ? result.weight : upr.weight;
                result.fromFloor = upr.fromFloor;
                result.toFloor = upr.toFloor;

                personDbContext.PersonDbDetails.Update(result);
                await personDbContext.SaveChangesAsync();
                return Ok(result);
            }
        }

        [HttpDelete]
        [Route("{id:guid}")]

        public async Task<IActionResult> deletePerson([FromRoute] Guid id)
        {
            if(id == null)
            {
                return BadRequest("enter  valid id");
            }
            else
            {
                var result = personDbContext.PersonDbDetails.FirstOrDefault(x=> x.personId.Equals(id));

                if (result == null)
                {
                    return BadRequest("personId not found");
                }

                personDbContext.PersonDbDetails.Remove(result);
                await personDbContext.SaveChangesAsync();
                return Ok(result);

            }
        }

        [HttpGet]
        [Route("/getWeight")]
        public async Task<IActionResult> getPersonsWeight()
        {
            return Ok(await personDbContext.PersonDbDetails.SumAsync(x => x.weight));
        }

        [HttpGet]
        [Route("/getPersons/{floor_num}")] 
        public async Task<IActionResult> getPersons([FromRoute] byte floor_num)
        {
            if(float.IsNaN(floor_num))
            {
                return BadRequest("enter valid number");
            }
            else
            {
                return Ok( await personDbContext.PersonDbDetails.FirstOrDefaultAsync(x=> x.toFloor.Equals(floor_num)));
            }
        }
    }
}
