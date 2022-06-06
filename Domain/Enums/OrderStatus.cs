namespace Domain.Enums
{
    public enum OrderStatus
    {
        Pending,
        AwaitingPayment,
        AwaitingShipment,
        AwaitingPickup,
        PartiallyShipped,
        Completed,
        Cancelled,
        Declined,
        Refunded
    }
}
