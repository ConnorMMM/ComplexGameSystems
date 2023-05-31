using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoints : MonoBehaviour
{
    private Collider m_collider;
    private Material m_material;

    private bool m_finishPoint;

    private void Awake()
    {
        if(gameObject.tag == "FinishPoint")
            m_finishPoint = true;

        m_collider = GetComponent<Collider>();
        m_material = new Material(GetComponentInChildren<MeshRenderer>().material);

        foreach(MeshRenderer mr in gameObject.GetComponentsInChildren<MeshRenderer>())
        {
            mr.material = m_material;
        }

        if(!m_finishPoint)
            m_material.color = Color.red;
    }

    private void OnTriggerEnter(Collider other)
    {
        CarGameManager.Instance.OnCheckPointTrigger(m_finishPoint);

        if(!m_finishPoint)
        {
            m_collider.enabled = false;
            m_material.color = Color.green;
        }
    }

    public void ResetCheckPoint()
    {
        if (!m_finishPoint)
        {
            m_collider.enabled = true;
            m_material.color = Color.red;
        }
    }
}
