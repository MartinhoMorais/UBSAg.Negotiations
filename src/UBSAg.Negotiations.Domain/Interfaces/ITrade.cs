namespace UBSAg.Negotiations.Domain.Interfaces
{
    internal interface ITrade
    {
        double Value { get; }
        string ClientSector { get; }
    }
}
