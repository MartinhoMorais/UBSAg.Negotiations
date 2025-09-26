using UBSAg.Negotiations.Domain.Interfaces;

namespace UBSAg.Negotiations.Domain.Entities
{
    public class Trade : ITrade
    {
        public double Value { get; }
        public string ClientSector { get; }

        public Trade() { }
        public Trade(double value, string clientSector)
        {
            Value=value;
            ClientSector=clientSector;
        }
    }
}
