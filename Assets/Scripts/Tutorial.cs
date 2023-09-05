using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    public Text pause;
    public Text magnify;
    public Text coinText;
    public Text rightText;
    public Text leftText;
    public Text gravityChange;
    public Text skipTutorial;
    public Text levelChangeText;
    public GameObject inputPanel;
    public GameObject Panel;
    public GameObject tutorialPanel;
    // Start is called before the first frame update
    void Awake(){
        if(!PlayerPrefs.HasKey("tutorialPassed")){
            inputPanel.SetActive(false);
            Panel.SetActive(false);
            PlayerPrefs.SetInt("tutorialPassed",0);
        }
        if(PlayerPrefs.GetString("language").Equals("TR")){
            PlayerPrefs.SetString("tutorailLang","TR");
        }else{
            PlayerPrefs.SetString("tutorailLang","EN");
        }
    }
    void Start()
    {
        if(PlayerPrefs.GetInt("tutorialPassed")==0)
            TextLanguage();
    }
    public void CloseTutorial(){
        inputPanel.SetActive(true);
        Panel.SetActive(true);
        tutorialPanel.SetActive(false);
        PlayerPrefs.SetInt("tutorialPassed1",1);
    }
    private void TextLanguage(){
        if(PlayerPrefs.GetString("language").Equals("TR")){
            pause.text="Durdur";
            magnify.text="Labirentin tamamını gör";
            coinText.text="Oyun madeni";
            rightText.text="Sağına Dokun!";
            leftText.text="Soluna Dokun!";
            gravityChange.text="Topun Yönünü Değiştirmek için Ekranın";
            skipTutorial.text="Tanıtımı Sonlandır";
            levelChangeText.text="Her 20 seviyede bir labirent büyür";
        }else{
            pause.text="Pause";
            magnify.text="View the Entire Maze";
            coinText.text="Coin";
            rightText.text="Tap to Right!";
            leftText.text="Tap to Left!";
            gravityChange.text="Change the Direction of the Ball";
            skipTutorial.text="Tap to Complete";
            levelChangeText.text="Maze Grows Every 20 Levels";
        }
    }
}
