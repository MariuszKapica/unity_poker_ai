using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]

public class Card
{
    public string Rank;
    public string Suit;
    public Material Icon;

    public Card(){

    }

    public Card(string rank, string suit, Material icon){
        Rank = rank;
        Suit = suit;
        Icon = icon;
    }
}
