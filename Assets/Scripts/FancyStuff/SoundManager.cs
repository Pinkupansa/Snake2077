using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] private AudioClip death, apple;
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        GameManager.instance.playerDied.AddListener(OnPlayerDied);
        GameManager.instance.appleEaten.AddListener(OnAppleEaten);
    }

    private void OnPlayerDied()
    {
        PlaySound(death);
    }
    private void OnAppleEaten()
    {
        PlaySound(apple);
    }
    public void PlaySound(AudioClip sound)
    {
        audioSource.PlayOneShot(sound);
    }
}
