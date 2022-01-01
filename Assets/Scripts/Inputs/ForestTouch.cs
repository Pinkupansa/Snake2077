using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForestTouch : MonoBehaviour, IInputSystem
{
    public static ForestTouch current;
    [SerializeField] private float sensitivity = 2f;
    private Vector2 input = Vector2.zero;
    private Direction lastInput;
    private void Awake()
    {
        if(current == null)
        {
            current = this;
        }
    }
    private void FixedUpdate()
    {
        if(SystemInfo.deviceType == DeviceType.Handheld)
        {
            if(Input.touchCount > 0)
            {
                input = Input.touches[0].deltaPosition;
            }
        }
        else
        {
            input = Input.GetAxis("Mouse X")* Vector2.right + Input.GetAxis("Mouse Y")*Vector2.up;
           
        }
    }
    private Direction CalculateDirection()
    {
        if(Mathf.Abs(input.x) > Mathf.Abs(input.y))
        {
            return input.x < 0? Direction.LEFT : Direction.RIGHT;
        }
        else
        {
            return input.y < 0? Direction.DOWN : Direction.UP;
        }
    }
    public Direction GetInput()
    {
        
        Direction dir = CalculateDirection();
        lastInput = dir; //Record the last input retrieved by the player.
        return dir;
        
    }
    public bool InputDetected()
    {
        return input.magnitude > 10f/sensitivity && CalculateDirection() != lastInput; //If the last input retrieved by the player is the same, don't return an input.
    }
}
