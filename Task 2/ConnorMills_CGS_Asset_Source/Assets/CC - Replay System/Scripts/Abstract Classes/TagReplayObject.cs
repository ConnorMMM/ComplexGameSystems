using UnityEngine;

public abstract class TagReplayObject : ReplayObject
{
    protected GameObject[] m_gameObjects;

    protected override void ApplyInitialFrame()
    {
        for (int i = 0; i < m_gameObjects.Length; i++)
            ApplyInitialFrameAt(i);
    }

    protected override void UpdateReplay()
    {
        for (int i = 0; i < m_gameObjects.Length; i++)
            UpdateReplayAt(i);
    }

    protected override void InitializeObjects(GameObject[] _gameObjects)
    {
        m_gameObjects = new GameObject[_gameObjects.Length];
        for (int i = 0; i < _gameObjects.Length; i++)
        {
            GameObject replayObject = Instantiate(_gameObjects[i], gameObject.transform);

            foreach (Rigidbody rb in replayObject.GetComponents<Rigidbody>())
                Destroy(rb);

            foreach (Collider c in replayObject.GetComponents<Collider>())
                Destroy(c);

            UpdateToGhost(replayObject);

            m_gameObjects[i] = replayObject;
        }
    }

    protected abstract void ApplyInitialFrameAt(int _index);

    protected abstract void UpdateReplayAt(int _index);

    protected abstract void UpdateToGhost(GameObject _replayObject);
}
