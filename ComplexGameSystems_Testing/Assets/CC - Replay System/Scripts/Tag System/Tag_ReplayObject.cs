using UnityEngine;

public class Tag_ReplayObject : ReplayObject
{
    private GameObject[] m_gameObjects;

    protected override void ApplyInitialFrame()
    {
        foreach (GameObject gameObject in m_gameObjects)
            gameObject.transform.position = ReadVector3();

        foreach (GameObject gameObject in m_gameObjects)
            gameObject.transform.localEulerAngles = ReadVector3();

        foreach (GameObject gameObject in m_gameObjects)
            gameObject.transform.localScale = ReadVector3();
    }

    protected override void UpdateReplay()
    {
        foreach (GameObject gameObject in m_gameObjects)
            gameObject.transform.position += RetrieveVector3();

        foreach (GameObject gameObject in m_gameObjects)
            gameObject.transform.localEulerAngles += RetrieveVector3();

        foreach (GameObject gameObject in m_gameObjects)
            gameObject.transform.localScale += RetrieveVector3();
    }

    protected override void InitializeObjects(GameObject[] _gameObjects)
    {
        m_gameObjects = new GameObject[_gameObjects.Length];
        for (int i = 0; i < _gameObjects.Length; i++)
        {
            GameObject replayObject = Instantiate(_gameObjects[i], gameObject.transform);

            Rigidbody rb = replayObject.GetComponent<Rigidbody>();
            if (rb != null)
                Destroy(rb);

            foreach (Collider c in replayObject.GetComponents<Collider>())
                Destroy(c);

            replayObject.GetComponent<Renderer>().material.color = Color.cyan;

            m_gameObjects[i] = replayObject;
        }
    }
}
