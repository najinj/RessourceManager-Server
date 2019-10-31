using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RessourceManager.Core.Models.V1;
using RessourceManager.Core.ViewModels.RessourceType;
using RessourceManager.Core.Services.Interfaces;
using RessourceManager.Core.Helpers;
using RessourceManager.Core.Exceptions.RessourceType;

namespace test_mongo_auth.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class RessourceTypeController : ControllerBase
    {
        private readonly IRessourceTypeService _ressourceTypeService;
        private readonly IErrorHandler _errorHandler;


        public RessourceTypeController(IRessourceTypeService ressourceTypeService, IErrorHandler errorHandler)
        {
            _ressourceTypeService = ressourceTypeService;
            _errorHandler = errorHandler;
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
        public async Task<ActionResult<RessourceType>> Create(RessourceTypeViewModel ressourceTypeIn)
        {
            if (!ModelState.IsValid)
                 return BadRequest(new ValidationProblemDetails(ModelState));
            var ressourceType = new RessourceType
            {
                Description = ressourceTypeIn.Description,
                Name = ressourceTypeIn.Name,
                Type = ressourceTypeIn.Type
            };
            try
            {
               await _ressourceTypeService.Create(ressourceType);
            }
            catch (RessourceTypeRepositoryException ex)
            {
                ModelState.AddModelError(ex.Field, ex.Message);
                return BadRequest(new ValidationProblemDetails(ModelState));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
            return CreatedAtRoute("GetRessourceType", new { id = ressourceType.Id.ToString() }, ressourceType);
        }

        // PUT: api/RessourceType/5
        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(string id, RessourceTypeViewModel ressourceTypeIn)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ValidationProblemDetails(ModelState));
            var ressourceTypeToUpdate = await _ressourceTypeService.Get(id);
            if (ressourceTypeToUpdate == null)
                return NotFound();
            try
            {
                ressourceTypeToUpdate.Type = ressourceTypeIn.Type;
                ressourceTypeToUpdate.Name = ressourceTypeIn.Name;
                ressourceTypeToUpdate.Description = ressourceTypeIn.Description;
                await _ressourceTypeService.Update(ressourceTypeToUpdate);
            }
            catch (RessourceTypeRepositoryException ex)
            {
                ModelState.AddModelError(ex.Field, ex.Message);
                return BadRequest(new ValidationProblemDetails(ModelState));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
            return CreatedAtRoute("GetRessourceType", new { id = ressourceTypeIn.Id.ToString() }, ressourceTypeIn);           
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

            await _ressourceTypeService.Remove(ressourceType.Id);

            return Ok();
        }
    }
}
