
namespace SrpgFramework.Cells.GridGenerator
{
    public interface IGridGenerator
    {
        void GenerateCellGridGUI(bool is2D);
        void GenerateCellGrid(bool is2D, float cellSize);
    }
}
