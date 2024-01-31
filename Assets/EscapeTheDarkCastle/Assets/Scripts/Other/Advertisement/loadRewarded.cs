using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.Android;

//TUTORIAL: https://www.youtube.com/watch?v=tNB1WiC-g8M

public class loadRewarded : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
{

    public string androidAdUnitId;
    public string iosAdUnitId;

    string adUnitId;

    //public AdvetisementObjecyHandler handler;
    public GameOverHandler GOHandler;

    [Header("Events")]
    public GameEvent PauseMusic;
    public GameEvent ResumeMusic;

    void Awake()
    {
        #if UNITY_IOS
                adUnitId = iosAdUnitId;
        #elif UNITY_ANDROID
                adUnitId = androidAdUnitId;
        #elif UNITY_EDITOR
                adUnitId = androidAdUnitId;
        #endif
    }

    public void LoadAd()
    {
        print("Loading rewarded");
        Advertisement.Load(adUnitId, this);
    }

    public void showAd()
    {
        print("showAd");
        PauseMusic.Raise();
        Advertisement.Show(adUnitId, this);
    }

    //LOAD
    void IUnityAdsLoadListener.OnUnityAdsAdLoaded(string placementId)
    {
        if (placementId.Equals(adUnitId))
        {
            print("Rewarded loaded!");
            //throw new System.NotImplementedException();

            showAd();
        }
    }

    void IUnityAdsLoadListener.OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        print("Rewarded failed to load!");
        //throw new System.NotImplementedException();
    }

    //SHOW
    void IUnityAdsShowListener.OnUnityAdsShowClick(string placementId)
    {
        print("OnUnityAdsShowClick");
        //throw new System.NotImplementedException();
    }

    void IUnityAdsShowListener.OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        if(placementId.Equals(adUnitId) && showCompletionState.Equals(UnityAdsCompletionState.COMPLETED))
        {
            print("OnUnityAdsShowComplete");
            //throw new System.NotImplementedException();

            ResumeMusic.Raise();
            GOHandler.adComplete();
        }
    }

    void IUnityAdsShowListener.OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        print("OnUnityAdsShowFailure");
        //throw new System.NotImplementedException();
    }

    void IUnityAdsShowListener.OnUnityAdsShowStart(string placementId)
    {
        print("OnUnityAdsShowStart");
        //throw new System.NotImplementedException();

        //SHOULDNT BE DONE HERE BUT AD SHOWN COMPLETE DOES NOT TRIGGER ON TEST BUILDS
        //ResumeMusic.Raise();
        //GOHandler.adComplete();
    }
}
