using System.Collections;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using GoogleMobileAds.Api;
using TMPro;

public class ScreenAdMob : MonoBehaviour
{
    private void Start()
    {
        MobileAds.Initialize((InitializationStatus initStatus) =>
        {
            // 광고가 초기화된 후 호출.
            Debug.Log("Google Mobile Ads SDK 초기화 완료");
            LoadInterstitialAd(); // SDK 초기화 후 광고 로드
        });
    }

    //-----------[광고 관련]------------
#if UNITY_ANDROID
    private string _adUnitId = "ca-app-pub-4956833096962057/6073199467";
#elif UNITY_IPHONE
      private string _adUnitId = "ca-app-pub-4956833096962057/6073199467";
#else
      private string _adUnitId = "unused";
#endif

    private InterstitialAd _interstitialAd;

    // 광고 로드
    public void LoadInterstitialAd()
    {
        // 이전 광고가 있으면 정리
        if (_interstitialAd != null)
        {
            _interstitialAd.Destroy();
            _interstitialAd = null;
        }

        Debug.Log("Loading the interstitial ad.");

        // 광고 요청 생성
        AdRequest adRequest = new AdRequest();

        // 광고 로드 요청
        InterstitialAd.Load(_adUnitId, adRequest, (InterstitialAd ad, LoadAdError error) =>
        {
            if (error != null || ad == null)
            {
                Debug.LogError("Interstitial ad failed to load an ad with error: " + error);
                return;
            }

            Debug.Log("Interstitial ad loaded with response: " + ad.GetResponseInfo());
            _interstitialAd = ad;

            RegisterEventHandlers(_interstitialAd);
        });
    }

    // 광고 표시
    public void ShowInterstitialAd()
    {
        StartCoroutine(showInterstitial());

        IEnumerator showInterstitial()
        {
            while (_interstitialAd == null && !_interstitialAd.CanShowAd())
            {
                yield return new WaitForSeconds(0.2f);
            }
            _interstitialAd.Show();
        }
        /*if (_interstitialAd != null && _interstitialAd.CanShowAd())
        {
            Debug.Log("Showing interstitial ad.");
            _interstitialAd.Show();
        }
        else
        {
            Debug.LogError("Interstitial ad is not ready yet.");
        }*/
    }

    private void RegisterReloadHandler(InterstitialAd interstitialAd)
    {
        // 전면 광고가 전체 화면 콘텐츠를 닫았을 때 호출됩니다.
        interstitialAd.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Interstitial Ad full screen content closed.");

            // 가능한 한 빨리 다른 광고를 보여줄 수 있도록 광고를 다시 로드합니다.
            LoadInterstitialAd();
        };

        // 전면 광고가 전체 화면 콘텐츠를 열지 못했을 때 호출됩니다.
        interstitialAd.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Interstitial ad failed to open full screen content " +
                       "with error : " + error);

            // 가능한 한 빨리 다른 광고를 보여줄 수 있도록 광고를 다시 로드합니다.
            LoadInterstitialAd();
        };
    }

    private void RegisterEventHandlers(InterstitialAd interstitialAd)
    {
        interstitialAd.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log(String.Format("Interstitial ad paid {0} {1}.", adValue.Value, adValue.CurrencyCode));
        };

        interstitialAd.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Interstitial ad recorded an impression.");
        };

        interstitialAd.OnAdClicked += () =>
        {
            Debug.Log("Interstitial ad was clicked.");
        };

        interstitialAd.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Interstitial ad full screen content opened.");
        };

        interstitialAd.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Interstitial ad full screen content closed.");
            SceneManager.LoadScene("MainMenu"); // 광고후 메뉴로 돌아가기
            LoadInterstitialAd(); // 광고 닫힌 후 새 광고 로드
        };

        interstitialAd.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Interstitial ad failed to open full screen content with error: " + error);
            LoadInterstitialAd(); // 광고 실패 후 새 광고 로드
        };
    }

    // 게임 오버시 광고 표시 
    public void GameOver()
    {
        ShowInterstitialAd();
    }
}
