using Roulette.Masiv.Common.CustomExceptions;
using Roulette.Masiv.Data.Repositories;
using Roulette.Masiv.Entities.DTO;
using Roulette.Masiv.Entities.Enums;
using Roulette.Masiv.Entities.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public string Create()
        {
            RouletteEntity roulette = new RouletteEntity
            {
                Id = Guid.NewGuid().ToString(),
                Status = false
            };

            _repository.Save(roulette);

            return roulette.Id;
        }

        public void Open(string id)
        {
            RouletteEntity roulette = _repository.GetById(id);

            ValidateRouletteExist(roulette);
            ValidateRouletteIsDefinitelyClosed(roulette);
            if (roulette.Status)
                throw new RouletteAlreadyOpenedException();

            roulette.Status = true;

            _repository.Save(roulette);
        }

        public void Bet(BetParameters parameters)
        {
            ValidateBetAmmount(parameters.Money);
            ValidateBetType(parameters);
            RouletteEntity roulette = _repository.GetById(parameters.RouletteId);
            ValidateRouletteExist(roulette);
            ValidateRouletteIsClosed(roulette);
            AddOrUpdateBet(parameters, roulette);

            _repository.Save(roulette);
        }

        public CloseDto Close(string id)
        {
            RouletteEntity roulette = _repository.GetById(id);
            ValidateRouletteExist(roulette);
            ValidateRouletteIsClosed(roulette);
            var winnerNumber = GetWinnerNumber();
            var winnerColor = GetWinnerColor(winnerNumber);
            CloseDto response = new CloseDto
            {
                Bets = GetBetsDto(roulette, winnerNumber, winnerColor),
                WinnerNumber = winnerNumber,
                WinnerColor = winnerColor
            };
        
            roulette.Status = false;
            _repository.Save(roulette);

            return response;
        }

        public List<RouletteEntity> GetAll()
        {
            return _repository.GetAll();
        }

        private List<BetDto> GetBetsDto(RouletteEntity roulette, int winnerNumber, string winnerColor)
        {
            return roulette.Bets.Select(b => new BetDto
            {
                MoneyBet = b.Money,
                UserId = b.UserId,
                Value = b.Value,
                Winner = ValidateWinner(b.Value, winnerNumber, winnerColor),
                Prize = ValidatePrize(b.Value, b.Money, winnerNumber, winnerColor)
            }).ToList();
        }

        private void ValidateRouletteIsDefinitelyClosed(RouletteEntity roulette)
        {
            if (!roulette.Status && roulette.Bets.Any())
                throw new RouletteIsCloseException();
        }

        private void ValidateRouletteIsClosed(RouletteEntity roulette)
        {
            if (!roulette.Status)
                throw new RouletteIsCloseException();
        }

        private void ValidateRouletteExist(RouletteEntity roulette)
        {
            if (roulette is null)
                throw new RouletteNotFoundException();
        }

        private bool ValidateWinner(string valueBet, int winnerNumber, string winnerColor)
        {
            int value = -1;
            if (!int.TryParse(valueBet, out value))
            {
                return winnerColor == valueBet;
            }

            return winnerNumber == value;
        }

        private decimal ValidatePrize(string valueBet, decimal moneyBet, int winnerNumber, string winnerColor)
        {
            decimal prize = 0;
            if (ValidateWinner(valueBet, winnerNumber, winnerColor))
            {
                prize = CalculatePrize(valueBet, moneyBet);
            }

            return prize;
        }

        private decimal CalculatePrize(string valueBet, decimal moneyBet)
        {
            decimal prize;
            if (int.TryParse(valueBet, out _))
            {
                prize = moneyBet * 5;
            }
            else
            {
                prize = moneyBet * 1.8m;
            }

            return prize;
        }

        private string GetWinnerColor(int winnerNumber)
        {
            return winnerNumber % 2 == 0 ? BetColorEnum.RED.ToString() : BetColorEnum.BLACK.ToString();
        }

        private int GetWinnerNumber()
        {
            return new Random().Next(0, 36);
        }

        private void ValidateBetType(BetParameters parameters)
        {
            switch (parameters.Type)
            {
                case BetTypeEnum.NUMBER:
                    var value = -1;
                    if (!int.TryParse(parameters.Value, out value))
                        throw new InvalidBetTypeException();

                    if (value < 0 || value > 36)
                        throw new InvalidBetTypeException();
                    break;
                case BetTypeEnum.COLOR:
                    if (parameters.Value.ToUpper() != BetColorEnum.RED.ToString() &&
                        parameters.Value.ToUpper() != BetColorEnum.BLACK.ToString())
                        throw new InvalidBetTypeException();
                    break;
                default:
                    throw new InvalidBetTypeException();
            }
        }

        private void ValidateBetAmmount(decimal ammount)
        {
            if (ammount <= 0 || ammount > 10000)
                throw new AmmountOutOfRangeException();
        }

        private void AddOrUpdateBet(BetParameters parameters, RouletteEntity roulette)
        {
            var existentBet = roulette.Bets.SingleOrDefault(b => b.UserId == parameters.UserId && b.Value == parameters.Value);

            if (existentBet is null)
            {
                roulette.Bets.Add(new Bet
                {
                    Money = parameters.Money,
                    UserId = parameters.UserId,
                    Value = parameters.Value.ToUpper()
                });
            }
            else
                existentBet.Money += parameters.Money;
        }
    }
}
