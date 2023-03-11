using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pacman_animation : MonoBehaviour{
    enum Loop{forward, backward}; 
    enum Direction{up, down, right, left};

    //moving the pacman mouth up and down 
    public Sprite [] spriteArray; 
    public SpriteRenderer spriteRenderer; 

    private int idx = 0; 
    private Loop currentLoop = Loop.forward; 

    //track movement of sprite
    private Vector2 startPos, direction; 
    private Direction currentPac = Direction.right;

    //rotation of sprite  
    public float rotate_speed; 

    // Start is called before the first frame update
    void Start(){
        InvokeRepeating("OutputTime", 0.1f, 0.1f);
    }

    void OutputTime(){
        //go through the slides in a loop that goes back and forth 
        if(currentLoop.Equals(Loop.forward)){
            idx += 1;
        }else{
            idx -= 1;
        }

        //go forward then backwards, vis versa, etc. 
        switch(idx){
            case 0:
                currentLoop = Loop.forward; 
                break; 
            case 4:
                currentLoop = Loop.backward;
                break; 
        }

        spriteRenderer.sprite = spriteArray[idx];
    }

    //rotate towards whatever direction the player is going in 
    void Rotation(float target){
        float angle = Mathf.MoveTowardsAngle(transform.eulerAngles.z, target, rotate_speed * Time.deltaTime);
        transform.eulerAngles = new Vector3(0, 0, angle);
    }

    // Update is called once per frame
    void Update(){

        if (Input.touchCount > 0){
            //get touch object 
            Touch touch = Input.GetTouch(0);
            
            //calculate direction of swipe 
            switch(touch.phase){
                case TouchPhase.Began: 
                    startPos = touch.position;
                    break; 
                case TouchPhase.Moved:
                    direction = touch.position - startPos; 

                    //going up 
                    if(direction.x >= 0 && direction.y >= 3){ 
                        currentPac = Direction.up;
                    //going down 
                    }else if(direction.x <= 0 && direction.y <= -3){
                        currentPac = Direction.down; 

                    //going right
                    }else if(direction.x >= 3 && direction.y <= 0){
                        currentPac = Direction.right; 
                    //going left
                    }else if(direction.x <= -3 && direction.y >= 0){
                        currentPac = Direction.left; 
                    }
                    
                    startPos = touch.position;
                    break; 

                case TouchPhase.Ended:
                    direction.x = 0f; 
                    direction.y = 0f;
                    break;
            }
        }

        //make the sprite rotate 
        switch (currentPac){
            case Direction.left:
                Rotation(180f); 
                break;
            case Direction.right:
                Rotation(0f); 
                break; 
            case Direction.up:
                Rotation(90f); 
                break; 
            case Direction.down:
                Rotation(270f); 
                break; 
        }
    }
}
