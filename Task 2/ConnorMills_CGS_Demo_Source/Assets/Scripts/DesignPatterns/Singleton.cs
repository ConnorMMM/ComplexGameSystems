using UnityEngine;

public abstract class Singleton<TYPE> : MonoBehaviour where TYPE : Singleton<TYPE>
{
    /// <summary> A static accessor for the singleton type. </summary>
    public static TYPE Instance
    {
        get
        {
            // Check if the instance has already been set
            if (instance == null)
            {
                // It hasn't been set, so attempt to find it
                instance = FindObjectOfType<TYPE>();

                //Checks again if the singleton is set, and if not, log an assertion and create one
                if (instance == null)
                {
                    Debug.LogAssertion($"Unable to find a GameObject with component {typeof(TYPE).Name}.");
                    instance = new GameObject($"{typeof(TYPE).Name} Singleton").AddComponent<TYPE>();
                }
            }

            // Return the cached instance
            return instance;
        }
    }

    /// <summary> The cached instance of this singleton. </summary>
    private static TYPE instance;

    /// <summary> Checks if the singleton has already been set, without attempting to set it. </summary>
    public static bool IsValid() => instance != null;

    /// <summary> Mark this singleton to never be destroyed by Unity, even when scene load. </summary>
    public static void FlagPersistant()
    {
        CreateSingletonInstance();
        DontDestroyOnLoad(instance.gameObject);
    }

    /// <summary> Gets the instance of the singleton if it is set, otherwise creates one. </summary>
    protected static TYPE CreateSingletonInstance() => Instance;

    /// <summary> Gets rid of the scene object as it is unneeded. </summary>
    protected virtual void Awake()
    {
        if (IsValid())
        {
            Destroy(gameObject);
        }
    }
}