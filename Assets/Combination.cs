using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combination : MonoBehaviour
{
    public string givenName;
    public int bet;
    public playPokerAgent gamer;

    void Start(){
        bet = 0;
    }

    public void  AddBet(){
        if (gamer.score >= 5){
            bet = bet + 5;
            gamer.score -=5;
            Debug.Log(("You bet "+ bet + " on " + givenName));
        }
    }
}
