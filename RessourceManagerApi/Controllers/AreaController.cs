using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
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
            _areaService.Create(area);

            return CreatedAtRoute("GetArea", new { id = area.Id.ToString() }, area);
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