using Roulette.Masiv.Entities.DTO;
using System.Collections.Generic;
using RouletteEntity = Roulette.Masiv.Entities.Persistence.Roulette;

namespace Roulette.Masiv.Business.Services
{
    public interface IRouletteService
    {
        List<RouletteEntity> GetAll();
        string Create();
        void Open(string id);
        void Bet(BetParameters parameters);
        List<BetDto> Close(string id);
    }
}
