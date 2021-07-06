using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{

    [Header("Enemy")]
    [SerializeField] private MeshRenderer m_MeshRenderer;
    [SerializeField] private Canvas m_CanvasPrefab;
    private Rigidbody m_Rigidbody;
    private Canvas m_Canvas;

    [Header("Health")]

    [SerializeField] private float m_MaxHealth;
    private HealthContainer m_HealthContainer;
    private float m_Health;

    [Header("Hit")]
    [SerializeField] private float m_HitIntensity;
    [SerializeField] private float m_HitDuration;
    [SerializeField] private float m_TimeInvincible;
    private Color m_DefaultColor;
    private bool m_IsInvincible;

    [Header("Death")]
    [SerializeField] private ParticleSystem m_DeathEffect;
    [SerializeField] private UnityEvent m_OnDeath;
    private bool m_IsDead;

    [Header("Debug")]
    [SerializeField] private bool m_CannotDie;

    private void Awake()
    {
        m_Canvas = Instantiate(m_CanvasPrefab, Vector3.zero, m_CanvasPrefab.transform.rotation);
        m_HealthContainer = m_Canvas.GetComponentInChildren<HealthContainer>();
        m_Rigidbody = GetComponent<Rigidbody>();
        m_DefaultColor = m_MeshRenderer.material.color;
        //canvas.worldCamera = m_HealthContainer.Camera;
    }

    private void OnEnable()
    {
        m_IsDead = false;
        m_Health = m_MaxHealth;

        m_HealthContainer.SetUp(m_Health, transform);

        //UpdateHearts(m_Health);
    }

    public void SetVitality(float amount)
    {
        m_MaxHealth = Mathf.Clamp(amount, 0, 999999);
        m_Health = m_MaxHealth;
        //UpdateHearts(m_Health);
    }

    public void ChangeVitality(float amount, bool forcedDamage = false)
    {
        if (amount < 0)
        {
            if (m_IsInvincible && !forcedDamage)
            {
                return;
            }

            StartCoroutine(HitIndicator());
            StartCoroutine(GiveInvincibility(m_TimeInvincible));
        }

        m_Health = Mathf.Clamp(m_Health + amount, 0, m_MaxHealth);
        UpdateHearts(m_Health);

        if (m_CannotDie || m_Health > 0 || m_IsDead)
        {
            return;
        }

        Die();
    }

    private void UpdateHearts(float health)
    {
        m_HealthContainer.UpdateHearts(health - 1, true);
    }

    private void Die()
    {
        if(m_DeathEffect)
        {
            ParticleSystem particleEffect = Instantiate(m_DeathEffect, transform.position, transform.rotation);
        }

        m_IsDead = true;

        Destroy(m_Canvas);

        gameObject.SetActive(false);
    }

    private IEnumerator HitIndicator()
    {
        m_MeshRenderer.material.color = Color.white * m_HitIntensity;
        yield return new WaitForSeconds(m_HitDuration);
        m_MeshRenderer.material.color = m_DefaultColor;
    }

    private IEnumerator GiveInvincibility(float time)
    {
        m_IsInvincible = true;

        yield return new WaitForSeconds(time);

        m_IsInvincible = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        ShieldArea blocked = other.GetComponent<ShieldArea>();
        if (blocked)
        {
            Vector3 knockbackDirection = (m_Rigidbody.position - blocked.Origin.position).normalized;
            knockbackDirection.y = 0;
            m_Rigidbody.AddForce(knockbackDirection * blocked.Knockback, ForceMode.Impulse);

            return;
        }

        AttackArea attacked = other.GetComponentInParent<AttackArea>();
        if (attacked)
        {
            Vector3 knockbackDirection = (m_Rigidbody.position - attacked.Origin.position).normalized;
            knockbackDirection.y = 0;
            m_Rigidbody.AddForce(knockbackDirection * attacked.Knockback, ForceMode.Impulse);

            ChangeVitality(-1f);

            return;
        }
    }
}
