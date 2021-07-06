using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floater : MonoBehaviour
{
    [SerializeField] private CharacterInputHandler m_Input;
    private Animator m_Animator;

    private void Awake()
    {
        m_Animator = GetComponent<Animator>();
    }

    private void Update()
    {
        m_Animator.SetBool("Moving", m_Input.MovementDirection != Vector3.zero);
    }

    private void OnDisable()
    {
        m_Animator.SetBool("Moving", false);
    }
}
