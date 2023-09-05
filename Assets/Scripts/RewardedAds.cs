using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;

[RequireComponent (typeof (Button))]
public class RewardedAds : MonoBehaviour, IUnityAdsListener {

    #if UNITY_IOS
    private string gameId = "3462990";
    #elif UNITY_ANDROID
    private string gameId = "3462991";
    #endif

    Button myButton;
    public string myPlacementId = "rewardedVideo";
    bool testMode = false;
    bool doubleCoinAdFinished=false;
    void Start () {   
        myButton = GetComponent <Button> ();

        // Set interactivity to be dependent on the Placement’s status:
        myButton.interactable = Advertisement.IsReady (myPlacementId); 

        // Map the ShowRewardedVideo function to the button’s click listener:
        if (myButton) myButton.onClick.AddListener (ShowRewardedVideo);

        // Initialize the Ads listener and service:
        Advertisement.AddListener (this);
        Advertisement.Initialize (gameId, testMode);
    }

    // Implement a function for showing a rewarded video ad:
    void ShowRewardedVideo () {
        Advertisement.Show (myPlacementId);
    }

    // Implement IUnityAdsListener interface methods:
    public void OnUnityAdsReady (string placementId) {
        // If the ready Placement is rewarded, activate the button: 
        if (placementId == myPlacementId) {
            if(doubleCoinAdFinished){
                myButton.interactable = false;
                doubleCoinAdFinished=false;        
            }
            else{
                myButton.interactable = true;
            }
        }
    }

    public void OnUnityAdsDidFinish (string placementId, ShowResult showResult) {
        // Define conditional logic for each ad completion status:
        if (showResult == ShowResult.Finished) {
            PlayerPrefs.SetInt("Coins",PlayerPrefs.GetInt("Coins")+(PlayerPrefs.GetInt("rewardedCoin")*2));
            PlayerPrefs.SetInt("rewardedCoin",0);
            doubleCoinAdFinished=true;
            myButton.interactable=false;
            Debug.Log("calisti1");
        } else if (showResult == ShowResult.Skipped) {
            PlayerPrefs.SetInt("rewardedCoin",0);
            Debug.Log("calisti2");
        } else if (showResult == ShowResult.Failed) {
            PlayerPrefs.SetInt("rewardedCoin",0);
            Debug.Log("calisti3");
        }
    }

    public void OnUnityAdsDidError (string message) {
        // Log the error.
    }

    public void OnUnityAdsDidStart (string placementId) {
        // Optional actions to take when the end-users triggers an ad.
    } 
}