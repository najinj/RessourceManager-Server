using System;
using System.Collections.Generic;
using Cronos;
using Microsoft.AspNetCore.Mvc;
using RessourceManager.Core.Models.V1;
using RessourceManager.Core.Services.Interfaces;
using RessourceManager.Core.ViewModels.Reservation;
using RessourceManagerApi.Exceptions.Reservation;


namespace RessourceManagerApi.Controllers
{
    [Route("api/[controller]")]
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
        public ActionResult<List<Reservation>> Get() => throw new NotImplementedException();
          //  _reservationService.Get();

        // GET: api/Reservation/5
        [HttpGet("{id}", Name = "GetReservation")]
        public ActionResult<Reservation> Get(string id) =>
            throw new NotImplementedException();

        // POST: api/Reservation
        [HttpPost]
        public ActionResult Post(ReservationViewModel reservationIn)
        {
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
                            UserId = reservationIn.UserId,
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
                        foreach (var startTime in occurrences)
                        {
                            var endTime = new DateTime(startTime.Year, startTime.Month, startTime.Day, reservationIn.End.Hour, reservationIn.End.Minute, 0);
                            reservations.Add(new Reservation
                            {
                                Start = startTime,
                                End = endTime,
                                ResourceId = reservationIn.ResourceId,
                                UserId = reservationIn.UserId,
                                Title = reservationIn.Title,
                                PeriodicId = string.Empty,
                                ResourceType = reservationIn.ResourceType,
                            }
                            );
                        }
                        _reservationService.Add(reservations);
                    }
                    
                }
                catch (ReservationDuplicateKeyException ex)
                {
                    ModelState.AddModelError("Name", ex.Message);
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
        public IActionResult Delete(string id)
        {
            var reservation = _reservationService.Get(id);

            if (reservation == null)
            {
                return NotFound();
            }
           // _reservationService.Remove(reservation.Id);

            return NoContent();
        }
    }
}
