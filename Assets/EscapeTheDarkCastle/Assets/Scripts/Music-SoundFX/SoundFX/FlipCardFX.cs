using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class FlipCardFX : MonoBehaviour
{
    [SerializeField] public Button button;
    void Start()
    {
        SoundFXPlayer soundFX = FindFirstObjectByType<SoundFXPlayer>();
        button.onClick.AddListener(() => soundFX.PlayFlipCard());
    }
}
