using UnityEngine;

public class MathUtility
{
    public static Vector2Int StringToVector2I(string str)
    {
        str = str.Replace("(", "").Replace(")", "");
        var s = str.Split(',');
        return new Vector2Int(int.Parse(s[0]), int.Parse(s[1]));
    }

    public static Vector3 StringToVector3(string str)
    {
        str = str.Replace("(", "").Replace(")", "");
        var s = str.Split(',');
        return new Vector3(float.Parse(s[0]), float.Parse(s[1]), float.Parse(s[2]));
    }
}
