using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class block_motion : MonoBehaviour{
    
    public GameObject pacman; 
    public float speed; 
    public float pac_speed;

    // Update is called once per frame
    void Update(){
        if(transform.position.y < 0.45f){
            transform.position += Vector3.up * speed * Time.deltaTime;
            pacman.transform.position += Vector3.up * pac_speed * Time.deltaTime;
        }
    }
}
