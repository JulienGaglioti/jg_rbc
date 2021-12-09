using UnityEngine;

  public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
  {
    protected static T instance;
    private static bool applicationIsQuitting = false;

    public static T Instance 
    { 
      get 
      {
        if (applicationIsQuitting)
        {
          Debug.LogWarning ("[Singleton] Instance '" +
            typeof(T) +
            "' already destroyed on application quit." +
            " Won't create again - returning null.");
          return null;
        }
        return instance;
      } 
    }

    private void Awake ()
    {
      if (instance == null)
      {
        instance = GetComponent<T> ();
        DontDestroyOnLoad (this);
      }
      else
      {
        Destroy (gameObject);
      }
    }
    
    protected virtual void OnDestroy ()
    {
      applicationIsQuitting = true;

      if (instance == this)
      {
        instance = null;
      }
    }
  }





