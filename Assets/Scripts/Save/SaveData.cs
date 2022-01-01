using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    private const string DEFAULT_SAVE = "defaultsave";
    private static SaveData _current;
    public static SaveData current
    {
        get
        {
            if(_current == null)
            {
                
                Load();
                if(_current == null)
                {
                    return new SaveData();
                }
                else
                {
                    if(_current.preferences == null)
                    {
                        _current.preferences = new Preferences();
                    }
                     
                    return _current;
                }
                
            }
            else
            {
                return _current;
            }
            
        }
    }
    public static bool Save()
    {
        return SerializationManager.Save(DEFAULT_SAVE, current);
    }
    public static void Load(string path = DEFAULT_SAVE)
    {
        _current = (SaveData)SerializationManager.Load(DEFAULT_SAVE);
    }
    
    public int gamesPlayed;
    public List<ScoreRecord> scores = new List<ScoreRecord>();

    public Preferences preferences = new Preferences();
    

}
[System.Serializable]
public struct ScoreRecord
{
    public int score { get; private set; }
    public string playerName { get; private set; }
    public ScoreRecord(int _score, string _playerName)
    {
        score = _score;
        playerName = _playerName;
    }
}
[System.Serializable]
public class Preferences
{
    public InputMode preferredInputMode = InputMode.DEFAULT;
    public int graphicsQuality = QualitySettings.GetQualityLevel(); 
}