using UnityEngine;
using UnityEngine.UI;


public class DescriptionAudio : MonoBehaviour
{
    public AudioSource audioSource;

    [SerializeField] public GameObject audioPlayer;
    [SerializeField] public Sprite playSprite;
    [SerializeField] public Sprite stopSprite;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (audioSource.isPlaying)
        {
            if(audioPlayer.GetComponent<Image>().sprite != stopSprite)
            {
                audioPlayer.GetComponent<Image>().sprite = stopSprite;

                audioPlayer.GetComponent<Button>().onClick.RemoveAllListeners();
                audioPlayer.GetComponent<Button>().onClick.AddListener(() => audioSource.Stop());
            }
        }
        else
        {
            if(audioPlayer.GetComponent<Image>().sprite != playSprite)
            {
                audioPlayer.GetComponent<Image>().sprite = playSprite;

                audioPlayer.GetComponent<Button>().onClick.RemoveAllListeners();
                audioPlayer.GetComponent<Button>().onClick.AddListener(() => audioSource.Play());
            }
        }
    }

}
