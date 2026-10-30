using Nova;
using UnityEngine;
using UnityEngine.InputSystem;

public class GlobalClickRouter : MonoBehaviour
{
    [SerializeField] private UIBlock rootBlock;

    void Update(){
        if(Mouse.current.leftButton.wasPressedThisFrame)
            TextFieldManager.OnGlobalPress(rootBlock);
    }
}