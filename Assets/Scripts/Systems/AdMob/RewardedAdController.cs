using GoogleMobileAds.Api;
using System;

public class RewardedAdController
{
    private static RewardedAdController instance;

    public delegate void AdLoadedHandler();
    public delegate void AdIsNotLoadedHandler();
    public event AdLoadedHandler OnAdLoaded;

    private RewardedAd rewardedAd;
    private AdSystem adSystem;

    private RewardedAdController() {
        adSystem = AdSystem.GetInstance();
    }

    public void LoadAd() {
        if(!IsReady()) {
            DestroyAd();
        }

        AdRequest adRequest = new AdRequest();
        RewardedAd.Load(adSystem.GetAdId(AdType.REWARDED), adRequest,
            (RewardedAd ad, LoadAdError error) => {
                if(ad != null && error == null) {
                    rewardedAd = ad;

                    rewardedAd.OnAdFullScreenContentClosed += () => {
                       adSystem.ChangeAdValue(-8);

                        LoadAd();
                    };

                    rewardedAd.OnAdFullScreenContentFailed += (AdError error) => {
                        LoadAd();
                    };

                    OnAdLoaded?.Invoke();
                }
            });
    }
    public void ShowAd(Action<Reward> rewardHandler, AdIsNotLoadedHandler adIsNotLoadedAction = null) {
        if(rewardedAd != null && rewardedAd.CanShowAd()) {
            rewardedAd.Show((Reward reward) => {
                rewardHandler?.Invoke(reward);
            });
        } else {
            if(adIsNotLoadedAction != null) {
                adIsNotLoadedAction();
            }
        }
    }
    private void DestroyAd() {
        if(rewardedAd != null) {
            rewardedAd.Destroy();
            rewardedAd = null;
        }
    }

    public bool IsReady() {
        return rewardedAd != null && rewardedAd.CanShowAd();
    }

    public static RewardedAdController GetInstance() {
        if(instance == null) {
            instance = new RewardedAdController();
        }

        return instance;
    }
}
