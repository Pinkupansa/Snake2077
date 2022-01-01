using UnityEngine;

public class VirtualJoystick : MonoBehaviour, IInputSystem
{
  
    private static VirtualJoystick _current;
    public static VirtualJoystick current
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
    
    private Vector2 input;
    
    private bool active;
    
    private float clampRadius = 60;
    private float hitRadius = 200;
    [SerializeField] private GameObject joystickImage;

    Direction lastInput; //The last direction recorded by the player;
    

    private void Awake()
    {
        
        if(current == null)
        {
            current = this;
            gameObject.SetActive(false);
        }
    }
    private void Start()
    {
        active = false;
    }
    private void Update()
    {
        if(active)
        {
            
            if((SystemInfo.deviceType == DeviceType.Handheld && Input.touchCount == 0 )|| Input.GetButtonUp("Fire1"))
            {
                OnRelease();
            }
            else
            {
                input = Vector3.zero;
                if(SystemInfo.deviceType == DeviceType.Handheld)
                {
                    input = Input.touches[0].position - (Vector2)transform.position;
                }
                else
                {
                    input = (Vector2)Input.mousePosition - (Vector2)transform.position;
                }
                if(input.magnitude > clampRadius)
                {
                    input /= input.magnitude;
                    input *= clampRadius;
                }
            }
            
        }
        else
        {
            input = Vector2.zero;
            if(SystemInfo.deviceType == DeviceType.Handheld)
            {
                if(Input.touchCount > 0  && (Input.touches[0].position - (Vector2)transform.position).magnitude < hitRadius)
                {
                    OnClicked();
                }
            }
            else
            {
                if(Input.GetButtonDown("Fire1") && ((Vector2)Input.mousePosition - (Vector2)transform.position).magnitude < hitRadius)
                {
                    OnClicked();
                }
            }
        }
        
        joystickImage.transform.position = (Vector2)transform.position + input;
    }
    private void OnRelease()
    {
        active = false;
        input = Vector3.zero;
    }
    public void OnClicked()
    {
        active = true;
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
        return input.magnitude > clampRadius/2 && CalculateDirection() != lastInput; //If the last input retrieved by the player is the same, don't return an input.
    }

}
