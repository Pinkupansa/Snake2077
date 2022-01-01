using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
public class AnimationManager : MonoBehaviour
{
    [SerializeField] private Volume postProcessing;
    [SerializeField] private float screenShakeAmplitude;
    [SerializeField] private AnimationCurve screenShakeCurve;
    [SerializeField] private GameObject scoreText;
    [SerializeField] private float scoreTextBlinkAmplitude;

    [SerializeField] private GameObject background;
    // Start is called before the first frame update
    private void Start()
    {
        GameManager.instance.appleEaten.AddListener(OnAppleEaten);
    }

    // Update is called once per frame
    private void OnAppleEaten()
    {
        
        ScreenShake();
        Shockwave();
    }
    private void ScreenShake()
    {
        Vector3 camPos = Camera.main.transform.position;
        
        if(GameManager.instance.playerScore % 20 == 0)
        {
            
            LeanTween.value(-2*screenShakeAmplitude, 2*screenShakeAmplitude, 0.5f).setEase(screenShakeCurve).setOnUpdate(ScreenShakeUpdate).setOnComplete(() => Camera.main.transform.position = camPos);

            LeanTween.scale(scoreText, Vector3.one * 2*scoreTextBlinkAmplitude, 0.25f).setEase(screenShakeCurve).setOnComplete(() => scoreText.transform.localScale = Vector3.one);
        }
        else if(GameManager.instance.playerScore % 5 == 0)
        {
            LeanTween.value(-2f*screenShakeAmplitude, 2f*screenShakeAmplitude, 0.25f).setEase(screenShakeCurve).setOnUpdate(ScreenShakeUpdate).setOnComplete(() => Camera.main.transform.position = camPos);

            LeanTween.scale(scoreText, Vector3.one * 2*scoreTextBlinkAmplitude, 0.125f).setEase(screenShakeCurve).setOnComplete(() => scoreText.transform.localScale = Vector3.one);
        }
        else
        {
            LeanTween.value(-screenShakeAmplitude, screenShakeAmplitude, 0.25f).setEase(screenShakeCurve).setOnUpdate(ScreenShakeUpdate).setOnComplete(() => Camera.main.transform.position = camPos);

            LeanTween.scale(scoreText, Vector3.one * scoreTextBlinkAmplitude, 0.125f).setEase(screenShakeCurve).setOnComplete(() => scoreText.transform.localScale = Vector3.one);
        }
    }
    private void ScreenShakeUpdate(float value)
    {
        Camera.main.transform.position = new Vector3(value + Camera.main.transform.position.x, Camera.main.transform.position.y, Camera.main.transform.position.z);
        
    }
    private void Shockwave()
    {
       Material backMat = background.GetComponent<MeshRenderer>().material;
       //Converting snake grid pos to uv coordinate
       Vector3 snakeOffset = GameManager.instance.CoordinatesToWorldSpace(Snake.instance.Head) - background.transform.position;
       Vector2 snakeUV = 0.5f*Vector2.one + new Vector2(snakeOffset.x/background.transform.localScale.x, snakeOffset.z/background.transform.localScale.z);
       backMat.SetVector("_FocalPoint", snakeUV);
       LeanTween.value(background, 0, 1, 1f).setOnUpdate(x => SetFloat(backMat, x, "_Phase"));
       
       
       
    }
    private void SetFloat(Material mat, float value, string property)
    {
        mat.SetFloat(property, value);
    }
}
