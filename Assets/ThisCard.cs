using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThisCard : MonoBehaviour
{
    public Card card;
    public string thisRank;
    public string thisSuit;
    public Material thisIcon;
    // Start is called before the first frame update

    public ThisCard (Card thisCard){
        card = thisCard;
    }

    // Update is called once per frame
    void Update()
    {
        if(card!=null){
            thisRank = card.Rank;
            thisSuit = card.Suit;
            thisIcon = card.Icon;
            Material thisMaterial = Resources.Load<Material>("Materials/"+thisRank+thisSuit);
            Renderer cardTransform = this.GetComponent<Renderer>();
            cardTransform.material = thisMaterial;
        }else{
            Renderer cardTransform = this.GetComponent<Renderer>();
            cardTransform.material = null;
        }
    }
        
}
