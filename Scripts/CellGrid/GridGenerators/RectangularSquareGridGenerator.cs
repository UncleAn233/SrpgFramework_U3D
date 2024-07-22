using SrpgFramework.CellGrid.Cells;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace SrpgFramework.Cells.GridGenerator
{
    public class RectangularSquareGridGenerator : IGridGenerator
    {
        private GameObject cellPrefab;
        private int x = 20;
        private int yz = 20;

        public void GenerateCellGridGUI(bool is2D)
        {
            Dictionary<string, object> dict = new();
            x = EditorGUILayout.IntField("X", x);
            yz = EditorGUILayout.IntField(is2D ? "Y" : "Z", yz);
            cellPrefab = (GameObject)EditorGUILayout.ObjectField("Cell Prefab", cellPrefab, typeof(GameObject), true, new GUILayoutOption[0]);
        }

        public void GenerateCellGrid(bool is2D, float cellSize)
        {
            if (cellPrefab is null)
                return;

            var parent = GameObject.Find("Cells");
            if (parent is null)
                parent = new GameObject("Cells");

            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < yz; j++)
                {
                    var cell = PrefabUtility.InstantiatePrefab(cellPrefab) as GameObject;
                    cell.transform.position = is2D ? new Vector3(i * cellSize, j * cellSize, 0) : new Vector3(i * cellSize, 0, j * cellSize);
                    cell.transform.parent = parent.transform;
                    cell.GetComponent<Cell>().Coord = new Vector2Int(i, j);
                    cell.GetComponent<Cell>().MoveCost = 1;
                }
            }
        }
    }
}