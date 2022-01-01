using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirtualArrowKeys : MonoBehaviour, IInputSystem
{
    private static VirtualArrowKeys _current;
    public static VirtualArrowKeys current
    {
        get
        {
            if(_current != null && !_current.gameObject.activeSelf)
            {
                _current.gameObject.SetActive(true);
            }
            return _current;
        }
        set
        {
            _current = value;
        }
    }
    private Direction lastInput;
    private bool inputDetected;

    private void Start()
    {
        if(current == null)
        {
            current = this;
            gameObject.SetActive(false);
        }
    }

    public void OnArrowPressed(string direction)
    {
        if(direction == "UP")
        {
            lastInput = Direction.UP;
        }
        else if(direction == "DOWN")
        {
            lastInput = Direction.DOWN;
        }
        else if(direction == "LEFT")
        {
            lastInput = Direction.LEFT;
        }
        else
        {
            lastInput = Direction.RIGHT;
        }
        inputDetected = true;
        
    }
    public bool InputDetected()
    {
        return inputDetected;
    }
    public Direction GetInput()
    {
        inputDetected = false;
        return lastInput;
    }
}
