using UnityEngine;

namespace Returninator.Gameplay
{
    [CreateAssetMenu(fileName = "CharacterSettings", menuName = "Returninator/Character Settings")]
    public class CharacterSettings: ScriptableObject
    {
        [SerializeField]
        private MovementParameter m_Acceleration = new MovementParameter(10f);
        [SerializeField]
        private MovementParameter m_Deacceleration = new MovementParameter(5f);
        [SerializeField]
        private MovementParameter m_AirAcceleration = new MovementParameter(4f);
        [SerializeField]
        private MovementParameter m_AirDeacceleration = new MovementParameter(2f);
        [SerializeField]
        private float m_MaxSpeed = 10f;

        public float MaxAccelerationSpeed => m_MaxSpeed;
        public float GetAcceleration(float speed, bool grounded)
            => grounded ?
                m_Acceleration.GetValue(speed / m_MaxSpeed) :
                m_AirAcceleration.GetValue(speed / m_MaxSpeed);

        public float GetDeacceleration(float speed, bool grounded)
            => grounded ?
                m_Deacceleration.GetValue(speed / m_MaxSpeed) :
                m_AirDeacceleration.GetValue(speed / m_MaxSpeed);
    }

}