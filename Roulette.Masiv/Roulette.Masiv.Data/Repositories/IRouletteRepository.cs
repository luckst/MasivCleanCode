using System.Collections.Generic;
using RouletteEntity = Roulette.Masiv.Entities.Persistence.Roulette;

namespace Roulette.Masiv.Data.Repositories
{
    public interface IRouletteRepository
    {
        RouletteEntity GetById(string id);
        List<RouletteEntity> GetAll();
        void Save(RouletteEntity roulette);
    }
}
