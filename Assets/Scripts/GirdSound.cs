using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GirdSound : MonoBehaviour, IPointerEnterHandler
{
    public AudioClip Casual_UI_Touch;
    private AudioSource audioSource;
    public float soundVolume = 0.5f;

    private void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.clip = Casual_UI_Touch;
        audioSource.volume = soundVolume; // 설정한 볼륨 값을 적용함
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (Casual_UI_Touch != null)
        {
            audioSource.PlayOneShot(Casual_UI_Touch);
        }
    }
}