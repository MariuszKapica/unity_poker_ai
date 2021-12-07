using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
    public List<Card> deck = new List<Card>();
    private List<string> suits = new List<string>();
    private List<string> ranks = new List<string>();
    // Start is called before the first frame update
    public List<Card> CreateDeck()
    {
        suits.Add("s");
        suits.Add("c");
        suits.Add("h");
        suits.Add("d");
        ranks.Add("2");
        ranks.Add("3");
        ranks.Add("4");
        ranks.Add("5");
        ranks.Add("6");
        ranks.Add("7");
        ranks.Add("8");
        ranks.Add("9");
        ranks.Add("10");
        ranks.Add("j");
        ranks.Add("q");
        ranks.Add("k");
        ranks.Add("as");
        int i;
        int j;
        for(i=0; i<4; i++){
            for(j=0; j<13; j++){
                Card newCard = new Card();
                newCard.Rank = ranks[j];
                newCard.Suit = suits[i];
                string icon_name = ranks[j] + suits[i];
                newCard.Icon = Resources.Load<Material>("Materials/"+icon_name);
                deck.Add(newCard);
            }
        }
        deck = shuffle(deck);
        deck = shuffle(deck);
        deck = shuffle(deck);
        deck = shuffle(deck);
        deck = shuffle(deck);
        deck = shuffle(deck);
        return deck;
    }

    public List<Card> shuffle(List<Card> a){
        for (int i = 0; i < a.Count - 1; i++){
            int rnd = UnityEngine.Random.Range(0, a.Count - 1);
            Card temp = a[i];
            a[i] = a[rnd];
            a[rnd] = temp;
        }
        return a;
    }
}
