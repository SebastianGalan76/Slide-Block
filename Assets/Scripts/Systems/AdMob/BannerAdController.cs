using GoogleMobileAds.Api;
using UnityEngine;

public class BannerAdController 
{
    private static BannerView bannerView;

    public static void CreateBannerView() {
        Destroy();

        bannerView = new BannerView(AdSystem.GetAdId(AdType.BANNER), AdSize.Banner, AdPosition.Bottom);
    }
    public static void LoadAd() {
        if(bannerView == null) {
            CreateBannerView();
        }

        if(bannerView != null) {
            AdRequest adRequest = new AdRequest();
            bannerView.LoadAd(adRequest);
        }
    }
    public static void Destroy() {
        if(bannerView != null) {
            bannerView.Destroy();
            bannerView = null;
        }
    }

    public static void HideAd() {
        if(bannerView != null ) {
            bannerView.Hide();
        }
    }
}
