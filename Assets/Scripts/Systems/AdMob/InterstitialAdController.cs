using GoogleMobileAds.Api;

public class InterstitialAdController
{
    private static InterstitialAdController instance;

    private InterstitialAd interstitialAd, videoAd;
    private AdSystem adSystem;

    private InterstitialAdController() {
        adSystem = AdSystem.GetInstance();
    }

    public void LoadAds() {
        LoadInterstitialAd();
        LoadVideoAd();
    }

    public void Show() {
        if(!ShowVideoAd()) {
            ShowInterstitialAd();
        }
    }

    public void CheckAdValue() {
        if(adSystem.GetAdValue() >= 10) {
            Show();
        }
    }

    public bool IsReady() {
        return (interstitialAd != null && interstitialAd.CanShowAd()) || (videoAd != null && videoAd.CanShowAd());
    }

    //Interstitial
    private void LoadInterstitialAd() {
        if(interstitialAd != null) {
            interstitialAd.Destroy();
            interstitialAd = null;
        }

        AdRequest adRequest = new AdRequest();
        InterstitialAd.Load(adSystem.GetAdId(AdType.INTERSTITIAL), adRequest,
            (InterstitialAd ad, LoadAdError error) => {
                if(ad != null && error == null) {
                    interstitialAd = ad;

                    interstitialAd.OnAdFullScreenContentClosed += () => {
                        adSystem.ChangeAdValue(-3);

                        LoadInterstitialAd();
                    };

                    interstitialAd.OnAdFullScreenContentFailed += (AdError error) => {
                        LoadInterstitialAd();
                    };
                }
            });

    }
    private bool ShowInterstitialAd() {
        if(interstitialAd != null && interstitialAd.CanShowAd()) {
            interstitialAd.Show();
            return true;
        }

        return false;
    }

    //Video
    private void LoadVideoAd() {
        if(videoAd != null) {
            videoAd.Destroy();
            videoAd = null;
        }

        AdRequest adRequest = new AdRequest();
        InterstitialAd.Load(adSystem.GetAdId(AdType.VIDEO), adRequest,
            (InterstitialAd ad, LoadAdError error) => {
                if(ad != null && error == null) {
                    videoAd = ad;

                    videoAd.OnAdFullScreenContentClosed += () => {
                        adSystem.ChangeAdValue(-5);

                        ShowInterstitialAd();
                        LoadVideoAd();
                    };

                    videoAd.OnAdFullScreenContentFailed += (AdError error) => {
                        ShowInterstitialAd();

                        LoadVideoAd();
                    };
                }
            });

    }
    private bool ShowVideoAd() {
        if(videoAd != null && videoAd.CanShowAd()) {
            videoAd.Show();
            return true;
        }

        return false;
    }

    public static InterstitialAdController GetInstance() {
        if(instance == null) {
            instance = new InterstitialAdController();
        }

        return instance;
    }
}
