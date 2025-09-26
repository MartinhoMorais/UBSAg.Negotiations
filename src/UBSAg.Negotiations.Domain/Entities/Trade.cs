using UBSAg.Negotiations.Domain.Interfaces;

namespace UBSAg.Negotiations.Domain.Entities
{
    public class Trade : ITrade
    {
        public decimal Value { get; }
        public string ClientSector { get; }        

        public Trade(decimal value, string clientSector)
        {
            Value=value;
            ClientSector=clientSector;
        }
    }
}
