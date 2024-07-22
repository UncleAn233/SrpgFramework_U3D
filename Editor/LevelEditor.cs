using Newtonsoft.Json;
using SrpgFramework.CellGrid.Cells;
using SrpgFramework.Cells.GridGenerator;
using SrpgFramework.Level;
using SrpgFramework.Units;
using System.IO;
using UnityEditor;
using UnityEngine;
namespace SrpgFramework.Editor
{
    public class LevelEditor : EditorWindow
    {
        //Cell Grid Generate
        int planeType;
        string[] planeTypeArray = { "XY", "XZ" };
        bool Is2D => planeType == 0;
        float cellSize = 1;

        IGridGenerator gridGenerator = new RectangularSquareGridGenerator();

        //Set Unit
        [SerializeField]
        GameObject unitParent;
        GameObject unitPrefab;
        int playerNumber;
        string unitId;
        bool unitSettingMod;

        string exportPath;
        string exportName;

        [MenuItem("Window/LevelEditor")]
        public static void ShowWindow()
        {
            var window = GetWindow(typeof(LevelEditor));
            window.titleContent.text = "Map Editor";
        }

        #region GUI
        public void OnGUI()
        {
            GenerateMapGUI();
            GUILayout.Space(10);
            SetUnitGUI();

            GUILayout.Space(20);

            exportPath = EditorGUILayout.TextField("Export Path", exportPath);
            exportName = EditorGUILayout.TextField("Export Name", exportName);
            if (exportName is not null && !exportName.EndsWith(".json"))
            {
                exportName += ".json";
            }
            if (GUILayout.Button("Export Map Data"))
            {
                ExportLevelData(exportPath, exportName);
            }
            if (GUILayout.Button("Clear Map"))
            {
                ClearMap();
            }
        }

        public void OnEnable()
        {
            SceneView.duringSceneGui -= OnSceneGUI;
            SceneView.duringSceneGui += OnSceneGUI;
        }
        public void OnDestroy()
        {
            SceneView.duringSceneGui -= OnSceneGUI;
        }

        private void OnSceneGUI(SceneView sceneView)
        {
            if (unitSettingMod && unitPrefab is not null)
            {
                SetUnit();
            }
        }

        private void GenerateMapGUI()
        {
            GUILayout.Label("MapGrid", EditorStyles.boldLabel);
            planeType = EditorGUILayout.Popup("Plane Type", planeType, planeTypeArray, new GUILayoutOption[0]);
            cellSize = EditorGUILayout.FloatField("Cell Size", cellSize);
            gridGenerator.GenerateCellGridGUI(Is2D);
            if (GUILayout.Button("Generate Cell Map"))
            {
                ClearMap();
                gridGenerator.GenerateCellGrid(Is2D, cellSize);
            }
        }

        private void CellPaintGUI()
        {

        }

        private void SetUnitGUI()
        {
            GUILayout.Label("Unit Setting", EditorStyles.boldLabel);

            playerNumber = EditorGUILayout.IntField("Player Number", playerNumber);
            GUILayout.Label("Default Player Number:   0-Player 1-Friend 2-Enemy 3-Third");
            GUILayout.Space(5);

            unitPrefab = (GameObject)EditorGUILayout.ObjectField("Unit prefab", unitPrefab, typeof(GameObject), false, new GUILayoutOption[0]);
            if (unitPrefab is null)
            {
                GUIStyle style = new GUIStyle(EditorStyles.boldLabel);
                style.normal.textColor = Color.red;
                GUILayout.Label("Request an Unit Prefab", style);
            }

            unitId = EditorGUILayout.TextField("Unit ID", unitId);
            if (unitId is null || unitId.Length == 0)
            {
                GUIStyle style = new GUIStyle(EditorStyles.boldLabel);
                style.normal.textColor = Color.red;
                GUILayout.Label("Not Set the Unit Id", style);
            }
            unitSettingMod = EditorGUILayout.Toggle("Unit Setting Mod", unitSettingMod);
        }

        #endregion

        #region Function
        private void SetUnit()
        {
            var cell = GetSelectedCell();
            if (cell is null)
                return;

            unitParent = GameObject.Find("Units");
            if (unitParent is null)
                unitParent = new GameObject("Units");

            Handles.color = Color.red;
            Handles.DrawWireDisc(cell.transform.position, Vector3.up, cellSize * 0.5f);
            Handles.DrawWireDisc(cell.transform.position, Vector3.forward, cellSize * 0.5f);
            HandleUtility.Repaint();

            if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
            {
                var unit = (PrefabUtility.InstantiatePrefab(unitPrefab) as GameObject).GetComponent<Unit>();

                unit.transform.parent = unitParent.transform;
                unit.transform.rotation = unitParent.transform.rotation;

                unit.ID = unitId;
                unit.PlayerNumber = playerNumber;
                unit.Cell = cell;
                unit.transform.localPosition = cell.transform.localPosition;
            }
        }

        private async void ExportLevelData(string path, string level)
        {
            var cellParent = GameObject.Find("Cells");
            if (cellParent is null)
                cellParent = new GameObject("Cells");

            LevelData mapData = new(Is2D, cellParent.GetComponentsInChildren<Cell>(), GameObject.FindObjectsByType<Unit>(FindObjectsSortMode.None));
            var jo = JsonConvert.SerializeObject(mapData);

            Debug.Log($"Start Export Level Json {level} To Path={path} ");
            await File.WriteAllTextAsync(path + "/" + level, jo, System.Text.Encoding.UTF8);
            Debug.Log($"Export Level Json {level} Success! ");
        }

        private void ClearMap()
        {
            GameObject.DestroyImmediate(GameObject.Find("Cells"));
            GameObject.DestroyImmediate(GameObject.Find("Units"));
        }

        private Cell GetSelectedCell()
        {
            var raycastHit2D = Physics2D.GetRayIntersection(HandleUtility.GUIPointToWorldRay(Event.current.mousePosition), Mathf.Infinity);
            if (raycastHit2D.transform != null && raycastHit2D.transform.GetComponent<Cell>() != null)
            {
                return raycastHit2D.transform.GetComponent<Cell>();
            }

            RaycastHit raycastHit3D;
            bool isRaycast3D = Physics.Raycast(HandleUtility.GUIPointToWorldRay(Event.current.mousePosition), out raycastHit3D);
            if (isRaycast3D && raycastHit3D.transform.GetComponent<Cell>() != null)
            {
                return raycastHit3D.transform.GetComponent<Cell>();
            }

            return null;
        }
        #endregion
    }
}
