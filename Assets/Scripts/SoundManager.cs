using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public AudioSource BGSound;
    public AudioClip[] BGlist;

    public static SoundManager instance;

    private void Awake()
    {
        SetVolume(0.14f);
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
      

        // 메인화면 씬이면 전용 배경음악을 재생
        if (scene.name == "MainScene")
        {
            if ( BGSound != null && BGSound.clip != BGlist[3])
            {
                BGSoundPlay(BGlist[3]);// BGlist[3] 메인화면 전용 음악
                SetVolume(0.14f);
            }
        }
        else if (scene.name == "Tutorial")
        {
            if (BGSound != null && BGSound.isPlaying)
            { BGSound.Stop(); }
        }
        else
        {
            //스테이지 씬에서는 메인 화면의 음악을 정지하고 씬 번호에 따라 배경음악 클립 인덱스를 계산하고 재생
            if (BGSound.isPlaying)
            {
                BGSound.Stop();
            }

            //씬 번호에 따라 배경음악 클립 인덱스를 계산하고 재생
            //BGlist[0~2] 0:초원,1:도시,2:해변
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
            //BGSound.volume = 0.15f;
            SetVolume(0.15f);
            BGSound.Play();
        }
       
    }

    public void BGSoundStop()
    {
        BGSound.Stop();
    }

    //볼륨 조절
    public void SetVolume(float volume)
    {
        BGSound.volume = volume;
    }
}
