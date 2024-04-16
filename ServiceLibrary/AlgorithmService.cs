using System;
using ModelLibrary;
using ServiceLibrary.Algorithm;

/*
 * This class is responsible for handling the algorithmic logic of the game.
 */
namespace ServiceLibrary
{
    public class AlgorithmService
    {
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
            _tradeBalance = new TradeBalance();
        }

        public bool Decide(Trade trade,Tribe originator, Tribe targetCpu)
        {
            var algoArgs = new AlgorithmDecisionEventArgs();
            int startGoodwill = targetCpu.GoodWill[originator];
            
            //Original Decisions
            bool selfBuildDecision = _selfBuild.Calculate(trade, targetCpu);
            bool buildEffectDecision = _buildEffect.Calculate(trade, targetCpu, originator);
            bool usefulnessDecision = _usefulness.Calculate(trade, targetCpu);
            bool tradeBalanceDecision = _tradeBalance.Calculate(trade, targetCpu, originator);

            algoArgs.SelfBuild = selfBuildDecision;
            algoArgs.BuildEffect = buildEffectDecision;
            algoArgs.Usefulness = usefulnessDecision;
            algoArgs.TradeBalance = tradeBalanceDecision;
            algoArgs.StartGoodwill = startGoodwill;
            
            //Randomise Decisions
            selfBuildDecision = _randomness.Calculate(_selfBuildRandomChance) ? !selfBuildDecision : selfBuildDecision;
            buildEffectDecision = _randomness.Calculate(_buildEffectRandomChance) ? !buildEffectDecision : buildEffectDecision;
            usefulnessDecision = _randomness.Calculate(_usefulnessRandomChance) ? !usefulnessDecision : usefulnessDecision;
            tradeBalanceDecision = _randomness.Calculate(_tradeBalanceRandomChance) ? !tradeBalanceDecision : tradeBalanceDecision;
            
            algoArgs.RndSelfBuild = selfBuildDecision;
            algoArgs.RndBuildEffect = buildEffectDecision;
            algoArgs.RndUsefulness = usefulnessDecision;
            algoArgs.RndTradeBalance = tradeBalanceDecision;
            algoArgs.EndGoodWill = targetCpu.GoodWill[originator];
            
            AlgorithmDecision?.Invoke(this, algoArgs);

            if (selfBuildDecision && buildEffectDecision && usefulnessDecision && tradeBalanceDecision)
            {
                return true;
            }

            targetCpu.GoodWill[originator] = startGoodwill;
            return false;
        }
        
        //fire event with all decisions of the algorithm to be able to debug in unity
        public class AlgorithmDecisionEventArgs : EventArgs
        {
            public bool SelfBuild;
            public bool BuildEffect;
            public bool Usefulness;
            public bool TradeBalance;
            
            public bool RndSelfBuild;
            public bool RndBuildEffect;
            public bool RndUsefulness;
            public bool RndTradeBalance;

            public int StartGoodwill;
            public int EndGoodWill;
        }

        public static event EventHandler<AlgorithmDecisionEventArgs>? AlgorithmDecision;
    }
}