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
            BGSound.Stop();
            BGSound.clip = null;            
            return;
        }
        else
        {
            int currentScene = SceneManager.GetActiveScene().buildIndex;
            int clipIndex = ((currentScene - 1) / 3) % BGlist.Length;
            BGSoundPlay(BGlist[clipIndex]);
        }
        
    }


    public void BGSoundPlay(AudioClip clip)
    {
        if (BGSound != null)
        {
            BGSound.clip = clip;
            BGSound.loop = true;
            BGSound.volume = 0.15f;
            BGSound.Play();
        }
        else
        {
            BGSound.Stop();
        }
    }
    public void BGSoundStop()
    {
        BGSound.Stop();
    }
}
