using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestInput : MonoBehaviour
{


    public void Click(InputAction.CallbackContext context)
    {
        print("Clicked!");
    }

}
