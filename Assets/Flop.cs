using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flop : MonoBehaviour
{
    public ThisCard card1;
    public ThisCard card2;
    public ThisCard card3;

    public void setFlop(Card Card1, Card Card2, Card Card3){
         card1.card = Card1;
         card2.card = Card2;
         card3.card = Card3;
    }
}
