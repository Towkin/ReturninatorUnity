using System;
using UnityEngine;

namespace Returninator.Gameplay
{
    [Serializable]
    public struct MovementParameter
    {
        public MovementParameter(float value, AnimationCurve factor = null)
        {
            m_Value = value;
            m_Factor = factor ?? AnimationCurve.Constant(0f, 1f, 1f);
        }

        [SerializeField]
        private float m_Value;
        [SerializeField]
        private AnimationCurve m_Factor;

        public float GetValue(float atTime)
            => m_Value * m_Factor.Evaluate(atTime);
    }
}