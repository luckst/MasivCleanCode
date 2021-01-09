using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Roulette.Masiv.Entities.Requests
{
    public class BetRequest
    {
        [Required]
        public int? Type { get; set; }
        [Required]
        public string Value { get; set; }
        [Required]
        public decimal? Money { get; set; }
    }
}
