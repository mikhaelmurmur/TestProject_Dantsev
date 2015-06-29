using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StartGame : MonoBehaviour {

    [SerializeField]
    Button startButton = null;
    [SerializeField]
    Button exitButton = null;

	void Start ()
    {
        startButton.onClick.AddListener(() => { StartLevel(); });
        exitButton.onClick.AddListener(() => { ExitGame(); });
        Screen.SetResolution(800, 400, false);
	}
	
    void ExitGame()
    {
        Application.Quit();
    }

    void StartLevel()
    {
        Application.LoadLevel("LevelScene");
    }
}
