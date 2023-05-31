using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transform_TagReplayObject : TagReplayObject
{
    protected override void ApplyInitialFrameAt(int _index)
    {
        m_gameObjects[_index].transform.position         = ReadVector3();
        m_gameObjects[_index].transform.localEulerAngles = ReadVector3();
        m_gameObjects[_index].transform.localScale       = ReadVector3();
    }

    protected override void UpdateReplayAt(int _index)
    {
        m_gameObjects[_index].transform.position         += RetrieveVector3();
        m_gameObjects[_index].transform.localEulerAngles += RetrieveVector3();
        m_gameObjects[_index].transform.localScale       += RetrieveVector3();
    }

    protected override void UpdateToGhost(GameObject _replayObject)
    {
        foreach (Renderer r in _replayObject.GetComponents<Renderer>())
            r.material.color = Color.cyan;
    }
}
