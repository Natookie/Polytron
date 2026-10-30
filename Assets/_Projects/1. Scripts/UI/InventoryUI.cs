using UnityEngine;
using Nova;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private Transform slotContainer;
    [SerializeField] private GameObject slotPrefab;

    void Start(){
        if(InventoryManager.Instance != null) InventoryManager.Instance.OnInventoryChanged += Refresh;
        Refresh();
    }

    void OnDisable(){
        if(InventoryManager.Instance != null) InventoryManager.Instance.OnInventoryChanged -= Refresh;
    }

    void Refresh(){
        for(int i = slotContainer.childCount - 1; i >= 0; i--) Destroy(slotContainer.GetChild(i).gameObject);

        var inv = InventoryManager.Instance;
        if(inv == null) return;

        int count = inv.Slots.Count;
        for(int i = 0; i < count; i++){
            var go = Instantiate(slotPrefab, slotContainer);
            var ui = go.GetComponent<InventorySlotUI>();
            ui.Bind(inv.Slots[i]);
        }
    }
}