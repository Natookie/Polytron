using UnityEngine;
using Nova;

public class InventorySlotUI : MonoBehaviour
{
    [SerializeField] private UIBlock2D baseBlock;
    [SerializeField] private UIBlock2D icon;
    [Space(10)]
    [SerializeField] private CoreButton btn;

    private InventoryManager.Slot slot;

    void Awake() => btn.OnClick.AddListener(HandleClick);
    public void Bind(InventoryManager.Slot slotData){
        if(slotData == null || slotData.IsEmpty) return;
        
        slot = slotData;
        gameObject.SetActive(true);
        icon.SetImage(slot.Item.Icon);
        baseBlock.SetImage(slot.Item.BorderColor);;
    }

    void HandleClick(){
        CurrencyManager.Instance.UpdatePlayerCurrency(slot.Item.SellValue);
        CurrencyManager.Instance.UpdateInventoryCurrency(-slot.Item.SellValue);
        InventoryManager.Instance.RemoveItem(slot.Item);
    }
}