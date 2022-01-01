using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioBars : MonoBehaviour
{
    [SerializeField] private GameObject audioBarPrefab;
    [SerializeField] private Transform leftTopLeft, leftBottomRight, rightTopLeft, rightBottomRight; //rectangle areas for spawning audio bars

    void Start()
    {
        GameManager.instance.appleEaten.AddListener(OnAppleEaten);
    }

    private void OnAppleEaten()
    {
        if(GameManager.instance.playerScore % 5 == 0)
        {
            int leftOrRight = Random.Range(0, 2);
            float minX = 0;
            float maxX = 0;
            float minY = 0;
            float maxY = 0;
            if(leftOrRight == 0)
            {
                minX = leftTopLeft.localPosition.x;
                maxX = leftBottomRight.localPosition.x;
                minY = leftBottomRight.localPosition.z;
                maxY = leftTopLeft.localPosition.z;
            }
            else
            {
                minX = rightTopLeft.localPosition.x;
                maxX = rightBottomRight.localPosition.x;
                minY = rightBottomRight.localPosition.z;
                maxY = rightTopLeft.localPosition.z;
            }
            GameObject audioBarInstance = Instantiate(audioBarPrefab, transform);
            audioBarInstance.transform.localPosition = new Vector3(Random.Range(minX, maxX), 0.5f, Random.Range(minY, maxY));
        }
    }
}
