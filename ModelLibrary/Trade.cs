namespace ModelLibrary
{
    public class Trade(InventoryItems requestedItem, int requestedAmount, InventoryItems offeredItem, int offeredAmount)
    {
        public InventoryItems RequestedItem { get; private set; } = requestedItem;
        public int RequestedAmount { get; private set; } = requestedAmount;

        public InventoryItems OfferedItem { get; private set; } = offeredItem;
        public int OfferedAmount { get; private set; } = offeredAmount;
        
        public Trade(InventoryItems requestedItem, int requestedAmount, InventoryItems offeredItem, int offeredAmount, string name, string item) : this(requestedItem, requestedAmount, offeredItem, offeredAmount)
        {
            RequestedItem = requestedItem;
            RequestedAmount = requestedAmount;
            OfferedItem = offeredItem;
            OfferedAmount = offeredAmount;
        }
    }
}