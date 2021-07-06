using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance { get; private set; }

    [SerializeField] private float m_MoveSpeed = 3f;
    [SerializeField] private float m_LerpTime = 2f;
    private float m_ChangeTime;

    private Transform m_Player;

    private Transform m_NewTarget;
    private Transform m_OldTarget;
    private Vector3 m_CameraOffset;

    private bool m_MoveTowardsTarget;

    private void Awake()
    {
        if(Instance)
        {
            Debug.LogError("An instance of this " + Instance.ToString() + " already exists");
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        FindPlayer();
    }

    private void LateUpdate()
    {
        if (m_MoveTowardsTarget)
        {
            MoveTowardsTarget();

            return;
        }

        Vector3 playerPos = m_Player.transform.position;
        transform.position = playerPos + m_CameraOffset;
    }

    private void FindPlayer()
    {
        m_MoveTowardsTarget = false;
        m_Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        m_CameraOffset = transform.position - m_Player.position;
    }

    private void MoveTowardsTarget()
    {
        float progress = (Time.time - m_ChangeTime) / m_LerpTime;

        Vector3 desiredPosition = m_NewTarget.position + m_CameraOffset;
        desiredPosition.x = transform.position.x;
        desiredPosition.z = transform.position.z;

        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, Mathf.Clamp01(progress));
        transform.position = smoothedPosition;

        if (progress >= 1)
        {
            m_MoveTowardsTarget = false;
        }
    }

    public void SetNewTarget(Transform target, float waitTime)
    {
        //Debug.Log("Set New Target");
        m_OldTarget = m_NewTarget;
        m_NewTarget = target;
    }
}
