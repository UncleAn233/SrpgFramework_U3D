using SrpgFramework.Global;
using SrpgFramework.Units;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SrpgFramework.Players
{
    public class AiPlayer : Player
    {
        public override void Play()
        {
            if (this.Units.Any())
            {
                GameManager.CellGridMgr.ToBlockInputState();
                StartCoroutine(PlayCoroutine());
            }
        }

        private void OnGameEnded()
        {
            StopAllCoroutines();
        }

        private IEnumerator PlayCoroutine()
        {
            foreach (var unit in SelectNextFirstByNearEnemy())
            {
                yield return (unit.Ai?.Execute());
            }
            GameManager.PlayerMgr.EndTurn();
            yield return null;
        }

        private IEnumerable<Unit> SelectNextFirstByNearEnemy()
        {
            return this.Units.OrderBy(unit =>
            {       
                return GameManager.UnitMgr.GetEnemyUnits(this).Min(enemy => unit.Cell.GetDistance(enemy.Cell)); //离敌人近的先动
            });
        }
    }
}