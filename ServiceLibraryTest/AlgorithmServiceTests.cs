using System;
using System.Collections.Generic;
using System.Linq;
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
        public void CalculateSelfBuild_HigherThanBorder_ReturnsFalse()
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
            FillPointsTable();
            FillInventory();

            // When & Then
            algorithmService.AlgorithmDecision += (sender, args) =>
            {
                    Assert.That(args.issuesWithTrade.Count, Is.EqualTo(expectedException.Count));
                args.issuesWithTrade.ForEach(issue => Assert.That(expectedException, Does.Contain(issue.GetType())));
            };
            algorithmService.Decide(trade, _tradeOriginator, _tradeTarget);
        }
        
        [Test, TestCaseSource(nameof(TestCasesForCreateNewTradeMethod))]
        public void CreateNewTrade_WhenCalled_ReturnsTrade(Dictionary<InventoryItems, int> list)
        {
            // Given
            var algorithmService = new AlgorithmService() {EnableRandomizedDecisions = false};
            foreach (var item in list)
            {
                _tradeOriginator.Inventory.AddToInventory(item.Key, item.Value);
            }

            // When
            var result = algorithmService.CreateNewTrade(_tradeOriginator, _tradeTarget);

            // Then
            Assert.That(result, Is.TypeOf<Trade>());
            Assert.That(result.OfferedItem, Is.EqualTo(list.Keys.First()));
            Assert.That(result.RequestedItem, Is.EqualTo(list.Keys.Last()));
        }
        
        private void FillInventory()
        {
            _tradeOriginator.Inventory.AddToInventory(InventoryItems.Wood, 6);
            _tradeOriginator.Inventory.AddToInventory(InventoryItems.Lenses, 6);
            _tradeOriginator.Inventory.AddToInventory(InventoryItems.Clay, 1);
            _tradeOriginator.Inventory.AddToInventory(InventoryItems.Gold, 6);
            _tradeOriginator.Inventory.AddToInventory(InventoryItems.Steel, 2);
            _tradeOriginator.Inventory.AddToInventory(InventoryItems.Insulation, 0);
            _tradeOriginator.Inventory.AddToInventory(InventoryItems.Fertilizer, 2);
            _tradeOriginator.Inventory.AddToInventory(InventoryItems.Stone, 2);
        
            _tradeTarget.Inventory.AddToInventory(InventoryItems.Wood, 4);
            _tradeTarget.Inventory.AddToInventory(InventoryItems.Lenses, 3);
            _tradeTarget.Inventory.AddToInventory(InventoryItems.Clay, 4);
            _tradeTarget.Inventory.AddToInventory(InventoryItems.Gold, 6);
            _tradeTarget.Inventory.AddToInventory(InventoryItems.Steel, 5);
            _tradeTarget.Inventory.AddToInventory(InventoryItems.Insulation, 3);
            _tradeTarget.Inventory.AddToInventory(InventoryItems.Fertilizer, 3);
            _tradeTarget.Inventory.AddToInventory(InventoryItems.Stone, 6);
        }

        private void FillPointsTable()
        {
            _tradeOriginator.PointTable = new Dictionary<(InventoryItems, Tribe), int>
            {
                [(InventoryItems.Wood, _tradeOriginator)] = 10,
                [(InventoryItems.Wood, _tradeTarget)] = -5,

                [(InventoryItems.Lenses, _tradeOriginator)] = 10,
                [(InventoryItems.Lenses, _tradeTarget)] = 5,

                [(InventoryItems.Clay, _tradeOriginator)] = 10,
                [(InventoryItems.Clay, _tradeTarget)] = 0,

                [(InventoryItems.Gold, _tradeOriginator)] = 10,
                [(InventoryItems.Gold, _tradeTarget)] = 5,

                [(InventoryItems.Steel, _tradeOriginator)] = 10,
                [(InventoryItems.Steel, _tradeTarget)] = 0,

                [(InventoryItems.Insulation, _tradeOriginator)] = 10,
                [(InventoryItems.Insulation, _tradeTarget)] = 5,
                
                [(InventoryItems.Fertilizer, _tradeOriginator)] = 10,
                [(InventoryItems.Fertilizer, _tradeTarget)] = 0,

                [(InventoryItems.Stone, _tradeOriginator)] = 10,
                [(InventoryItems.Stone, _tradeTarget)] = 0,
            };
            
            _tradeTarget.PointTable = new Dictionary<(InventoryItems, Tribe), int>
            {
                [(InventoryItems.Wood, _tradeOriginator)] = 0,
                [(InventoryItems.Wood, _tradeTarget)] = 10,

                [(InventoryItems.Lenses, _tradeOriginator)] = -5,
                [(InventoryItems.Lenses, _tradeTarget)] = 10,

                [(InventoryItems.Clay, _tradeOriginator)] = 5,
                [(InventoryItems.Clay, _tradeTarget)] = 10,

                [(InventoryItems.Gold, _tradeOriginator)] = 5,
                [(InventoryItems.Gold, _tradeTarget)] = 10,

                [(InventoryItems.Steel, _tradeOriginator)] = 0,
                [(InventoryItems.Steel, _tradeTarget)] = 10,

                [(InventoryItems.Insulation, _tradeOriginator)] = 5,
                [(InventoryItems.Insulation, _tradeTarget)] = 10,

                [(InventoryItems.Fertilizer, _tradeOriginator)] = 0,
                [(InventoryItems.Fertilizer, _tradeTarget)] = 10,

                [(InventoryItems.Stone, _tradeOriginator)] = 0,
                [(InventoryItems.Stone, _tradeTarget)] = 10,
            };
        }

        public static TestCaseData[] TestCasesForCreateNewTradeMethod
        {
            get
            {
                var array = new[]
                {
                    new TestCaseData(
                        new Dictionary<InventoryItems, int>
                        {
                            { InventoryItems.Clay, 2 }, // Offered
                            { InventoryItems.Wood, 6 } // Requested
                        }
                    ),
                    new TestCaseData(
                        new Dictionary<InventoryItems, int>
                        {
                            { InventoryItems.Steel, 4 }, // Offered
                            { InventoryItems.Gold, 5 } // Requested
                        }),
                    new TestCaseData(
                        new Dictionary<InventoryItems, int>
                        {
                            { InventoryItems.Insulation, 4 }, // Offered
                            { InventoryItems.Stone, 7 } // Requested
                        })
                };
                
                return array;
            }
        }
        
        public static TestCaseData[] TestCasesForDecideMethod
        {
            get
            {
                var array = new[]
                {
                    new TestCaseData(
                        new Trade(InventoryItems.Clay,1,InventoryItems.Lenses,1, "A", "B"),
                        new List<Type> { typeof(UsefulnessException) }
                        ),
                    new TestCaseData(
                        new Trade(InventoryItems.Gold, 3, InventoryItems.Wood, 3, "A", "B"),
                        new List<Type> { typeof(SelfBuildException) }
                    ),
                    new TestCaseData(
                        new Trade(InventoryItems.Clay, 2, InventoryItems.Stone, 1, "A", "B"),
                        new List<Type> { typeof(TradeBalanceException) }
                    ),
                    new TestCaseData(
                        new Trade(InventoryItems.Lenses, 1, InventoryItems.Stone, 1, "A", "B"),
                        new List<Type> { typeof(BuildEffectException) }
                    ),
                    new TestCaseData(
                        new Trade(InventoryItems.Insulation,1,InventoryItems.Wood,2, "A", "B"),
                        new List<Type>()) // No exceptions;
                    // Add more test cases as needed
                };
                return array;
            }
        }
    }
}
