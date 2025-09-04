using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneHandler : MonoBehaviour
{
    public static SceneHandler Instance { get; private set; }

    [Header("Fade")]
    [SerializeField] private Image fadeImage;
    [SerializeField] private float fadeTime = 2f;

    private void Start()
    {
        // Set instance and keep loaded, or destroy if already existing
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

        fadeImage.material = new(fadeImage.material);
    }

    public async Task LoadScene(string scene)
    {
        Player.Instance.enabled = false;

        await FadeIn();
        SceneManager.LoadScene(scene);
        await FadeOut();

        Player.Instance.enabled = true;
    }

    public async Task FadeIn()
    {
        float time = 0;

        while (time < fadeTime)
        {
            time += Time.deltaTime;
            fadeImage.material.SetFloat("_Fade", Mathf.Clamp(time / fadeTime, 0, 1));
            await Awaitable.EndOfFrameAsync();
        }
    }

    public async Task FadeOut()
    {
        float time = 0;

        while (time < fadeTime)
        {
            time += Time.deltaTime;
            fadeImage.material.SetFloat("_Fade", Mathf.Clamp(1 - time / fadeTime, 0, 1));
            await Awaitable.EndOfFrameAsync();
        }
    }
}