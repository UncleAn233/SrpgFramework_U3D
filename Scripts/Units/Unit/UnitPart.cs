
namespace SrpgFramework.Units
{
    public partial class Unit
    {
        public MoveUnit Move => GetComponent<MoveUnit>();
        public AiUnit Ai => GetComponent<AiUnit>();
    }
}