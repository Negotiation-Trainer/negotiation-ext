using System.Collections.Generic;
using ModelLibrary;

namespace ModelLibraryTest
{
    public class TribeTests
    {
        [Test]
        public void PointTable_GetPoints_ReturnsPoints()
        {
            //Given
            Tribe tribe = new Tribe("tribe");
            Tribe tribe2 = new Tribe("tribe2");
            tribe.PointTable = new Dictionary<(InventoryItems, Tribe), int>
            {
                [(InventoryItems.Wood, tribe)] = 10,
                [(InventoryItems.Wood, tribe2)] = -5
            };
            
            //Then
            Assert.That(tribe.PointTable[(InventoryItems.Wood,tribe)], Is.EqualTo(10));
        }
    }
}
