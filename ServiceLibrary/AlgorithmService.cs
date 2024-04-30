using System;
using System.Collections.Generic;
using System.Linq;
using ModelLibrary;
using ModelLibrary.Exceptions;
using ServiceLibrary.Algorithm;

/*
 * This class is responsible for handling the algorithmic logic of the game.
 */
namespace ServiceLibrary
{
    public class AlgorithmService
    {
        public bool EnableRandomizedDecisions { get; set; } = true;
        public event EventHandler<AlgorithmDecisionEventArgs>? AlgorithmDecision;

        private readonly float _selfBuildRandomChance = 0.1f;
        private readonly float _buildEffectRandomChance = 0.1f;
        private readonly float _usefulnessRandomChance = 0.1f;
        private readonly float _tradeBalanceRandomChance = 0.1f;

        private readonly SelfBuild _selfBuild;
        private readonly Randomness _randomness;
        private readonly BuildEffect _buildEffect;
        private readonly Usefulness _usefulness;
        private readonly TradeBalance _tradeBalance;

        public AlgorithmService()
        {
            Random random = new Random();
            _selfBuild = new SelfBuild(random);
            _randomness = new Randomness(random);
            _buildEffect = new BuildEffect(random);
            _usefulness = new Usefulness(random);
            _tradeBalance = new TradeBalance(random);
        }

        public Trade CreateNewTrade(Tribe originator)
        {
            var originatorInventory = originator.Inventory;
            
            var resources = Enum.GetValues(typeof(InventoryItems)).Cast<InventoryItems>().ToList();
            var requestItems = resources.Where(item => originatorInventory.GetInventoryAmount(item) >= 5).ToList();
            var offerItems = resources.Where(item =>
                    originatorInventory.GetInventoryAmount(item) > 0 &&
                    originatorInventory.GetInventoryAmount(item) < 5)
                .ToList();
            
            var requestedItem = requestItems[new Random().Next(requestItems.Count)];
            var offeredItem = offerItems[new Random().Next(offerItems.Count)];

            var requestedAmount = _randomness.CalculateAmount(1, originatorInventory.GetInventoryAmount(requestedItem));
            var offeredAmount = _randomness.CalculateAmount(1, originatorInventory.GetInventoryAmount(offeredItem));
            
            return new Trade(requestedItem, requestedAmount, offeredItem, offeredAmount);
        }

        public void Decide(Trade trade, Tribe originator, Tribe targetCpu)
        {
            if (!targetCpu.GoodWill.Keys.Contains(originator))
            {
                targetCpu.GoodWill[originator] = 0;
            }

            int startGoodwill = targetCpu.GoodWill[originator];
            List<OfferDeclinedException> exceptions = new();

            //Original Decisions
            ExecuteAndCatch<SelfBuildException>(() => _selfBuild.Calculate(trade, targetCpu), exceptions);
            ExecuteAndCatch<BuildEffectException>(() => _buildEffect.Calculate(trade, targetCpu, originator),
                exceptions);
            ExecuteAndCatch<UsefulnessException>(() => _usefulness.Calculate(trade, targetCpu), exceptions);
            ExecuteAndCatch<TradeBalanceException>(() => _tradeBalance.Calculate(trade, targetCpu, originator),
                exceptions);

            //Randomise Decisions
            if (EnableRandomizedDecisions) {
                ExecuteAndCatch<SelfBuildException>(() =>
                {
                    if (_randomness.Calculate(_selfBuildRandomChance))
                        throw new SelfBuildException(trade, "I Want to build the build myself.");
                }, exceptions);

                ExecuteAndCatch<BuildEffectException>(() =>
                {
                    if (_randomness.Calculate(_buildEffectRandomChance))
                        throw new BuildEffectException(trade, "This trade has a negative effect on my tribe.");
                }, exceptions);

                ExecuteAndCatch<UsefulnessException>(() =>
                {
                    if (_randomness.Calculate(_usefulnessRandomChance))
                        throw new UsefulnessException(trade, "This trade is not useful for me.");
                }, exceptions);

                ExecuteAndCatch<TradeBalanceException>(() =>
                {
                    if (_randomness.Calculate(_tradeBalanceRandomChance))
                        throw new TradeBalanceException(trade, startGoodwill, "Trade is not balanced.");
                }, exceptions);
            }

            AlgorithmDecisionEventArgs algoArgs;

            if (exceptions.Count == 0)
            {
                algoArgs = new AlgorithmDecisionEventArgs(exceptions, true, null);
            }
            else
            {
                try
                {
                    Trade counterOffer = CreateCounterTrade(trade, originator, targetCpu, exceptions);
                    algoArgs = new AlgorithmDecisionEventArgs(exceptions, false, counterOffer);
                }
                catch (OfferDeclinedException e)
                {
                    exceptions.Add(e);
                    algoArgs = new AlgorithmDecisionEventArgs(exceptions, false, null);
                }
            }

            AlgorithmDecision?.Invoke(this, algoArgs);
        }

        private void ExecuteAndCatch<T>(Action action, List<OfferDeclinedException> exceptions)
            where T : OfferDeclinedException
        {
            try
            {
                action();
            }
            catch (T e)
            {
                exceptions.Add(e);
            }
        }

        private Trade CreateCounterTrade(Trade trade, Tribe originator, Tribe targetCpu,
            List<OfferDeclinedException> exceptions)
        {
            var counterOfferTrade = trade;
            if (exceptions.OfType<SelfBuildException>().Any()) //selfbuild
            {
                counterOfferTrade = _selfBuild.CalculateCounter(trade, targetCpu);
            }

            if (exceptions.OfType<BuildEffectException>().Any()) //buildeffect
            {
                counterOfferTrade = _buildEffect.CalculateCounter(trade, targetCpu, originator);
            }

            if (exceptions.OfType<UsefulnessException>().Any()) //usefulness
            {
                counterOfferTrade = _usefulness.CalculateCounter(trade, targetCpu);
            }

            if (exceptions.OfType<TradeBalanceException>().Any()) //trade balance
            {
                counterOfferTrade = _tradeBalance.CalculateCounter(trade);
            }

            if (targetCpu.Inventory.GetInventoryAmount(counterOfferTrade.RequestedItem) <
                counterOfferTrade.RequestedAmount) //trade not possible
            {
                throw new InsufficientResourcesException(counterOfferTrade,
                    targetCpu.Inventory.GetInventoryAmount(trade.RequestedItem) - trade.RequestedAmount,
                    "I do not have enough resources to complete the trade");
            }

            counterOfferTrade = new Trade(counterOfferTrade.OfferedItem, counterOfferTrade.OfferedAmount, counterOfferTrade.RequestedItem, counterOfferTrade.RequestedAmount);
            
            return counterOfferTrade;
        }

        private bool TradePossible(Trade trade, Tribe originator, Tribe targetCpu)
        {
            return originator.Inventory.GetInventoryAmount(trade.OfferedItem) >= trade.OfferedAmount &&
                   targetCpu.Inventory.GetInventoryAmount(trade.RequestedItem) >= trade.RequestedAmount;
        }

        //fire event with all decisions of the algorithm to be able to debug in unity
        public class AlgorithmDecisionEventArgs(
            List<OfferDeclinedException> issues,
            bool tradeAccepted,
            Trade counterOffer) : EventArgs
        {
            public List<OfferDeclinedException> issuesWithTrade { get; } = issues;
            public bool tradeAccepted { get; } = tradeAccepted;
            public Trade? counterOffer { get; } = counterOffer;
        }
    }
}