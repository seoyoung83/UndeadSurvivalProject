using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    AudioSource UIAudio;

    [Header("Get Pickup")]
    [SerializeField] AudioClip getPickup;//Wrong_14

    [Header("Button UI")]
    [SerializeField] AudioClip buttonClick;//Click_06

    [Header("Roulette UI")]
    [SerializeField] AudioClip rouletteUIOpen;// Collect_14 
    [SerializeField] AudioClip rouletteUISpin;// Collect_10 
    [SerializeField] AudioClip rouletteItem; // Click_17 
    [SerializeField] AudioClip rouletteItemSelected; // Seq_15
    [SerializeField] AudioClip rouletteDescription; //Click_18

    [Header("Button UI")]
    [SerializeField] AudioClip skillSelectUIOpen;//Collect_18

    [Header("Boss Warnning Display UI")]
    [SerializeField] AudioClip warnningUIOpen; // Lose_18  

    private void Awake()
    {
        Instance = this;

        UIAudio = GetComponent<AudioSource>();
    }

    public void WarnningDisplayUIOpen()
    {
        UIAudio.PlayOneShot(warnningUIOpen);
    }

    public void GetPickupAudioEvent()
    {
        UIAudio.PlayOneShot(getPickup);
    }

    public void OnClickButtonAudioEvent()
    {
        UIAudio.PlayOneShot(buttonClick);
    }

    public void RouletteUIOpen()
    {
        UIAudio.PlayOneShot(rouletteUIOpen);
    }

    public void RouletteStartSpin()
    {
        UIAudio.PlayOneShot(rouletteUISpin);
    }

    public void RouletteItemAudioEvent(bool isSelected)
    {
        if(!isSelected)
            UIAudio.PlayOneShot(rouletteItem);
        else if (isSelected)
            UIAudio.PlayOneShot(rouletteItemSelected);
    }

    public void RouletteDescriptioOpen()
    {
        UIAudio.PlayOneShot(rouletteDescription);
    }

    public void SkillSelectUIOpen(bool _open)
    {
        if (_open)
            UIAudio.PlayOneShot(skillSelectUIOpen);
        else
            UIAudio.Stop();
    }
}
