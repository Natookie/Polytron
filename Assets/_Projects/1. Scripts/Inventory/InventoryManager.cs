using System;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    [Serializable]
    public class Slot{
        [SerializeField] private ItemData item;
        [SerializeField] private int quantity;

        public ItemData Item => item;
        public int Quantity => quantity;
        public bool IsEmpty => item == null || quantity <= 0;
        public int RemainingSpace => IsEmpty ? 0 : item.MaxStackPerSlot - quantity;

        public Slot(ItemData item, int quantity){
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

    [Header("CONFIG")]
    [SerializeField][Min(1)] private int maxSlots = 200;

    [Header("DEBUG")]
    [SerializeField][Min(1)] private int debugAmount = 1;
    [SerializeField] private ItemData debugItemData;

    [Header("RUNTIME")]
    [ReadOnly] public List<Slot> slots = new();

    public IReadOnlyList<Slot> Slots => slots;
    public int MaxSlots => maxSlots;

    public event Action OnInventoryChanged;

    private static readonly Comparison<Slot> SellValueComparison = (a, b) => {
        int va = a.Item != null ? a.Item.SellValue : int.MinValue;
        int vb = b.Item != null ? b.Item.SellValue : int.MinValue;
        return vb.CompareTo(va);
    };

    void Awake(){
        if(Instance != null && Instance != this){
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public int AddItem(ItemData item, int amount = 1){
        if(item == null || amount <= 0) return 0;

        int remaining = amount;

        foreach(var slot in slots){
            if(remaining <= 0) break;
            if(slot.Item != item) continue;

            int space = item.MaxStackPerSlot - slot.Quantity;
            if(space <= 0) continue;

            int toAdd = Mathf.Min(space, remaining);
            slot.Add(toAdd);
            remaining -= toAdd;
        }

        while(remaining > 0 && slots.Count < maxSlots){
            int toAdd = Mathf.Min(item.MaxStackPerSlot, remaining);
            slots.Add(new Slot(item, toAdd));
            remaining -= toAdd;
        }

        int added = amount - remaining;
        if(added > 0){
            SortSlots();
            OnInventoryChanged?.Invoke();
        }
        CurrencyManager.Instance.UpdateInventoryCurrency(item.SellValue);
        return added;
    }

    public int RemoveItem(ItemData item, int amount = 1){
        if(item == null || amount <= 0) return 0;

        int remaining = amount;
        for(int i = slots.Count - 1; i >= 0 && remaining > 0; i--){
            var slot = slots[i];
            if(slot.Item != item) continue;

            remaining -= slot.Remove(remaining);
            if(slot.IsEmpty) slots.RemoveAt(i);
        }

        int removed = amount - remaining;
        if(removed > 0){
            SortSlots();
            OnInventoryChanged?.Invoke();
        }
        return removed;
    }

    public int CountOf(ItemData item){
        int total = 0;
        foreach(var slot in slots)
            if(slot.Item == item) total += slot.Quantity;
        return total;
    }

    public bool HasItem(ItemData item, int amount = 1) => CountOf(item) >= amount;
    public bool IsFull => slots.Count >= maxSlots;

    public int SellItem(ItemData item, int amount = 1){
        if(item == null || amount <= 0) return 0;
        int removed = RemoveItem(item, amount);
        int gold = removed * item.SellValue;
        if(gold > 0) Debug.Log($"Sold {removed}x {item.Rarity} for {gold} gold.");
        return gold;
    }

    void SortSlots() => slots.Sort(SellValueComparison);

    [Button("Debug Add Item")]
    public void DebugAdd() => AddItem(debugItemData, debugAmount);
    [Button("Debug Remove Item")]
    public void DebugRemove() => RemoveItem(debugItemData, debugAmount);
    [Button("Debug Clear Inventory")]
    public void DebugClear(){
        slots.Clear();
        OnInventoryChanged?.Invoke();
    }
}