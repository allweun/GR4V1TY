using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;
using System;

public class DailyReward : MonoBehaviour, IUnityAdsListener
{
    [Header("Zamanın çekildiği api")]
    public string url = "http://worldclockapi.com/api/json/est/now";
    private bool connected = false;
    private bool canTakeReward = false;
    private string currentDate = "";
    private string currentTime = "";

    [Header("Debug Only")]
    public bool debugMode = false;
    [Tooltip("yyyy-mm-dd formatında")]
    public string debugDate;

    //ADS
     #if UNITY_IOS
    private string gameId = "3462990";
    #elif UNITY_ANDROID
    private string gameId = "3462991";
    #endif

    public Button dailyButton;
    public Text dailyText;
    string myPlacementId = "dailyReward";
    bool testMode = false;
    string LDate="";

    void Start()
    {
        Advertisement.AddListener (this);
        Advertisement.Initialize (gameId, testMode);
        dailyButton.interactable = Advertisement.IsReady (myPlacementId); 
        StartCoroutine(CheckConnection());
    }
    
    void ShowRewardedVideo () {
        Advertisement.Show (myPlacementId);
    }
    void FixedUpdate(){
        if(!canTakeReward){
            dailyButton.interactable=false;
        }else{
            dailyButton.interactable=true;
        }
        dailyText.text = LDate;
    }
    
    private IEnumerator CheckConnection(){
        WWW www = new WWW(url);
        yield return www;
        if(string.IsNullOrEmpty(www.text)){
            connected = false;
        }else{
            connected = true;
            StartCoroutine(GetDate());
        }
    }

    private IEnumerator RefreshRemainingTime(){
        WWW www = new WWW(url);
        yield return www;
        string[] splitDate = www.text.Split(new string[] {"currentDateTime\":\""}, StringSplitOptions.None);
        string buttonDate = splitDate[1].Substring(0,10);
        string buttonTime = splitDate[1].Substring(11,11).Substring(0,5);
        string[] date = buttonDate.Split(new string[] { "-" }, StringSplitOptions.None);
        string[] time = buttonTime.Split(new string[] { ":" }, StringSplitOptions.None);
        DateTime currentRefreshDate = new DateTime(int.Parse(date[0]), int.Parse(date[1]), int.Parse(date[2]), int.Parse(time[0]), int.Parse(time[1]), 0);
        buttonDate = PlayerPrefs.GetString("oldDate");
        buttonTime= PlayerPrefs.GetString("oldTime");
        date = buttonDate.Split(new string[] { "-" }, StringSplitOptions.None);
        time = buttonTime.Split(new string[] { ":" }, StringSplitOptions.None);
        DateTime oldRefreshDate = new DateTime(int.Parse(date[0]), int.Parse(date[1]), int.Parse(date[2]), int.Parse(time[0]), int.Parse(time[1]), 0);
        TimeSpan resfreshdata = currentRefreshDate.Subtract(oldRefreshDate);
        LDate = (23-int.Parse(resfreshdata.Hours.ToString())) + ":" + (59-int.Parse(resfreshdata.Minutes.ToString()));
    }
    private void RefreshRemainingTimeFunction(){
        StartCoroutine(RefreshRemainingTime());
    }


    private IEnumerator dataForButton(){
        WWW www = new WWW(url);
        yield return www;
        string[] splitDate = www.text.Split(new string[] {"currentDateTime\":\""}, StringSplitOptions.None);
        currentDate = splitDate[1].Substring(0,10);
        currentTime = splitDate[1].Substring(11,11).Substring(0,5);
        PlayerPrefs.SetString("oldDate", currentDate);
        PlayerPrefs.SetString("oldTime", currentTime);
    }

    private IEnumerator GetDate(){
        if(connected){
            WWW www = new WWW(url);
            yield return www;
            string[] splitDate = www.text.Split(new string[] {"currentDateTime\":\""}, StringSplitOptions.None);
            currentDate = splitDate[1].Substring(0,10);
            currentTime = splitDate[1].Substring(11,11).Substring(0,5);
            if(debugMode){
                currentDate = debugDate;
            }
            CheckDayElapsed();
            
        }
    }

    private void CheckDayElapsed(){
        if(!PlayerPrefs.HasKey("oldDate") && !PlayerPrefs.HasKey("oldTime")){
            PlayerPrefs.SetString("oldDate", currentDate);
            PlayerPrefs.SetString("oldTime", currentTime);
            canTakeReward = true;
        }else{
            string tempDate = PlayerPrefs.GetString("oldDate");
            string tempTime = PlayerPrefs.GetString("oldTime");
            string[] date = tempDate.Split(new string[] { "-" }, StringSplitOptions.None);
            string[] time = tempTime.Split(new string[] { ":" }, StringSplitOptions.None);
            
            DateTime oldTotalDate = new DateTime(int.Parse(date[0]), int.Parse(date[1]), int.Parse(date[2]), int.Parse(time[0]), int.Parse(time[1]), 0);
            
            string[] todayDate = currentDate.Split(new string[] { "-" }, StringSplitOptions.None);
            string[] todayTime = currentTime.Split(new string[] { ":" }, StringSplitOptions.None);

            DateTime currentTotalDate = new DateTime(int.Parse(todayDate[0]), int.Parse(todayDate[1]), int.Parse(todayDate[2]), int.Parse(todayTime[0]), int.Parse(todayTime[1]), 0);
            //DateTime _today = Convert.ToDateTime(currentDate);
            //DateTime _old = Convert.ToDateTime(tempDate);

            //TimeSpan difference = _today.Subtract(_old);

            TimeSpan totalDifference = currentTotalDate.Subtract(oldTotalDate);
            //Debug.Log(totalDifference.Days +" "+ totalDifference.Hours +" " +totalDifference.Minutes);
            if(totalDifference.Days >= 1 && connected){
                canTakeReward = true;
                //PlayerPrefs.SetString("oldDate", currentDate);
                //PlayerPrefs.SetString("oldTime", currentTime);
                //Debug.Log("ödül");
            }else{
                canTakeReward = false;
                //Debug.Log("ödül yok");
                InvokeRepeating("RefreshRemainingTimeFunction",0f,60f);
            }
        }
    }

    public void RewardButton(){
        if(canTakeReward && connected){
            canTakeReward = false;
            dailyButton.interactable = Advertisement.IsReady (myPlacementId);


            StartCoroutine(dataForButton());
            InvokeRepeating("RefreshRemainingTimeFunction",0f,60f);
            ShowRewardedVideo();
            //burada reklam çağır ve ödül ver
        }else{
            return;
        }
    }
    public void OnUnityAdsReady (string placementId) {
        // If the ready Placement is rewarded, activate the button: 
        if (placementId == myPlacementId) {
            if(canTakeReward&&connected){
                dailyButton.interactable = Advertisement.IsReady (myPlacementId);        
            }
            else{
                dailyButton.interactable = false;
            }
        }
    }
    public void OnUnityAdsDidFinish (string placementId, ShowResult showResult) {
        // Define conditional logic for each ad completion status:
        if (placementId == myPlacementId)
        if (showResult == ShowResult.Finished) {
            PlayerPrefs.SetInt("Coins",(PlayerPrefs.GetInt("Coins")+1000));
        } else if (showResult == ShowResult.Skipped) {
        } else if (showResult == ShowResult.Failed) {
        }
    }
    public void OnUnityAdsDidError (string message) {
        //Debug.Log(message);
    }

    public void OnUnityAdsDidStart (string placementId) {
        // Optional actions to take when the end-users triggers an ad.
    } 
}
