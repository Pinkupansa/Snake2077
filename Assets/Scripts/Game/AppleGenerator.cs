using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class AppleGenerator : MonoBehaviour
{

    public static AppleGenerator instance;
    [SerializeField] private GameObject applePrefab;
    [SerializeField] private GameObject appleDestructionParticles;
    [HideInInspector] public Vector2Int applePosition;
    private List<Vector2Int> allPositions = new List<Vector2Int>();
    private GameObject appleInstance;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }
    
    private void Start()
    {
        for(int x = 2; x < GameManager.SIZE - 2; x++)
        {
            for(int y = 2; y < GameManager.SIZE - 2; y++)
            {
                allPositions.Add(new Vector2Int(x, y));
            }
        }
        GenerateApple();
        GameManager.instance.appleEaten.AddListener(GenerateApple);
        
    }
    private void GenerateApple()
    {
        if(appleInstance != null)
        {
            GameObject particles = Instantiate(appleDestructionParticles, appleInstance.transform.position, Quaternion.identity);
            
            Destroy(appleInstance);
            Destroy(particles, particles.GetComponent<ParticleSystem>().main.duration);
        }
        List<Vector2Int> snakeBody = Snake.instance.Body;
        List<Vector2Int> possiblePositions = allPositions.Where(x => !snakeBody.Contains(x)).ToList();

        applePosition = possiblePositions[Random.Range(0, possiblePositions.Count - 1)];
        appleInstance = Instantiate(applePrefab, GameManager.instance.CoordinatesToWorldSpace(applePosition), Quaternion.identity);
        appleInstance.transform.localScale = Vector3.zero;
        LeanTween.scale(appleInstance, Vector3.one, 0.25f).setEase(LeanTweenType.easeInSine);
    }
}
