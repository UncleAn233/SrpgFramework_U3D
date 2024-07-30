using SrpgFramework.Units.Units;
using UnityEngine;

namespace SrpgFramework.Ai.Evaluators
{
    public abstract class Evaluator<T> : ScriptableObject
    {
        public const string ResourcesPath = "SO/Evaluators/";

        public float Weight = 1;

        public virtual void PreCalculate(Unit unit) { }
        public abstract float Evaluate(T toEvaluate, Unit unit);
    }
}