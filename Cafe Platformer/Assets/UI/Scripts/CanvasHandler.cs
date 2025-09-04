using UnityEngine;

public class CanvasHandler : MonoBehaviour
{
    public static CanvasHandler Instance { get; private set; }

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
            return;
        }
    }
}
