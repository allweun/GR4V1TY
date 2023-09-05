using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{

    [SerializeField(), Range(0, 50)]
    public int maxSpeed = 0;

    [SerializeField(), Range(0, 10)]
    public int gravityMultiplier = 4;

    private Rigidbody2D body;

    private bool fallingVertical = true;
    private bool fallingHorizontal = false;

    private bool hitEdge = false;
    private bool hitSoundControl = true;
    public Transform hitPosition;
    public float hitRange;
    public GameObject hitParticle;
    public LayerMask maze;

    public bool isVelocity = false;
    private Transform startPos;
    
    private bool playFallSound = false;   

    public GameObject levelEndPanel;
    Animator levelEndAnim;
    public MenuMaster menuMaster;
    public Text levelCounter;
    public GameObject TutorialPanel;

    void Awake()
    {
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 0;
        body = GetComponent<Rigidbody2D>();
        levelEndAnim = levelEndPanel.GetComponent<Animator>();
        
        startPos = body.transform;
        //Physics2D.gravity = new Vector2(0, Physics2D.gravity.y * gravityMultiplier);
        if(!PlayerPrefs.HasKey("LevelIndex")){
            PlayerPrefs.SetInt("LevelIndex", 0);
        }
        if(!PlayerPrefs.HasKey("isLevelEnded0"))
            PlayerPrefs.SetInt("isLevelEnded0",0);
        //if(PlayerPrefs.GetInt("tutorialPassed")!=1){
        //    TutorialPanel.SetActive(true);
        //}
    }

    void Start()
    {
        if(PlayerPrefs.GetInt("LevelIndex")==0){
            levelCounter.text="0";
        }
        else{
            levelCounter.text=PlayerPrefs.GetInt("LevelIndex").ToString();
        }
        startPos.position = body.transform.position;
    }

    void FixedUpdate()
    {
        if(PlayerPrefs.GetInt("LevelIndex")==0){
            levelCounter.text="0";
        }
        else{
            levelCounter.text=PlayerPrefs.GetInt("LevelIndex").ToString();
        }
        hitEdge = Physics2D.OverlapCircle(hitPosition.position, hitRange, maze);
        if(body.velocity.magnitude >= 5){
            isVelocity=true;
            //playFallSound = true;
        }else{
            isVelocity=false;
            playFallSound = false;
            FindObjectOfType<AudioManager>().Stop("Fall");
        }
    }

    void Update()
    {
        body.velocity = new Vector2(Mathf.Clamp(body.velocity.x, -maxSpeed, maxSpeed), Mathf.Clamp(body.velocity.y, -maxSpeed, maxSpeed));
        if(fallingVertical){
            body.velocity = new Vector2(0, body.velocity.y);
        }
        if(fallingHorizontal){
            body.velocity = new Vector2(body.velocity.x, 0);
        }
        if(Input.GetKeyDown(KeyCode.LeftArrow)){
            TurnLeft();
        }
        if(Input.GetKeyDown(KeyCode.RightArrow)){
            TurnRight();
        }
        if(hitEdge && hitSoundControl && isVelocity){
            if(PlayerPrefs.GetInt("SFX") == 1){
                FindObjectOfType<AudioManager>().Play("Hit");
                Instantiate(hitParticle, hitPosition.position, Quaternion.identity);
                playFallSound = false;
                FindObjectOfType<AudioManager>().Stop("Fall");
            }
            hitSoundControl = false;
        }
        if(playFallSound){
            FindObjectOfType<AudioManager>().Play("Fall");
            playFallSound = false;
        }
    }
    
    public void TurnLeft(){
        if(PlayerPrefs.GetInt("SFX") == 1){
            FindObjectOfType<AudioManager>().Play("Turn");
        }
        body.velocity = new Vector2(0, 0);
        Vector2 tempGravity = Physics2D.gravity;
        if(tempGravity.x == 0 && tempGravity.y > 0){
            Physics2D.gravity = new Vector2(Mathf.Abs(tempGravity.y), 0);
        }
        if(tempGravity.x > 0 && tempGravity.y == 0){
            Physics2D.gravity = new Vector2(0, Mathf.Abs(tempGravity.x) * -1);
        }
        if(tempGravity.x == 0 && tempGravity.y < 0){
            Physics2D.gravity = new Vector2(Mathf.Abs(tempGravity.y) * -1, 0);
        }
        if(tempGravity.x < 0 && tempGravity.y == 0){
            Physics2D.gravity = new Vector2(0, Mathf.Abs(tempGravity.x));
        }
        transform.Rotate(Vector3.forward * -90);
        fallingVertical = !fallingVertical;
        fallingHorizontal = !fallingHorizontal;
        StartCoroutine(hitSoundDeadZone());
    }

    public void TurnRight(){
        if(PlayerPrefs.GetInt("SFX") == 1){
            FindObjectOfType<AudioManager>().Play("Turn");
        }
        body.velocity = new Vector2(0, 0);
        Vector2 tempGravity = Physics2D.gravity;
        if(tempGravity.x == 0 && tempGravity.y > 0){
            Physics2D.gravity = new Vector2(Mathf.Abs(tempGravity.y) * -1, 0);
        }
        if(tempGravity.x > 0 && tempGravity.y == 0){
            Physics2D.gravity = new Vector2(0, Mathf.Abs(tempGravity.x));
        }
        if(tempGravity.x == 0 && tempGravity.y < 0){
            Physics2D.gravity = new Vector2(Mathf.Abs(tempGravity.y), 0);
        }
        if(tempGravity.x < 0 && tempGravity.y == 0){
            Physics2D.gravity = new Vector2(0, Mathf.Abs(tempGravity.x) * -1);
        }
        transform.Rotate(Vector3.forward * 90);
        fallingVertical = !fallingVertical;
        fallingHorizontal = !fallingHorizontal;
        StartCoroutine(hitSoundDeadZone());
    }

    private IEnumerator hitSoundDeadZone(){
        yield return new WaitForSeconds(0.005f);
        hitSoundControl = true;
    }    
    IEnumerator Dead(){
        yield return new WaitForSeconds(2.2f);
        body.gameObject.SetActive(true);
    }
    void OnTriggerEnter(Collider collider){
        if(collider.tag == "trap"){
            body.gameObject.SetActive(false);
            body.transform.position = startPos.position; 
            StartCoroutine(Dead());
        }
    }
    void OnTriggerEnter2D(Collider2D coll){
        if(coll.tag=="End"){
            PlayerPrefs.SetInt("rewardedCoin",PlayerPrefs.GetInt("currentCoins"));
            levelEndAnim.SetBool("levelEnd",true);
            PlayerPrefs.SetInt("isLevelEnded0",1);
            menuMaster.GameUI(false);
    }}
    void OnDrawGizmos(){
        Gizmos.color = Color.gray;
        Gizmos.DrawWireSphere(hitPosition.position, hitRange);
    }
}
