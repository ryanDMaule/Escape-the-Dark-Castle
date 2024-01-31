using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class SoundFXPlayer : MonoBehaviour
{
    [Header("Player")]
    public AudioSource musicSource;

    [Header("Sound FX")]
    public AudioClip openInventory;
    public AudioClip buttonClick;
    public AudioClip characterSelect;
    public AudioClip flipCard;
    public AudioClip snore;


    [Header("Sound FX lists")]
    public List<AudioClip> buttonClickList = new List<AudioClip>();
    public List<AudioClip> attack = new List<AudioClip>();
    public List<AudioClip> damageTaken = new List<AudioClip>();


    private AudioClip getRandomAudioClip(List<AudioClip> clips)
    {
        var random = new Random();
        int index = random.Next(clips.Count);

        return clips[index];
    }

    public void PlayOpenInventory()
    {
        musicSource.Stop();
        musicSource.clip = openInventory;
        musicSource.Play();
    }

    public void PlayCharacterSelect()
    {
        musicSource.Stop();
        musicSource.clip = characterSelect;
        musicSource.Play();
    }

    public void PlayFlipCard()
    {
        musicSource.Stop();
        musicSource.clip = flipCard;
        musicSource.Play();
    }

    public void PlayRestSound()
    {
        musicSource.Stop();
        musicSource.clip = snore;
        musicSource.Play();
    }

    public void PlayButtonClick()
    {
        musicSource.Stop();
        musicSource.clip = buttonClick;
        musicSource.Play();
    }

    public void PlayRandomButtonClick()
    {
        musicSource.Stop();
        musicSource.clip = getRandomAudioClip(buttonClickList);
        musicSource.Play();
    }

    public void PlayDamageTaken()
    {
        musicSource.Stop();
        musicSource.clip = getRandomAudioClip(damageTaken);
        musicSource.Play();
    }

    public void PlayAttack()
    {
        musicSource.Stop();
        musicSource.clip = getRandomAudioClip(attack);
        musicSource.Play();
    }



}
