using System;
using Microsoft.AspNetCore.Mvc;
using WebApplication.Models;
using WebApplication.Services;

namespace WebApplication.Controllers
{
    [ApiController]
    [Route("api/doctor")]
    public class DoctorController : ControllerBase
    {
        private readonly IDbServices _services;

        public DoctorController(IDbServices services)
        {
            _services = services;
        }

        [HttpGet]
        [Route("get/{id}")]
        public IActionResult GetDoctor(int id)
        {
            DoctorResponse doctor;
            try
            {
                doctor = _services.GetDoctor(id);
            }
            catch (Exception)
            {
                return BadRequest("Nie istnieje doktor o podanym id");
            }

            return Ok(doctor);
        }

        [HttpPost]
        [Route("add")]
        public IActionResult AddDoctor(DoctorResponse doc)
        {
            if (_services.AddDoctor(doc))
            {
                return Created("", "Dodano doktora do bazy");
            }

            return Problem("Nie udalo sie dodac doktora do bazy");
        }

        [HttpPost]
        [Route("modify")]
        public IActionResult ModifyDoctor(DoctorRequest doc)
        {
            if (_services.ModifyDoctor(doc))
            {
                return Created("", "Zmodyfikowano dane");
            }

            return Problem("Nie udalo sie zmodyfikowac danych");
        }

        [HttpDelete]
        [Route("delete/{id}")]
        public IActionResult DeleteDoctor(int id)
        {
            if (_services.DeleteDoctor(id))
            {
                return Ok("Usunieto doktora");
            }

            return BadRequest("Doktor o podanym id nie istnieje w bazie");
        }
    }
}