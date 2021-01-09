using Microsoft.AspNetCore.Mvc;
using Roulette.Masiv.Business.Services;
using Roulette.Masiv.Common.CustomExceptions;
using Roulette.Masiv.Entities.DTO;
using Roulette.Masiv.Entities.Enums;
using Roulette.Masiv.Entities.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Roulette.Masiv.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RouletteController : ControllerBase
    {
        private readonly IRouletteService _service;

        public RouletteController(IRouletteService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_service.GetAll());
        }

        [HttpPost]
        public IActionResult Create()
        {
            var id = _service.Create();
            return Ok(id);
        }

        [HttpPut("{id}/open")]
        public IActionResult Open(string id)
        {
            try
            {
                _service.Open(id);
                return Ok();
            }
            catch (RouletteNotFoundException)
            {
                return NotFound($"La ruleta con identificador {id} no existe");
            }
            catch (RouletteAlreadyOpenedException)
            {
                return BadRequest($"La ruleta con identificador {id} ya se encuentra abierta");
            }
            catch (RouletteIsCloseException)
            {
                return BadRequest($"La ruleta con identificador {id} esta cerrada definitivamente");
            }
        }

        [HttpPut("{id}/bet")]
        public IActionResult Bet(string id, [FromHeader] string userId, [FromBody] BetRequest request)
        {
            try
            {
                var parameters = new BetParameters
                {
                    Value = request.Value,
                    Money = request.Money.Value,
                    RouletteId = id,
                    Type = (BetTypeEnum)request.Type.Value,
                    UserId = userId
                };

                _service.Bet(parameters);
                return Ok();
            }
            catch (InvalidBetTypeException)
            {
                string message = "No se ha ingresado una apuesta valida";
                switch (request.Type)
                    {
                        case 1:
                        message = $"{message}, Debe ser un número entre 0 y 36";
                            break;
                    case 2:
                        message = $"{message}, Debe ser un color: RED o BLACK";
                        break;
                }
                return BadRequest(message);
            }
            catch (AmmountOutOfRangeException)
            {
                return BadRequest($"El monto de la apuesta debe estar entre $1 y $10.000");
            }
            catch (RouletteIsCloseException)
            {
                return BadRequest($"La ruleta con identificador {id} esta cerrada");
            }
        }

        [HttpPut("{id}/close")]
        public IActionResult Close(string id)
        {
            try
            {
                var bets = _service.Close(id);
                return Ok(bets);
            }
            catch (RouletteNotFoundException)
            {
                return NotFound($"La ruleta con identificador {id} no existe");
            }
            catch (RouletteIsCloseException)
            {
                return BadRequest($"La ruleta con identificador {id} ya se encuentra cerrada");
            }
        }
    }
}
