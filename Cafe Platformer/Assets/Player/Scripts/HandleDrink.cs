using UnityEngine;

public class HandleDrink : MonoBehaviour
{
    [HideInInspector] public GetDrink getDrink;
    [HideInInspector] public GiveDrink giveDrink;

    [HideInInspector] public bool holdingDrink = false;
    [HideInInspector] public Drink currentDrink;

    private void Awake()
    {
        getDrink = GetComponent<GetDrink>();
        giveDrink = GetComponent<GiveDrink>();
    }

    public void SetDrink(Drink drink)
    {
        holdingDrink = true;
        currentDrink = drink;
    }

    public void ResetDrink()
    {
        holdingDrink = false;

        Destroy(currentDrink.gameObject);
        currentDrink = null;
    }
}