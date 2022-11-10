namespace Domain.Enums
{
    public enum OrderStatusEnum
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

    public static class OrderStatusEnumExtension
    {
        public static string getValue(this OrderStatusEnum orderStatus)
        {
            switch (orderStatus)
            {
                case OrderStatusEnum.Pending:
                    return "pending";
                case OrderStatusEnum.AwaitingPayment:
                    return "awaiting payment";
                case OrderStatusEnum.AwaitingShipment:
                    return "awaiting shipment";
                case OrderStatusEnum.AwaitingPickup:
                    return "awaiting pickup";
                case OrderStatusEnum.PartiallyShipped:
                    return "partially shipped";
                case OrderStatusEnum.Completed:
                    return "completed";
                case OrderStatusEnum.Cancelled:
                    return "cancelled";
                case OrderStatusEnum.Declined:
                    return "declined";
                case OrderStatusEnum.Refunded:
                    return "refunded";
                default:
                    throw new ArgumentException("Wrong order status");
            }
        }
    }
}
