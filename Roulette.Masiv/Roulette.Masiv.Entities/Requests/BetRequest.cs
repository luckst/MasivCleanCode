using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace Roulette.Masiv.Entities.Requests
{
    [SwaggerSchema(Required = new[] { "Description" })]
    public class BetRequest
    {
        [Required]
        [SwaggerSchema("Type of the bet, 1: NUMBER, 2: COLOR")]
        public int? Type { get; set; }

        [Required]
        [SwaggerSchema("Value of the bet; If type is 1: value between 0 and 36, If type is 2: value must be RED or BLACK")]
        public string Value { get; set; }

        [Required]
        [SwaggerSchema("Ammount of money bet")]
        public decimal? Money { get; set; }
    }
}
