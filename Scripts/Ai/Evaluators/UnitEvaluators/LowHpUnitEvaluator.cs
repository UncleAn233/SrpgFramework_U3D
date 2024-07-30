using SrpgFramework.Ai.Evaluators;
using SrpgFramework.Units.Units;
using UnityEngine;

namespace SrpgFramework.Players.AI.Evaluators
{
    [CreateAssetMenu(fileName = nameof(LowHpUnitEvaluator), menuName = "AI/Evaluator/Unit/LowHp")]
    public class LowHpUnitEvaluator : Evaluator<Unit>
    {
        public override float Evaluate(Unit toEvaluate, Unit unit)
        {
            if (unit.Player.IsEnemy(toEvaluate.Player))
            {
                return (1 - toEvaluate.HpPercent)*Weight;
            }
            else if (unit.Player.IsFriend(toEvaluate.Player))
            {
                return (toEvaluate.HpPercent - 1) * Weight;
            }
            else
            {
                return 0;
            }
        }
    }
}