using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoints : MonoBehaviour
{
    private bool m_finishPoint;

    private void Start()
    {
        if(gameObject.tag == "FinishPoint")
            m_finishPoint = true;
    }

    private void OnTriggerEnter(Collider other)
    {
       CarGameManager.Instance.OnCheckPointTrigger(m_finishPoint);
    }

}
