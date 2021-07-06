using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageAble : MonoBehaviour
{
    [SerializeField] private float m_Knockback;
    public float Knockback => m_Knockback;
}
