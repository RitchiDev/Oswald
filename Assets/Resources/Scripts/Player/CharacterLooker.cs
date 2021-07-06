using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterLooker : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private CharacterInputHandler m_InputHandler;

    [Header("Rotation")]
    [SerializeField] private float m_RotateSpeed;
    private Rigidbody m_Rigidbody;

    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        UpdateCharacterRotation();
    }

    private void UpdateCharacterRotation()
    {
        Vector3 forward = m_Rigidbody.transform.forward;

        //Vector3 look = m_InputHandler.LookDirection;
        Vector3 look = new Vector3(m_InputHandler.LookDirection.x, 0, m_InputHandler.LookDirection.z);
        if(look == Vector3.zero)
        {
            //Vector3 look = m_InputHandler.MovementDirection;
            look = new Vector3(m_InputHandler.MovementDirection.x, 0, m_InputHandler.MovementDirection.z);
        }


        float goalSpeed = m_RotateSpeed * Time.deltaTime;

        Vector3 lookGoal = Vector3.RotateTowards(forward, look, goalSpeed, 0);
        m_Rigidbody.rotation = Quaternion.LookRotation(lookGoal, Vector3.up);
    }
}
