using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(LineRenderer))]
public class Snake : MonoBehaviour
{
    
    public static Snake instance;
    private bool isDead = false;
    [SerializeField] private float speed = 0.5f;
    private float moveTimer = 0; //Timer qui attend pour faire bouger le serpent
  
    private Direction lastDirection = Direction.UP;//Dernière direction prise par le serpent
    private List<Vector2Int> body = new List<Vector2Int>(); //Liste des coordonnées des parties du corps du serpent 
    [SerializeField] private GameObject crawlingParticles;
    [SerializeField] private LineRenderer graphics; //LineRenderer correspondant au corps du serpent
    [SerializeField] private float speedBoost; //Temporary speed boost on apple
    [SerializeField] private float speedBoostTime;
    [SerializeField] private float speedGainPerApple;

    [SerializeField] private GameObject floor;
    
    private int inputsSize = 3;
    private Queue<Direction> inputs = new Queue<Direction>(); //three last unused inputs

    private IInputSystem inputSystem;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }
    private void Start()
    {
        
        SetInputSystem();
        body.Add(new Vector2Int(5,5));
        body.Add(new Vector2Int(5,4));
        
        graphics = GetComponent<LineRenderer>();
        graphics.positionCount = body.Count;
        for (int i = 0; i < graphics.positionCount; i++)
        {
            graphics.SetPosition(i, GameManager.instance.CoordinatesToWorldSpace(body[i]));
        }
    }
    private void Update()
    {
        if(!isDead)
        {
            GetInputs();
            CheckDeath();
        }
        Render();
        
    }
    private void FixedUpdate()
    {
  
        if(!isDead)
        {

            moveTimer += Time.fixedDeltaTime;
            if(moveTimer >= 1f/speed)
            {
                Move();
                moveTimer = 0;
                
            }
            
        }
        

    }
    private void GetInputs()
    {
        if(inputs.Count < inputsSize && inputSystem.InputDetected())
        {
            inputs.Enqueue(inputSystem.GetInput());
        }
        
    }
    public void Move()
    {
        Direction direction = Opposite(lastDirection);
        while(direction == Opposite(lastDirection) && inputs.Count > 0)
        {
            direction = inputs.Dequeue();
        }
        if(direction == Opposite(lastDirection))
        {
            direction = lastDirection;
        }
        if (body[0] == AppleGenerator.instance.applePosition)
        {
            EatApple();
        }
        for(int i = body.Count - 1; i >= 1; i--)
        {
            body[i] = body[i-1];
        }
        switch(direction)
        {
            case Direction.UP:
                body[0] += Vector2Int.up;
                break;
            case Direction.DOWN:
                body[0] += Vector2Int.down;
                break;
            case Direction.LEFT:
                body[0] += Vector2Int.left;
                break;
            case Direction.RIGHT:
                body[0] += Vector2Int.right;
                break;
        }
        lastDirection = direction;
        
    }

    private void CheckDeath()
    {
        if(body[0].x >= GameManager.SIZE || body[0].y >= GameManager.SIZE || body[0].x < 0 || body[0].y < 0|| body.Skip(1).Contains(body[0]))
        {
            Die();
        }
    }
    private void Render()
    {
        graphics.positionCount = body.Count;
        
        for(int i = 0; i < graphics.positionCount; i++)
        {
            graphics.SetPosition(i, Vector3.Lerp(graphics.GetPosition(i), GameManager.instance.CoordinatesToWorldSpace(body[i]), speed*Time.deltaTime));
        }
        //crawlingParticles.transform.position = graphics.GetPosition(0);
        
    }

    private void Die()
    {
        isDead = true;
        GameManager.instance.PlayerDied();
    }
    private void EatApple()
    {
        GameManager.instance.AppleEaten();
        
        body.Add(body[body.Count - 1]);
        graphics.positionCount = body.Count;
        graphics.SetPosition(graphics.positionCount - 1, GameManager.instance.CoordinatesToWorldSpace(body[body.Count - 1]));
        StartCoroutine(SpeedBoost());
    }
    private IEnumerator SpeedBoost()
    {
        speed += speedBoost;
        yield return new WaitForSeconds(speedBoostTime);
        speed += speedGainPerApple - speedBoost;
        
    }

    public List<Vector2Int> Body
    {
        get
        {
            return body;
        }
    }
    private Direction Opposite(Direction direction)
    {
        switch(direction)
        {
            case Direction.RIGHT:
                return Direction.LEFT;
            case Direction.LEFT:
                return Direction.RIGHT;
            case Direction.UP:
                return Direction.DOWN;
            case Direction.DOWN:
                return Direction.UP;
        }
        return Direction.UP;
    }

    public Vector2Int Head
    {
        get
        {
            return body[0];
        }
    }
    private void SetInputSystem()
    {
        InputMode inputMode = SaveData.current.preferences.preferredInputMode;
       
        switch(inputMode)
        {
            case InputMode.DEFAULT:
                if(SystemInfo.deviceType == DeviceType.Handheld)
                {
                    goto case InputMode.FOREST_TOUCH;
                }
                else
                {
                    goto case InputMode.KEYBOARD;
                }
            case InputMode.KEYBOARD:
                inputSystem = new Keyboard();
                break;
            case InputMode.FOREST_TOUCH:
                inputSystem = ForestTouch.current;
                break;
            case InputMode.VIRTUAL_ARROW_KEYS:
                inputSystem = VirtualArrowKeys.current;
                break;
            case InputMode.VIRTUAL_JOYSTICK:
                inputSystem = VirtualJoystick.current;
                break;
        }
    }
}
