﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RessourceManager.Core.Exceptions.RessourceType;
using RessourceManager.Core.Models.V1;
using RessourceManager.Core.Services.Interfaces;
using RessourceManager.Core.Exceptions.Space;
using RessourceManager.Core.Exceptions.Asset;
using RessourceManager.Core.ViewModels.Space;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace RessourceManagerApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class SpaceController : ControllerBase
    {
        private readonly ISpaceService _spaceService;
        private readonly IAssetService _assetService;

        public SpaceController(ISpaceService spaceService,IAssetService assetService)
        {
            _spaceService = spaceService;
            _assetService = assetService;
        }


        [HttpGet]
        public async Task<ActionResult<List<Space>>> Get()
        {
            var list = await _spaceService.Get();
            return list;
        }
            
        [HttpGet("{id:length(24)}", Name = "GetSpace")]
        public async Task<ActionResult<Space>> Get(string id)
        {
            var space = await _spaceService.Get(id);

            if (space == null)
            {
                return NotFound();
            }

            return space;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Space>> Create(SpaceViewModel spaceIn)
        {
            if (ModelState.IsValid)
            {
                var space = new Space
                {
                    Capacity = spaceIn.Capacity,
                    Name = spaceIn.Name,
                    SpaceTypeId = spaceIn.SpaceTypeId,
                    Tags = spaceIn.Tags
                };
                try
                {               
                    if (spaceIn.assets.Any())
                    {
                        var assets = await _assetService.Get(spaceIn.assets);
                        space.assests = assets;
                    }              
                    var result = await _spaceService.Create(space);
                }
                catch (RessourceTypeRepositoryException ex)
                {
                    ModelState.AddModelError(ex.Field, ex.Message);
                    return BadRequest(new ValidationProblemDetails(ModelState));
                }
                catch(SpaceRepositoryException ex)
                {
                    ModelState.AddModelError(ex.Field, ex.Message);
                    return BadRequest(new ValidationProblemDetails(ModelState));
                }
                catch (AssetRepositoryException ex)
                {
                    ModelState.AddModelError(ex.Field, ex.Message);
                    return BadRequest(new ValidationProblemDetails(ModelState));
                }
                return CreatedAtRoute("GetSpace", new { id = space.Id.ToString() }, space);
            }
            return BadRequest(new ValidationProblemDetails(ModelState)); 
        }

        [HttpPut("{id:length(24)}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(string id, SpaceViewModel spaceIn)
        {
            if (ModelState.IsValid)
            {
                var space = await _spaceService.Get(id);

                if (space == null)
                {
                    return NotFound();
                }
                try
                {
                    if (spaceIn.assets.Any())
                    {
                        var assets = await _assetService.Get(spaceIn.assets);
                        space.assests = assets;
                    }
                    else
                        space.assests = new List<Asset>();
                    space.Capacity = spaceIn.Capacity;
                    space.Name = spaceIn.Name;
                    space.SpaceTypeId = spaceIn.SpaceTypeId;
                    space.Tags = spaceIn.Tags;
                    await _spaceService.Update(space);
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
                return CreatedAtRoute("GetSpace", new { id = space.Id.ToString() }, space);
            }
            return BadRequest(new ValidationProblemDetails(ModelState));

        }

        [HttpDelete("{id:length(24)}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(string id)
        {
            var space = await _spaceService.Get(id);

            if (space == null)
            {
                return NotFound();
            }
            try
            {
               await _spaceService.Remove(space.Id);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
            return Ok();
        }

        [HttpGet("{assetId}/{spaceId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RemoveAssetFromSpace(string assetId,string spaceId)
        {
            Space space;
            try
            {
                space = await _spaceService.RemoveAssetFromSpace(assetId, spaceId);
                if(space == null)
                    return StatusCode(500, "Internal server error");
            }
            catch (SpaceRepositoryException ex)
            {
                return StatusCode(500, ex.Message);
            }
            catch (AssetRepositoryException ex)
            {
                return StatusCode(500, ex.Message);
            }
            return Ok(space);
        }

        [HttpGet("{assetId}/{spaceId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddAssetToSpace(string assetId, string spaceId)
        {
            Space space;
            try
            {
                space = await _spaceService.AddAssetToSpace(assetId, spaceId);
                if (space == null)
                    return StatusCode(500, "Internal server error");
            }
            catch (SpaceRepositoryException ex)
            {
                return StatusCode(500, ex.Message);
            }
            catch (AssetRepositoryException ex)
            {
                return StatusCode(500, ex.Message);
            }
            return Ok(space);
        }
    }
}