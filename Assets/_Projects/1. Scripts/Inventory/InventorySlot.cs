using System;
using UnityEngine;

[Serializable]
public class InventorySlot
{
    [SerializeField] private ItemData item;
    [SerializeField] private int quantity;

    public ItemData Item => item;
    public int Quantity => quantity;
    public bool IsEmpty => item == null || quantity <= 0;

    public InventorySlot(ItemData item, int quantity){
        this.item = item;
        this.quantity = quantity;
    }

    public void Add(int amount) => quantity += amount;

    public int Remove(int amount){
        int removed = Mathf.Min(amount, quantity);
        quantity -= removed;
        if(quantity <= 0) Clear();
        return removed;
    }

    public void Clear(){
        item = null;
        quantity = 0;
    }
}