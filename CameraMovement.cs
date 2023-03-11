using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraMovement : MonoBehaviour{

    private float field_view, aspect_ratio; 

    private Text scoreCounter, livesCounter;

    private GameObject [] buttons;

    // Start is called before the first frame update
    void Start(){
        //get displays
        scoreCounter = GameObject.Find("Canvas: Score Display").GetComponent<Text>();
        livesCounter = GameObject.Find("Canvas: Life Display").GetComponent<Text>();

        buttons = GameObject.FindGameObjectsWithTag("Control Button");

        //get screen measurement 
        aspect_ratio = Camera.main.aspect;

        //adjust button placement for certain aspect ratio 
        if(aspect_ratio > 0.5f){
            foreach (GameObject i in buttons) {
                i.transform.position = new Vector3(i.transform.position.x - 200f, i.transform.position.y - 100f, i.transform.position.z);
                i.transform.localScale = new Vector3(1.2f, 1.2f, 0);
            }
        }else if(aspect_ratio <= 0.5f && aspect_ratio > 0.48f){
            foreach (GameObject i in buttons) {
                i.transform.position = new Vector3(i.transform.position.x, i.transform.position.y - 50f, i.transform.position.z);
            }
        }else if(aspect_ratio <= 0.48f && aspect_ratio >= 0.3f){
            foreach (GameObject i in buttons) {
                i.transform.position = new Vector3(i.transform.position.x, i.transform.position.y + 50f, i.transform.position.z);
            }
        }
    }

    // Update is called once per frame
    void Update(){
        //change size of camera to aspect 
        if(aspect_ratio > 0.5f){
            field_view = 65f; 
        }else if(aspect_ratio <= 0.5f && aspect_ratio > 0.48f){
            field_view = 70f; 

            //adjust font size and line spacing for screen size 
            scoreCounter.fontSize = 18;
            scoreCounter.lineSpacing = 5.5f; 

            livesCounter.fontSize = 20; 
            livesCounter.lineSpacing = 4.8f;
        }else if(aspect_ratio <= 0.48f && aspect_ratio >= 0.3f){
            field_view = 80f; 

            //adjust font size and line spacing for screen size 
            scoreCounter.fontSize = 18;
            scoreCounter.lineSpacing = 5.5f; 

            livesCounter.fontSize = 20; 
            livesCounter.lineSpacing = 4.8f;
        }

        Camera.main.fieldOfView = field_view;
    }
}
