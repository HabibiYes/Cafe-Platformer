using UnityEngine;

public class OrderDrink : MonoBehaviour
{
    Customer customer;

    private GameObject icon;

    [HideInInspector] public DrinkData orderedDrink;

    private void Awake()
    {
        customer = GetComponent<Customer>();

        // Get and deactivate icon
        icon = transform.Find("Icon").gameObject;
        icon.SetActive(false);
    }

    public void Order()
    {
        int index = Random.Range(0, GameData.Instance.drinks.Count);
        orderedDrink = GameData.Instance.drinks[index];

        // Set and activate icon
        icon.GetComponent<MeshRenderer>().material.SetFloat("_Index", index);
        icon.SetActive(true);
    }
}