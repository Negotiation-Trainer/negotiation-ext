﻿using System;
using ModelLibrary;

namespace ServiceLibrary
{
    public class BuildingService
    {
        private const int BuildingResourceCost = 10;
        
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

        public void RemoveBuildingResourcesFromInventory(Tribe builder, InventoryItems resource)
        {
            builder.Inventory.RemoveFromInventory(resource, BuildingResourceCost);
        }
        
        public void AddBuildingPoints(Tribe[] tribes, InventoryItems resource, Tribe builder)
        {
            foreach (var tribe in tribes)
            {
                if (tribe.PointTable != null)
                {
                    tribe.Points += tribe.PointTable[(resource, builder)];
                }
                else
                {
                    
                }
            }
        }
    }
}