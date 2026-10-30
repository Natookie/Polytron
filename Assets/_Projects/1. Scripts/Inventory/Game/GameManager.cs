using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("CONFIG")]
    [SerializeField] private bool autoQuitAfterLose = false;

    [Header("REFERENCES")]
    [SerializeField] private CurrencyManager currencyManager;
    [SerializeField] private RollManager rollManager;

    void Awake(){
        if(Instance == null) Instance = this;
        else{
            Destroy(gameObject);
            return;
        }
    }

    void Start(){
        if(currencyManager == null) currencyManager = CurrencyManager.Instance;
        if(InventoryManager.Instance != null) InventoryManager.Instance.OnInventoryChanged += CheckWinCondition;
    }

    void CheckWinCondition(){
        int totalCurrency = currencyManager.playerCurrency + currencyManager.inventoryCurrency;
        if(totalCurrency < rollManager.GachaPrice){
            Debug.Log("Game Over");
            if(autoQuitAfterLose) QuitGame();
        }
    }

    public void QuitGame(){
        Application.Quit();

        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}