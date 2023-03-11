using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//use because there's a random class in System and UnityEngine 
using Random = System.Random; 

/**
binary key for a ghost's availble directions
    0b0001 -> right 1
    0b0010 -> left 2
    0b0100 -> up 4
    0b1000 -> down 8
**/

//directions
public enum Direction{up, down, right, left, stop};

//controls intersections in maze and choose ghost movement 
public class Intersection{ 
    private List<Direction> paths = new List<Direction>(); 
    private double y_coor; 
    private double x_coor; 

    //constructor
    public Intersection(double x, double y, int dir){
        x_coor = x; 
        y_coor = y;

        // check 1st digit (right)
        if((dir & ~0b1110) == 0x1){ 
            paths.Add(Direction.right); 
        }
        // check if 2nd digit (left)
        if(((dir & ~0b1101) >> 0x1) == 0x1){
            paths.Add(Direction.left); 
        }
        // check if 3rd digit (up)
        if(((dir & ~0b1011) >> 0x2) == 0x1){
            paths.Add(Direction.up); 
        }
        // check if 4th digit (down)
        if(((dir & ~0b0111) >> 0x3) == 0x1){
            paths.Add(Direction.down); 
        }
    }

    //checks if the direction is contained in the list 
    public bool contain_Dir(Direction dir){
        for(int i = 0; i < paths.Count; i++){
            if(paths[i].Equals(dir)){
                return true; 
            }
        }
        return false; 
    }

    //generate a number between 0 and 100
    private int generate_Chance(){
        Random rand = new Random();
        return rand.Next(100);
    }

    private Direction find_Reverse(Direction current){
        switch(current){
            case Direction.up:
                return Direction.down;
            case Direction.down:
                return Direction.up;
            case Direction.right:
                return Direction.left;
            default:
                return Direction.right;
        }
    }

    private float get_Highest(float num_1, float num_2){
        float smallest = Mathf.Max(Mathf.Abs(num_1), Mathf.Abs(num_2));

        if(smallest/num_1 == 1 || smallest/num_1 == -1){
            return num_1; 
        }else{
            return num_2; 
        }
    }

    private float get_Lowest(float num_1, float num_2){
        float smallest = Mathf.Min(Mathf.Abs(num_1), Mathf.Abs(num_2));

        if(smallest/num_1 == 1 || smallest/num_1 == -1){
            return num_1; 
        }else{
            return num_2; 
        }
    }

    //get random direction for ghost movement 
    public Direction random_Movement(Direction current){
        Random rand = new Random();
        Direction choice = paths[(int)rand.Next(paths.Count)]; 

        //60% chance of the ghost moving in its same direction 
        if(contain_Dir(current)){
            if(generate_Chance() < 60){
                return current; 
            }
        }

        //stop ghosts from moving back and forth
        if(contain_Dir(find_Reverse(current))){
            while(choice.Equals(find_Reverse(current))){
                choice = paths[(int)rand.Next(paths.Count)];
            }
        }

        return choice; 
    }

    //close the farthest distance between pacman and the ghost 
    public Direction targeted_Movement(Direction current, GameObject pacman, GameObject ghost){
        //vector between pacman and ghost
        Vector2 distance = pacman.transform.position - ghost.transform.position;
        Direction chosen; 

        float biggest = get_Highest(distance.x, distance.y); 
        //direction to pacman is in the x direction 
        if(biggest.Equals(distance.x)){
            if(biggest < 0){
                chosen = Direction.left; 
            }else{
                chosen = Direction.right; 
            }
        
        //direction to pacman is in the y direction 
        }else{
            if(biggest < 0){
                chosen = Direction.down; 
            }else{
                chosen = Direction.up; 
            }
        }

        //only start following when the ghost is somewhat close to the pacman 
        if(contain_Dir(chosen) && get_Lowest(distance.x, distance.y) < 0.75f){
            return chosen; 
        }
        
        return random_Movement(current); 
    }

    //get x_coor
    public double get_x(){
        return x_coor; 
    }

    //get y_coor
    public double get_y(){
        return y_coor; 
    }
}

public class ghostMove : MonoBehaviour{    
    //movement speed
    public float speed;

    //current direction and location 
    private Direction current_Dir;
    private double x_coor, y_coor;

    //the player
    public GameObject pacman, ghost; 

    //get the opposite direction 
    Direction reverse(Direction current){
        switch(current){
            case Direction.up:
                return Direction.down;
            case Direction.down:
                return Direction.up;
            case Direction.right:
                return Direction.left;
            default:
                return Direction.right;
        }
    } 

    //stagger when intersections are checked for smoother animation 
    void Start(){ 
         InvokeRepeating("OutputTime", 0.25f, 0.25f);
         x_coor = transform.position.x; 
         y_coor = transform.position.y;

         GhostMove(); 
    }

