using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TUTORIAL: https://www.youtube.com/watch?v=lq-5Ws-h0Kc&t=1053s

public class MusicPlayer : MonoBehaviour
{

    #region attributes

    public List<AudioClip> musicClips = new List<AudioClip>();

    public List<AudioClip> menuMusic = new List<AudioClip>();
    public List<AudioClip> chapterMusic = new List<AudioClip>();
    public List<AudioClip> bossMusic = new List<AudioClip>();


    private AudioSource musicSource;

    AudioClip currentTrack;

    private float length;

    private Coroutine musicLoop;

    private MusicQueue musicQueue;

    #endregion

    void Start()
    {
        musicQueue = new MusicQueue(musicClips);

        musicSource = GetComponent<AudioSource>();

        StartMusic();
    }
  
    public void PlayMusicClip(AudioClip music)
    {
        musicSource.Stop();
        musicSource.clip = music;
        musicSource.Play();
    }

    public void StopMusic()
    {
        if(musicLoop != null)
        {
            StopCoroutine(musicLoop);
        }
    }

    public void StartMusic()
    {
        musicLoop = StartCoroutine(musicQueue.LoopMusic(this, 0, PlayMusicClip));
    }

    public void pauseMusic()
    {
        musicSource.Pause();
    }

    public void resumeMusic()
    {
        musicSource.UnPause();
    }

}

public class MusicQueue
{
    private List<AudioClip> clips;

    public MusicQueue(List<AudioClip> clips)
    {
        this.clips = clips;
    }

    public IEnumerator LoopMusic(MonoBehaviour player, float delay, System.Action<AudioClip> playFunction)
    {
        while (true)
        {
            yield return player.StartCoroutine(Run(RandomizeList(clips), delay, playFunction));
        }
    }

    public IEnumerator Run(List<AudioClip> tracks, float delay, System.Action<AudioClip> playFunction)
    {
        foreach(AudioClip clip in tracks)
        {
            playFunction(clip);

            yield return new WaitForSeconds(clip.length + delay);
        }
    }

    public List<AudioClip> RandomizeList(List<AudioClip> list)
    {
        List<AudioClip> copy = new List<AudioClip>(list);

        int n = copy.Count;

        while(n > 1)
        {
            n--;

            int k = Random.Range(0, n + 1);

            AudioClip value = copy[k];

            copy[k] = copy[n];
            copy[n] = value;
        }

        return copy;

    }
}
