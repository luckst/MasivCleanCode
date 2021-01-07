using System;
using System.Collections.Generic;
using System.Text;

namespace Roulette.Masiv.Entities.Persistence
{
    [Serializable]
    public class Bet
    {
        public string Value { get; set; }
        public string UserId { get; set; }
        public decimal Money { get; set; }
    }
}
