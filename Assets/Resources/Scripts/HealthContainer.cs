using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthContainer : MonoBehaviour
{
    [SerializeField] private bool m_Follow;
    [SerializeField] private Vector3 m_ContainerOfsset;
    private Transform m_Target;

    [SerializeField] private GameObject m_HeartPrefab;
    [SerializeField] private Color m_TransparantColor;
    [SerializeField] private Color m_DefaultColor;
    private List<Image> m_Hearts = new List<Image>();
    private Camera m_Camera;
    public Camera Camera => m_Camera;

    private void Awake()
    {
        m_Camera = Camera.main;

    }

    public void SetUp(float amount, Transform target)
    {
        m_Target = target;

        for (int i = 0; i < amount; i++)
        {
            GameObject heart = Instantiate(m_HeartPrefab, transform, false);
            Image renderer = heart.GetComponent<Image>();
            m_Hearts.Add(renderer);
        }

        m_DefaultColor = m_Hearts[0].color;
    }

    public void UpdateHearts(float health, bool destroy = false)
    {
        //Debug.Log(health);

        for (int i = 0; i < m_Hearts.Count; i++)
        {
            Animator animator = m_Hearts[i].GetComponent<Animator>();

            if(i >= health)
            {
                if(destroy)
                {
                    GameObject heart = m_Hearts[i].gameObject;
                    m_Hearts.Remove(m_Hearts[i]);
                    Destroy(heart);
                }
                else
                {
                    //Debug.Log(i + ".");

                    m_Hearts[i].color = m_TransparantColor;
                    if(animator)
                    {
                        animator.SetBool("Flip", false);
                    }
                }
            }
            else
            {
                m_Hearts[i].color = m_DefaultColor;
                if (animator)
                {
                    animator.SetBool("Flip", true);
                }
            }
        }

        //m_Hearts[(int)health].color = m_TransparantColor;
        //Destroy(m_Hearts[(int)health].gameObject);

        //for (int i = 0; i < m_Hearts.Count; i++)
        //{
        //    if(destroy)
        //    {
        //        Destroy(m_Hearts[(int)health]);
        //        //m_Hearts.Remove(m_Hearts[i]);
        //        return;
        //    }

        //    m_Hearts[i].color = m_TransparantColor;
        //}
    }

    private void LateUpdate()
    {
        if(!m_Follow)
        {
            return;
        }

        RotateContainer();
        MoveContainer();
    }

    private void RotateContainer()
    {
        //transform.LookAt(m_Camera.transform);
    }

    private void MoveContainer()
    {
        transform.position = m_Target.position + m_ContainerOfsset;
    }
}
