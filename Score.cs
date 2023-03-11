using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour{
    //types of modes for player 
    enum Playmode{on, off, pause};
    
    //keep track and update the score 
    private Text scoreCounter;
    private string [] score = new string[2];
    private string full_scoreline;
    private int num_score; 

    //array of Pacman dots and sprites
    private GameObject [] dots, ghosts; 
    private GameObject player;

    //retrieve the current score 
    void get_Values(){
        //split string into two parts 
        full_scoreline = (string)scoreCounter.text; 
        score[0] = full_scoreline.Substring(0, 9); 
        score[1] = full_scoreline.Substring(9); 

        //convert the string portion into a numeric value
        int.TryParse(score[1], out num_score);
    }

    //called at the beginning of the program 
    void Start(){
        //get text display
        scoreCounter = GameObject.Find("Canvas: Score Display").GetComponent<Text>();

        //get array of pacman dot points
        dots = GameObject.FindGameObjectsWithTag("Point");
        ghosts = GameObject.FindGameObjectsWithTag("Ghost");

        //get player 
        player = GameObject.Find("Pacman"); 

        foreach(GameObject i in dots){
            i.transform.localScale = new Vector3(1.2f, 1.2f, 0);
        }
    }

    // Update is called once per frame
    void Update(){
        //get current score 
        get_Values();

        //score for pacman
        for(int i = 0; i < dots.Length; i++){
            //checks if the pacman hits a point 
            if(dots[i].transform.position.x - 0.1 <= player.transform.position.x && dots[i].transform.position.x + 0.1 >= player.transform.position.x){
                if(dots[i].transform.position.y - 0.1 <= player.transform.position.y && dots[i].transform.position.y + 0.1 >= player.transform.position.y){
                    //control score 
                    if(dots[i].GetComponent<Renderer>().enabled == true){
                        //get rid of dot 
                        dots[i].GetComponent<Renderer>().enabled= false;

                        //add to score
                        num_score += 1;
                        scoreCounter.text = score[0] + " " + num_score;

                        break; 
                    }
                }
            }
        }
    }
}
