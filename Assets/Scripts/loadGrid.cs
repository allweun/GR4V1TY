using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class loadGrid : MonoBehaviour
{
    public GameObject loaderCanvas;
    //public GameObject loaderPanel;
    Animator transition;
    public float transitionTime=0.0f;
    public MazeGenerator loader;
    public GameObject levelEnder;
    private Animator LevelEndAnim;
    public bool DeleteAllPrefs = false;
    void Start(){

        if(DeleteAllPrefs){
            PlayerPrefs.SetInt("currentLevel",0);
        }
        LevelEndAnim = levelEnder.GetComponent<Animator>();
        //transition = loaderPanel.GetComponent<Animator>();
        if(!PlayerPrefs.HasKey("currentLevel"))
            PlayerPrefs.SetInt("currentLevel",0);
        loader.NextLevel();
        PlayerPrefs.SetInt("maxCoin",GameObject.FindGameObjectsWithTag("Coin").Length);
    }
    void Update()
    {
        if(PlayerPrefs.GetInt("isLevelEnded")==1){
            PlayerPrefs.SetInt("LevelIndex",(PlayerPrefs.GetInt("LevelIndex")+1));
            LoadGrid();
            PlayerPrefs.SetInt("isLevelEnded",0);
        }
        if(DeleteAllPrefs){
            PlayerPrefs.DeleteAll();
        }
        PlayerPrefs.SetInt("maxCoin",GameObject.FindGameObjectsWithTag("Coin").Length);
    }
    void LoadGrid(){
        //loaderCanvas.SetActive(true);
        
        loader.NextLevel();
        //loaderCanvas.SetActive(false);
        LevelEndAnim.SetBool("levelEnd",false);
    }
}
