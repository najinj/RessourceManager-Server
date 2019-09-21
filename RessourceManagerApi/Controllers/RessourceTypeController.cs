using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using RessourceManagerApi.Exceptions;
using RessourceManagerApi.Exceptions.RessourceType;
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
        //  [Authorize(Roles = "Admin")]
        //[Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult<RessourceType> Create(RessourceType ressourceType)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _ressourceTypeService.Create(ressourceType);
                }
                catch (RessourceTypeDuplicateKeyException ex)
                {
                    ModelState.AddModelError("Name", ex.Message);
                    return BadRequest(new ValidationProblemDetails(ModelState));
                }
                catch (Exception ex)
                {
                    var test = ex.GetType();
                    return StatusCode(500, "Internal server error");
                }

                return CreatedAtRoute("GetRessourceType", new { id = ressourceType.Id.ToString() }, ressourceType);
            }
            return BadRequest(new ValidationProblemDetails(ModelState));

        }

        // PUT: api/RessourceType/5
        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, RessourceType ressourceTypeIn)
        {
            if (ModelState.IsValid)
            {
                var post = _ressourceTypeService.Get(id);

                if (post == null)
                {
                    return NotFound();
                }

                _ressourceTypeService.Update(id, ressourceTypeIn);

                return NoContent();
            }
            return BadRequest(new ValidationProblemDetails(ModelState));
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
