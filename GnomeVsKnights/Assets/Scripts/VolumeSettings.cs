using UnityEngine;
using UnityEngine.Audio;
using Unity.UI;
using UnityEngine.UIElements;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField] private AudioMixer MusicSource;
    [SerializeField] private Slider musicSlider; 
    [SerializeField] private AudioMixer SoundSource;
    [SerializeField] private Slider soundSlider;

    public void SetMusicVolume()
    {

    }

}
