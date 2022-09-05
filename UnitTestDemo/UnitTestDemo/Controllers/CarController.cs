using Microsoft.AspNetCore.Mvc;
using System;
using UnitTestDemo.Models;
using UnitTestDemo.Services;

namespace UnitTestDemo.Controllers
{
    [Route("api/data/cars")]
    [ApiController]
    public class CarController : ControllerBase
    {
        private ICarService _carService;

        public CarController(ICarService carService)
        {
            _carService = carService;
        }

        [Route("")]
        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            var result = await _carService.GetCars().ConfigureAwait(false);

            return Ok(result);
        }

        [Route("{id}")]
        [HttpGet]
        public async Task<ActionResult> Get(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest();
            }

            var result = await _carService.GetCar(id).ConfigureAwait(false);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [Route("{id}")]
        [HttpPost]
        public async Task<ActionResult> Create(Car car)
        {
            var createdCar = await _carService.CreateCar(car).ConfigureAwait(false);

            return Created($"data/api/cars/${createdCar.Id}", createdCar);
        }

        public bool IsEven(int number)
        {
            return number % 2 == 0;
        }
    }
}
