using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MultiLanguage : MonoBehaviour
{
    public GameObject Store;
    public GameObject redeemCode;
    public GameObject prize;
    public GameObject langButt;
    public Text tapToPlay;
    public Text tapToNext;
    public Text redeemText;
    public Text uWantThis;
    public Sprite EN;
    public Sprite TR;
    public Sprite redeemEN;
    public Sprite redeemTR;
    public Sprite storeEN;
    public Sprite storeTR;
    public Sprite prizeEN;
    public Sprite prizeTR;


    void Start()
    {
        if(!PlayerPrefs.HasKey("language")){
            if(Application.systemLanguage == SystemLanguage.English){
                PlayerPrefs.SetString("language","EN");
            }else if(Application.systemLanguage == SystemLanguage.Turkish){
                PlayerPrefs.SetString("language","TR");
            }else
                PlayerPrefs.SetString("language","EN");
        }
        controlLang();
    }
    void FixedUpdate(){
        controlLang();
    }
    public void LangButton(){
        if(PlayerPrefs.GetString("language").Equals("EN")){
            PlayerPrefs.SetString("language","TR");
        }else{
            PlayerPrefs.SetString("language","EN");
        }
    }
    void controlLang(){
        if(PlayerPrefs.GetString("language").Equals("TR")){
            ChangeLangTR();
        }else{
            ChangeLangEN();
        }
    }
    void ChangeLangTR(){
        if(PlayerPrefs.GetString("language").Equals("TR")){
            langButt.GetComponent<Image>().sprite=TR;
            redeemCode.GetComponent<Image>().sprite=redeemTR;
            Store.GetComponent<Image>().sprite=storeTR;
            prize.GetComponent<Image>().sprite=prizeTR;
            redeemText.text="PROMO KOD";
            uWantThis.text="Emin misin?";
            tapToNext.text="İlerlemek İçin DOKUN!";
            tapToPlay.text="Oynamak İçin DOKUN!";
        }
    }
    void ChangeLangEN(){
        if(PlayerPrefs.GetString("language").Equals("EN")){
            langButt.GetComponent<Image>().sprite=EN;
            redeemCode.GetComponent<Image>().sprite=redeemEN;
            Store.GetComponent<Image>().sprite=storeEN;
            prize.GetComponent<Image>().sprite=prizeEN;
            tapToNext.text="Tap to NEXT!";
            uWantThis.text="U Want This?";
            redeemText.text="REDEEM CODE";
            tapToPlay.text="Tap to PLAY!";
        }
    }
}
