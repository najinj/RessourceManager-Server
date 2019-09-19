using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using test_mongo_auth.Models.RessourceTypes;
using test_mongo_auth.Services;

namespace test_mongo_auth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RessourceTypeController : ControllerBase
    {
        private readonly RessourceTypeService _ressourceTypeService;


        public RessourceTypeController(RessourceTypeService ressourceTypeService)
        {
            _ressourceTypeService = ressourceTypeService;
        }


        // GET: api/RessourceType
        [HttpGet]
        public ActionResult<List<RessourceType>> Get() =>
            _ressourceTypeService.Get();

        // GET: api/RessourceType/5
        [HttpGet("{id:length(24)}", Name = "GetRessourceType")]
        public ActionResult<RessourceType> Get(string id)
        {
            var ressourceType = _ressourceTypeService.Get(id);

            if (ressourceType == null)
            {
                return NotFound();
            }

            return ressourceType;
        }

        // POST: api/RessourceType
        [HttpPost]
        public ActionResult<RessourceType> Create(RessourceType ressourceType)
        {
            _ressourceTypeService.Create(ressourceType);

            return CreatedAtRoute("GetPost", new { id = ressourceType.Id.ToString() }, ressourceType);
        }

        // PUT: api/RessourceType/5
        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, RessourceType ressourceTypeIn)
        {
            var post = _ressourceTypeService.Get(id);

            if (post == null)
            {
                return NotFound();
            }

            _ressourceTypeService.Update(id, ressourceTypeIn);

            return NoContent();
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var ressourceType = _ressourceTypeService.Get(id);

            if (ressourceType == null)
            {
                return NotFound();
            }

            _ressourceTypeService.Remove(ressourceType.Id);

            return NoContent();
        }
    }
}
