using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinManager : MonoBehaviour
{
    private int coinValue = 0;
    public Slider slider;
    private int currentCoin;
    private int max;

    void Start()
    {
        if(!PlayerPrefs.HasKey("Coins")){
            PlayerPrefs.SetInt("Coins", 0);
            coinValue = PlayerPrefs.GetInt("Coins");
        }else{
            coinValue = PlayerPrefs.GetInt("Coins");
        }
    }

    void Update()
    {       
        if(PlayerPrefs.GetInt("isLevelEnded0")==1){
            PlayerPrefs.SetInt("maxCoin",GameObject.FindGameObjectsWithTag("Coin").Length);
            PlayerPrefs.SetInt("Coins",PlayerPrefs.GetInt("Coins")+Mathf.CeilToInt(PlayerPrefs.GetInt("currentCoins")*1.5f));
            PlayerPrefs.SetInt("currentCoins",0);
            currentCoin=0;
        }
    }
    void incrementCoins(int amount){
        currentCoin +=amount;
        PlayerPrefs.SetInt("currentCoins", currentCoin);
        slider.value = currentCoin;
    }

    string getActiveCoin(){
        return PlayerPrefs.GetInt("currentCoins").ToString();
    }

    void OnTriggerEnter2D(Collider2D col){
        if(col.CompareTag("Coin")){
            Destroy(col.gameObject);
            incrementCoins(1);
            if(PlayerPrefs.GetInt("SFX") == 1){
                FindObjectOfType<AudioManager>().Play("Coin");
            }
        }
    }
}
