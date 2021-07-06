using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private CharacterInputHandler m_Input;

    [Header("Movement")]
    [SerializeField] private float m_MaxSpeed = 8f;
    [SerializeField] private float m_Acceleration = 200f;
    [SerializeField] private AnimationCurve m_AccelerationFactorFromDot;
    [SerializeField] private float m_MaxAccelerationForce = 150f;
    [SerializeField] private AnimationCurve m_MaxAccelerationForceFactorFromDot;
    [SerializeField] private Vector3 m_ForceScale = new Vector3(1f, 0f, 1f);
    [SerializeField] private float m_GravityScaleDrop = 10f;
    private float m_MovementDisabledTimer;
    private Vector3 m_UnitGoal;
    private Vector3 m_GoalVelocity;
    private float m_SpeedMultiplier;
    private float m_MaxAccelerationMultiplier;

    private Rigidbody m_Rigidbody;

    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        m_SpeedMultiplier = 1.0f;
        m_MaxAccelerationMultiplier = 1.0f;
    }

    private void FixedUpdate()
    {
        UpdateCharacterVelocity();
    }

    private void UpdateCharacterVelocity()
    {
        Vector3 move = m_Input.MovementDirection;

        if(m_MovementDisabledTimer > 0)
        {
            move = Vector3.zero;
            m_MovementDisabledTimer -= Time.fixedDeltaTime;
        }

        if(move.magnitude > 1.0f)
        {
            move.Normalize();
        }

        m_UnitGoal = move;

        Vector3 unitVel = m_GoalVelocity.normalized;

        float velDot = Vector3.Dot(m_UnitGoal, unitVel);
            
        float accel = m_Acceleration * m_AccelerationFactorFromDot.Evaluate(velDot);

        Vector3 goalVel = m_UnitGoal * m_MaxSpeed * m_SpeedMultiplier;

        //Vector3 groundVel = m_Rigidbody.GetPointVelocity(hit.position);

        m_GoalVelocity = Vector3.MoveTowards(m_GoalVelocity, (goalVel), accel * Time.fixedDeltaTime);
        //m_GoalVelocity = Vector3.MoveTowards(m_GoalVelocity, (goalVel) + (groundVel), accel * Time.fixedDeltaTime);

        Vector3 neededAccel = (m_GoalVelocity - m_Rigidbody.velocity) / Time.fixedDeltaTime;

        float maxAccel = m_MaxAccelerationForce * m_MaxAccelerationForceFactorFromDot.Evaluate(velDot) * m_MaxAccelerationMultiplier;

        neededAccel = Vector3.ClampMagnitude(neededAccel, maxAccel);

        m_Rigidbody.AddForce(Vector3.Scale(neededAccel * m_Rigidbody.mass, m_ForceScale));
    }

    private void UpdateCharacterVelocityPlaceholder()
    {
        m_Rigidbody.velocity = new Vector3(0, m_Rigidbody.velocity.y, 0);
        m_Rigidbody.velocity += m_Input.MovementDirection * m_MaxSpeed;
    }
}
