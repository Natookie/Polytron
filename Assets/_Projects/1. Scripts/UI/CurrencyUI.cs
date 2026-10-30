using UnityEngine;
using Nova;

public class CurrencyUI : MonoBehaviour
{
    [SerializeField] private TextBlock playerCurrencyText;
    [SerializeField] private TextBlock inventoryCurrencyText;

    void Start(){
        CurrencyManager.Instance.OnPlayerCurrencyChanged    += HandlePlayerChanged;
        CurrencyManager.Instance.OnInventoryCurrencyChanged += HandleInventoryChanged;

        HandlePlayerChanged(CurrencyManager.Instance.playerCurrency);
        HandleInventoryChanged(CurrencyManager.Instance.inventoryCurrency);
    }

    void OnDisable(){
        CurrencyManager.Instance.OnPlayerCurrencyChanged    -= HandlePlayerChanged;
        CurrencyManager.Instance.OnInventoryCurrencyChanged -= HandleInventoryChanged;
    }

    void HandlePlayerChanged(int value) => playerCurrencyText.Text          = $"Coin    : {value}";
    void HandleInventoryChanged(int value) => inventoryCurrencyText.Text    = $"Storage : {value}";
}