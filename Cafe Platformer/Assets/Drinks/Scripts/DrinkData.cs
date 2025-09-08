using UnityEngine;

[CreateAssetMenu(fileName = "NewDrinkData", menuName = "Drink Data")]
public class DrinkData : ScriptableObject
{
    new public string name = "Drink";
    public float price = 0f;
    public Color color = Color.white;
}
