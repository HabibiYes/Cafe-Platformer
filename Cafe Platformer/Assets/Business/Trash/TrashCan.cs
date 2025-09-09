using UnityEngine;

public class TrashCan : MonoBehaviour
{
    public int capacity = 5;
    [HideInInspector] public int trashAmount = 0;

    public bool AddTrash()
    {
        if (trashAmount < capacity)
        {
            trashAmount += 1;
            return true;
        }
        return false;
    }

    public void EmptyTrash()
    {
        trashAmount = 0;
    }
}