using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;
public class UIManager : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_Text score;
    [SerializeField] private TMPro.TMP_Text hiScore;
    [SerializeField] private GameObject recordMenu;
    [SerializeField] private GameObject retryMenu;
    [SerializeField] private TMPro.TMP_InputField nameInputField;
    
    private void Start()
    {
        UpdateHiScore();
        recordMenu.SetActive(false);
        retryMenu.SetActive(false);
        OnAppleEaten();
        GameManager.instance.appleEaten.AddListener(OnAppleEaten);
        GameManager.instance.playerDied.AddListener(OnPlayerDead);
    }
    private void OnAppleEaten()
    {
        score.text = "SCORE : " + GameManager.instance.playerScore;
    }
    private void OnPlayerDead()
    {
        ShowMenu(recordMenu);
    }
    private void ShowMenu(GameObject menu)
    {
        retryMenu.SetActive(false);
        recordMenu.SetActive(false);
        menu.SetActive(true);
        menu.transform.localScale = Vector3.zero;
        LeanTween.scale(menu, Vector3.one, 0.5f).setEase(LeanTweenType.easeOutSine);
        LeanTween.value(-1000, 0, 1f).setEase(LeanTweenType.easeOutSine).setOnUpdate(x => menu.GetComponent<RectTransform>().anchoredPosition = x * Vector2.up);
        if(menu == recordMenu)
        {
            if (SaveData.current.gamesPlayed > 0)
            {
                nameInputField.SetTextWithoutNotify(SaveData.current.scores[SaveData.current.scores.Count - 1].playerName);
                nameInputField.ActivateInputField();
                
            }
            //nameInputField.ActivateInputField();
            
             
        }
    }
    public void Reload()
    {
        SceneManager.LoadScene(1);
    }
    public void GoBackToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void RecordScore()
    {
        GameManager.instance.RecordScore(nameInputField.textComponent.text);
        ShowMenu(retryMenu);
    }
    private void UpdateHiScore()
    {
        if (SaveData.current.scores.Count > 0)
        {
            hiScore.text = "hi-score : " + SaveData.current.scores.Select(x => x.score).Max().ToString();
        }
        else
        {
            hiScore.text = "hi-score : NONE";
        }
    }
}
