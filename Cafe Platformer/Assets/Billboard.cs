using UnityEngine;

public class Billboard : MonoBehaviour
{
    Camera cam;

    private void Start()
    {
        cam = Camera.main;
    }

    private void LateUpdate()
    {
        transform.rotation = Quaternion.LookRotation(cam.transform.forward);
    }
}
