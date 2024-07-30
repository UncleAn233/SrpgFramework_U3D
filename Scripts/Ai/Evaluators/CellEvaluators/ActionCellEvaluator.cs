using SrpgFramework.Ai.Evaluators;
using SrpgFramework.CellGrid.Cells;
using SrpgFramework.Units.Units;
using UnityEngine;

namespace SrpgFramework.Players.AI.Evaluators
{
    //能行动的格子获得对应行动的加分
    [CreateAssetMenu(fileName = nameof(ActionCellEvaluator), menuName = "AI/Evaluator/Cell/Action")]
    public class ActionCellEvaluator : Evaluator<Cell>
    {
        public override float Evaluate(Cell toEvaluate, Unit unit)
        {
            if (unit.Points["Action"] == 0)
                return 0;

            float top = 0f;
            foreach (var action in unit.Ai.ActionBrains)
            {
                if (action.ShouldExecute(unit, toEvaluate))
                {
                    var score = action.Evaluate(unit, toEvaluate);
                    top = score > top ? score : top;
                }
            }
            return top * Weight;
        }
    }
}