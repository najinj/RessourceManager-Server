using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Cronos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RessourceManager.Core.Exceptions.Reservation;
using RessourceManager.Core.Models.V1;
using RessourceManager.Core.Services.Interfaces;
using RessourceManager.Core.ViewModels.Reservation;


namespace RessourceManagerApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ReservationController : ControllerBase
    {
        private readonly IReservationService _reservationService;
        public ReservationController(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }
        // GET: api/Reservation
        [HttpGet]
        public async Task<ActionResult<List<Reservation>>> Get() => 
            await _reservationService.Get();

        // GET: api/Reservation/5
        [HttpGet("{id}", Name = "GetReservation")]
        public async Task<ActionResult<Reservation>> Get(string Id) => 
            await _reservationService.Get(Id);


        public async Task<ActionResult<dynamic>> Availability(DateTime start,DateTime end,RType resourceType)
        {
            var freeResources = await _reservationService.Availability(start, end, resourceType);
            return freeResources;
        } 

        // POST: api/Reservation
        [HttpPost]
        public ActionResult Post(ReservationViewModel reservationIn)
        {
            var userId = User.Claims.Where(claim=>claim.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value;
            if (ModelState.IsValid)
            {
                try
                {             
                    if (string.IsNullOrWhiteSpace(reservationIn.CronoExpression)){
                        var reservation = new Reservation
                        {
                            Start = reservationIn.Start,
                            End = reservationIn.End,
                            ResourceId = reservationIn.ResourceId,
                            UserId = userId,
                            Title = reservationIn.Title,
                            PeriodicId = string.Empty,
                            ResourceType = reservationIn.ResourceType,
                        };
                        _reservationService.Add(reservation);
                    }
                    else
                    {                       
                        var expression = CronExpression.Parse(reservationIn.CronoExpression);
                        var occurrences = expression.GetOccurrences(
                            reservationIn.Start,
                            reservationIn.End,
                            fromInclusive: true,
                            toInclusive: true);
                        if (occurrences == null)
                            return null;
                        var reservations = new List<Reservation>();
                        var periodicId = Guid.NewGuid().ToString();
                        foreach (var startTime in occurrences)
                        {
                            var endTime = new DateTime(startTime.Year, startTime.Month, startTime.Day, reservationIn.End.Hour, reservationIn.End.Minute, 0);
                            reservations.Add(new Reservation
                            {
                                Start = startTime,
                                End = endTime,
                                ResourceId = reservationIn.ResourceId,
                                UserId = userId,
                                Title = reservationIn.Title,
                                PeriodicId = periodicId,
                                ResourceType = reservationIn.ResourceType,
                            }
                            );
                        }
                        _reservationService.Add(reservations);
                    }
                    
                }
                catch (ReservationServiceException ex)
                {
                    foreach(var field in ex.Fields)
                        ModelState.AddModelError(field, ex.Message);
                    return BadRequest(new ValidationProblemDetails(ModelState));
                }
                catch (Exception ex)
                {
                    var test = ex.GetType();
                    return StatusCode(500, "Internal server error");
                }

              //  return CreatedAtRoute("GetReservation", new { id = reservationIn..ToString() }, reservationIn);
            }
            return BadRequest(new ValidationProblemDetails(ModelState));

        }

        // PUT: api/Reservation/5
        [HttpPut("{id:length(24)}")]
        public ActionResult Put(string id, Reservation reservationIn)
        {
            return null;
            /*
            if (string.IsNullOrEmpty(reservationIn.AssetId) && string.IsNullOrEmpty(reservationIn.SpaceId))
            {
                var validationMessage = "Please provide Asset or Space.";
                ModelState.AddModelError("AssetId", validationMessage);
                ModelState.AddModelError("SpaceId", validationMessage);
            }

            if (!string.IsNullOrEmpty(reservationIn.AssetId) && !string.IsNullOrEmpty(reservationIn.SpaceId))
            {
                var validationMessage = "Please provide either Asset or Space.";
                ModelState.AddModelError("AssetId", validationMessage);
                ModelState.AddModelError("SpaceId", validationMessage);
            }
            if (ModelState.IsValid)
            {
                var reservation = _reservationService.Get(id);

                if (reservation == null)
                {
                    return NotFound();
                }
                try
                {
                    _reservationService.Update(id, reservationIn);
                }
                catch (Exception ex)
                {
                    return StatusCode(500, "Internal server error");
                }
                return NoContent();
            }
            return BadRequest(new ValidationProblemDetails(ModelState));
            */
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Remove(string id)
        {          
            try
            {
                var reservation = await _reservationService.Get(id);
                if (reservation == null)
                {
                    return NotFound();
                }
                var userId = User.Claims.Where(claim => claim.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value;
                var isAdmin = User.Claims.Where(claim => claim.Type == ClaimTypes.Role).FirstOrDefault().Value.Contains("Admin");
                await _reservationService.Remove(reservation.Id, userId, isAdmin);
            }
            catch(ReservationServiceException ex)
            {
                foreach (var field in ex.Fields)
                    ModelState.AddModelError(field, ex.Message);
                return BadRequest(new ValidationProblemDetails(ModelState));
            }
            catch(Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
            return Ok();
        }
        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> RemovePeriodicReservations(string periodicId)
        {
            try
            {
                var userId = User.Claims.Where(claim => claim.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value;
                var isAdmin = User.Claims.Where(claim => claim.Type == ClaimTypes.Role).FirstOrDefault().Value.Contains("Admin");
                await _reservationService.RemovePeriodicReservations(periodicId,userId,isAdmin);
            }
            catch (ReservationServiceException ex)
            {
                foreach (var field in ex.Fields)
                    ModelState.AddModelError(field, ex.Message);
                return BadRequest(new ValidationProblemDetails(ModelState));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
            return Ok();
        }
    }
}
