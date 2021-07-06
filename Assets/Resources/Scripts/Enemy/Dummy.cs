using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummy : MonoBehaviour
{
    [SerializeField] private float m_BlinkIntensity;
    [SerializeField] private float m_BlinkDuration;
    [SerializeField] private MeshRenderer m_MeshRenderer;
    private Color m_DefaultColor;

    private Rigidbody m_Rigidbody;

    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        m_DefaultColor = m_MeshRenderer.material.color;
    }

    private IEnumerator Blink()
    {
        m_MeshRenderer.material.color = Color.white * m_BlinkIntensity;
        yield return new WaitForSeconds(m_BlinkDuration);
        m_MeshRenderer.material.color = m_DefaultColor;
    }

    private void OnTriggerEnter(Collider other)
    {
        ShieldArea blocked = other.GetComponent<ShieldArea>();
        if(blocked)
        {
            Vector3 knockbackDirection = (m_Rigidbody.position - blocked.Origin.position).normalized;
            knockbackDirection.y = 0;
            m_Rigidbody.AddForce(knockbackDirection * blocked.Knockback, ForceMode.Impulse);

            return;
        }

        AttackArea attacked = other.GetComponentInParent<AttackArea>();
        if(attacked)
        {
            Vector3 knockbackDirection = (m_Rigidbody.position - attacked.Origin.position).normalized;
            knockbackDirection.y = 0;
            m_Rigidbody.AddForce(knockbackDirection * attacked.Knockback, ForceMode.Impulse);

            StartCoroutine(Blink());

            return;
        }
    }
}
