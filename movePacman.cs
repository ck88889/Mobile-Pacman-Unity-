using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class movePacman : MonoBehaviour{
    public float speed;

    private Vector2 startPos, direction;

    //buttons
    public Button pause;

    //get lives 
    private Text livesCounter; 
    private int num_lives = 4; 

    //other sprites 
    private GameObject [] ghosts, dots; 
    private float [,] ghost_coor = {{-3.1f, 3.23f}, {3.16f, 3.23f}, {-3.1f, -3.76f}, {3.14f, -3.76f}}; 

    private GameObject block; 
    private float block_start, block_start2; 

    //keep track of score
    private Text scoreCounter;
    private Direction direction_current;
    private Direction swipe = Direction.stop;  

    void Start(){
        //get lives display
        livesCounter = GameObject.Find("Canvas: Life Display").GetComponent<Text>();

        //get other sprites 
        dots = GameObject.FindGameObjectsWithTag("Point");
        ghosts = GameObject.FindGameObjectsWithTag("Ghost");
        block = GameObject.Find("Border (58) - Barrier"); 

        block_start = block.transform.position.y; 
        block_start2 = block.transform.position.x; 

        //get score text
        scoreCounter = GameObject.Find("Canvas: Score Display").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update(){ 
        //move character with swipes

        //if an user is touching the screen, becomes 1 
        //checks if the user is currently touching the screen 
        if (Input.touchCount > 0){
            //get touch object 
            Touch touch = Input.GetTouch(0);
            
            //calculate direction of swipe 
            switch(touch.phase){
                case TouchPhase.Began: 
                    startPos = touch.position;
                    break; 
                case TouchPhase.Moved:
                    //gets slope of movement 
                    direction = touch.position - startPos; 

                    //checks swipe movements (works like a cartesian plane with a pos x pos y quadrant, pos x neg y quadrant, etc.)
                    //going up 
                    if(direction.x >= 0 && direction.y >= 3){ 
                       swipe = Direction.up; 
                    //going down 
                    }else if(direction.x <= 0 && direction.y <= -3){
                        swipe = Direction.down;

                    //going right
                    }else if(direction.x >= 3 && direction.y <= 0){
                        swipe = Direction.right;

                    //going left
                    }else if(direction.x <= -3 && direction.y >= 0){
                        swipe = Direction.left;
                    }
                    
                    startPos = touch.position;
                    break; 

                case TouchPhase.Ended:
                    direction.x = 0f; 
                    direction.y = 0f;
                    break;
            }
        }

        //pacman movement 
        if(!(GameObject.Find("Border (58) - Barrier").transform.position.y < 0.45f)){
            switch (swipe){
                case Direction.left:
                    transform.position += Vector3.left * speed * Time.deltaTime;
                    break;
                case Direction.right:
                    transform.position += Vector3.right * speed * Time.deltaTime;
                    break; 
                case Direction.up:
                    transform.position += Vector3.up * speed * Time.deltaTime;
                    break; 
                case Direction.down:
                    transform.position += Vector3.down * speed * Time.deltaTime;
                    break; 
            }
        }

    }

    void reset_Game(){
        //reset point dotes
        for(int i = 0; i < dots.Length; i++){
            dots[i].GetComponent<Renderer>().enabled= true;
        }

        //reset ghosts 
        for(int i = 0; i < ghosts.Length; i++){
            ghosts[i].transform.position = new Vector3(ghost_coor[i , 0], ghost_coor[i , 1], transform.position.z);
        }

        //reset pacman
        transform.position = new Vector3(0, 0, transform.position.z);
        transform.rotation = Quaternion.identity; 

        //reset
        block.transform.position = new Vector3(block_start2, block_start, transform.position.z);

        //reset buttons
        pause.GetComponent<Image>().color = Color.white;
    }

    //reset score to zero and lives to 4
    void reset_Score(){
        //reset score
        scoreCounter.text = (string)scoreCounter.text.Substring(0, 9) + " " + 0;

        //reset lives
        num_lives = 4;

        string msg = "\n"; 
        for(int i = 0; i < num_lives; i++){
            msg += "○ ";
        }
        msg += "   "; 

        livesCounter.text = msg;
    }

    void OnCollisionEnter2D(Collision2D collision){
        //controls pacman lives display 
        if (collision.gameObject.tag == "Ghost"){
            //decreases lives
            num_lives -= 1; 

            //displays lives 
            string msg = "\n"; 

            for(int i = 0; i < num_lives; i++){
                msg += "○ ";
            }

            msg += "   "; 
            livesCounter.text = msg; 

            //reset game
            reset_Game(); 
        }

        //reset scores if you run out of lives
        if(num_lives == 0){
            reset_Score(); 
        }

        //stop pacman
        swipe = Direction.stop;
    }
}
