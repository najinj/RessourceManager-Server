using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using RessourceManagerApi.Exceptions;
using test_mongo_auth.Models.Ressource;
using test_mongo_auth.Services;

namespace test_mongo_auth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AreaController : ControllerBase
    {
        private readonly AreaService _areaService;

        public AreaController(AreaService areaService)
        {
            _areaService = areaService;
        }


        [HttpGet]
        public ActionResult<List<Area>> Get() =>
            _areaService.Get();

        [HttpGet("{id:length(24)}", Name = "GetArea")]
        public ActionResult<Area> Get(string id)
        {
            var area = _areaService.Get(id);

            if (area == null)
            {
                return NotFound();
            }

            return area;
        }

        [HttpPost]
        public ActionResult<Area> Create(Area area)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = _areaService.Create(area);
                }
                catch (RessourceTypeNotFoundException e)
                {
                    ModelState.AddModelError("AreaTypeId", e.Message);
                    return BadRequest(new ValidationProblemDetails(ModelState));
                }
                return CreatedAtRoute("GetArea", new { id = area.Id.ToString() }, area);
            }
            else
            {
                string errorMessage = string.Join(", ", ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
                return BadRequest(new ValidationProblemDetails(ModelState));
            }
          
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, Area areaIn)
        {
            var area = _areaService.Get(id);

            if (area == null)
            {
                return NotFound();
            }

            _areaService.Update(id, areaIn);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var area = _areaService.Get(id);

            if (area == null)
            {
                return NotFound();
            }

            _areaService.Remove(area.Id);

            return NoContent();
        }
    }
}