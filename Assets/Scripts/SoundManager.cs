using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public AudioSource BGSound;
    public AudioClip[] BGlist;

    public static SoundManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MainScene")
        {
            if (BGSound != null && BGSound.isPlaying) 
            { BGSound.Stop(); }
            //BGSound.clip = null;
            //BGSound.mute = true;
                        
            //return;
        }
        else
        {
            int currentScene = SceneManager.GetActiveScene().buildIndex -2;
            int clipIndex = ((currentScene - 1) / 3) % BGlist.Length;
            BGSoundPlay(BGlist[clipIndex]);
        }
        
    }


    public void BGSoundPlay(AudioClip clip)
    {
        if (BGSound != null)
        {
            BGSound.mute = false;
            BGSound.clip = clip;
            BGSound.loop = true;
            BGSound.volume = 0.15f;
            BGSound.Play();
        }
       
    }
    public void BGSoundStop()
    {
        BGSound.Stop();
    }
}
