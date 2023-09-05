using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreManager : MonoBehaviour
{
    [SerializeField]
    public GameObject player;
    [SerializeField]
    public Button[] commonList;
    public GameObject[] blacks;
    public GameObject SelectedBallCircle;
    [SerializeField]
    [Header("CommonBalls")]
    public Sprite defaultBall;
    public Sprite blueBall;
    public Sprite orangeBall;
    public Sprite greenBall; 
    public Sprite yellowBall;
    public Sprite redBall;
    public Sprite blackBall;
    public Sprite purpleBall;
    public Sprite brownOrangeBall;
    public Sprite pinkPurpleBall;
    public Sprite purpleOrangeBall;
    public Sprite redWhiteBall;
    public Sprite yellowBlueBall;
    [Header("RareBalls")]
    public Sprite greenredRare;
    public Sprite orangeredRare;
    public Sprite pinkpurpleRare;
    public Sprite purplewhiteRare;
    public Sprite redblackRare;
    public Sprite redblueRare;
    public Sprite yellowblackRare;
    public Sprite yelloworangeRare;
    [Header("LegendaryBalls")]
    public Sprite worldLegendary;
    public Sprite venusLegendary;
    public Sprite saturnLegendary;


    public GameObject whiteBall;
    
    public GameObject buyPanel;

    public Text CoinBox; 
    
    private string currentBallSprite;

    private int selectedBallNumber;

    private Animation storeAnim;
    private Sprite currentBall;
    private Sprite currentItem;
    private int currentCoin;
    private string selectedItemColor;
    private int selectedItemCost;
    void Start()
    {
        currentBall = player.GetComponent<SpriteRenderer>().sprite;
        isThatBall();
    }
    void FixedUpdate(){
        IsSelledItem();
        currentCoin = PlayerPrefs.GetInt("Coins");
        CoinBox.text = currentCoin.ToString();
    }
    void Update(){
        isThatBall();
    }
    void IsSelledItem(){
        for (int i = 0; i < commonList.Length; i++)
        {
            string itemID = commonList[i].image.sprite.GetInstanceID().ToString();
                if(PlayerPrefs.GetInt(itemID)==1){
                    blacks[i].gameObject.SetActive(false);
                }
        }
    }
    void isThatBall(){
        if(PlayerPrefs.GetString("ballSpriteID")==currentBall.GetInstanceID().ToString()){
            SelectedBallCircle.transform.position=whiteBall.transform.position;
        }
        else{
            for(int i=0; i<commonList.Length;i++){
                if(PlayerPrefs.GetString("ballSpriteID")==commonList[i].image.sprite.GetInstanceID().ToString()){
                    SelectedBallCircle.transform.position=blacks[i].transform.position;
                    return;
                }
            }
        }
    }
    public void SelectedItemColor(string color){
        selectedItemColor = color;
    }
    public void SelectedItemCost(int cost){
        selectedItemCost = cost;
    }
    public void SelectBall(string color){
        Sprite selectedBall = searchColor(color);
        PlayerPrefs.SetString("ballSpriteID",selectedBall.GetInstanceID().ToString());
        isThatBall();
    }
    public void BuyMechanism(){
        FindObjectOfType<AudioManager>().Play("Button");
        if(selectedItemCost >= currentCoin){
            return;
        }
        else
        {
            buyPanel.SetActive(true);
        }
    }
    public void CloseBuyPanel(){
        buyPanel.SetActive(false);
    }
    public void BuyThisBall(){
        int cost = selectedItemCost;
        string color = selectedItemColor;
        if (cost <= currentCoin ){
            currentItem = searchColor(color);
            int newCoin = currentCoin - cost;
            PlayerPrefs.SetInt("Coins", newCoin);
            player.GetComponent<SpriteRenderer>().sprite = currentItem;
            string currentId = currentItem.GetInstanceID().ToString(); 
            PlayerPrefs.SetString("ballSpriteID", currentId);
            PlayerPrefs.SetInt(currentId, 1);
            IsSelledItem();
            CloseBuyPanel();
        }else
        {
            return;
        }
    }
    Sprite searchColor(string color){
        if(color == "blue")
            return blueBall;
        else if(color == "orange")
            return orangeBall;
        else if(color == "red")
            return redBall;
        else if(color == "black")
            return blackBall;
        else if(color == "green")
            return greenBall;
        else if(color == "yellow")
            return yellowBall;
        else if(color == "purple")
            return purpleBall;
        else if(color == "brownorange")
            return brownOrangeBall;
        else if(color == "pinkpurple")
            return pinkPurpleBall;
        else if(color == "purpleorange")
            return purpleOrangeBall;
        else if(color == "redwhite")
            return redWhiteBall;
        else if(color == "yellowblue")
            return yellowBlueBall;
        else if(color == "greenredRare")
            return greenredRare;
        else if(color == "orangeredRare")
            return orangeredRare;
        else if(color == "pinkpurpleRare")
            return pinkpurpleRare;
        else if(color == "purplewhiteRare")
            return purplewhiteRare;
        else if(color == "redblackRare")
            return redblackRare;
        else if(color == "redblueRare")
            return redblueRare;
        else if(color == "yellowblackRare")
            return yellowblackRare;
        else if(color == "yelloworangeRare")
            return yelloworangeRare; 
        else if(color == "worldLegendary")
            return worldLegendary;   
        else if(color == "venusLegendary")
            return venusLegendary;
        else if(color == "saturnLegendary")
            return saturnLegendary;
        else if(color == "default")
            return defaultBall;  
        else
            return null;
    }
}
