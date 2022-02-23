using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [SerializeField] private CharacterInputHandler m_Input;
    [SerializeField] private LayerMask m_TargetLayer;
    [SerializeField] private LayerMask m_ObstacleLayer;

    [Header("Attack")]
    [SerializeField] private AttackArea m_AttackAreaPrefab;
    [SerializeField] private ParticleSystem m_SlashEffect;
    [SerializeField] private Animator m_AttackAnimator;
    [SerializeField] private Transform m_AttackAreaSpawn;
    [SerializeField] private float m_AttackTime = 0.4f;
    [SerializeField] private float m_AttackAreaTime = 0.1f;
    [SerializeField] private float m_AttackKnockback = 3f;
    [SerializeField] private float m_AttackRadius = 3f;
    [SerializeField] private float m_AttackAngle = 3f;
    private ParticleSystem.MainModule m_Slash;
    public float AttackRadius => m_AttackRadius;
    public float AttackAngle => m_AttackAngle;

    [Header("Shield")]
    [SerializeField] private ShieldArea m_ShieldArea;
    [SerializeField] private Animator m_ShieldAnimator;
    [SerializeField] private float m_ShieldTime = 1.4f;
    [SerializeField] private float m_ShieldKnockback = 3f;

    private float m_DisableTime;

    private void Awake()
    {
        m_Slash = m_SlashEffect.main;
    }
    private void Update()
    {
        if (m_DisableTime > 0)
        {
            m_DisableTime -= Time.deltaTime;

            if(m_DisableTime <= 0 && m_ShieldAnimator.GetBool("Shield"))
            {
                m_ShieldAnimator.SetBool("Shield", false);
            }

            return;
        }

        if(m_Input.Attack)
        {
            Attack();
        }

        if(m_Input.Shield)
        {
            Shield();
        }
        else if(!m_Input.Shield)
        {
            m_ShieldAnimator.SetBool("Shield", false);
        }
    }

    private void AttackArea()
    {
        
    }
    
    private void Attack()
    {
        if (m_Input.Shield)
        {
            return;
        }

        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, m_AttackRadius, m_TargetLayer);

        if (rangeChecks.Length != 0)
        {
            for (int i = 0; i < rangeChecks.Length; i++)
            {
                //v.vertex.y += cos((v.vertex.x + _Time.y * _Speed) * _Frequency) * _Amplitude * v.vertex.x;
                //v.vertex.y += cos((v.vertex.x + _Time.y * 0.25) * 1) * 1 * v.vertex.x;

                //v.vertex.y += sin((v.vertex.x + _Time.w * _Speed) * _Frequency) * _Amplitude * v.vertex.x);
                //o.vertex.y += sin(worldPos.z + _Time.w * _Speed);

                Transform target = rangeChecks[i].transform;
                //Vector3 directionToTarget = (target.position - transform.position).normalized;

                Vector3 test = rangeChecks[i].ClosestPointOnBounds(transform.position);
                Vector3 directionToTarget = (test - transform.position).normalized;

                if (Vector3.Angle(transform.forward, directionToTarget) < m_AttackAngle / 2)
                {
                    //float distanceToTarget = Vector3.Distance(transform.position, target.position);
                    float distanceToTarget = Vector3.Distance(transform.position, test);

                    if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, m_ObstacleLayer))
                    {
                        target.GetComponent<EnemyHealth>().ChangeVitality(-1, transform, m_AttackKnockback);
                    }
                }
            }
        }

        m_AttackAnimator.ResetTrigger("Attack");
        m_AttackAnimator.SetTrigger("Attack");
        m_DisableTime = m_AttackTime;
    }

    //private void Attack()
    //{
    //    if(m_Input.Shield)
    //    {
    //        return;
    //    }

    //    //Vector2 slashDirection = m_Input.MovementDirection != Vector3.zero ? m_Input.MovementDirection : m_Input.LookDirection;
    //    //m_Slash.startRotation = Mathf.Atan2(-slashDirection.y, slashDirection.x);
    //    //m_Slash.startRotation = transform.rotation.eulerAngles.y * Mathf.Deg2Rad;

    //    AttackArea attackArea = Instantiate(m_AttackAreaPrefab, m_AttackAreaSpawn.position, m_Input.transform.rotation);
    //    attackArea.SetUp(m_AttackAreaTime, m_Input.transform, m_AttackKnockback);

    //    m_AttackAnimator.ResetTrigger("Attack");
    //    m_AttackAnimator.SetTrigger("Attack");
    //    m_DisableTime = m_AttackTime;
    //}

    private void Shield()
    {
        if (m_Input.Attack)
        {
            return;
        }

        m_ShieldArea.SetUp(m_Input.transform, m_ShieldKnockback);

        m_ShieldAnimator.SetBool("Shield", true);
        m_DisableTime = m_ShieldTime;
    }
}
