using System.Collections.Generic;
using RouletteEntity = Roulette.Masiv.Entities.Persistence.Roulette;

namespace Roulette.Masiv.Business.Services
{
    public interface IRouletteService
    {
        List<RouletteEntity> GetAll();
    }
}
