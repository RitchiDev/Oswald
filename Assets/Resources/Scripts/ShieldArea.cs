using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldArea : MonoBehaviour
{
    private Transform m_Origin;
    public Transform Origin => m_Origin;

    private float m_Knockback;
    public float Knockback => m_Knockback;

    public void SetUp(Transform origin, float knockback = 1f)
    {
        m_Origin = origin;
        m_Knockback = knockback;
    }
}
