using SrpgFramework.Global;

namespace SrpgFramework.Players
{
    public class HumanPlayer : Player
    {
        public override void Play()
        {
            BattleManager.CellGridMgr.ToIdleState();
        }
    }
}