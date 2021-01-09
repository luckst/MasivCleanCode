namespace Roulette.Masiv.Entities.DTO
{
    public class BetDto
    {
        public string Value { get; set; }
        public string UserId { get; set; }
        public decimal MoneyBet { get; set; }
        public bool Winner { get; set; }
        public decimal Prize { get; set; }
    }
}
