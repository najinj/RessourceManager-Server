﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RessourceManagerApi.Exceptions.Reservation;
using RessourceManagerApi.Models;
using RessourceManagerApi.Services;

namespace RessourceManagerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationController : ControllerBase
    {
        private readonly ReservationService _reservationService;
        public ReservationController(ReservationService reservationService)
        {
            _reservationService = reservationService;
        }
        // GET: api/Reservation
        [HttpGet]
        public ActionResult<List<Reservation>> Get() =>
            _reservationService.Get();

        // GET: api/Reservation/5
        [HttpGet("{id}", Name = "GetReservation")]
        public ActionResult<Reservation> Get(string id) =>
            _reservationService.Get(id);

        // POST: api/Reservation
        [HttpPost]
        public ActionResult Post(Reservation reservation)
        {
            if (string.IsNullOrEmpty(reservation.AssetId) && string.IsNullOrEmpty(reservation.SpaceId))
            {
                var validationMessage = "Please provide Asset or Space.";
                ModelState.AddModelError("AssetId", validationMessage);
                ModelState.AddModelError("SpaceId", validationMessage);
            }

            if (!string.IsNullOrEmpty(reservation.AssetId) && !string.IsNullOrEmpty(reservation.SpaceId))
            {
                var validationMessage = "Please provide either Asset or Space.";
                ModelState.AddModelError("AssetId", validationMessage);
                ModelState.AddModelError("SpaceId", validationMessage);
            }
            if (ModelState.IsValid)
            {

                try
                {
                    _reservationService.Create(reservation);
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

                return CreatedAtRoute("GetReservation", new { id = reservation.Id.ToString() }, reservation);
            }
            return BadRequest(new ValidationProblemDetails(ModelState));

        }

        // PUT: api/Reservation/5
        [HttpPut("{id:length(24)}")]
        public ActionResult Put(string id, Reservation reservationIn)
        {
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
            _reservationService.Remove(reservation.Id);

            return NoContent();
        }
    }
}