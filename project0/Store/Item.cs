namespace Store
{
    public class Item
    { 
        private static uint itemTally;
        readonly uint itemId;
        private readonly string itemName;
        public Item(string itemName)
        {
            this.itemName = itemName;
            this.itemId = itemTally++;
        }
        public string ItemName => itemName;

        public uint ItemId => itemId;
        public override string ToString() {
            return this.itemName.ToString();
        }
    }
}