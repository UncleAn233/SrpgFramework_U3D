using SrpgFramework.Ai.Evaluators;
using SrpgFramework.Units.Units;
using UnityEngine;

namespace SrpgFramework.Players.AI.Evaluators
{
    [CreateAssetMenu(fileName = nameof(HateUnitEvaluator), menuName = "AI/Evaluator/Unit/Hate")]
    public class HateUnitEvaluator : Evaluator<Unit>
    {
        public override float Evaluate(Unit toEvaluate, Unit unit)
        {
            return unit.Player.IsEnemy(toEvaluate.Player) ? toEvaluate.Hate * Weight : 0;
        }
    }
}