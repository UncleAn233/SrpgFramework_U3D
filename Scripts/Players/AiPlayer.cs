using SrpgFramework.Global;
using SrpgFramework.Units.Units;
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
                BattleManager.CellGridMgr.ToBlockInputState();
                StartCoroutine(PlayCoroutine());
            }
        }

        private void OnGameEnd()
        {
            StopAllCoroutines();
        }

        private IEnumerator PlayCoroutine()
        {
            foreach (var unit in SelectNextFirstByNearEnemy())
            {
                yield return (unit.Ai?.Execute());
            }
            BattleManager.PlayerMgr.NextPlayer();
            yield return null;
        }

        private IEnumerable<Unit> SelectNextFirstByNearEnemy()
        {
            return this.Units.OrderBy(unit =>
            {       
                return BattleManager.UnitMgr.GetEnemyUnits(this).Min(enemy => unit.Cell.GetDistance(enemy.Cell)); //离敌人近的先动
            });
        }
    }
}