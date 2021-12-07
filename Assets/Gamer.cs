using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gamer : MonoBehaviour
{   
   public int gameSection;
   public int score;
   public string gamerAction;
    // Start is called before the first frame update
    void Start()
    {
        gameSection = 0;
        score = 50;
        gamerAction = "";
    }
}
