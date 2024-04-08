using GoogleMobileAds.Api;
using System;

public class RewardedAdController
{
    private static RewardedAd rewardedAd;

    public delegate void AdLoadedHandler();
    public delegate void AdIsNotLoadedHandler();

    public static event AdLoadedHandler OnAdLoaded;

    public static void LoadAd() {
        if(!IsReady()) {
            DestroyAd();
        }

        AdRequest adRequest = new AdRequest();
        RewardedAd.Load(AdSystem.GetAdId(AdType.REWARDED), adRequest,
            (RewardedAd ad, LoadAdError error) => {
                if(ad != null && error == null) {
                    rewardedAd = ad;

                    rewardedAd.OnAdFullScreenContentClosed += () => {
                        AdSystem.ChangeAdValue(-8);

                        LoadAd();
                    };

                    rewardedAd.OnAdFullScreenContentFailed += (AdError error) => {
                        LoadAd();
                    };

                    OnAdLoaded?.Invoke();
                }
            });
    }
    public static void ShowAd(Action<Reward> rewardHandler, AdIsNotLoadedHandler adIsNotLoadedAction = null) {
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
    private static void DestroyAd() {
        if(rewardedAd != null) {
            rewardedAd.Destroy();
            rewardedAd = null;
        }
    }

    public static bool IsReady() {
        return rewardedAd != null && rewardedAd.CanShowAd();
    }
}
