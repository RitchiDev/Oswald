using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private GameObject m_Avatar;
    [SerializeField] private GameObject m_HitAvatar;
    [SerializeField] private List<MeshRenderer> m_MeshRenderers;
    private Rigidbody m_Rigidbody;

    [Header("Health")]
    [SerializeField] private float m_MaxHealth;
    [SerializeField] private HealthContainer m_HealthContainer;
    private float m_Health;

    [Header("Hit")]
    [SerializeField] private Color m_TransparantColor;
    [SerializeField] private float m_HitIntensity;
    [SerializeField] private float m_HitDuration;
    [SerializeField] private float m_TimeInvincible;
    private List<Color> m_DefaultColors = new List<Color>();
    private bool m_IsInvincible;

    [Header("Death")]
    [SerializeField] private ParticleSystem m_DeathEffect;
    [SerializeField] private UnityEvent m_OnDeath;
    private bool m_IsDead;

    [Header("Debug")]
    [SerializeField] private bool m_CannotDie;

    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        for (int i = 0; i < m_MeshRenderers.Count; i++)
        {
            m_DefaultColors.Add(m_MeshRenderers[i].material.color);
        }
    }

    private void OnEnable()
    {
        m_IsDead = false;
        m_Health = m_MaxHealth;
        m_HealthContainer.SetUp(m_Health, transform);
    }

    private void UpdateHearts(float health)
    {
        m_HealthContainer.UpdateHearts(health);
    }

    public void SetVitality(float amount)
    {
        m_MaxHealth = Mathf.Clamp(amount, 0, 999999);
        m_Health = m_MaxHealth;
    }

    public void ChangeVitality(float amount, bool forcedDamage = false)
    {
        //if (m_Health <= 0)
        //{
        //    return;
        //}

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

    private void Die()
    {
        if (m_DeathEffect)
        {
            ParticleSystem particleEffect = Instantiate(m_DeathEffect, transform.position, transform.rotation);
        }

        m_IsDead = true;

        if (m_OnDeath != null)
        {
            m_OnDeath.Invoke();
        }

        gameObject.SetActive(false);
    }

    public void IncreaseMaxHealth(int amount)
    {
        m_MaxHealth = Mathf.Clamp(m_MaxHealth + amount, 0f, 6f);
    }

    private IEnumerator HitIndicator()
    {
        for (int i = 0; i < m_MeshRenderers.Count; i++)
        {
            m_MeshRenderers[i].material.color = Color.white * m_HitIntensity;
        }

        yield return new WaitForSeconds(m_HitDuration);

        for (int i = 0; i < m_MeshRenderers.Count; i++)
        {
            m_MeshRenderers[i].material.color = m_DefaultColors[i];
        }
    }

    private IEnumerator GiveInvincibility(float time)
    {
        float remainingTime = time;
        m_IsInvincible = true;

        yield return new WaitForSeconds(m_HitDuration);
        remainingTime -= m_HitDuration;

        m_Avatar.SetActive(false);
        m_HitAvatar.SetActive(true);

        //for (int i = 0; i < m_MeshRenderers.Count; i++)
        //{
        //    //Debug.Log(m_MeshRenderers[i].name);
        //    m_MeshRenderers[i].material.color = m_TransparantColor;
        //}

        yield return new WaitForSeconds(remainingTime);

        m_Avatar.SetActive(true);
        m_HitAvatar.SetActive(false);

        //for (int i = 0; i < m_MeshRenderers.Count; i++)
        //{
        //    m_MeshRenderers[i].material.color = m_DefaultColors[i];
        //}

        m_IsInvincible = false;
    }

    private void OnTriggerStay(Collider other)
    {
        DamageAble attacked = other.GetComponent<DamageAble>();
        if(attacked)
        {
            if(!m_IsInvincible)
            {
                Vector3 knockbackDirection = (m_Rigidbody.position - attacked.transform.position).normalized;
                knockbackDirection.y = 0;
                m_Rigidbody.AddForce(knockbackDirection * attacked.Knockback, ForceMode.Impulse);
            }

            ChangeVitality(-1f);
        }
    }
}
