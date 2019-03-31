using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Returninator.Gameplay
{
    public class PlayerInputChannel: IInputChannel
    {
        private InputState m_LastInput = default;

        public InputChange GetInputChange()
        {
            var axes = new Dictionary<InputAxis, float>();
            var actions = new Dictionary<InputAction, bool>();

            var horizontal = Input.GetAxis("Horizontal");
            if (Mathf.Abs(horizontal - m_LastInput.Horizontal) > 0.0001f)
                axes.Add(InputAxis.Horizontal, horizontal);

            var change = new InputChange(actions, axes);
            m_LastInput.UpdateState(change);
            return change;
        }
    }
}