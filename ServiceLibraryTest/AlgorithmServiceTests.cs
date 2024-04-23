using System;
using System.Collections.Generic;
using ModelLibrary;
using ModelLibrary.Exceptions;
using ServiceLibrary;
using ServiceLibrary.Algorithm;

namespace ServiceLibraryTest
{
    public class AlgorithmServiceTests
    {
        private Random random;
        
        [SetUp]
        public void Setup()
        {
            random = new Random();
        }
        // A Test behaves as an ordinary method
        [Test]
        public void CalculateRandomness_HighRandomness_ReturnsTrue()
        {
            //Given
            Randomness randomness = new Randomness(random);

            //when
            var result = randomness.Calculate(1f);
            
            //Then
            Assert.IsTrue(result);
        }
        
        [Test]
        public void CalculateRandomness_LowRandomness_ReturnsFalse()
        {
            //Given
            Randomness randomness = new Randomness(random);

            //when
            var result = randomness.Calculate(0f);
            
            //Then
            Assert.IsFalse(result);
        }
        
        [Test]
        public void CalculateSelfBuild_HigerThanBorder_ReturnsFalse()
        {
            //Given
            SelfBuild selfBuild = new SelfBuild(random);
            Tribe tribe = new Tribe("test");
            Trade trade = new Trade(InventoryItems.Wood,3,InventoryItems.Stone,3, "target", "originator");
            
            //When
            tribe.Inventory.AddToInventory(InventoryItems.Wood,8);
            
            //Then
            Assert.Throws<SelfBuildException>(() => selfBuild.Calculate(trade, tribe));
        }
        
        [Test]
        public void CalculateSelfBuild_LowerThanBorder_ReturnsTrue()
        {
            //Given
            SelfBuild selfBuild = new SelfBuild(random);
            Tribe tribe = new Tribe("test");
            Trade trade = new Trade(InventoryItems.Wood,3,InventoryItems.Stone,3, "target", "originator");
            
            //When
            tribe.Inventory.AddToInventory(InventoryItems.Wood,1);
            
            //Then
            Assert.DoesNotThrow(() => selfBuild.Calculate(trade, tribe));
        }
        
        [Test]
        public void CalculateBuildEffect_GoodEffect_ReturnsTrue()
        {
            //Given
            BuildEffect buildEffect = new BuildEffect(random);
            Tribe originator = new Tribe("originator");
            Tribe target = new Tribe("target");
            Trade trade = new Trade(InventoryItems.Wood,3,InventoryItems.Stone,3, target.Name, originator.Name);
            
            //When
            target.PointTable = new Dictionary<(InventoryItems, Tribe), int>
            {
                [(InventoryItems.Wood, target)] = 10,
                [(InventoryItems.Wood, originator)] = 5
            };
            
            //Then
            Assert.DoesNotThrow(() => buildEffect.Calculate(trade, target, originator));
        }
        
        [Test]
        public void CalculateBuildEffect_BadEffect_ReturnsFalse()
        {
            //Given
            BuildEffect buildEffect = new BuildEffect(random);
            Tribe originator = new Tribe("originator");
            Tribe target = new Tribe("target");
            Trade trade = new Trade(InventoryItems.Wood,3,InventoryItems.Stone,3, target.Name, originator.Name);
            
            //When
            target.PointTable = new Dictionary<(InventoryItems, Tribe), int>
            {
                [(InventoryItems.Wood, target)] = 10,
                [(InventoryItems.Wood, originator)] = -5
            };
            
            //Then
            Assert.Throws<BuildEffectException>(() => buildEffect.Calculate(trade, target, originator));
        }
        
        [Test]
        public void CalculateUsefulness_IsUseful_ReturnsTrue()
        {
            //Given
            Usefulness usefulness = new Usefulness(random);
            Tribe originator = new Tribe("originator");
            Tribe target = new Tribe("target");
            Trade trade = new Trade(InventoryItems.Wood,3,InventoryItems.Stone,3, target.Name, originator.Name);
            
            //When
            target.Inventory.AddToInventory(InventoryItems.Stone,3);
            
            //Then
            Assert.DoesNotThrow(() => usefulness.Calculate(trade, target));
        }
        
        [Test]
        public void CalculateUsefulness_IsNotUseful_ReturnsFalse()
        {
            //Given
            Usefulness usefulness = new Usefulness(random);
            Tribe originator = new Tribe("originator");
            Tribe target = new Tribe("target");
            Trade trade = new Trade(InventoryItems.Wood,1,InventoryItems.Stone,1, target.Name, originator.Name);
            
            //When
            target.Inventory.AddToInventory(InventoryItems.Stone,3);
            
            //Then
            Assert.Throws<UsefulnessException>(() => usefulness.Calculate(trade, target));
        }
        
        [Test]
        public void CalculateTradeBalance_IsFavorable_ReturnsTrue()
        {
            //Given
            TradeBalance tradeBalance = new TradeBalance(random);
            Tribe originator = new Tribe("originator");
            Tribe target = new Tribe("target");
            originator.GoodWill.Add(target, 0);
            target.GoodWill.Add(originator, 0);
            Trade trade = new Trade(InventoryItems.Wood,1,InventoryItems.Stone,2, target.Name, originator.Name);
            
            //When & Then
            Assert.DoesNotThrow(() => tradeBalance.Calculate(trade,target,originator));
        }
        
