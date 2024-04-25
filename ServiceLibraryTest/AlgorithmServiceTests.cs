using System;
using System.Collections;
using System.Collections.Generic;
using ModelLibrary;
using ModelLibrary.Exceptions;
using ServiceLibrary;
using ServiceLibrary.Algorithm;

namespace ServiceLibraryTest
{
    public class AlgorithmServiceTests
    {
        private Random _random;
        private Tribe _tradeOriginator;
        private Tribe _tradeTarget;
        
        [SetUp]
        public void Setup()
        {
            _random = new Random();
            _tradeOriginator = new Tribe("A");
            _tradeTarget = new Tribe("B");
        }
        // A Test behaves as an ordinary method
        [Test]
        public void CalculateRandomness_HighRandomness_ReturnsTrue()
        {
            //Given
            Randomness randomness = new Randomness(_random);

            //when
            var result = randomness.Calculate(1f);
            
            //Then
            Assert.IsTrue(result);
        }
        
        [Test]
        public void CalculateRandomness_LowRandomness_ReturnsFalse()
        {
            //Given
            Randomness randomness = new Randomness(_random);

            //when
            var result = randomness.Calculate(0f);
            
            //Then
            Assert.IsFalse(result);
        }
        
        [Test]
        public void CalculateSelfBuild_HigerThanBorder_ReturnsFalse()
        {
            //Given
            SelfBuild selfBuild = new SelfBuild(_random);
            Trade trade = new Trade(InventoryItems.Wood,3,InventoryItems.Stone,3, _tradeTarget.Name, _tradeOriginator.Name);
            
            //When
            _tradeTarget.Inventory.AddToInventory(InventoryItems.Wood,8);
            
            //Then
            Assert.Throws<SelfBuildException>(() => selfBuild.Calculate(trade, _tradeTarget));
        }
        
        [Test]
        public void CalculateSelfBuild_LowerThanBorder_ReturnsTrue()
        {
            //Given
            SelfBuild selfBuild = new SelfBuild(_random);
            Tribe tribe = new Tribe("test");
            Trade trade = new Trade(InventoryItems.Wood,3,InventoryItems.Stone,3, _tradeTarget.Name, _tradeOriginator.Name);
            
            //When
            tribe.Inventory.AddToInventory(InventoryItems.Wood,1);
            
            //Then
            Assert.DoesNotThrow(() => selfBuild.Calculate(trade, tribe));
        }
        
        [Test]
        public void CalculateBuildEffect_GoodEffect_ReturnsTrue()
        {
            //Given
            BuildEffect buildEffect = new BuildEffect(_random);
            Trade trade = new Trade(InventoryItems.Wood,3,InventoryItems.Stone,3, _tradeTarget.Name, _tradeOriginator.Name);
            
            //When
            _tradeTarget.PointTable = new Dictionary<(InventoryItems, Tribe), int>
            {
                [(InventoryItems.Wood, _tradeTarget)] = 10,
                [(InventoryItems.Wood, _tradeOriginator)] = 5
            };
            
            //Then
            Assert.DoesNotThrow(() => buildEffect.Calculate(trade, _tradeTarget, _tradeOriginator));
        }
        
        [Test]
        public void CalculateBuildEffect_BadEffect_ReturnsFalse()
        {
            //Given
            BuildEffect buildEffect = new BuildEffect(_random);
            Trade trade = new Trade(InventoryItems.Wood,3,InventoryItems.Stone,3, _tradeTarget.Name, _tradeOriginator.Name);
            
            //When
            _tradeTarget.PointTable = new Dictionary<(InventoryItems, Tribe), int>
            {
                [(InventoryItems.Wood, _tradeTarget)] = 10,
                [(InventoryItems.Wood, _tradeOriginator)] = -5
            };
            
            //Then
            Assert.Throws<BuildEffectException>(() => buildEffect.Calculate(trade, _tradeTarget, _tradeOriginator));
        }
        
        [Test]
        public void CalculateUsefulness_IsUseful_ReturnsTrue()
        {
            //Given
            Usefulness usefulness = new Usefulness(_random);
            Trade trade = new Trade(InventoryItems.Wood,3,InventoryItems.Stone,3, _tradeTarget.Name, _tradeOriginator.Name);
            
            //When
            _tradeTarget.Inventory.AddToInventory(InventoryItems.Stone,3);
            
            //Then
            Assert.DoesNotThrow(() => usefulness.Calculate(trade, _tradeTarget));
        }
        
        [Test]
        public void CalculateUsefulness_IsNotUseful_ReturnsFalse()
        {
            //Given
            Usefulness usefulness = new Usefulness(_random);
            Trade trade = new Trade(InventoryItems.Wood,1,InventoryItems.Stone,1, _tradeTarget.Name, _tradeOriginator.Name);
            
            //When
            _tradeTarget.Inventory.AddToInventory(InventoryItems.Stone,3);
            
            //Then
            Assert.Throws<UsefulnessException>(() => usefulness.Calculate(trade, _tradeTarget));
        }
        