    //stupid way for dealing with intersections
    Intersection [] paths = {new Intersection(-3.1, 3.23, 0b1001), new Intersection(-1.87, 3.23, 0b1011), new Intersection(-0.38, 3.23, 0b1010), new Intersection(0.385, 3.23, 0b1001), new Intersection(1.878, 3.23, 0b1011), new Intersection(3.16, 3.23, 0b1010), 
                             new Intersection(-3.1, 2.23, 0b101), new Intersection(-1.866, 2.23, 0b1111), new Intersection(-1.12, 2.23, 0b1011), new Intersection(-0.36, 2.23, 0b0111), new Intersection(0.385, 2.23, 0b0111), new Intersection(1.13, 2.23, 0b1011), new Intersection(1.88, 2.23, 0b1111), new Intersection(3.145, 2.23, 0b1110), 
                             new Intersection(-3.1, 1.485, 0b0101), new Intersection(-1.868, 1.485, 0b1110), new Intersection(-1.11, 1.485, 0b0101), new Intersection(-0.38, 1.485, 0b1010), new Intersection(0.37, 1.485, 0b1001), new Intersection(1.13, 1.485, 0b0110), new Intersection(1.885, 1.485, 0b1101), new Intersection(3.15 , 1.485, 0b0110), new Intersection(3.135427 , 1.464284, 0b0110),
                             new Intersection(-1.111, 0.727, 0b1001), new Intersection(-0.37, 0.727, 0b0111), new Intersection(0.37, 0.727, 0b0111), new Intersection(1.13, 0.727, 0b1010),
                             new Intersection(-1.85, 0.01, 0b1111), new Intersection(-1.1, 0.01, 0b1110), new Intersection(1.18, 0.01, 0b1101), new Intersection(1.89, 0.01, 0b1111),
                             new Intersection(-1.111, -0.762, 0b1101), new Intersection(1.111, -0.762, 0b1110),
                             new Intersection(-1.86, -1.525, 0b1111), new Intersection(-1.12, -1.525, 0b0111), new Intersection(-0.387, -1.525, 0b1010), new Intersection(0.38, -1.525, 0b1001), new Intersection(1.14, -1.525, 0b0111), new Intersection(1.88, -1.525, 0b1111), 
                             new Intersection(-3.1, -1.522512, 0b1001), new Intersection(-1.85, -1.5225, 0b1111), new Intersection(-1.1, -1.5225, 0b0111), new Intersection(-0.38, -1.5225, 0b1010), new Intersection(0.38, -1.5225, 0b1001), new Intersection(1.15, -1.5225, 0b0111), new Intersection(1.89, -1.5225, 0b1111), new Intersection(3.15, -1.5225, 0b1010),
                             new Intersection(-3.1, -2.25, 0b0101),new Intersection(-2.59, -2.25, 0b1010),new Intersection(-1.85, -2.25, 0b1101),new Intersection(-1.1, -2.25 , 0b1011),new Intersection(-0.38, -2.25, 0b0111),new Intersection(0.38, -2.25, 0b0111),new Intersection(1.15, -2.25, 0b1011),new Intersection(1.89, -2.25, 0b1110),new Intersection(2.65, -2.25 , 0b1001),new Intersection(3.15, -2.25, 0b0110),
                             new Intersection(-3.05, -3.01, 0b1001), new Intersection(-2.59, -3.01, 0b0101),new Intersection(-1.85, -3.01, 0b0110),new Intersection(-1.1, -3.01 , 0b0101),new Intersection(-0.38, -3.01, 0b1010),new Intersection(0.38, -3.01, 0b1001),new Intersection(1.15, -3.01, 0b0110),new Intersection(1.89, -3.01, 0b0101),new Intersection(2.65, -3.01 , 0b0111), new Intersection(3.15, -3.01, 0b1010),
                             new Intersection(-3.1, -3.76, 0b0101),new Intersection(-0.38, -3.76, 0b0111),new Intersection(0.38, -3.76, 0b0111),new Intersection(3.14, -3.76, 0b0110)}; 

    void OutputTime() {
        GhostMove(); 
    }

    void GhostMove(){
        //control sprite movement
        for(int i = 0; i < paths.Length; i++){
            if(paths[i].get_x() - 0.1 < x_coor && paths[i].get_x() + 0.1 > x_coor){
                if(paths[i].get_y() - 0.1 < y_coor && paths[i].get_y() + 0.1 > y_coor){

                    //only red ghost has targeted motion 
                    if(ghost.name == "Red ghost"){
                        current_Dir = paths[i].targeted_Movement(current_Dir, pacman, ghost);
                    }else{
                        current_Dir = paths[i].random_Movement(current_Dir);
                    }
                    break; 
                }
            }
        }
    }

    // Update is called once per frame
    void Update(){
        //get current location 
        x_coor = transform.position.x; 
        y_coor = transform.position.y; 

        //always upright
        transform.rotation = Quaternion.identity;

        //make the sprite move 
        switch (current_Dir){
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

    //on collision make ghosts bounce off each other 
    void OnCollisionEnter2D(Collision2D collision){
        if (collision.gameObject.name == "Border (58) - Barrier (2)" || collision.gameObject.name == "Border (58) - Barrier (1)" || collision.gameObject.tag == "Ghost"){
            current_Dir = reverse(current_Dir); 
        }
    }
}
