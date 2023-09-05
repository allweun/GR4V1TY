using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/**
    Bu scriptteki prefs yönetimi şu şekilde çalışmaktadır;
        valueler içerisindeki renk aranır
        Eğer renk varsa aktif sprite belirlenmiş renge çevirilir
        Eğer renk yoksa default olarak beyaz kullanılır
        
    BU YÜZDEN STORE A EKLENEN HER RENK BURAYA DA EKLENMEK ZORUNDADIR.
        -InstanceID's
        --default ball = 13216
        --red ball =  12700
        --empty ball = 12300
        --purple ball = 12290
        --blue ball = 11902
        --green ball = 12328
        --yellow ball = 11770
        --orange ball = 13314

**/


public class currentBall : MonoBehaviour
{
    public GameObject player;
    public Sprite[] allSprites;
    private Sprite currentSprite;
    private string ballID;
    public bool refreshBall;

    void Start()
    {
        if(refreshBall)
            PlayerPrefs.DeleteKey("ballSpriteID");
        
        if(PlayerPrefs.HasKey("ballSpriteID")){
            ballID = PlayerPrefs.GetString("ballSpriteID");
        }else{
            PlayerPrefs.SetString("ballSpriteID", allSprites[0].GetInstanceID().ToString());
            ballID = PlayerPrefs.GetString("ballSpriteID");
            currentSprite = allSprites[0];
            player.GetComponent<SpriteRenderer>().sprite=allSprites[0];
        }
        //Debug.Log(allSprites[0].GetInstanceID());
        //PlayerPrefs.DeleteAll();
        //PlayerPrefs.SetString("ballSpriteID", "13216");
        player.GetComponent<SpriteRenderer>().sprite=currentSprite;
    }
    void Update()
    {
        if(refreshBall)
            PlayerPrefs.DeleteKey("ballSpriteID");
        
        for (int i = 0; i < allSprites.Length; i++)
        {
            if(allSprites[i].GetInstanceID().ToString()==ballID){
                currentSprite=allSprites[i];
                //player.GetComponent<SpriteRenderer>().sprite=currentSprite;
            }
        }
        //Debug.Log(player.GetComponent<SpriteRenderer>().sprite.GetInstanceID().ToString());
    }
    void FixedUpdate(){
        player.GetComponent<SpriteRenderer>().sprite=currentSprite;
        ballID = PlayerPrefs.GetString("ballSpriteID");
        
    }
}
