using UnityEngine;

public class MatchBones : MonoBehaviour
{
    SkinnedMeshRenderer targetMeshRenderer;
    SkinnedMeshRenderer thisMeshRenderer;

    private void Start()
    {
        thisMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        targetMeshRenderer = Player.Instance.GetComponentInChildren<SkinnedMeshRenderer>();

        // Match bones
        thisMeshRenderer.rootBone = targetMeshRenderer.rootBone;
        thisMeshRenderer.bones = targetMeshRenderer.bones;
    }
}
