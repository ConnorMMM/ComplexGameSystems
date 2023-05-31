using UnityEngine;

public abstract class TagRecorder : Recorder
{
    protected GameObject[] m_gameObjects = new GameObject[0];

    protected override void CollectPreviousFrame()
    {
        if (m_gameObjects.Length == 0)
            return;

        for (int i = 0; i < m_gameObjects.Length; i++)
            CollectPreviousFrameAt(i);
    }

    protected override void RecordInitialFrame()
    {
        if (m_gameObjects.Length == 0)
            return;

        for (int i = 0; i < m_gameObjects.Length; i++)
            RecordInitialFrameAt(i);
    }

    protected override void UpdateRecording()
    {
        if (m_gameObjects.Length == 0)
            return;

        for (int i = 0; i < m_gameObjects.Length; i++)
            UpdateRecordingAt(i);
    }

    /// <summary>
    /// Takes in an array and stores it. Updates the storage array lengths to correspond with the passed in array.
    /// </summary>
    /// <param name="_gameObjects"></param>
    public void SetGameObjects(GameObject[] _gameObjects)
    {
        m_gameObjects = _gameObjects;
        SetRecorderLists(m_gameObjects.Length);
    }

    /// <summary>
    /// Stores the previous frame's values of an object in the array for later use.
    /// </summary>
    protected abstract void CollectPreviousFrameAt(int _index);

    /// <summary>
    /// Adds the starting values  of an object in the array to the MemoryStream.
    /// </summary>
    protected abstract void RecordInitialFrameAt(int _index);

    /// <summary>
    /// Adds the change between the current frame and the previously recorded frame of an object in the array to the MemoryStream.
    /// </summary>
    protected abstract void UpdateRecordingAt(int _index);

    /// <summary>
    /// Sets the length of the storage arrays.
    /// </summary>
    protected abstract void SetRecorderLists(int _length);
}
