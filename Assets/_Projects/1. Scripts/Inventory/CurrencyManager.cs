using NaughtyAttributes;
using UnityEngine;
using System;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance { get; private set; }

    [SerializeField] private int defaultPlayerCurrency = 200;
    [ReadOnly] public int playerCurrency;
    [ReadOnly] public int inventoryCurrency;

    public event Action<int> OnPlayerCurrencyChanged;
    public event Action<int> OnInventoryCurrencyChanged;

    void Awake(){
        if(Instance == null) Instance = this;
        else{
            Destroy(gameObject);
            return;
        }

        playerCurrency = defaultPlayerCurrency;
    }

    public void UpdatePlayerCurrency(int value){
        playerCurrency += value;
        OnPlayerCurrencyChanged?.Invoke(playerCurrency);
    }
    public void UpdateInventoryCurrency(int value){
        inventoryCurrency += value;
        OnInventoryCurrencyChanged?.Invoke(inventoryCurrency);
    }
}