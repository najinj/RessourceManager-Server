using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using RessourceManager.Core.Models.V1;
using RessourceManagerApi.Exceptions.Asset;
using test_mongo_auth.Services;

namespace test_mongo_auth.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AssetController : ControllerBase
    {
        private readonly AssetService _assetService;

        public AssetController(AssetService assetService)
        {
            _assetService = assetService;
        }


        [HttpGet]
        public ActionResult<List<Asset>> Get() =>
            _assetService.Get();

        [HttpGet("{id:length(24)}", Name = "GetAsset")]
        public ActionResult<Asset> Get(string id)
        {
            var asset = _assetService.Get(id);

            if (asset == null)
            {
                return NotFound();
            }

            return asset;
        }


        [HttpPost]
        public ActionResult<Asset> Create(Asset asset)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _assetService.Create(asset);
                }
                catch(AssetDuplicateKeyException ex)
                {
                    ModelState.AddModelError("Name", ex.Message);
                    return BadRequest(new ValidationProblemDetails(ModelState));
                }
                catch(Exception ex)
                {
                    return StatusCode(500, "Internal server error");
                }

                return CreatedAtRoute("GetAsset", new { id = asset.Id.ToString() }, asset);
            }
            
            return BadRequest(new ValidationProblemDetails(ModelState));
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, Asset assetIn)
        {
            if (ModelState.IsValid)
            {
                var space = _assetService.Get(id);

                if (space == null)
                {
                    return NotFound();
                }
                try
                {
                    _assetService.Update(id, assetIn);
                }
                catch (Exception ex)
                {
                    return StatusCode(500, "Internal server error");
                }
                return CreatedAtRoute("GetAsset", new { id = assetIn.Id.ToString() }, assetIn);
            }
            return BadRequest(new ValidationProblemDetails(ModelState));


        }

        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var asset = _assetService.Get(id);

            if (asset == null)
            {
                return NotFound();
            }

            _assetService.Remove(asset.Id);

            return NoContent();
        }

    }
}