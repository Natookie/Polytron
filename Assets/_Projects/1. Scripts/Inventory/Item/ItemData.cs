using UnityEngine;

[CreateAssetMenu(fileName = "NewItemData", menuName = "Inventory/ItemData", order = 1)]
public class ItemData : ScriptableObject
{
    [Header("IDENTITY")]
    [SerializeField] private Sprite icon;
    [SerializeField] private Sprite borderColor;

    [Header("VALUE")]
    [SerializeField] private string rarity;
    [SerializeField] private int sellValue;

    [Header("CONFIG")]
    [SerializeField] private int maxStackPerSlot = 1;

    public Sprite Icon => icon;
    public string Rarity => rarity;
    public int SellValue => sellValue;
    public int MaxStackPerSlot => maxStackPerSlot;
    public Sprite BorderColor => borderColor;
}