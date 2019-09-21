using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using RessourceManagerApi.Exceptions;
using RessourceManagerApi.Exceptions.RessourceType;
using RessourceManagerApi.Exceptions.Space;
using test_mongo_auth.Models.Ressource;
using test_mongo_auth.Services;

namespace test_mongo_auth.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SpaceController : ControllerBase
    {
        private readonly SpaceService _spaceService;
        private readonly AssetService _assetService;

        public SpaceController(SpaceService spaceService, AssetService assetService)
        {
            _spaceService = spaceService;
            _assetService = assetService;
        }


        [HttpGet]
        public ActionResult<List<Space>> Get() =>
            _spaceService.Get();

        [HttpGet("{id:length(24)}", Name = "GetSpace")]
        public ActionResult<Space> Get(string id)
        {
            var space = _spaceService.Get(id);

            if (space == null)
            {
                return NotFound();
            }

            return space;
        }

        [HttpPost]
        public ActionResult<Space> Create(Space space)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = _spaceService.Create(space);
                }
                catch (RessourceTypeNotFoundException e)
                {
                    ModelState.AddModelError("SpaceTypeId", e.Message);
                    return BadRequest(new ValidationProblemDetails(ModelState));
                }
                catch(SpaceDuplicateKeyException ex)
                {
                    ModelState.AddModelError("Name", ex.Message);
                    return BadRequest(new ValidationProblemDetails(ModelState));
                }
                return CreatedAtRoute("GetSpace", new { id = space.Id.ToString() }, space);
            }
            return BadRequest(new ValidationProblemDetails(ModelState));

          
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, Space spaceIn)
        {
            if (ModelState.IsValid)
            {
                var space = _spaceService.Get(id);

                if (space == null)
                {
                    return NotFound();
                }
                try
                {
                    _spaceService.Update(id, spaceIn);
                }
                catch (Exception ex)
                {
                    return StatusCode(500, "Internal server error");
                }

                return NoContent();
            }
            return BadRequest(new ValidationProblemDetails(ModelState));

        }

        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var space = _spaceService.Get(id);

            if (space == null)
            {
                return NotFound();
            }
            try
            {
                _spaceService.Remove(space.Id);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }

            return NoContent();
        }

        [HttpGet("{assetId}/{spaceId}")]
        public IActionResult RemoveAssetFromSpace(string assetId,string spaceId)
        {
            var spaceIn = _spaceService.Get(spaceId);
            var assetIn = _assetService.Get(assetId);
            if (spaceIn == null)
                return NotFound("Space Not found");
            if(assetIn == null)
                return NotFound("Asset Not found");
            var assetToRemove = spaceIn.assests.Where(asset => asset.Id == assetId).FirstOrDefault();
            if (assetToRemove == null)
                return NotFound($"Asset Not found in {spaceIn.Name} ");
            spaceIn.assests.Remove(assetToRemove);
            try
            {
               // if (assetIn.Status == Status.Chained) 
                    assetIn.Status = Status.Unchained;  // if we remove a chained asset from space it becomes unchained and would be possible to reserve it           
                _spaceService.Update(spaceId, spaceIn);
                _assetService.Update(assetId, assetIn); // updating the status of the asset 
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
            return Ok(spaceIn);
        }

        [HttpGet("{assetId}/{spaceId}")]
        public IActionResult AddAssetToSpace(string assetId, string spaceId)
        {
            var spaceIn = _spaceService.Get(spaceId);
            var assetIn = _assetService.Get(assetId);
            if (spaceIn == null)
                return NotFound("Space Not found");
            if (assetIn == null)
                return NotFound("Asset Not found");
            spaceIn.assests.Add(assetIn);
            try
            {
                 assetIn.Status = Status.Chained;
                _spaceService.Update(spaceId, spaceIn);
                _assetService.Update(assetId, assetIn); // updating the status of the asset 
            }
            catch(Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }   
            return Ok(spaceIn);
        }
    }
}