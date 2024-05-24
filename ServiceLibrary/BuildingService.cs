using System;
using ModelLibrary;

namespace ServiceLibrary
{
    public class BuildingService
    {
        private const int BuildingResourceCost = 10;
        
        /// <summary>
        /// Checks if its possible for a tribe to build a building
        /// </summary>
        /// <param name="tribe">The tribe trying to build a building</param>
        /// <returns>Returns the InventoryItem needed to build the building when its possible otherwise NULL is returned. </returns>
        public InventoryItems? CheckIfBuildingPossible(Tribe tribe)
        {
            var resources = Enum.GetValues(typeof(InventoryItems));
            foreach (InventoryItems resource in resources)
            {
                if(resource == InventoryItems.Fertilizer) continue;
                if (tribe.Inventory.GetInventoryAmount(resource) >= BuildingResourceCost)
                {
                    return resource;
                }
            }
            return null;
        }

        /// <summary>
        /// Removes the resources needed to build a building from the inventory of the tribe
        /// </summary>
        /// <param name="builder">The tribe trying to build the building</param>
        /// <param name="resource">The resource needed to build the building</param>
        public void RemoveBuildingResourcesFromInventory(Tribe builder, InventoryItems resource)
        {
            builder.Inventory.RemoveFromInventory(resource, BuildingResourceCost);
        }
        
        /// <summary>
        /// Add the points to the tribes based on the resource and the builder
        /// </summary>
        /// <param name="tribes">Collection of all the tribes</param>
        /// <param name="resource">The resources needed for the build</param>
        /// <param name="builder">The builder of the building</param>
        public void AddBuildingPoints(Tribe[] tribes, InventoryItems resource, Tribe builder)
        {
            foreach (var tribe in tribes)
            {
                if (tribe.PointTable != null)
                {
                    tribe.Points += tribe.PointTable[(resource, builder)];
                }
            }
        }
    }
}