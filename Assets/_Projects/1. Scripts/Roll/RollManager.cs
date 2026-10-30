using System;
using System.Collections.Generic;
using UnityEngine;
using Nova;

public class RollManager : MonoBehaviour
{
    [Serializable]
    public class Pair{
        [SerializeField] private TextBlock percentageInput;
        [SerializeField] private TextBlock placeholderText;
        [SerializeField] private ItemData itemData;
        [SerializeField][Range(0, 100)] private int defaultWeight;

        public TextBlock PercentageInput => percentageInput;
        public ItemData ItemData => itemData;
        public int DefaultWeight => defaultWeight;

        public int GetWeight(){
            if(defaultWeight > 0) return defaultWeight;
            if(percentageInput == null) return 0;
            if(!int.TryParse(percentageInput.Text, out int raw)) return 0;
            return Mathf.Clamp(raw, 0, 100);
        }

        public void InitPlaceholder(){
            if(placeholderText == null) return;
            placeholderText.Text = $"Default: {defaultWeight}";
        }
    }

    [SerializeField] private Pair[] pairs;
    [SerializeField] private CurrencyManager currencyManager;
    [SerializeField] private int gachaPrice = 30;
    public int GachaPrice => gachaPrice;

    void Start(){
        if(currencyManager == null) currencyManager = CurrencyManager.Instance;
        foreach(var pair in pairs) pair?.InitPlaceholder();
    }

    public void Roll(){
        if(currencyManager == null || pairs == null || pairs.Length == 0) return;
        if(currencyManager.playerCurrency < gachaPrice){
            Debug.Log($"Not Enough Coin");
            return;
        }

        var picked = PickWeighted();
        if(picked == null){
            Debug.Log("picked null");
            return;
        }

        currencyManager.UpdatePlayerCurrency(-gachaPrice);
        Debug.Log($"Rolled: {picked.name}");

        InventoryManager.Instance.AddItem(picked);
    }

    ItemData PickWeighted(){
        // 1) Sum Weight + Build Cummulative
        var weights = new List<int>(pairs.Length);
        var thresholds = new List<int>(pairs.Length);
        int sum = 0;

        foreach(var pair in pairs){
            if(pair == null || pair.ItemData == null) continue;

            int w = pair.GetWeight();
            weights.Add(w);
            sum += w;
            thresholds.Add(sum);
        }

        if(weights.Count == 0) return null;

        //2) Kalo weight semua 0, pick qeual
        if(sum <= 0) return PickEqual(weights);

        //3) Roll 1..sum
        int roll = UnityEngine.Random.Range(1, sum + 1);

        //4) Cari threshold >= roll pertama
        int index = 0;
        for(int i = 0; i < thresholds.Count; i++){
            if(roll <= thresholds[i]){
                index = i;
                break;
            }
        }

        //5) Cari matching itemdata
        return GetItemAtValidIndex(index);
    }

    ItemData GetItemAtValidIndex(int validIndex){
        int seen = 0;
        foreach(var pair in pairs){
            if(pair == null || pair.ItemData == null) continue;

            if(seen == validIndex) return pair.ItemData;
            seen++;
        }
        return null;
    }

    ItemData PickEqual(List<int> weights){
        var valid = new List<ItemData>();
        foreach (var pair in pairs){
            if(pair == null || pair.ItemData == null) continue;
            valid.Add(pair.ItemData);
        }
        if(valid.Count == 0) return null;
        return valid[UnityEngine.Random.Range(0, valid.Count)];
    }
}