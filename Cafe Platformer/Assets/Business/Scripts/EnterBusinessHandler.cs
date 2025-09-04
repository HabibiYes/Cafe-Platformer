using UnityEngine;

public class EnterBusinessHandler : MonoBehaviour
{
    public async void EnterBusiness()
    {
        await SceneHandler.Instance.LoadScene("Business");
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("EnterBusiness"))
        {
            EnterBusiness();
        }
    }
}
