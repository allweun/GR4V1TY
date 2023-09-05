using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LookUp : MonoBehaviour
{
    public Camera cam;
    private float targetZoom;
    private float zoomFactor = -1.5f;
    [SerializeField]private float zoomLerpSpeed = 10f;
    private bool isZoomed=false;
    private bool zoom=true;
    private bool counterFinished=false;
    public Text timer;
    private float counter = 15f;
    public Button look;
    void Start()
    {
        targetZoom = cam.orthographicSize;
    }
    void Update(){
        if(zoom){
            StartCoroutine(waitingforZoom());
        }if(isZoomed){
            StartCoroutine(waitingforPan());
        }
    }
    void LateUpdate(){
        if(counterFinished){
            look.interactable=false;
            timer.gameObject.SetActive(true);
            counter-=Time.deltaTime;
            timer.text=Mathf.CeilToInt(counter).ToString();
            if(Mathf.CeilToInt(counter)==0)
                counterFinished=false;
        }else{
            timer.gameObject.SetActive(false);
            look.interactable=true;
            counter=15f;
        }
    }
    public void LookUP(){
        look.interactable=false;
        targetZoom -= PlayerPrefs.GetFloat("magnify")*zoomFactor;
        zoom = true;
        counterFinished=true;
    }
    IEnumerator waitingforZoom(){
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetZoom, Time.deltaTime*zoomLerpSpeed);
        yield return new WaitForSeconds(1f);
        zoom=false;
        isZoomed=true;
    }
    IEnumerator waitingforPan(){
        yield return new WaitForSeconds(1f);
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, 5f, Time.deltaTime*zoomLerpSpeed);
        targetZoom = cam.orthographicSize;
        isZoomed=false;
        
    }   
}
