using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using test_mongo_auth.Models.Ressource;
using test_mongo_auth.Services;

namespace test_mongo_auth.Controllers
{
    [Route("api/[controller]")]
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
            _assetService.Create(asset);

            return CreatedAtRoute("GetArea", new { id = asset.Id.ToString() }, asset);
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, Asset assetIn)
        {
            var area = _assetService.Get(id);

            if (area == null)
            {
                return NotFound();
            }

            _assetService.Update(id, assetIn);

            return NoContent();
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