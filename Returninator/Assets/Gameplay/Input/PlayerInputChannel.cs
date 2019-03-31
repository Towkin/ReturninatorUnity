using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Returninator.Gameplay
{
    public class PlayerInputChannel: IInputChannel
    {
        private InputState m_LastInput = default;

        private void UpdateAxis(InputAxis axis, Dictionary<InputAxis, float> values)
        {
            var inputValue = Input.GetAxis(axis.ToString());
            if (Mathf.Abs(inputValue - m_LastInput[axis]) > 0.0001f)
                values.Add(axis, inputValue);
        }
        private void UpdateAction(InputAction action, Dictionary<InputAction, bool> values)
        {
            var inputValue = Input.GetButton(action.ToString());
            if (inputValue != m_LastInput[action])
                values.Add(action, inputValue);
        }

        public InputChange GetInputChange()
        {
            var axes = new Dictionary<InputAxis, float>();
            UpdateAxis(InputAxis.Horizontal, axes);
            UpdateAxis(InputAxis.Vertical, axes);
            
            var actions = new Dictionary<InputAction, bool>();
            UpdateAction(InputAction.Fire, actions);
            UpdateAction(InputAction.Interact, actions);
            UpdateAction(InputAction.Jump, actions);

            var change = new InputChange(actions, axes);
            m_LastInput.UpdateState(change);
            return change;
        }
    }
}