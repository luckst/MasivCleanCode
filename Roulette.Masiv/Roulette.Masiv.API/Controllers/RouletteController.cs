using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Roulette.Masiv.Business.Services;
using Roulette.Masiv.Common.CustomExceptions;
using Roulette.Masiv.Entities.DTO;
using Roulette.Masiv.Entities.Enums;
using Roulette.Masiv.Entities.Requests;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;

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

        [SwaggerOperation(
            Summary = "Gets all roulettes",
            OperationId = "GetAll"
        )]
        [SwaggerResponse(200, "List of roulettes", typeof(List<Entities.Persistence.Roulette>))]
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_service.GetAll());
        }

        [SwaggerOperation(
            Summary = "Create roullete",
            OperationId = "Create"
        )]
        [SwaggerResponse(200, "return Id created", typeof(string))]
        [HttpPost]
        public IActionResult Create()
        {
            var id = _service.Create();
            return Ok(id);
        }

        [SwaggerOperation(
            Summary = "Open roullete",
            OperationId = "Open"
        )]
        [SwaggerResponse(StatusCodes.Status200OK, "return roulette was opened")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Roulette not found")]
        [SwaggerResponse(StatusCodes.Status406NotAcceptable, "Roulette is already open")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Roulette is definetly closed")]
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
                return StatusCode(StatusCodes.Status406NotAcceptable, $"La ruleta con identificador {id} ya se encuentra abierta");
            }
            catch (RouletteIsCloseException)
            {
                return BadRequest($"La ruleta con identificador {id} esta cerrada definitivamente");
            }
        }

        [SwaggerOperation(
            Summary = "Place a bet",
            OperationId = "Bet"
        )]
        [SwaggerResponse(StatusCodes.Status200OK, "return bet was placed successfully")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Roulette not found")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid bet")]
        [SwaggerResponse(StatusCodes.Status402PaymentRequired, "Invalid bet ammount")]
        [SwaggerResponse(StatusCodes.Status406NotAcceptable, "Roulette is closed")]
        [HttpPut("{id}/bet")]
        public IActionResult Bet(string id, [FromHeader, SwaggerParameter("User id", Required = true)] string userId, [FromBody, SwaggerRequestBody("Bet request", Required = true)] BetRequest request)
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
            catch (RouletteNotFoundException)
            {
                return NotFound($"La ruleta con identificador {id} no existe");
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
                return StatusCode(StatusCodes.Status402PaymentRequired, $"El monto de la apuesta debe estar entre $1 y $10.000");
            }
            catch (RouletteIsCloseException)
            {
                return StatusCode(StatusCodes.Status406NotAcceptable, $"La ruleta con identificador {id} esta cerrada");
            }
        }

        [SwaggerOperation(
            Summary = "Close roulette",
            OperationId = "Close"
        )]
        [SwaggerResponse(StatusCodes.Status200OK, "return bet was placed successfully", typeof(CloseDto))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Roulette not found")]
        [SwaggerResponse(StatusCodes.Status406NotAcceptable, "Roulette is closed")]
        [HttpPut("{id}/close")]
        public IActionResult Close(string id)
        {
            try
            {
                var response = _service.Close(id);
                return Ok(response);
            }
            catch (RouletteNotFoundException)
            {
                return NotFound($"La ruleta con identificador {id} no existe");
            }
            catch (RouletteIsCloseException)
            {
                return StatusCode(StatusCodes.Status406NotAcceptable, $"La ruleta con identificador {id} ya se encuentra cerrada");
            }
        }
    }
}
