using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UIElements;

public class AudioManager : MonoBehaviour
{
    [Header("------Audio Source-----")]
    [SerializeField] private AudioSource MusicSource;
    [SerializeField] private Slider musicSlider; 
    [SerializeField] private AudioSource SFXSource;
    [SerializeField] private Slider soundSlider;

    [Header("------Audio Clips-----")]
    public AudioClip backgroundMusic;


    private void Start()
    {
        MusicSource.clip = backgroundMusic;
        MusicSource.Play();
    }
   
}
