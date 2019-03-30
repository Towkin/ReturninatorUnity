using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

namespace Returninator.Gameplay
{
    public struct InputChange
    {
        public IReadOnlyDictionary<InputAction, bool> Actions => m_Actions;
        public IReadOnlyDictionary<InputAxis, float> Axes => m_Axes;
        private Dictionary<InputAction, bool> m_Actions;
        private Dictionary<InputAxis, float> m_Axes;

        public int Count => (m_Actions?.Count ?? 0) + (m_Axes?.Count ?? 0);
    }
}