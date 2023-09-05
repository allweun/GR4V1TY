using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuMaster : MonoBehaviour
{
    [Header("Buttons")]
    public Button Pause;
    public Button store;
    public Button exitButton;
    public Button music;
    public Button sfx;
    public Button credits;
    public Button magnify;
    public Button tapToPlay;
    public Button rewardedButton;
    [Header("Text")]
    public Text CoinPanelMenu;
    [Header("Level Elements")]
    public GameObject slider;
    public GameObject coinImage;
    public GameObject levelCounter;


    private bool musicOn = true;
    private bool sfxOn = true;
    [Header("Panels")]
    public GameObject menuPanel;
    public GameObject storePanel;
    public GameObject levelEndPanel;
    [Header("Images")]
    [SerializeField]
    public Image sfxClose;
    public Image sfxOnImage;
    [SerializeField]
    public Image musicClose;
    public Image musicOnImage;

    Animator animator;
    Animator storeAnim;
    Animator levelEndAnim;


    private bool firstBoot = true;

    void Awake()
    {
        if(!PlayerPrefs.HasKey("isLevelEnded0"))
            PlayerPrefs.SetInt("isLevelEnded0",0);
        storeAnim = storePanel.GetComponent<Animator>();
        animator = menuPanel.GetComponent<Animator>();
        levelEndAnim = levelEndPanel.GetComponent<Animator>();
        FindObjectOfType<AudioManager>().Play("music1");
    }
    
    void FixedUpdate(){

        CoinPanelMenu.text=PlayerPrefs.GetInt("Coins").ToString();
    }
    void Update(){
            CoinPanelMenu.text=PlayerPrefs.GetInt("Coins").ToString();
    }
    void Start(){        
        if(!PlayerPrefs.HasKey("SFX")){
            PlayerPrefs.SetInt("SFX", 1);
        }else{
            sfxOn = (PlayerPrefs.GetInt("SFX") == 1) ? true : false;
        }

        FindObjectOfType<AudioManager>().Play("music1");
        if(!PlayerPrefs.HasKey("MUSIC")){
            PlayerPrefs.SetInt("MUSIC", 1);
            musicOn=true;
        }else{
            musicOn = (PlayerPrefs.GetInt("MUSIC") == 1) ? true : false;
        }

        if(!musicOn){
            musicClose.gameObject.SetActive(true);
            musicOnImage.gameObject.SetActive(false);
            FindObjectOfType<AudioManager>().Stop("music1");
        }else{
            musicClose.gameObject.SetActive(false);
            musicOnImage.gameObject.SetActive(true);
            FindObjectOfType<AudioManager>().Play("music1");
        }

        if(!(PlayerPrefs.GetInt("MUSIC") == 1)){
            musicClose.gameObject.SetActive(true);
            FindObjectOfType<AudioManager>().Stop("music1");
        }else{
            musicClose.gameObject.SetActive(false);
            FindObjectOfType<AudioManager>().Play("music1");
        }

        if(!sfxOn){
            sfxClose.gameObject.SetActive(true);
            sfxOnImage.gameObject.SetActive(false);
        }else{
            sfxClose.gameObject.SetActive(false);
            sfxOnImage.gameObject.SetActive(true);
        }

        if(firstBoot){
            MusicButtonBoot();
            MusicButtonBoot();
        }
    }
    
    public void GameUI(bool a){
        Pause.gameObject.SetActive(a);
        magnify.gameObject.SetActive(a);
        levelCounter.SetActive(a);
        coinImage.SetActive(a);
        slider.SetActive(a);
    }


    public void MusicButtonBoot(){
        if(sfxOn){
            //FindObjectOfType<AudioManager>().Play("Button");
        }
        if(musicOn){
            musicClose.gameObject.SetActive(true);
            musicOnImage.gameObject.SetActive(false);
            musicOn = false;
            PlayerPrefs.SetInt("MUSIC", 0);
            FindObjectOfType<AudioManager>().Stop("music1");
        }else{
            musicClose.gameObject.SetActive(false);
            musicOnImage.gameObject.SetActive(true);
            musicOn = true;
            PlayerPrefs.SetInt("MUSIC", 1);
            FindObjectOfType<AudioManager>().Play("music1");
        }
    }

    public void MusicButton(){
        if(sfxOn){
            FindObjectOfType<AudioManager>().Play("Button");
        }
        if(musicOn){
            musicClose.gameObject.SetActive(true);
            musicOnImage.gameObject.SetActive(false);
            musicOn = false;
            PlayerPrefs.SetInt("MUSIC", 0);
            FindObjectOfType<AudioManager>().Stop("music1");
        }else{
            musicClose.gameObject.SetActive(false);
            musicOnImage.gameObject.SetActive(true);
            musicOn = true;
            PlayerPrefs.SetInt("MUSIC", 1);
            FindObjectOfType<AudioManager>().Play("music1");
        }
    }

    public void SFXButton(){
        if(sfxOn){
            FindObjectOfType<AudioManager>().Play("Button");
            sfxClose.gameObject.SetActive(true);
            sfxOnImage.gameObject.SetActive(false);
            sfxOn = false;
            PlayerPrefs.SetInt("SFX", 0);
        }else{
            sfxClose.gameObject.SetActive(false);
            sfxOnImage.gameObject.SetActive(true);
            sfxOn = true;
            PlayerPrefs.SetInt("SFX", 1);
        }
    }

    public void playButton(){
        if(sfxOn){
            FindObjectOfType<AudioManager>().Play("Button");
        }
        animator.SetBool("openPanel", false);
        GameUI(true);
        //Advertisement.Banner.Show();
    }
    public void PauseButton(){
        animator.SetBool("openPanel", true);
        if(sfxOn){
            FindObjectOfType<AudioManager>().Play("Button");
        }
        CoinPanelMenu.text=PlayerPrefs.GetInt("Coins").ToString();
        //Advertisement.Banner.Hide(false);
        GameUI(false);

    }
    public void OpenStore(){
        storePanel.SetActive(true);
        if(sfxOn){
            FindObjectOfType<AudioManager>().Play("Button");
        }
        storeAnim.SetBool("openStore", true);
        tapToPlay.interactable=false;
    }
    public void CloseStore(){
        if(sfxOn){
            FindObjectOfType<AudioManager>().Play("Button");
        }
        storeAnim.SetBool("openStore", false);
        storePanel.SetActive(false);
        tapToPlay.interactable=true;
    }
    public void QuitGame(){
        Application.Quit();
    }
    void LevelEnded(){
        PlayerPrefs.SetInt("isLevelEnded",1);
        PlayerPrefs.SetInt("isLevelEnded0",0);
    }
    public void NextLevel(){
        rewardedButton.interactable=true;
        if(PlayerPrefs.GetInt("isLevelEnded0")==1){
            GameUI(true);
            LevelEnded();
        }
        //Advertisement.Banner.Hide(true);
    }
    public void OpenUrl(){
        Application.OpenURL("https://allweun.com/");
    }
}
