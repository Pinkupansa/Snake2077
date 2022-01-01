using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioReactiveObject : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private float updateStep = 0.1f;
    [SerializeField] private int sampleDataLength = 1024;

    private float currentUpdateTime = 0f;

    
    private float[] clipSampleData;

    [SerializeField] private float sizeFactor = 1;

    [SerializeField] private float minSize = 0;
    [SerializeField] private float maxSize = 500;

    [SerializeField] private bool scaleX, scaleY, scaleZ;

    [SerializeField] private Vector3 baseScale;
    private void Awake()
    {
        
        clipSampleData = new float[sampleDataLength];
    }
    private void Start()
    {
        if (audioSource == null)
            audioSource = Camera.main.GetComponent<AudioSource>();
    }
    private void Update()
    {
        currentUpdateTime += Time.deltaTime;
        if(currentUpdateTime >= updateStep)
        {
            currentUpdateTime = 0;
            audioSource.clip.GetData(clipSampleData, audioSource.timeSamples);
            float clipLoudness = 0;
            foreach (var sample in clipSampleData)
            {
                clipLoudness += Mathf.Abs(sample);
            }
            clipLoudness /= sampleDataLength;
            clipLoudness *= sizeFactor;
            clipLoudness = Mathf.Clamp(clipLoudness, minSize, maxSize);
            gameObject.transform.localScale = baseScale + new Vector3(scaleX?clipLoudness:0, scaleY?clipLoudness: 0, scaleZ ? clipLoudness :0);
        }
    }
}
