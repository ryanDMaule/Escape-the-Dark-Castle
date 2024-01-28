using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

//TUTORIAL: https://www.youtube.com/watch?v=tNB1WiC-g8M

public class initializeAds : MonoBehaviour, IUnityAdsInitializationListener
{

    public string androidGameId;
    public string iosGameId;

    public bool isTestingMode = true;

    string gameId;

    void Awake()
    {
        InitializeAds();
    }

    void InitializeAds()
    {
        #if UNITY_IOS
                gameId = iosGameId;
        #elif UNITY_ANDROID
                gameId = androidGameId;
        #elif UNITY_EDITOR
                gameId = androidGameId;
        #endif


        if(!Advertisement.isInitialized && Advertisement.isSupported)
        {
            Advertisement.Initialize(gameId, isTestingMode, this);
        }

    }

    void IUnityAdsInitializationListener.OnInitializationComplete()
    {
        //throw new System.NotImplementedException();
        print("OnInitializationComplete");
    }

    void IUnityAdsInitializationListener.OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        //throw new System.NotImplementedException();
        print("OnInitializationFailed");
    }
}
