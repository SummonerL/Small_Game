using UnityEngine;

public class PostFXSingleton : MonoBehaviour
{
    private static PostFXSingleton _instance;

    public static PostFXSingleton Instance { get { return _instance; } }


    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }
    }
}
