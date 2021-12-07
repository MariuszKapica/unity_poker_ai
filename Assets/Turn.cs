using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turn : MonoBehaviour
{
    public ThisCard card1;

    public void setTurn(Card Card1){
        card1.card = Card1;
    }
}
