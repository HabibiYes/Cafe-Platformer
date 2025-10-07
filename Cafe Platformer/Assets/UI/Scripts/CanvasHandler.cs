using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CanvasHandler : MonoBehaviour
{
    public static CanvasHandler Instance { get; private set; }

    [HideInInspector] public GraphicRaycaster graphicRaycaster;

    [Header("HUD")]
    public Image healthBar;
    public TMP_Text moneyText;

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

        graphicRaycaster = GetComponent<GraphicRaycaster>();
    }
}
