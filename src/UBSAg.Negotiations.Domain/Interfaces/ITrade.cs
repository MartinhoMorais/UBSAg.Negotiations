namespace UBSAg.Negotiations.Domain.Interfaces
{
    internal interface ITrade
    {
        decimal Value { get; }
        string ClientSector { get; }
    }
}
