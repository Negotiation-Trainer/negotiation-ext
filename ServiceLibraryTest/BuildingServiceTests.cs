using System.Collections.Generic;
using Enums;
using Models;
using ServiceLibrary;

namespace ServiceLibraryTest
{
    public class BuildingServiceTests
    {
        [Test]
        public void CheckIfBuildingPossible_NotPossible_ReturnsNull()
        {
            //Given
            BuildingService buildingService = new BuildingService();
            Tribe tribe = new Tribe("tribe");

            //when
            var result = buildingService.CheckIfBuildingPossible(tribe);

            //Then
            Assert.IsNull(result);
        }

        [Test]
        public void CheckIfBuildingPossible_IsPossible_ReturnsInventoryItem()
        {
            //Given
            BuildingService buildingService = new BuildingService();
            Tribe tribe = new Tribe("tribe");
            tribe.Inventory.AddToInventory(InventoryItems.Gold, 10);

            //when
            var result = buildingService.CheckIfBuildingPossible(tribe);

            //Then
            Assert.That(result, Is.EqualTo(InventoryItems.Gold));
        }

        [Test]
        public void AddBuildingPoints_BuildWood_AddPoints()
        {
            //Given
            BuildingService buildingService = new BuildingService();
            Tribe tribe = new Tribe("tribe");
            Tribe tribe2 = new Tribe("tribe2");
            tribe.PointTable = new Dictionary<(InventoryItems, Tribe), int>
            {
                [(InventoryItems.Wood, tribe)] = 10,
                [(InventoryItems.Wood, tribe2)] = -5
            };
            tribe2.PointTable = new Dictionary<(InventoryItems, Tribe), int>
            {
                [(InventoryItems.Wood, tribe)] = -5,
                [(InventoryItems.Wood, tribe2)] = 10
            };
            Tribe[] tribes = new[] {tribe, tribe2};

            //when
            buildingService.AddBuildingPoints(tribes,InventoryItems.Wood,tribe);

            //Then
            Assert.That(tribe.Points, Is.EqualTo(10));
            Assert.That(tribe2.Points, Is.EqualTo(-5));
        }
        
        [Test]
        public void RemoveBuildingResourcesFromInventory_BuildWood_RemovesResources()
        {
            //Given
            BuildingService buildingService = new BuildingService();
            Tribe tribe = new Tribe("tribe");
            tribe.Inventory.AddToInventory(InventoryItems.Wood,10);

            //when
            buildingService.RemoveBuildingResourcesFromInventory(tribe,InventoryItems.Wood);
            var result = tribe.Inventory.GetInventoryAmount(InventoryItems.Wood);

            //Then
            Assert.That(result, Is.EqualTo(0));
        }
    }
}
