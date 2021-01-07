using System;
using System.Collections.Generic;
using System.Text;

namespace Roulette.Masiv.Entities.Persistence
{
    [Serializable]
    public class Roulette
    {
        public string Id { get; set; }
        public bool Status { get; set; }
        public List<Bet> Bets { get; set; } = new List<Bet>();
        public DateTime CreationDate { get; set; }
        public DateTime? OpenDate { get; set; }
        public DateTime? CloseDate { get; set; }
    }
}
