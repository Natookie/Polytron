using UnityEngine;
using System.Collections.Generic;
using Nova;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private Transform slotContainer;
    [SerializeField] private GameObject slotPrefab;

    private readonly List<InventorySlotUI> children = new();

    void Start(){
        if(InventoryManager.Instance != null) InventoryManager.Instance.OnInventoryChanged += Refresh;
        Refresh();
    }

    void OnDisable(){
        if(InventoryManager.Instance != null) InventoryManager.Instance.OnInventoryChanged -= Refresh;
    }

    void Refresh(){
        var inv = InventoryManager.Instance;
        if(inv == null) return;

        var slots = inv.Slots;
        int count = slots.Count;
    
        while(children.Count < count){
            var go = Instantiate(slotPrefab, slotContainer);
            children.Add(go.GetComponent<InventorySlotUI>());
        }

        for(int i = 0; i < children.Count; i++){
            var ui = children[i];

            if(i < count){
                ui.gameObject.SetActive(true);
                ui.Bind(slots[i]);
            }
            else ui.gameObject.SetActive(false);
        }
    }
}