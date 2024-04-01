using System;
using System.Collections;
using System.Collections.Generic;
using GoogleMobileAds;
using GoogleMobileAds.Api;
using GoogleMobileAds.Api.AdManager;
using UnityEngine;

public class AdManager : MonoBehaviour
{
  // These ad units are configured to always serve test ads.
  #if UNITY_ANDROID
  private string _adUnitId = "ca-app-pub-8309052672141776/5559680299";
  #elif UNITY_IPHONE
  private string _adUnitId = "ca-app-pub-3940256099942544/4411468910";
  #else
  private string _adUnitId = "unused";
  #endif
  

  private InterstitialAd _interstitialAd;
  public int TotalStarsForShop { get; private set; }

  /// <summary>
  /// Loads the interstitial ad.
  /// </summary>
  /// 
  // This ad unit is configured to always serve test ads.
  private string _adUnitIdRewarded = "ca-app-pub-8309052672141776/7177520893";

  private RewardedAd _rewardedAd;

  /// <summary>
  /// Loads the rewarded ad.
  /// </summary>
  public void LoadRewardedAd()
  {
      // Clean up the old ad before loading a new one.
      if (_rewardedAd != null)
      {
            _rewardedAd.Destroy();
            _rewardedAd = null;
      }

      Debug.Log("Loading the rewarded ad.");

      // create our request used to load the ad.
      var adRequest = new AdManagerAdRequest();

      // send the request to load the ad.
      RewardedAd.Load(_adUnitIdRewarded, adRequest,
          (RewardedAd ad, LoadAdError error) =>
          {
              // if error is not null, the load request failed.
              if (error != null || ad == null)
              {
                  Debug.LogError("Rewarded ad failed to load an ad " +
                                 "with error : " + error);
                  return;
              }

              Debug.Log("Rewarded ad loaded with response : "
                        + ad.GetResponseInfo());

              _rewardedAd = ad;
          });
  }


  public void Start()
  {
    TotalStarsForShop = PlayerPrefs.GetInt("TotalStarsForShop");
    // Initialize the Google Mobile Ads SDK.
    MobileAds.Initialize(initStatus => { });
    LoadInterstitialAd();
    LoadRewardedAd();
  }
  public void ShowRewardedAd()
{
    LoadRewardedAd();
    const string rewardMsg =
        "Rewarded ad rewarded the user. Type: {0}, amount: {1}.";

    if (_rewardedAd != null && _rewardedAd.CanShowAd())
    {
        _rewardedAd.Show((Reward reward) =>
        {
            // TODO: Reward the user.
            Debug.Log(String.Format(rewardMsg, reward.Type, reward.Amount));
            TotalStarsForShop += 20; // Corrected incrementation
            PlayerPrefs.SetInt("TotalStarsForShop", TotalStarsForShop);
            Debug.Log("TotalStarsForShop updated: " + TotalStarsForShop); // Debug log statement
        });
    }
}

public void Update()
{
    TotalStarsForShop = PlayerPrefs.GetInt("TotalStarsForShop");
}
  public void LoadInterstitialAd(Action onLoaded = null)
  {
    // Clean up the old ad before loading a new one.
    if (_interstitialAd != null)
    {
      _interstitialAd.Destroy();
      _interstitialAd = null;
    }

    Debug.Log("Loading the interstitial ad.");

    // Create our request used to load the ad.
    var adRequest = new AdRequest();

    // Send the request to load the ad.
    InterstitialAd.Load(_adUnitId, adRequest,
      (InterstitialAd ad, LoadAdError error) =>
      {
        // Handle error if loading failed
        if (error != null || ad == null)
        {
          Debug.LogError("Interstitial ad failed to load an ad " +
                         "with error : " + error);
          return;
        }

        Debug.Log("Interstitial ad loaded with response : "
                  + ad.GetResponseInfo());

        _interstitialAd = ad;

        // Call the callback function if provided after successful load
        if (onLoaded != null)
        {
          onLoaded();
        }
      });
  }

  public void ShowInterstitialAd()
  {
    
    LoadInterstitialAd();
    if (_interstitialAd != null && _interstitialAd.CanShowAd())
    {
      Debug.Log("Showing interstitial ad.");
      _interstitialAd.Show();
    }
    else
    {
      Debug.LogError("Interstitial ad is not ready yet.");

    }
  }

  private void RegisterEventHandlers(InterstitialAd interstitialAd)
  {
    // Register event handlers for additional functionalities (optional)
    interstitialAd.OnAdPaid += (AdValue adValue) =>
    {
      Debug.Log(String.Format("Interstitial ad paid {0} {1}.",
                              adValue.Value,
                              adValue.CurrencyCode));
    };

    // Implement other event handlers here (OnAdImpressionRecorded, etc.)
  }
}
