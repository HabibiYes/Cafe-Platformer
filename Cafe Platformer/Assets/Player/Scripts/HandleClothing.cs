using UnityEngine;

public class HandleClothing : MonoBehaviour
{
    Player player;

    [SerializeField] private SkinnedMeshRenderer clothingMeshRenderer;

    private void Start()
    {
        player = GetComponent<Player>();

        player.handleInventory.clothingChanged += ChangeClothing;
    }

    public void ChangeClothing(Mesh mesh)
    {
        clothingMeshRenderer.sharedMesh = mesh;
    }
}