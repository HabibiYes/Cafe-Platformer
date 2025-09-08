using UnityEngine;

public class Drink : MonoBehaviour
{
    public DrinkData data;
    [SerializeField] private int drinkMaterialIndex = 1;

    private MeshRenderer meshRenderer;

    private void Start()
    {
        // Get mesh renderer and set drink color
        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.materials[drinkMaterialIndex].color = data.color;
    }
}
