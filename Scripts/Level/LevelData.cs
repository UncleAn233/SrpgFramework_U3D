using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SrpgFramework.CellGrid.Cells;
using SrpgFramework.Cells;
using SrpgFramework.Units;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace SrpgFramework.Level
{
    [JsonConverter(typeof(LevelDataConverter))]
    public class LevelData
    {
        public bool Is2D;
        public string CellPrefab;
        public List<CellInfo> Cells;
        public List<UnitInfo> Units;

        public LevelData()
        {
            Cells = new();
            Units = new();
        }

        public LevelData(bool is2D, Cell[] cells, Unit[] units)
        {
            Is2D = is2D;

            CellPrefab = cells[0].gameObject.name;
            Cells = new();
            foreach (var cell in cells)
            {
                Cells.Add(new CellInfo(cell));
            }

            Units = new();
            foreach (var unit in units)
            {
                Units.Add(new UnitInfo(unit));
            }
        }
    }

    public class LevelDataConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(LevelData).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            LevelData data = new();
            var jo = JObject.Load(reader);
            data.Is2D = jo[nameof(LevelData.Is2D)].ToObject<Boolean>();
            data.CellPrefab = jo[nameof(LevelData.CellPrefab)].ToString();

            foreach (var c in jo[nameof(LevelData.Cells)])
            {
                var coord = c[nameof(CellInfo.Coord)].ToString();
                var pos = c[nameof(CellInfo.Position)].ToString();

                data.Cells.Add(new CellInfo(MathUtility.StringToVector2I(coord), MathUtility.StringToVector3(pos)));
            }

            foreach (var u in jo[nameof(LevelData.Units)])
            {
                data.Units.Add(new UnitInfo(u as JObject));
            }

            return data;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value is null)
                return;

            var mapData = value as LevelData;

            JObject mainJo = new();
            mainJo.Add(nameof(LevelData.Is2D), mapData.Is2D);
            mainJo.Add(nameof(LevelData.CellPrefab), mapData.CellPrefab);

            JArray cellArray = new();
            foreach (var cell in mapData.Cells)
            {
                cellArray.Add(cell.ToJObject());
            }
            mainJo.Add(nameof(LevelData.Cells), cellArray);

            JArray unitArray = new();
            foreach (var unit in mapData.Units)
            {
                unitArray.Add(unit.ToJObject());
            }
            mainJo.Add(nameof(LevelData.Units), unitArray);

            writer.Formatting = Formatting.Indented;
            serializer.Serialize(writer, mainJo);
        }

    }

    public struct CellInfo
    {
        public Vector2Int Coord;
        public Vector3 Position;

        public CellInfo(Cell cell)
        {
            Coord = cell.Coord;
            Position = cell.transform.localPosition;
        }

        public CellInfo(Vector2Int coord, Vector3 position)
        {
            Coord = coord;
            Position = position;
        }

        public JObject ToJObject()
        {
            var jo = new JObject();
            jo.Add(nameof(Coord), Coord.ToString());
            jo.Add(nameof(Position), Position.ToString());
            return jo;
        }
    }

    public struct UnitInfo
    {
        public UnitType Type;
        public string Id;
        public int PlayerNumber;
        public Vector2 Cell;

        public UnitInfo(Unit unit)
        {
            Type = unit.UnitType;
            Id = unit.ID;
            PlayerNumber = unit.PlayerNumber;
            Cell = unit.Cell.Coord;
        }


        public UnitInfo(JObject jo)
        {
            switch (jo[nameof(Type)].ToString())
            {
                case nameof(UnitType.PC): this.Type = UnitType.PC; break;
                case nameof(UnitType.NPC): default: this.Type = UnitType.NPC; break;
            }
            this.Id = jo[nameof(Id)].ToString();
            this.PlayerNumber = int.Parse(jo[nameof(PlayerNumber)].ToString());
            this.Cell = MathUtility.StringToVector2I(jo[nameof(Cell)].ToString());
        }

        public JObject ToJObject()
        {
            var jo = new JObject();
            jo.Add(nameof(Type), Type.ToString());
            jo.Add(nameof(Id), Id);
            jo.Add(nameof(PlayerNumber), PlayerNumber);
            jo.Add(nameof(Cell), Cell.ToString());
            return jo;
        }
    }
}