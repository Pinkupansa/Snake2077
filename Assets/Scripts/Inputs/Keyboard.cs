using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keyboard : IInputSystem
{
    public bool InputDetected()
    {
        return Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow); 
    }
    public Direction GetInput()
    {
        if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            return Direction.UP;
        }
        else if(Input.GetKeyDown(KeyCode.DownArrow))
        {
            return Direction.DOWN;
        }
        else if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            return Direction.LEFT;
        }
        else
        {
            return Direction.RIGHT;
        }
    }
}