        [Test]
        public void CalculateTradeBalance_IsFavorable_ReturnsTrue()
        {
            //Given
            TradeBalance tradeBalance = new TradeBalance(_random);
            _tradeOriginator.GoodWill.Add(_tradeTarget, 0);
            _tradeTarget.GoodWill.Add(_tradeOriginator, 0);
            Trade trade = new Trade(InventoryItems.Wood,1,InventoryItems.Stone,2, _tradeTarget.Name, _tradeOriginator.Name);
            
            //When & Then
            Assert.DoesNotThrow(() => tradeBalance.Calculate(trade,_tradeTarget,_tradeOriginator));
        }
        
        [Test]
        public void CalculateTradeBalance_IsEqual_ReturnsTrue()
        {
            //Given
            TradeBalance tradeBalance = new TradeBalance(_random);
            _tradeOriginator.GoodWill.Add(_tradeTarget, 0);
            _tradeTarget.GoodWill.Add(_tradeOriginator, 0);
            Trade trade = new Trade(InventoryItems.Wood,1,InventoryItems.Stone,1, _tradeTarget.Name, _tradeOriginator.Name);
            
            //When & Then
            Assert.DoesNotThrow(() => tradeBalance.Calculate(trade,_tradeTarget,_tradeOriginator));
        }
        
        [Test]
        public void CalculateTradeBalance_IsUnfavorable_ReturnsFalse()
        {
            //Given
            TradeBalance tradeBalance = new TradeBalance(_random);
            _tradeOriginator.GoodWill.Add(_tradeTarget, 0);
            _tradeTarget.GoodWill.Add(_tradeOriginator, 0);
            Trade trade = new Trade(InventoryItems.Wood,2,InventoryItems.Stone,1, _tradeTarget.Name, _tradeOriginator.Name);
            
            //When & Then
            Assert.Throws<TradeBalanceException>(() => tradeBalance.Calculate(trade, _tradeTarget, _tradeOriginator));
        }
        
        [Test]
        public void CalculateTradeBalance_IsFavorable_ReturnsTrueGetGoodWill()
        {
            //Given
            TradeBalance tradeBalance = new TradeBalance(_random);
            _tradeOriginator.GoodWill.Add(_tradeTarget, 0);
            _tradeTarget.GoodWill.Add(_tradeOriginator, 0);
            Trade trade = new Trade(InventoryItems.Wood,1,InventoryItems.Stone,2, _tradeTarget.Name, _tradeOriginator.Name);
            
            
            //When & Then
            Assert.DoesNotThrow(() => tradeBalance.Calculate(trade,_tradeTarget,_tradeOriginator));
            Assert.That(_tradeTarget.GoodWill[_tradeOriginator], Is.EqualTo(1));
        }

        [Test]
        public void CalculateTradeBalance_IsUnfavorableButHasGoodWill_ReturnsTrue()
        {
            //Given
            TradeBalance tradeBalance = new TradeBalance(_random);
            _tradeOriginator.GoodWill.Add(_tradeTarget, 0);
            _tradeTarget.GoodWill.Add(_tradeOriginator, 0);
            Trade trade = new Trade(InventoryItems.Wood, 2, InventoryItems.Stone, 1, _tradeTarget.Name,
                _tradeOriginator.Name);

            //When & Then
            _tradeTarget.GoodWill[_tradeOriginator] = 1;
            Assert.DoesNotThrow(() => tradeBalance.Calculate(trade, _tradeTarget, _tradeOriginator));
            Assert.That(_tradeTarget.GoodWill[_tradeOriginator], Is.EqualTo(0));
        }

        [Test, TestCaseSource(nameof(TestCasesForDecideMethod))]
        public void Decide_WithSpecificPointsTable(Trade trade, List<Type> expectedException)
        {
            // Given
            var algorithmService = new AlgorithmService() {EnableRandomizedDecisions = false};

            // When & Then
            algorithmService.AlgorithmDecision += (sender, args) =>
            {
                Assert.That(args.issuesWithTrade.Count, Is.EqualTo(expectedException.Count));
                args.issuesWithTrade.ForEach(issue => Assert.That(expectedException, Does.Contain(issue.GetType())));
            };
            algorithmService.Decide(trade, _tradeOriginator, _tradeTarget);
        }
        public static TestCaseData[] TestCasesForDecideMethod
        {
            get
            {
                var array = new[]
                {
                    new TestCaseData(
                        new Trade(InventoryItems.Wood, 2, InventoryItems.Stone, 1, "A", "B"),
                        new List<Type> { typeof(TradeBalanceException) }
                    )
                    // Add more test cases as needed
                };
                return array;
            }
        }
    }
}
