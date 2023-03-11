using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class pacman_buttons : MonoBehaviour{
    //types of movement for game 
    enum Playmode{play, pause}; 
    private Playmode mode = Playmode.play; 

    public Button pause, restart; 

    //get lives 
    private Text livesCounter; 
    private int num_lives = 4; 

    //other sprites 
    private GameObject [] ghosts; 
    private float [,] ghost_current = {{-3.1f, 3.23f}, {3.16f, 3.23f}, {-3.1f, -3.76f}, {3.14f, -3.76f}};
    private float [,] ghost_coor = {{-3.1f, 3.23f}, {3.16f, 3.23f}, {-3.1f, -3.76f}, {3.14f, -3.76f}}; 
    private GameObject [] dots;

    //player
    private GameObject pacman, block; 
    private float block_start, block_start2; 
    private float [] pacman_coor = {0, 0};

    //keep track of score
    private Text scoreCounter;

    // Start is called before the first frame update
    void Start(){
        InvokeRepeating("OutputTime", 0.2f, 0.2f);

        //get lives display
        livesCounter = GameObject.Find("Canvas: Life Display").GetComponent<Text>();

        //get other sprites 
        dots = GameObject.FindGameObjectsWithTag("Point");
        ghosts = GameObject.FindGameObjectsWithTag("Ghost");
        pacman = GameObject.Find("Pacman");
        block = GameObject.Find("Border (58) - Barrier"); 

        block_start = block.transform.position.y; 
        block_start2 = block.transform.position.x; 

        //get score text
        scoreCounter = GameObject.Find("Canvas: Score Display").GetComponent<Text>();
    }

    void OutputTime() {
        if (Input.touchCount > 0){
            Touch touch = Input.GetTouch(0);

            //pause button clicked 
            if(isClicked(touch, pause.transform.position.x, pause.transform.position.y)){
                //change button colours 
                //pause
                if(mode.Equals(Playmode.play)){
                    pause.GetComponent<Image>().color = new Color(0.1f, 0.65f, 1.0f);
                    mode = Playmode.pause;
                //play
                }else{
                    pause.GetComponent<Image>().color = Color.white;
                    mode = Playmode.play;
                } 
            }
        }
    }

    // Update is called once per frame
    void Update(){
        if (Input.touchCount > 0){
            Touch touch = Input.GetTouch(0);
            //restart button clicked
            if(isClicked(touch, restart.transform.position.x, restart.transform.position.y)){
                complete_Reset();
            }

        }

        //pause and un-pause game
        switch(mode){
            //pause game
            case Playmode.pause:
                for(int i = 0; i < ghosts.Length; i++){
                    stop_Movement(ghosts[i], ghost_current[i,0], ghost_current[i,1]);
                }
                stop_Movement(pacman, pacman_coor[0], pacman_coor[1]);
                break;
            //resume game 
            default:
                for(int i = 0; i < ghosts.Length; i++){
                    ghost_current[i,0] = ghosts[i].transform.position.x;
                    ghost_current[i,1] = ghosts[i].transform.position.y;
                }

                pacman_coor[0] = pacman.transform.position.x;
                pacman_coor[1] = pacman.transform.position.y;
                break; 
        }
    }

    //check if the user clicks button
    bool isClicked(Touch touch, float x_pos, float y_pos){
        if(x_pos - 45f <= touch.position.x && x_pos + 45f >= touch.position.x){
            if(y_pos - 45f <= touch.position.y && y_pos + 45f >= touch.position.y){
                return true; 
            }
        }

        return false; 
    }

    void stop_Movement(GameObject obj, float x, float y){
        obj.transform.position = new Vector3(x, y, transform.position.z);
    }

    //reset the game 
    void complete_Reset(){
        reset_Game(); 
        reset_Score();
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
}
