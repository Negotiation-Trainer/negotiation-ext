using ModelLibrary;

namespace ModelLibraryTest
{
    public class InventoryTests
    {
        [Test]
        public void Add_OneResources_ReturnsSameNumber()
        {
            //Given
            Inventory inventory = new Inventory();
            
            //When
            inventory.AddToInventory(InventoryItems.Wood,1);
            var result = inventory.GetInventoryAmount(InventoryItems.Wood);
            
            //Then
            Assert.That(result, Is.EqualTo(1));
        }
    
        [Test]
        public void Add_ZeroResources_ReturnsSameNumber()
        {
            //Given
            Inventory inventory = new Inventory();
            
            //When
            inventory.AddToInventory(InventoryItems.Wood,0);
            var result = inventory.GetInventoryAmount(InventoryItems.Wood);
            
            //Then
            Assert.That(result, Is.EqualTo(0));
        }
    
        [Test]
        public void Remove_FromEmptyInventory_ReturnsZero()
        {
            //Given
            Inventory inventory = new Inventory();
            
            //When
            inventory.RemoveFromInventory(InventoryItems.Wood,1);
            var result = inventory.GetInventoryAmount(InventoryItems.Wood);
            
            //Then
            Assert.That(result, Is.EqualTo(0));
        }
    
        [Test]
        public void Remove_FromFilledInventory_ReturnsLowerNumber()
        {
            //Given
            Inventory inventory = new Inventory();
            
            //When
            inventory.AddToInventory(InventoryItems.Wood,2);
            inventory.RemoveFromInventory(InventoryItems.Wood,2);
            var result = inventory.GetInventoryAmount(InventoryItems.Wood);
            
            //Then
            Assert.That(result, Is.EqualTo(0));
        }
    
        [Test]
        public void Event_InventoryUpdate_Add_Invoked()
        {
            //Given
            Inventory inventory = new Inventory();
            bool eventInvoked = false;
            inventory.InventoryUpdate += (sender, args) => eventInvoked = true;
        
            //When
            inventory.AddToInventory(InventoryItems.Wood,1);
        
            //Then
            Assert.IsTrue(eventInvoked);
        }
    
        [Test]
        public void Event_InventoryUpdate_Remove_Invoked()
        {
            //Given
            Inventory inventory = new Inventory();
            inventory.AddToInventory(InventoryItems.Wood,1);
            bool eventInvoked = false;
            inventory.InventoryUpdate += (sender, args) => eventInvoked = true;
        
            //When
            inventory.RemoveFromInventory(InventoryItems.Wood,1);
        
            //Then
            Assert.IsTrue(eventInvoked);
        }
    
        [Test]
        public void Inventory_ToString_ReturnsCorrectString()
        {
            //Given
            Inventory inventory = new Inventory();
            inventory.AddToInventory(InventoryItems.Wood,1);
            inventory.AddToInventory(InventoryItems.Stone,2);
        
            //When
            string expected = "Wood: 1 Lenses: 0 Clay: 0 Gold: 0 Steel: 0 Insulation: 0 Fertilizer: 0 Stone: 2";
            var result = inventory.ToString();
        
            //Then
            Assert.That(result, Is.EqualTo(expected));
        }
    }
}
