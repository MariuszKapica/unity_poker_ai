using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class playPokerAgent2 : Agent
{
   [SerializeField] private Flop flopData;
   [SerializeField] private Turn turnData;
   [SerializeField] private River riverData;
   [SerializeField] private Player[] playersData;
   [SerializeField] private Combination[] combinationData;
   [SerializeField] private Gamer gamerData;
   [SerializeField] private Material winMaterial;
   [SerializeField] private Material loseMaterial;
   [SerializeField] private MeshRenderer floorMeshRenderer;
   private int beforeDecisionScore;
   private int afterDecisionScore;
   private int numberOfBets;

   private float value_rank(string Rank){
       if(Rank == "2"){
           return 2f;
       }
       if(Rank == "3"){
           return 3f;
       }
       if(Rank == "4"){
           return 4f;
       }
       if(Rank == "5"){
           return 5f;
       }
       if(Rank == "6"){
           return 6f;
       }
       if(Rank == "7"){
           return 7f;
       }
       if(Rank == "8"){
           return 8f;
       }
       if(Rank == "9"){
           return 9f;
       }
       if(Rank == "10"){
           return 10f;
       }
       if(Rank == "j"){
           return 11f;
       }
       if(Rank == "q"){
           return 12f;
       }
       if(Rank == "k"){
           return 13f;
       }
       if(Rank == "as"){
           return 14f;
       }
       if(Rank == "" || Rank == null){
           return 100f;
       }
       return 0f;
   }

   private float value_suit(string Suit){
       if(Suit == "h"){
           return 1f;
       }
       if(Suit == "d"){
           return 2f;
       }
       if(Suit == "s"){
           return 3f;
       }
       if(Suit == "c"){
           return 4f;
       }
       if(Suit == "" || Suit == null){
           return 100f;
       }
       return 0f;
   }
   
   public override void OnEpisodeBegin(){
       gamerData.score = 50;
       numberOfBets = 0;
       beforeDecisionScore = 0;
       afterDecisionScore = 0;
       for(int i=0; i<playersData.Length; i++){
        playersData[i].bet = 0;
       }
       for(int i=0; i<combinationData.Length; i++){
        combinationData[i].bet = 0;
       }
   }

   public override void CollectObservations(VectorSensor sensor){
       //24 Observations for players
       for(int i=0; i<playersData.Length; i++){
        sensor.AddObservation(value_rank(playersData[i].card1.thisRank));
        sensor.AddObservation(value_suit(playersData[i].card1.thisSuit));
        sensor.AddObservation(value_rank(playersData[i].card2.thisRank));
        sensor.AddObservation(value_suit(playersData[i].card2.thisSuit));
       }
       //10 Observations table
       sensor.AddObservation(value_rank(flopData.card1.thisRank));
       sensor.AddObservation(value_suit(flopData.card1.thisSuit));
       sensor.AddObservation(value_rank(flopData.card2.thisRank));
       sensor.AddObservation(value_suit(flopData.card2.thisSuit));
       sensor.AddObservation(value_rank(flopData.card3.thisRank));
       sensor.AddObservation(value_suit(flopData.card3.thisSuit));
       sensor.AddObservation(value_rank(turnData.card1.thisRank));
       sensor.AddObservation(value_suit(turnData.card1.thisSuit));
       sensor.AddObservation(value_rank(riverData.card1.thisRank));
       sensor.AddObservation(value_suit(riverData.card1.thisSuit));
       
   }
   public override void OnActionReceived(ActionBuffers actions){
        int continueGame = actions.DiscreteActions[0];
        int betPlayer = actions.DiscreteActions[1];
        int betCombination = actions.DiscreteActions[2];
        Debug.Log(actions.DiscreteActions[0]);
        if(continueGame == 1){
            gamerData.gamerAction = "e";
        }else if(gamerData.score>0 && gamerData.gameSection<4 && numberOfBets < 3){
            if(betPlayer == 0){
                gamerData.gamerAction = "z";
                AddReward(1f);
                Debug.Log("Added reward 1");
                numberOfBets++;
            }
            if(betPlayer == 1){
                gamerData.gamerAction = "x";
                AddReward(1f);
                Debug.Log("Added reward 1");
                numberOfBets++;
            }
            if(betPlayer == 2){
                gamerData.gamerAction = "c";
                AddReward(1f);
                Debug.Log("Added reward 1");
                numberOfBets++;
            }
            if(betPlayer == 3){
                gamerData.gamerAction = "v";
                AddReward(1f);
                Debug.Log("Added reward 1");
                numberOfBets++;
            }
            if(betPlayer == 4){
                gamerData.gamerAction = "b";
                AddReward(1f);
                Debug.Log("Added reward 1");
                numberOfBets++;
            }
            if(betPlayer == 5){
                gamerData.gamerAction = "n";
                AddReward(1f);
                Debug.Log("Added reward 1");
                numberOfBets++;
            }
            if(betCombination == 0){
                gamerData.gamerAction = "1";
                AddReward(1f);
                Debug.Log("Added reward 1");
                numberOfBets++;
            }
            if(betCombination == 1){
                gamerData.gamerAction = "2";
                AddReward(1f);
                Debug.Log("Added reward 1");
                numberOfBets++;
            }
            if(betCombination == 2){
                gamerData.gamerAction = "3";
                AddReward(1f);
                Debug.Log("Added reward 1");
                numberOfBets++;
            }
            if(betCombination == 3){
                gamerData.gamerAction = "4";
                AddReward(1f);
                Debug.Log("Added reward 1");
                numberOfBets++;
            }
            if(betCombination == 4){
                gamerData.gamerAction = "5";
                AddReward(1f);
                Debug.Log("Added reward 1");
                numberOfBets++;
            }
            if(betCombination == 5){
                gamerData.gamerAction = "6";
                AddReward(1f);
                Debug.Log("Added reward 1");
                numberOfBets++;
            }
            if(betCombination == 6){
                gamerData.gamerAction = "7";
                AddReward(1f);
                Debug.Log("Added reward 1");
                numberOfBets++;
            }
            if(betCombination == 7){
                gamerData.gamerAction = "8";
                AddReward(1f);
                Debug.Log("Added reward 1");
                numberOfBets++;
            }
            if(betCombination == 8){
                gamerData.gamerAction = "9";
                AddReward(1f);
                Debug.Log("Added reward 1");
                numberOfBets++;
            }
            if(betCombination == 9){
                gamerData.gamerAction = "0";
                AddReward(1f);
                Debug.Log("Added reward 1");
                numberOfBets++;
            }
            beforeDecisionScore = gamerData.score;
        }else{
            gamerData.gamerAction = "e";
        }
        
        if(gamerData.gameSection == 5){
        afterDecisionScore = gamerData.score;
        if (afterDecisionScore >= beforeDecisionScore){
            AddReward(10f);
            Debug.Log("Added reward 10");
        }else{
            AddReward(-10f);
            Debug.Log("Added reward -10");
        }
        if(gamerData.score > 500){
            AddReward(1000f);
            Debug.Log("Added reward 1000");
            gamerData.gamerAction = "e";
            EndEpisode();
        }
        if(gamerData.score == 0){
            AddReward(-100f);
            Debug.Log("Added reward -100");
            gamerData.gamerAction = "e";
            EndEpisode();
        }
        numberOfBets = 0;    
        }
   }    
}
