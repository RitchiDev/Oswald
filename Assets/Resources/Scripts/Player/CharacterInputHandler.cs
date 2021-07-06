using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Timeline;

public class CharacterInputHandler : MonoBehaviour
{
    private Vector3 m_MovementDirection;
    public Vector3 MovementDirection => m_MovementDirection;

    private Vector3 m_LookDirection;
    public Vector3 LookDirection => m_LookDirection;

    private bool m_Attack;
    public bool Attack => m_Attack;

    private bool m_ChargeAttack;
    public bool ChargeAttack => m_ChargeAttack;

    private bool m_Shield;
    public bool Shield => m_Shield;

    private bool m_ShieldBash;
    public bool ShieldBash => m_ShieldBash;

    public void MovementContext(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            m_MovementDirection = new Vector3(context.ReadValue<Vector2>().x, 0, context.ReadValue<Vector2>().y);
        }

        if (context.canceled)
        {
            m_MovementDirection = Vector3.zero;
        }
    }

    public void LookContext(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            m_LookDirection = new Vector3(context.ReadValue<Vector2>().x, 0, context.ReadValue<Vector2>().y);
        }

        if (context.canceled)
        {
            m_LookDirection = Vector3.zero;
        }
    }

    public void AttackContext(InputAction.CallbackContext context)
    {
        if(!m_Attack && context.performed)
        {
            StartCoroutine(AttackTimer());
        }

        m_ChargeAttack = context.performed;
    }

    public void ShieldContext(InputAction.CallbackContext context)
    {
        m_Shield = context.performed;
    }

    private IEnumerator AttackTimer()
    {
        m_Attack = true;

        yield return new WaitForEndOfFrame();

        m_Attack = false;
    }
}
