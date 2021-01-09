using System;
using System.Collections.Generic;
using System.Text;

namespace Roulette.Masiv.Entities.DTO
{
    public class CloseDto
    {
        public int WinnerNumber { get; set; }
        public string WinnerColor { get; set; }
        public List<BetDto> Bets { get; set; } = new List<BetDto>();
    }
}
