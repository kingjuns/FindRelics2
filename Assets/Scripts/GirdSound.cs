using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GirdSound : MonoBehaviour, IPointerEnterHandler
{
    public AudioClip GirdSounds;
    public float soundVolume = 0.3f; // 볼륨 값을 설정합니다.
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.clip = GirdSounds;
        audioSource.volume = soundVolume; // 설정한 볼륨 값을 적용합니다.
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (GirdSounds != null)
        {
            audioSource.PlayOneShot(GirdSounds);
        }
    }
}