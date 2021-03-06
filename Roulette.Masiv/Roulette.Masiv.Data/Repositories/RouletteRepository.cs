﻿using EasyCaching.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using RouletteEntity = Roulette.Masiv.Entities.Persistence.Roulette;

namespace Roulette.Masiv.Data.Repositories
{
    public class RouletteRepository: IRouletteRepository
    {
        private readonly IEasyCachingProviderFactory _cachingFactory;
        private IEasyCachingProvider cachingProvider;

        private string TableKey = Environment.GetEnvironmentVariable("RouletteTable");

        public RouletteRepository(IEasyCachingProviderFactory cachingFactory)
        {
            _cachingFactory = cachingFactory;
            cachingProvider = _cachingFactory.GetCachingProvider("roulettedb");
        }

        public RouletteEntity GetById(string id)
        {
            var roulette = cachingProvider.Get<RouletteEntity>($"{TableKey}{id}");

            return roulette.Value;
        }

        public List<RouletteEntity> GetAll()
        {
            var rouletes = this.cachingProvider.GetByPrefix<RouletteEntity>(TableKey);
            if (rouletes.Count == 0)
            {
                return new List<RouletteEntity>();
            }
            return new List<RouletteEntity>(rouletes.Select(x => x.Value.Value));
        }

        public void Save(RouletteEntity roulette)
        {
            cachingProvider.Set($"{TableKey}{roulette.Id}", roulette, TimeSpan.FromDays(10));
        }
    }
}
