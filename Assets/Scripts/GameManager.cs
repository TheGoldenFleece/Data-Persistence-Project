using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class GameManager : MonoBehaviour
{
    public TMP_InputField nameInput;

    public TextMeshProUGUI BestScoreText;
    public static GameManager Instanse;
    public string player;
    public string name;
    public int bestScore;
    private void Awake()
    {
        if (Instanse != null)
        {
            Destroy(gameObject);
        }

        Instanse = this;
        DontDestroyOnLoad(gameObject);

        LoadScoreData();
        DisplayBestScore(BestScoreText);

    }

    private void Start()
    {
        nameInput.text = player;
    }
    public void InputName()
    {
        player = nameInput.text;
    }

    public void DisplayBestScore(TextMeshProUGUI bestScoreText)
    {
        if (Instanse.name == null)
        {
            bestScoreText.text = "Best Score : " + Instanse.bestScore;
        }
        else
        {
            bestScoreText.text = "Best Score : " + Instanse.name + " : " + Instanse.bestScore;
        }
    }
    public void StartGame()
    {
        SceneManager.LoadScene("main");
    }
    public void QuitGame()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }

    public void SaveScoreData()
    {
        string path = Application.persistentDataPath + "/score.json";
        File.WriteAllText(path, BestPlayer.TransformToJson(name, player, bestScore));
    }
    public void LoadScoreData()
    {
        string path = Application.persistentDataPath + "/score.json";

        if (File.Exists(path))
        {
            string json =  File.ReadAllText(path);
            BestPlayer player = BestPlayer.CreateFromJson(json);

            this.name = player.Name;
            bestScore = player.Score;
            this.player = player.PreviousPlayer;
        }
    }

    [System.Serializable]
    public class BestPlayer
    {
        public string Name;
        public string PreviousPlayer;
        public int Score;

        public BestPlayer(string name, string previousPlayer, int score)
        {
            Name = name;
            PreviousPlayer = previousPlayer;
            Score = score;
        }

        public static BestPlayer CreateFromJson(string jsonString)
        {
            return JsonUtility.FromJson<BestPlayer>(jsonString);
        }
        public static string TransformToJson(string name, string previousPlayer, int score)
        {
            string json = JsonUtility.ToJson(new BestPlayer(name, previousPlayer, score));
            return json;
        }
    }
}
