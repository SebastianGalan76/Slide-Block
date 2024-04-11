using GoogleMobileAds.Api;
using UnityEngine;

public class BannerAdController 
{
    private static BannerAdController instance;

    private BannerView bannerView;
    private AdSystem adSystem;

    private BannerAdController() {
        adSystem = AdSystem.GetInstance();
    }

    public void CreateBannerView() {
        Destroy();

        bannerView = new BannerView(adSystem.GetAdId(AdType.BANNER), AdSize.Banner, AdPosition.Bottom);
    }
    public void LoadAd() {
        if(bannerView == null) {
            CreateBannerView();
        }

        if(bannerView != null) {
            AdRequest adRequest = new AdRequest();
            bannerView.LoadAd(adRequest);
        }
    }
    public void Destroy() {
        if(bannerView != null) {
            bannerView.Destroy();
            bannerView = null;
        }
    }

    public void HideAd() {
        if(bannerView != null ) {
            bannerView.Hide();
        }
    }

    public static BannerAdController GetInstance() {
        if(instance == null) {
            instance = new BannerAdController();
        }
        return instance;
    }
}
