using UnityEngine;
using UnityEngine.SceneManagement;

public class PersistantGroup : MonoBehaviour
{

    public static PersistantGroup Instance;

    // private void Awake()
    // {
    //     if (Instance == null)
    //     {
    //         Instance = this;
    //         DontDestroyOnLoad(gameObject);
    //     }
    //     else
    //     {
    //         Destroy(gameObject);
    //     }
    // }
}

