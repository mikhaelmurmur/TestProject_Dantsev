using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StartGame : MonoBehaviour {

    [SerializeField]
    Button start_button = null;
    [SerializeField]
    Button exit_button = null;
	// Use this for initialization
	void Start ()
    {
        start_button.onClick.AddListener(() => { StartLevel(); });
        exit_button.onClick.AddListener(() => { ExitGame(); });
        Screen.SetResolution(640, 400, false);
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
