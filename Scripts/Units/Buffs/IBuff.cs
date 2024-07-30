
namespace SrpgFramework.Units.Buffs
{
    public interface IBuff
    {
        void Tick();
        bool CheckLifeTime();
    }
}