        [Test]
        public void CalculateTradeBalance_IsEqual_ReturnsTrue()
        {
            //Given
            TradeBalance tradeBalance = new TradeBalance(random);
            Tribe originator = new Tribe("originator");
            Tribe target = new Tribe("target");
            originator.GoodWill.Add(target, 0);
            target.GoodWill.Add(originator, 0);
            Trade trade = new Trade(InventoryItems.Wood,1,InventoryItems.Stone,1, target.Name, originator.Name);
            
            //When & Then
            Assert.DoesNotThrow(() => tradeBalance.Calculate(trade,target,originator));
        }
        
        [Test]
        public void CalculateTradeBalance_IsUnfavorable_ReturnsFalse()
        {
            //Given
            TradeBalance tradeBalance = new TradeBalance(random);
            Tribe originator = new Tribe("originator");
            Tribe target = new Tribe("target");
            originator.GoodWill.Add(target, 0);
            target.GoodWill.Add(originator, 0);
            Trade trade = new Trade(InventoryItems.Wood,2,InventoryItems.Stone,1, target.Name, originator.Name);
            
            //When & Then
            Assert.Throws<TradeBalanceException>(() => tradeBalance.Calculate(trade, target, originator));
        }
        
        [Test]
        public void CalculateTradeBalance_IsFavorable_ReturnsTrueGetGoodWill()
        {
            //Given
            TradeBalance tradeBalance = new TradeBalance(random);
            Tribe originator = new Tribe("originator");
            Tribe target = new Tribe("target");
            originator.GoodWill.Add(target, 0);
            target.GoodWill.Add(originator, 0);
            Trade trade = new Trade(InventoryItems.Wood,1,InventoryItems.Stone,2, target.Name, originator.Name);
            
            
            //When & Then
            Assert.DoesNotThrow(() => tradeBalance.Calculate(trade,target,originator));
            Assert.That(target.GoodWill[originator], Is.EqualTo(1));
        }
        
        [Test]
        public void CalculateTradeBalance_IsUnfavorableButHasGoodWill_ReturnsTrue()
        {
            //Given
            TradeBalance tradeBalance = new TradeBalance(random);
            Tribe originator = new Tribe("originator");
            Tribe target = new Tribe("target");
            originator.GoodWill.Add(target, 0);
            target.GoodWill.Add(originator, 0);
            Trade trade = new Trade(InventoryItems.Wood,2,InventoryItems.Stone,1, target.Name, originator.Name);
            
            //When & Then
            target.GoodWill[originator] = 1;
            Assert.DoesNotThrow(() => tradeBalance.Calculate(trade, target, originator));
            Assert.That(target.GoodWill[originator], Is.EqualTo(0));
        }
        
        [Test]
        public void Decide_1()
        {
            // Given
            var algorithmService = new AlgorithmService();
            var tradeTarget = new Tribe("A");
            var tradeOriginator = new Tribe("B");
            var trade = new Trade(InventoryItems.Stone, 2, InventoryItems.Wood, 2, tradeTarget.Name, tradeOriginator.Name);
            
            tradeOriginator.PointTable = new Dictionary<(InventoryItems, Tribe), int>
            {
                [(InventoryItems.Wood, tradeOriginator)] = 10,
                [(InventoryItems.Wood, tradeTarget)] = -5,

                [(InventoryItems.Lenses, tradeOriginator)] = 10,
                [(InventoryItems.Lenses, tradeTarget)] = 5,

                [(InventoryItems.Clay, tradeOriginator)] = 10,
                [(InventoryItems.Clay, tradeTarget)] = 0,

                [(InventoryItems.Gold, tradeOriginator)] = 10,
                [(InventoryItems.Gold, tradeTarget)] = 5,

                [(InventoryItems.Steel, tradeOriginator)] = 10,
                [(InventoryItems.Steel, tradeTarget)] = 0,

                [(InventoryItems.Insulation, tradeOriginator)] = 10,
                [(InventoryItems.Insulation, tradeTarget)] = 5,

                [(InventoryItems.Fertilizer, tradeOriginator)] = 10,
                [(InventoryItems.Fertilizer, tradeTarget)] = 0,

                [(InventoryItems.Stone, tradeOriginator)] = 10,
                [(InventoryItems.Stone, tradeTarget)] = 0,
            };

            tradeTarget.PointTable = new Dictionary<(InventoryItems, Tribe), int>
            {
                [(InventoryItems.Wood, tradeOriginator)] = 0,
                [(InventoryItems.Wood, tradeTarget)] = 10,

                [(InventoryItems.Lenses, tradeOriginator)] = -5,
                [(InventoryItems.Lenses, tradeTarget)] = 10,

                [(InventoryItems.Clay, tradeOriginator)] = 5,
                [(InventoryItems.Clay, tradeTarget)] = 10,

                [(InventoryItems.Gold, tradeOriginator)] = 5,
                [(InventoryItems.Gold, tradeTarget)] = 10,

                [(InventoryItems.Steel, tradeOriginator)] = 0,
                [(InventoryItems.Steel, tradeTarget)] = 10,

                [(InventoryItems.Insulation, tradeOriginator)] = 5,
                [(InventoryItems.Insulation, tradeTarget)] = 10,

                [(InventoryItems.Fertilizer, tradeOriginator)] = 0,
                [(InventoryItems.Fertilizer, tradeTarget)] = 10,

                [(InventoryItems.Stone, tradeOriginator)] = 0,
                [(InventoryItems.Stone, tradeTarget)] = 10,
            };
            // When & Then
            algorithmService.Decide(trade, tradeOriginator, tradeTarget);
            algorithmService.AlgorithmDecision += (sender, args) =>
            {
                Assert.That(args.issuesWithTrade, Is.Empty);
                Assert.That(args.counterOffer?.originName, Is.EqualTo(trade.targetName));
                Assert.That(args.counterOffer?.targetName, Is.EqualTo(trade.originName));
            };
        }
    }
}
