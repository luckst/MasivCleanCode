using Roulette.Masiv.Data.Repositories;
using System;
using System.Collections.Generic;
using RouletteEntity = Roulette.Masiv.Entities.Persistence.Roulette;

namespace Roulette.Masiv.Business.Services
{
    public class RouletteService : IRouletteService
    {
        private readonly IRouletteRepository _repository;

        public RouletteService(IRouletteRepository repository)
        {
            _repository = repository;
        }

        public List<RouletteEntity> GetAll()
        {
            return _repository.GetAll();
        }
    }
}
