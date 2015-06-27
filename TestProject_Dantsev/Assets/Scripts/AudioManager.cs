using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour 
{
    [SerializeField]
    AudioClip clicksound = null;
    [SerializeField]
    AudioClip winning = null;
    [SerializeField]
    AudioSource sound_player = null;
    [SerializeField]
    AudioSource back_music_player = null;
    [SerializeField]
    AudioClip normal_music = null;
    [SerializeField]
    AudioClip bomb_music = null;
	
	void Start () 
    {
        DontDestroyOnLoad(gameObject);
	}
	
    public void PlayClick()
    {
        back_music_player.Pause();
        sound_player.PlayOneShot(clicksound);
        back_music_player.Play();
    }

    public void PlaySuccess()
    {
        back_music_player.Pause();
        sound_player.PlayOneShot(winning);
        back_music_player.Play();
    }

    public void PlayBomb()
    {
        back_music_player.Pause();
        sound_player.PlayOneShot(bomb_music);
        back_music_player.Play();
    }

}
