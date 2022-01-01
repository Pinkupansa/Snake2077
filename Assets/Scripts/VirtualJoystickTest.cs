using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirtualJoystickTest : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_Text text;

    void Update()
    {
        if(VirtualJoystick.current.InputDetected())
        {
            text.text= VirtualJoystick.current.GetInput().ToString();
        }
        
    }
}
