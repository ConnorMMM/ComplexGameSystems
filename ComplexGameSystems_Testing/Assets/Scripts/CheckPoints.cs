using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoints : MonoBehaviour
{
    [SerializeField] private CarGameManager m_carGameManager;

    private bool m_finishPoint;

    void Start()
    {
        if(gameObject.tag == "FinishPoint")
            m_finishPoint = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        m_carGameManager.OnCheckPointTrigger(m_finishPoint);
    }

}
