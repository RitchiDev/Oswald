using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackArea : MonoBehaviour
{
    private Transform m_Origin;
    public Transform Origin => m_Origin;

    private float m_Knockback;
    public float Knockback => m_Knockback;

    public void SetUp(float time, Transform origin, float knockback = 1f)
    {
        m_Origin = origin;
        m_Knockback = knockback;
        StartCoroutine(Timer(time));
    }

    private IEnumerator Timer(float time)
    {
        yield return new WaitForSeconds(time);
        m_Origin = null;
        Destroy(gameObject);
        //gameObject.SetActive(false);
    }
}
