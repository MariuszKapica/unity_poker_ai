using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public ThisCard card1;
    public ThisCard card2;
    public int score;
    public int bet;
    public playPokerAgent gamer;

    public void setCards(Card Card1, Card Card2){
         card1.card = Card1;
         card2.card = Card2;
    }

    void Start(){
        bet = 0;
    }
    public void  AddBet(){
        if (gamer.score >= 5){
                    bet += 5;
                    gamer.score -=5;
                    Debug.Log(("You bet "+ bet +" on " + card1.thisRank + card1.thisSuit + card2.thisRank +card2.thisSuit));
        }
    }
}
