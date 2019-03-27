using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Returninator.Gameplay
{
    public struct CharacterMovement
    {
        float m_Magnitude;
        Vector2 m_Direction;
        
        public float Speed
        {
            get => m_Magnitude;
            set => m_Magnitude = value;
        }
        public float SpeedX
        {
            get => Direction.x * Speed;
            set => Velocity = new Vector2(value, SpeedY);
            
        }
        public float SpeedY
        {
            get => Direction.y * Speed;
            set => Velocity = new Vector2(SpeedX, value);
        }

        public Vector2 Direction
        {
            get => m_Direction;
            set
            {
                if (value == Vector2.zero)
                    return;

                m_Direction = value.normalized;
            }
        }

        public Vector2 Velocity
        {
            get => m_Direction * m_Magnitude;
            set
            {
                m_Magnitude = value.magnitude;
                if (m_Magnitude > float.Epsilon)
                    m_Direction = value / m_Magnitude;
            }
        }
    }

    public class Character: MonoBehaviour
    {
        private InputState CurrentInput { get; } = new InputState();
        private CharacterMovement Movement { get; } = new CharacterMovement();
    }
}