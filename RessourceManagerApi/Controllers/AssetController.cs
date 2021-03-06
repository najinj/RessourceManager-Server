﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RessourceManager.Core.Exceptions.Asset;
using RessourceManager.Core.Exceptions.RessourceType;
using RessourceManager.Core.Exceptions.Space;
using RessourceManager.Core.Models.V1;
using RessourceManager.Core.Services.Interfaces;


namespace RessourceManagerApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
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
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Asset>> Create(Asset asset)
        {
            if (ModelState.IsValid)
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        var result = await _assetService.Create(asset);
                    }
                    catch (RessourceTypeRepositoryException ex)
                    {
                        ModelState.AddModelError(ex.Field, ex.Message);
                        return BadRequest(new ValidationProblemDetails(ModelState));
                    }
                    catch (SpaceRepositoryException ex)
                    {
                        ModelState.AddModelError(ex.Field, ex.Message);
                        return BadRequest(new ValidationProblemDetails(ModelState));
                    }
                    catch (AssetRepositoryException ex)
                    {
                        ModelState.AddModelError(ex.Field, ex.Message);
                        return BadRequest(new ValidationProblemDetails(ModelState));
                    }
                    return CreatedAtRoute("GetSpace", new { id = asset.Id.ToString() }, asset);
                }
                return BadRequest(new ValidationProblemDetails(ModelState));
            }
            
            return BadRequest(new ValidationProblemDetails(ModelState));
        }

        [HttpPut("{id:length(24)}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(string id, Asset assetIn)
        {
            if (ModelState.IsValid)
            {
                var space = await _assetService.Get(id);

                if (space == null)
                {
                    return NotFound();
                }
                try
                {
                    assetIn.Id = id;
                    await _assetService.Update(assetIn);
                }
                catch (RessourceTypeRepositoryException ex)
                {
                    ModelState.AddModelError(ex.Field, ex.Message);
                    return BadRequest(new ValidationProblemDetails(ModelState));
                }
                catch (AssetRepositoryException ex)
                {
                    ModelState.AddModelError(ex.Field, ex.Message);
                    return BadRequest(new ValidationProblemDetails(ModelState));
                }
                return CreatedAtRoute("GetSpace", new { id = assetIn.Id.ToString() }, assetIn);
            }
            return BadRequest(new ValidationProblemDetails(ModelState));


        }

        [HttpDelete("{id:length(24)}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(string id)
        {
            var space = await _assetService.Get(id);

            if (space == null)
            {
                return NotFound();
            }
            try
            {
                await _assetService.Remove(space.Id);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
            return Ok();
        }

    }
}