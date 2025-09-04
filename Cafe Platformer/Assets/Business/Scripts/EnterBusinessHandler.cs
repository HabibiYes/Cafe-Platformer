using UnityEngine;
using UnityEngine.SceneManagement;

public class EnterBusinessHandler : MonoBehaviour
{
    public void EnterBusiness()
    {
        SceneManager.LoadScene("Business");
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("EnterBusiness"))
        {
            EnterBusiness();
        }
    }
}
