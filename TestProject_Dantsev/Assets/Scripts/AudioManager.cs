using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour 
{
    [SerializeField]
    AudioClip clickSound = null;
    [SerializeField]
    AudioClip winning = null;
    [SerializeField]
    AudioSource soundPlayer = null;
    [SerializeField]
    AudioSource backMusicPlayer = null;
    [SerializeField]
    AudioClip normalMusic = null;
    [SerializeField]
    AudioClip bombMusic = null;
	
	void Start () 
    {
        DontDestroyOnLoad(gameObject);
	}
	
    public void PlayClick()
    {
        backMusicPlayer.Pause();
        soundPlayer.PlayOneShot(clickSound);
        backMusicPlayer.Play();
    }

    public void PlaySuccess()
    {
        backMusicPlayer.Pause();
        soundPlayer.PlayOneShot(winning);
        backMusicPlayer.Play();
    }

    public void PlayBomb()
    {
        backMusicPlayer.Pause();
        soundPlayer.PlayOneShot(bombMusic);
        backMusicPlayer.Play();
    }

}
