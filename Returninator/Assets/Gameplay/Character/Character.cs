using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Returninator.Gameplay
{
    public struct CharacterMovement
    {
        private Vector2 m_Direction;
        private float m_Magnitude;
        public float Speed
        {
            get => m_Magnitude;
            set
            {
                if (value < 0f)
                {
                    m_Direction = -m_Direction;
                    m_Magnitude = -value;
                }
                else
                {
                    m_Magnitude = value;
                    Debug.Log("Speed: " + m_Magnitude);
                }
            }
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
                Debug.Log("Direction: " + m_Direction);
            }
        }

        public Vector2 Velocity
        {
            get => Direction * Speed;
            set
            {
                Speed = value.magnitude;
                if (Speed > float.Epsilon)
                    m_Direction = value / Speed;
            }
        }
    }

    public class Character: MonoBehaviour, ICharacter
    {
        private InputState m_CurrentInput;
        private IInputChannel m_InputChannel;
        private CharacterMovement m_Movement;

        public Rigidbody2D Body { get; private set; }

        private void Awake() => Reset();
        public void Reset()
        {
            Body = GetComponentInChildren<Rigidbody2D>();
            m_CurrentInput = default;
            m_Movement = default;
        }

        public void SetResetPosition(Vector2 resetPosition)
        {

        }

        public void SetInput(IInputChannel input)
        {
            m_InputChannel = input;
        }

        public void FixedUpdate() => Tick();
        public void Tick()
        {
            //UpdateInput();
            UpdateVelocity();
            UpdatePosition();
        }

        private void UpdateInput()
        {
            m_CurrentInput.UpdateState(m_InputChannel.GetInputChange());
        }

        private void UpdateVelocity()
        {
            var inputVector = new Vector2(m_CurrentInput.Horizontal, 0f);

            m_Movement.SpeedX += Input.GetAxis("Horizontal") * 25.0f * Time.fixedDeltaTime;

        }

        private static PhysicsMaterial2D m_DefaultPhsyicsMaterial;
        private static PhysicsMaterial2D DefaultPhysicsMaterial
        {
            get
            {
                if (m_DefaultPhsyicsMaterial == null)
                    m_DefaultPhsyicsMaterial = new PhysicsMaterial2D()
                    {
                        name = "DefaultPhysicsMaterial",
                        bounciness = 0.15f,
                        friction = 0.1f
                    };
                return m_DefaultPhsyicsMaterial;
            }
        }

        private static List<ContactPoint2D> ContactHits { get; } = new List<ContactPoint2D>();
        private static List<RaycastHit2D> MovementHits { get; } = new List<RaycastHit2D>();
        private void UpdatePosition()
        {
            var filter = new ContactFilter2D()
            {
                layerMask = Physics2D.GetLayerCollisionMask(Body.gameObject.layer),
                useLayerMask = true,

                useDepth = false,
                useNormalAngle = false,
                useTriggers = false,
            };

            var physicsMaterial = Body.sharedMaterial ?? DefaultPhysicsMaterial;
            var groundNormal = -Physics2D.gravity.normalized;
            var bodyGravity = Physics2D.gravity * Body.gravityScale;

            var groundedMaxDotProduct = 0.5f;
            var groundedMinDotProduct = 0.2f;

            Body.GetContacts(filter, ContactHits);
            for (int i = 0; i < ContactHits.Count; i++)
            {
                if (Vector2.Dot(ContactHits[i].normal, groundNormal) > 0f)
                {
                    bodyGravity = Vector2.zero;
                    break;
                }
            }
            m_Movement.Velocity += bodyGravity * Time.fixedDeltaTime;

            var beginSpeed = m_Movement.Speed;

            var timeLeft = Time.fixedDeltaTime;
            var alongGround = 0f;

            const int MaxIterations = 10;
            for (int iteration = 0; iteration < MaxIterations && m_Movement.Speed > 0.0001f &&
                Body.Cast(m_Movement.Direction, filter, MovementHits, m_Movement.Speed * timeLeft) > 0; 
                iteration++)
            {
                var hit = MovementHits[0];

                var timeSlice = Time.fixedDeltaTime * (hit.distance / (m_Movement.Speed * Time.fixedDeltaTime));
                timeLeft -= timeSlice;

                // Step the body before recalculating the velocity.
                transform.position += (Vector3)(m_Movement.Direction * hit.distance);

                // Perform friction based on time slice and how much we're following along the ground.
                m_Movement.Speed = Mathf.Max(0f, m_Movement.Speed - alongGround * physicsMaterial.friction * timeSlice);

                var againstGround = -Vector2.Dot(m_Movement.Direction, hit.normal);

                var groundedDirection = m_Movement.Direction - 
                    (m_Movement.Direction * againstGround);

                var reflectedDirection = Vector2.Reflect(m_Movement.Direction, hit.normal);

                
                m_Movement.Velocity =
                    Vector2.Lerp(groundedDirection, reflectedDirection, physicsMaterial.bounciness) * 
                    m_Movement.Speed;

                // Set alongGround to 'How much are we following the collider' times 'How much is the collider "the ground"'.
                alongGround = (1f - Vector2.Dot(m_Movement.Direction, hit.normal)) * 
                    Mathf.Clamp01(Mathf.InverseLerp(groundedMinDotProduct, groundedMaxDotProduct, 
                    Vector2.Dot(groundNormal, hit.normal)));
            }

            transform.position += (Vector3)(m_Movement.Velocity * timeLeft);
            m_Movement.Speed = Mathf.Max(0f, m_Movement.Speed
                - Body.drag * Time.fixedDeltaTime 
                - alongGround * physicsMaterial.friction * timeLeft);
            //Body.velocity = m_Movement.Velocity;
        }
    }
}