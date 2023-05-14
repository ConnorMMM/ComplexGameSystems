using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class RecorderManager : MonoBehaviour
{
    [SerializeField] private Recorder recorder;
    [SerializeField, Range(1, 10)] private int numOfReplaysToCollect = 1;
    [SerializeField] private ReplayObject replayObjectPrefab;
    [SerializeField] private List<ReplayObject> replayObjects;
    [SerializeField] private List<MemoryStream> memoryStreams;

    private int numOfCollectedReplays = 0;

    private bool replayObjectsInitialized = false;

    private void Awake()
    {
        replayObjects = new List<ReplayObject>(numOfReplaysToCollect);
        memoryStreams = new List<MemoryStream>(numOfReplaysToCollect);
    }

    public void StartRecording()
    {
        recorder.StartRecording();
    }

    public void StopRecording()
    {
        recorder.StopRecording();
    }

    public void CollectRecording()
    {
        if(numOfReplaysToCollect == 1)
        {
            memoryStreams.Insert(0, recorder.GetMemoryStream());
            numOfCollectedReplays = 1;
        }
        else if(numOfCollectedReplays < numOfReplaysToCollect)
        {
            memoryStreams.Insert(numOfCollectedReplays, recorder.GetMemoryStream());
            numOfCollectedReplays++;
        }
        else
        {
            for(int i = numOfCollectedReplays - 2; i >= 0; i--)
            {
                MemoryStream tempStream = memoryStreams[i + 1];
                memoryStreams.Insert(i + 1, memoryStreams[i]);
                tempStream.Dispose();
            }
            memoryStreams.Insert(0, recorder.GetMemoryStream());
        }

        recorder.StopRecording();
    }

    public void FinishRecording()
    {

    }

    public void RestartReplay()
    {
        if(!replayObjectsInitialized)
        {
            for(int i = 0; i < numOfCollectedReplays; i++)
            {
                replayObjects.Insert(i, Instantiate(replayObjectPrefab));
                replayObjects[i].SetMemoryStream(memoryStreams[i]);
            }
        }

        for (int i = 0; i < numOfCollectedReplays; i++)
        {
            replayObjects[i].StartReplay();
        }
    }

    public void PlayReplay()
    {
        if (!replayObjectsInitialized)
            return;

        for (int i = 0; i < numOfCollectedReplays; i++)
        {
            replayObjects[i].PlayReplay();
        }
    }

    public void PauseReplay()
    {
        if (!replayObjectsInitialized)
            return;

        for (int i = 0; i < numOfCollectedReplays; i++)
        {
            replayObjects[i].PauseReplay();
        }
    }

    public void DestroyReplays()
    {
        if (!replayObjectsInitialized)
            return;

        for(int i = 0; i < numOfCollectedReplays; i++)
        {
            Destroy(replayObjects[i].gameObject);
        }

        replayObjectsInitialized = false;
    }
}
