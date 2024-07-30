
namespace SrpgFramework.Units.Damages
{
    public interface IDamage
    {
        public void Apply()
        {
            BeforeTick();
            Tick();
            AfterTick();
        }

        void BeforeTick();
        void Tick();
        void AfterTick();
    }
}