using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using UnityEngine.UI;
public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject mainMenu, settingsMenu;
    [SerializeField] private GameObject hiScores;

    [SerializeField] private GameObject graphicsSlider;
    [SerializeField] private GameObject inputModeDropdown;
    [SerializeField] private TMPro.TMP_Text graphicsText;
    
    private void Start()
    {
        ShowMainMenu();
        DisplayHiScores();
        SetPreferences();
    }

    // Update is called once per frame
    public void Play()
    {
        SceneManager.UnloadScene(0);
        SceneManager.LoadScene(1);
        
    }
    public void Quit()
    {
        Application.Quit();
    }
    public void ShowMainMenu()
    {
        ShowMenu(mainMenu);
    }
    public void ShowSettingsMenu()
    {

        ShowMenu(settingsMenu);
        graphicsSlider.GetComponent<Slider>().value = SaveData.current.preferences.graphicsQuality;
        graphicsText.text = QualitySettings.names[SaveData.current.preferences.graphicsQuality];

    }
    public void UpdateGraphics()
    {
        int value = (int)graphicsSlider.GetComponent<Slider>().value;
        QualitySettings.SetQualityLevel(value, true);
        graphicsText.text = QualitySettings.names[value];
        SaveData.current.preferences.graphicsQuality = value;
        SaveData.Save();
    }
    public void UpdateInputMode()
    {
        int value = inputModeDropdown.GetComponent<TMPro.TMP_Dropdown>().value;
        SaveData.current.preferences.preferredInputMode = (InputMode)value;
        SaveData.Save();
    }
    
    private void ShowMenu(GameObject menu)
    {
        mainMenu.SetActive(false);
        settingsMenu.SetActive(false);
        menu.SetActive(true);
        menu.transform.localScale = new Vector3(1, 0, 1);
        LeanTween.scale(menu, Vector3.one, 0.5f).setEase(LeanTweenType.easeOutSine);
    }
    private void DisplayHiScores()
    {
        string hiscores = "";
        
        if(SaveData.current.gamesPlayed > 0)
        {
            List<ScoreRecord> scores = SaveData.current.scores.OrderByDescending(x => x.score).ToList();
            for (int i = 0; i < Mathf.Min(5, scores.Count); i++)
            {
                hiscores += string.Format("{0}. {1} : {2}\n", new object[3] { i + 1, scores[i].playerName, scores[i].score });
            }
        }
        else
        {
            hiscores = "u no played";
        }
        hiScores.GetComponent<TMPro.TMP_Text>().text = hiscores;
    }

    //Updates the game with the preferences saved in the SaveData
    private void SetPreferences()
    {
        QualitySettings.SetQualityLevel(SaveData.current.preferences.graphicsQuality);
        inputModeDropdown.GetComponent<TMPro.TMP_Dropdown>().value = (int)SaveData.current.preferences.preferredInputMode;
    }

}
