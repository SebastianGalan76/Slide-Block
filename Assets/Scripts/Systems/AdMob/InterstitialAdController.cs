using GoogleMobileAds.Api;

public class InterstitialAdController
{
    private static InterstitialAd interstitialAd, videoAd;

    public static void LoadAds() {
        LoadInterstitialAd();
        LoadVideoAd();
    }

    public static void Show() {
        if(!ShowVideoAd()) {
            ShowInterstitialAd();
        }
    }

    public static void CheckAdValue() {
        if(AdSystem.GetAdValue() >= 10) {
            Show();
        }
    }

    public static bool IsReady() {
        return (interstitialAd != null && interstitialAd.CanShowAd()) || (videoAd != null && videoAd.CanShowAd());
    }

    //Interstitial
    private static void LoadInterstitialAd() {
        if(interstitialAd != null) {
            interstitialAd.Destroy();
            interstitialAd = null;
        }

        AdRequest adRequest = new AdRequest();
        InterstitialAd.Load(AdSystem.GetAdId(AdType.INTERSTITIAL), adRequest,
            (InterstitialAd ad, LoadAdError error) => {
                if(ad != null && error == null) {
                    interstitialAd = ad;

                    interstitialAd.OnAdFullScreenContentClosed += () => {
                        AdSystem.ChangeAdValue(-3);

                        LoadInterstitialAd();
                    };

                    interstitialAd.OnAdFullScreenContentFailed += (AdError error) => {
                        LoadInterstitialAd();
                    };
                }
            });

    }
    private static bool ShowInterstitialAd() {
        if(interstitialAd != null && interstitialAd.CanShowAd()) {
            interstitialAd.Show();
            return true;
        }

        return false;
    }

    //Video
    private static void LoadVideoAd() {
        if(videoAd != null) {
            videoAd.Destroy();
            videoAd = null;
        }

        AdRequest adRequest = new AdRequest();
        InterstitialAd.Load(AdSystem.GetAdId(AdType.VIDEO), adRequest,
            (InterstitialAd ad, LoadAdError error) => {
                if(ad != null && error == null) {
                    videoAd = ad;

                    videoAd.OnAdFullScreenContentClosed += () => {
                        AdSystem.ChangeAdValue(-5);

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
    private static bool ShowVideoAd() {
        if(videoAd != null && videoAd.CanShowAd()) {
            videoAd.Show();
            return true;
        }

        return false;
    }
}
