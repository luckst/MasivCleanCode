using Roulette.Masiv.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Roulette.Masiv.Entities.DTO
{
    public class BetParameters
    {
        public string RouletteId { get; set; }
        public string UserId { get; set; }
        public BetTypeEnum Type { get; set; }
        public string Value { get; set; }
        public decimal Money { get; set; }
    }
}
