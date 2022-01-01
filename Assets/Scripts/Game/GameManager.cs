using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject test;
    public const int SIZE = 20; //taille de la grille
    [SerializeField] private Transform topLeft, bottomRight;
    private float maxX, maxY, minX, minY;
    
    [HideInInspector] public int playerScore = 0;

    
    private void Awake()
    {
        maxY = topLeft.position.z;
        maxX = bottomRight.position.x;
        minX = topLeft.position.x;
        minY = bottomRight.position.z;
        if(instance == null)
        {
            instance = this;
        }
    }
    public Vector3 CoordinatesToWorldSpace(Vector2Int coordinates)
    {
        float x = (maxX - minX)/SIZE * (coordinates.x + 0.5f) + minX;
        float y = (maxY - minY)/SIZE * (coordinates.y + 0.5f) + minY;

        return new Vector3(x, 0.01f, y);
    }
    public UnityEvent playerDied = new UnityEvent();
    public void PlayerDied()
    {
        playerDied.Invoke();
        
    }
    public UnityEvent appleEaten = new UnityEvent();
    public void AppleEaten()
    {
        playerScore += 1;
        if(playerScore % 20 == 0)
        {
            Camera.main.GetComponent<AudioSource>().pitch += 0.01f;
        }
       
        appleEaten.Invoke();
    }
    public void RecordScore(string name)
    {
        SaveData.current.gamesPlayed += 1;
        SaveData.current.scores.Add(new ScoreRecord(playerScore, name));
        
        SaveData.Save();
    }

}
