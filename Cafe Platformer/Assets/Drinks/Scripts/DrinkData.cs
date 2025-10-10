using UnityEngine;

[CreateAssetMenu(fileName = "NewDrinkData", menuName = "Drink Data")]
public class DrinkData : ScriptableObject
{
    new public string name = "Drink";
    public int price = 0;
    public Color color = Color.white;
}
