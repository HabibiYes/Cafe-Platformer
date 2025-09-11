using UnityEngine;

public class OrderDrink : MonoBehaviour
{
    Customer customer;

    private GameObject icon;
    [SerializeField] private float iconCircleColorMultiplier = 0.5f;

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
        int index = Random.Range(0, GameData.Instance.business.drinks.Count);
        orderedDrink = GameData.Instance.business.drinks[index];

        // Set and activate icon
        MeshRenderer meshRenderer = transform.Find("Icon").GetComponent<MeshRenderer>();
        meshRenderer.material.SetFloat("_Index", index);
        meshRenderer.material.SetColor("_Color", orderedDrink.color * iconCircleColorMultiplier);
        icon.SetActive(true);
    }
}