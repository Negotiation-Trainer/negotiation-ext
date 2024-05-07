namespace ModelLibrary
{
    public class Trade(InventoryItems requestedItem, int requestedAmount, InventoryItems offeredItem, int offeredAmount, string targetName, string originName)
    {
        public InventoryItems RequestedItem { get; private set; } = requestedItem;
        public int RequestedAmount { get; private set; } = requestedAmount;

        public InventoryItems OfferedItem { get; private set; } = offeredItem;
        public int OfferedAmount { get; private set; } = offeredAmount;

        public string targetName { get; private set; } = targetName;
        public string originName { get; private set; } = originName;
    }
}