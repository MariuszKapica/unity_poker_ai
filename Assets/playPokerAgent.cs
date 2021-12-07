using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class playPokerAgent : Agent
{
   [SerializeField] private Deck deck_creator;
   [SerializeField] private List<Card> deck;
   [SerializeField] private Flop flopData;
   [SerializeField] private Turn turnData;
   [SerializeField] private River riverData;
   [SerializeField] private Player[] playersData;
   [SerializeField] private Combination[] combinationData;
   [SerializeField] private Material winMaterial;
   [SerializeField] private Material loseMaterial;
   [SerializeField] private MeshRenderer floorMeshRenderer;
   private int gameSection;
   public int score;
   int topCard = 0;
   public (Player, string) won = (null, "");

   void Start(){
        gameSection = 0;
        score = 50;
        deck = deck_creator.CreateDeck();
   }

   public override void OnEpisodeBegin(){
       score = 50;
       for(int i=0; i<playersData.Length; i++){
        playersData[i].bet = 0;
       }
       for(int i=0; i<combinationData.Length; i++){
        combinationData[i].bet = 0;
       }
   }

   private void Update(){
        if (gameSection == 0){
            foreach(Player player in playersData){
                Card card1 = deck[topCard];
                topCard++;
                Card card2 = deck[topCard];
                topCard++;
                player.setCards(card1, card2);
            }
            gameSection++;
        }else if(gameSection == 1){
            Card card1 = deck[topCard];
            topCard++;
            Card card2 = deck[topCard];
            topCard++;
            Card card3 = deck[topCard];
            topCard++;
            flopData.setFlop(card1, card2, card3);
            gameSection++;
        }else if(gameSection == 2){
            Card card1 = deck[topCard];
            topCard++;
            turnData.setTurn(card1);
            gameSection++;
        }else if(gameSection == 3){
            Card card1 = deck[topCard];
            topCard++;
            riverData.setRiver(card1);
            gameSection++;
        }else if(gameSection == 4){
            //Check combinations save results to list
            List<(List<int>, string, Player)> results = new List<(List<int>, string, Player)>();
            foreach(Player player in playersData){
               (List<int>, string, Player) result= check_player_combination(flopData, turnData, riverData, player);
               results.Add(result);
            }
            
            //Decide who won
            int winner = 0;
            for(int i=0; i<results.Count; i++){
                if(winner != i){
                    if(results[winner].Item1[0]<results[i].Item1[0]){
                       winner = i;
                    }
                    if(results[winner].Item1[0]==results[i].Item1[0]){
                        if(results[winner].Item1.Count == results[i].Item1.Count){
                                for(int k=1; k<results[i].Item1.Count; k++){
                                if(results[winner].Item2 == "pair" || results[winner].Item2 == "double_pair"){
                                    if(results[winner].Item1[k]<results[i].Item1[k] && results[winner].Item1[k]>1 && results[i].Item1[k]>1){
                                        winner = i;
                                    }
                                if(results[winner].Item2 == "triple"){
                                    if(results[winner].Item1[k]<results[i].Item1[k] && results[winner].Item1[k]>2 && results[i].Item1[k]>2){
                                        winner = i;
                                    }
                                }
                                if(results[winner].Item2 == "quads"){
                                    if(results[winner].Item1[k]<results[i].Item1[k] && results[winner].Item1[k]>3 && results[i].Item1[k]>3){
                                        winner = i;
                                    }
                                }
                                if(results[winner].Item2 == "full_house"){
                                    if(results[winner].Item1[k]<results[i].Item1[k] && results[winner].Item1[k]>2 && results[i].Item1[k]>2){
                                        winner = i;
                                    }
                                    if(results[winner].Item1[k]==results[i].Item1[k] && results[winner].Item1[k]>2 && results[i].Item1[k]>2){
                                        if(results[winner].Item1[k]<results[i].Item1[k] && results[winner].Item1[k]>1 && results[i].Item1[k]>1){
                                        winner = i;
                                        }
                                    }
                                }
                                if(results[winner].Item2 == "straight" || results[winner].Item2 == "flush" || results[winner].Item2 == "royal_flush" ||results[winner].Item2 == "high_card"){
                                    if(results[winner].Item1[k]<results[i].Item1[k] && results[winner].Item1[k]>0 && results[i].Item1[k]>0){
                                        winner = i;
                                    }
                                }
                                }
                        }
                        
                        }
                    }
                }
            }

            Debug.Log(("The winner is player ", results[winner].Item3, "by having ", results[winner].Item2));
            won = (results[winner].Item3, results[winner].Item2);
            foreach(Player player in playersData){
                if (player == results[winner].Item3){
                    if(player.bet>0){
                        score += (player.bet*2);
                        AddReward(player.bet*2);
                        Debug.Log(("Added reward ", player.bet*2));
                        player.bet = 0;
                    }
                }else if(player != results[winner].Item3 && player.bet>0){
                    AddReward(-player.bet);
                    Debug.Log(("Added reward - wrong bet ", -player.bet));
                    player.bet = 0;
                }
                
            }
            foreach(Combination combination in combinationData){
                if(combination.bet > 0 && combination.givenName == results[winner].Item2){
                    score += (combination.bet*2);
                    AddReward(combination.bet*2);
                    Debug.Log(("Added reward ", combination.bet*2));
                    combination.bet = 0;
                }else if (combination.bet > 0 && combination.givenName != results[winner].Item2){
                    AddReward(-combination.bet);
                    Debug.Log(("Added reward - wrong bet ", -combination.bet));
                    combination.bet = 0;
                }
            }
        gameSection++;
        }else if(gameSection == 5){
            //Reset Game
            if(score > 500){
                AddReward(1000f);
                Debug.Log("Added reward 1000");
                EndEpisode();
            }
            if(score == 0){
                AddReward(-50f);
                Debug.Log("Added reward -50");
                EndEpisode();
            }
            Card card1 = new Card(null, null, null);
            foreach(Player player in playersData){
                player.setCards(card1, card1);
            }
            flopData.setFlop(card1, card1, card1);
            turnData.setTurn(card1);
            riverData.setRiver(card1);
            gameSection = 0;
            topCard = 0;
            won = (null, "");
            deck = deck_creator.shuffle(deck);
            deck = deck_creator.shuffle(deck);
            deck = deck_creator.shuffle(deck);
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
        if(score>0 && gameSection<4){
            if(actions.DiscreteActions[0]==1){
                if(actions.ContinuousActions[0] > 0.9){
                    playersData[0].AddBet();
                }
                if(actions.ContinuousActions[1] > 0.9){
                   playersData[1].AddBet();
                }
                if(actions.ContinuousActions[2] > 0.9){
                   playersData[2].AddBet();
                }
                if(actions.ContinuousActions[3] > 0.9){
                   playersData[3].AddBet();
                }
                if(actions.ContinuousActions[4] > 0.9){
                   playersData[4].AddBet();
                }
                if(actions.ContinuousActions[5] > 0.9){
                   playersData[5].AddBet();
                }
                if(actions.ContinuousActions[6] > 0.9){
                    combinationData[0].AddBet();
                }
                if(actions.ContinuousActions[7] > 0.9){
                    combinationData[1].AddBet();
                }
                if(actions.ContinuousActions[8] > 0.9){
                    combinationData[2].AddBet();
                }
                if(actions.ContinuousActions[9] > 0.9){
                    combinationData[3].AddBet();
                }
                if(actions.ContinuousActions[10] > 0.9){
                    combinationData[4].AddBet();
                }
                if(actions.ContinuousActions[11] > 0.9){
                    combinationData[5].AddBet();
                }
                if(actions.ContinuousActions[12] > 0.9){
                    combinationData[6].AddBet();
                }
                if(actions.ContinuousActions[13] > 0.9){
                    combinationData[7].AddBet();
                }
                if(actions.ContinuousActions[14] > 0.9){
                    combinationData[8].AddBet();
                }
                if(actions.ContinuousActions[15] > 0.9){
                    combinationData[9].AddBet();
                }
            }
        }
   }

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

   public (List<int>, string, Player) check_player_combination(Flop flop, Turn turn, River river, Player player){
        bool high_card = false;
        bool pair = false;
        bool double_pair = false;
        bool triple = false;
        bool straight = false;
        bool flush = false;
        bool full_house = false;
        bool quads = false;
        bool royal_flush = false;
        bool poker = false;

        List<int> score = new List<int>();
        score.Add(0);
        string combination_name = "";

        List<ThisCard> all_7_cards = new List<ThisCard>();
        all_7_cards.Add(player.card1);
        all_7_cards.Add(player.card2);
        all_7_cards.Add(flop.card1);
        all_7_cards.Add(flop.card2);
        all_7_cards.Add(flop.card3);
        all_7_cards.Add(turn.card1);
        all_7_cards.Add(river.card1);
    
        //Check if flush
        int s = 0;
        int c = 0;
        int d = 0;
        int h = 0;
        for(int i=0; i<all_7_cards.Count; i++){
            if(all_7_cards[i].thisSuit == "s"){s++;}
            if(all_7_cards[i].thisSuit == "c"){c++;}
            if(all_7_cards[i].thisSuit == "d"){d++;}
            if(all_7_cards[i].thisSuit == "h"){h++;}
            if(s>=5 || c>=5 || d>=5 || h>=5){
                flush = true;
                if (score[0]<5){
                    score[0] =5;
                }
            }
        }

        List<int> count_cards = new List<int>();
        for(int i=0; i<14; i++){
            count_cards.Add(0);
        }
        foreach(ThisCard card in all_7_cards){
            if(card.thisRank == "2"){count_cards[0]++;}
            if(card.thisRank == "3"){count_cards[1]++;}
            if(card.thisRank == "4"){count_cards[2]++;}
            if(card.thisRank == "5"){count_cards[3]++;}
            if(card.thisRank == "6"){count_cards[4]++;}
            if(card.thisRank == "7"){count_cards[5]++;}
            if(card.thisRank == "8"){count_cards[6]++;}
            if(card.thisRank == "9"){count_cards[7]++;}
            if(card.thisRank == "10"){count_cards[8]++;}
            if(card.thisRank == "j"){count_cards[9]++;}
            if(card.thisRank == "q"){count_cards[10]++;}
            if(card.thisRank == "k"){count_cards[11]++;}
            if(card.thisRank == "as"){count_cards[12]++;}
        }

        int straight_count = 0;
        for(int i=0; i<count_cards.Count; i++){
            if(count_cards[i] == 2 && pair == true){
                double_pair = true;
                if (score[0]<2){
                    score[0] = 2;
                    }
                }
            if(count_cards[i] == 4){
                quads = true;
                if(score[0]<7){
                    score[0] = 7;
                    }
                }
            if(count_cards[i] == 3){
                triple = true;
                if(score[0]<3){
                    score[0] = 3;
                    }
                }
            if(count_cards[i] == 2){
                pair = true;
                if(score[0]<1){
                    score[0] = 1;
                    }
                }
            if (straight_count == 5){
                straight = true;
                if(score[0]<4){
                    score[0] = 4;
                    }
                }
            if(count_cards[i] >= 1){
                straight_count++;
            }else{
                straight_count = 0;
            }
            if(triple==true && pair==true){
                full_house = true;
                if(score[0]<6){
                    score[0] = 6;
                }
            }
            if(straight == true && flush == true){
                royal_flush = true;
                if (score[0]<8){
                    score[0] = 8;
                }
                if (count_cards[12]==1 && count_cards[11]==1 && count_cards[10]==1 && count_cards[9]==1 && count_cards[8]==1){
                    poker = true;
                    if (score[0]<9){
                        score[0] =  9;
                    }
                }
            }
            if (score[0] == 0){
                high_card = true;
            }

        
        }
        
        for(int p=12; p>0; p--){
            if(count_cards[p]>0){
                score.Add(count_cards[p]);
            }
        }
         
        if(high_card==true){combination_name="high_card";}
        if(pair==true){combination_name="pair";}
        if(double_pair==true){combination_name="double_pair";}
        if(triple==true){combination_name="triple";}
        if(straight==true){combination_name="straight";}
        if(flush==true){combination_name="flush";}
        if(full_house==true){combination_name="full_house";}
        if(quads==true){combination_name="quads";}
        if(royal_flush==true){combination_name="royal_flush";}
        if(poker==true){combination_name="poker";}


        return (score, combination_name, player);
        }
   }
