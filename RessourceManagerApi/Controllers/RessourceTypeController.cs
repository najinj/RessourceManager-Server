using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RessourceManager.Core.Models.V1;
using RessourceManager.Core.ViewModels.RessourceType;
using RessourceManagerApi.Exceptions.RessourceType;
using RessourceManager.Core.Services;
using RessourceManager.Core.Services.Interfaces;

namespace test_mongo_auth.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class RessourceTypeController : ControllerBase
    {
        private readonly IRessourceTypeService _ressourceTypeService;


        public RessourceTypeController(IRessourceTypeService ressourceTypeService)
        {
            _ressourceTypeService = ressourceTypeService;
        }


        // GET: api/RessourceType
        [HttpGet]
        public async Task<ActionResult<List<RessourceType>>> Get() {
                var list = await _ressourceTypeService.Get();
                return list;
        }
            

        // GET: api/RessourceType/5
        [HttpGet("{id:length(24)}", Name = "GetRessourceType")]
        public async Task<ActionResult<RessourceType>> Get(string id)
        {
            var ressourceType = await _ressourceTypeService.Get(id);

            if (ressourceType == null)
            {
                return NotFound();
            }

            return ressourceType;
        }

        [HttpGet]
        public async  Task<ActionResult<List<RessourceType>>> GetRessourceTypeByType(int type)
        {
            var ressourceTypes = await _ressourceTypeService.GetByType(type);

            if (ressourceTypes == null)
            {
                return NotFound();
            }

            return ressourceTypes;
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
        public async Task<IActionResult> Update(string id, RessourceTypeViewModel ressourceTypeIn)
        {
            if (ModelState.IsValid)
            {
                var ressourceTypeToUpdate = await _ressourceTypeService.Get(id);
                if (ressourceTypeToUpdate == null)
                    return NotFound();
                try
                {
                    ressourceTypeToUpdate.Type = ressourceTypeIn.Type;
                    ressourceTypeToUpdate.Name = ressourceTypeIn.Name;
                    ressourceTypeToUpdate.Description = ressourceTypeIn.Description;
                    _ressourceTypeService.Update(ressourceTypeToUpdate);
                }
                catch (Exception ex)
                {
                    return StatusCode(500, "Internal server error");
                }

                return CreatedAtRoute("GetRessourceType", new { id = ressourceTypeIn.Id.ToString() }, ressourceTypeIn);
            }
            return BadRequest(new ValidationProblemDetails(ModelState));
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            var ressourceType = await _ressourceTypeService.Get(id);

            if (ressourceType == null)
            {
                return NotFound();
            }

            _ressourceTypeService.Remove(ressourceType.Id);

            return NoContent();
        }
    }
}
