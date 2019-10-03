using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RessourceManager.Core.Models.V1;
using RessourceManager.Core.Services.Interfaces;
using RessourceManagerApi.Exceptions.Asset;


namespace test_mongo_auth.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AssetController : ControllerBase
    {
        private readonly IAssetService _assetService;

        public AssetController(IAssetService assetService)
        {
            _assetService = assetService;
        }


        [HttpGet]
        public async Task<ActionResult<List<Asset>>> Get()
        {
            var list = await _assetService.Get();
            return list;
        }
            

        [HttpGet("{id:length(24)}", Name = "GetAsset")]
        public async Task<ActionResult<Asset>> Get(string id)
        {
            var asset = await _assetService.Get(id);

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
                     assetIn.Id = id;  // if asset comes without an id 
                    _assetService.Update(assetIn);
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
        public async Task<IActionResult> Delete(string id)
        {
            var asset = await _assetService.Get(id);

            if (asset == null)
            {
                return NotFound();
            }

            _assetService.Remove(asset.Id);

            return NoContent();
        }

    }
}