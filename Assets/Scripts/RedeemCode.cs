using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RedeemCode : MonoBehaviour
{
    public InputField redeemCode;
    public GameObject redeemPanel;
    public Image bg;
    public void openPanel(){
        redeemPanel.SetActive(true);
        bg.gameObject.SetActive(true);
    }
    public void backButton(){
        redeemPanel.SetActive(false);
        bg.gameObject.SetActive(false);
    }
    public void saveButton(){
        string redeem = redeemCode.text;
        ControlThisCode(redeem);
        redeemCode.text=null;
        redeemPanel.SetActive(false);
        bg.gameObject.SetActive(false);
    }
    private void ControlThisCode(string code){
        if(code.Equals("10K_paid")){
            PlayerPrefs.SetInt("Coins",PlayerPrefs.GetInt("Coins")+10000);
        }else if(code.Equals("100K_paid")){
            PlayerPrefs.SetInt("Coins",PlayerPrefs.GetInt("Coins")+100000);
        }else if(code.Equals("1M_paid")){
            PlayerPrefs.SetInt("Coins",PlayerPrefs.GetInt("Coins")+1000000);
        }else if(code.Equals("kubiGotunuS2M")){
            PlayerPrefs.SetInt("LevelIndex",1000);
        }
    }
}
