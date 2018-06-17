using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour {

    public static int MaxScore = 15;

    public Player player1;
    public Player player2;
    [HideInInspector] public Player activePlayer; // dynamic, either player1 or player2
    public bool playerCanPlay; // false between player turns

    [HideInInspector] public int round; 
    [HideInInspector] public int roundScore; 

    [HideInInspector] public Global.Season roundSeason; 

    public Text roundText;
    public Text roundScorePlayer1;
    public Text roundScorePlayer2;
    public Text roundScoreText;
    public Text gameScorePlayer1;
    public Text gameScorePlayer2;
    public Image black;


    public FruitSpot[] fruitSpots;
    public Text[] fruitNameSpots;
    public GamePrefabs prefabs;
    public NoFruit noFruit;
    public SceneBackground sceneBackground;

    public void Start() {

        black.gameObject.SetActive(true);

        iTween.FadeTo(commentatorModal.gameObject, iTween.Hash(
            "alpha", 0f, "time", 0.1f, "easetype", iTween.EaseType.easeInQuad));


        StartCoroutine(StartGame());
    }

    public IEnumerator StartGame() {

        yield return new WaitForSeconds(2f);

        commentatorModal.gameObject.SetActive(true);


        CommentatorText(new string[] {
            "Bienvenue au Clash des Saisons!"
        }, duration: 4f);

        yield return new WaitForSeconds(1f);
        Global.s.sounds.cheer.Play();
        yield return new WaitForSeconds(1f);
       

        iTween.FadeTo(black.gameObject, iTween.Hash(
           "alpha", 0f, "time", 4.5f, "easetype", iTween.EaseType.linear));
        yield return new WaitForSeconds(4.5f);

        player1.gameScore = 0;
        player2.gameScore = 0;
        roundScore = 0;
        UpdateGameScoresText();

        playerCanPlay = false;
        round = 1;
        StartRound(round, keepRoundScores: false);
    }

    /** 
     * Correspondance between fruit spot (2D game object) 
     * and its name in the canvas (UI)
     */
    Text fruitNameSpot(FruitSpot fruitSpot)
    {
        return fruitNameSpots[System.Array.IndexOf(fruitSpots, fruitSpot)];
    }

    public string activePlayerNumber() { // for UI
        if(activePlayer == player1) {
            return "1";
        } else if (activePlayer == player2) {
            return "2";
        } else {
            return "0";
        }
    }

    public Player otherPlayer(Player p) {
        if (p == player1) {
            return player2;
        }
        else if (p == player2) {
            return player1;
        }
        else {
            Debug.LogError("player " + p);
            return null;
        }
    }

    public void setActivePlayer(Player p) {
        activePlayer = p;
        //p.transform.localScale = 1.1f * p.initialScale;

        //Player op = otherPlayer(p);
        //op.transform.localScale = op.initialScale;
    }

    public Text gameScoreTextForPlayer(Player p)
    {
        if(p == player1) {
            return gameScorePlayer1;
        } else if(p == player2) {
            return gameScorePlayer2;
        } else {
            return null;
        }

    }

	public void UpdateRoundScoresText()
	{
        roundScorePlayer1.text = string.Format("{0}", player1.roundScore);
        roundScorePlayer2.text = string.Format("{0}", player2.roundScore);
        roundScoreText.text = string.Format("{0}", roundScore);
	}

    public void UpdateGameScoresText()
    {
        gameScorePlayer1.text = string.Format("{0}", player1.gameScore);
        gameScorePlayer2.text = string.Format("{0}", player2.gameScore);
    }

    /**
     * When the last move was a succesful "NoFruit" answer, 
     * we keep the scores of this round and cumulate them for the next round!
     * (In this case keepRoundScores is true)
     */ 
    public void StartRound(int roundNumber, bool keepRoundScores) {

        if(keepRoundScores && activePlayer != null) {
            setActivePlayer(otherPlayer(activePlayer));
        } else {
            // the regular case
            setActivePlayer(roundNumber % 2 == 1 ? player1 : player2);
        }

        roundSeason = Global.GetSeasonFromRoundNumber(roundNumber);

        Global.s.sounds.ChangeMusic(roundSeason);

        roundText.text = string.Format("Manche {0}", roundNumber);
       
        sceneBackground.setFocus(roundSeason);

        if(!keepRoundScores) {
            // reset round scores
            player1.roundScore = 0;
            player2.roundScore = 0;
            roundScore = 0;
        }
        UpdateRoundScoresText();

        // reset fruit choices
        foreach(FruitSpot fruitSpot in fruitSpots) {
            if(fruitSpot.currentFruit != null) {
                Destroy(fruitSpot.currentFruit.gameObject);
                fruitSpot.currentFruit = null;
            }
            fruitSpot.fruitsLeft = 1;
            refillFruitSpot(fruitSpot);
        }

        CommentatorText(new string[] {
            string.Format("Manche {0}", roundNumber)//,
            //string.Format("Nous sommes {0} {1}", roundSeason == Global.Season.Spring ? "au" : "en",  Global.SeasonName(roundSeason)),
            //string.Format("Le joueur {0} commence !", activePlayerNumber()),
        });


        playerCanPlay = true;
        activePlayer.SetFocus(true);

    }

	public void playerPickedFruit(FruitGO fruitGO) {
		StartCoroutine (_playerPickedFruit (fruitGO));
	}

	private static float walkTime = 1f;

    public void playerPickedNoFruit()
    {
        StartCoroutine(_playerPickedNoFruit());
    }

    public IEnumerator _playerPickedNoFruit()
    {
        playerCanPlay = false;
        activePlayer.SetFocus(false);

        Vector3 targetPos = new Vector3(
            noFruit.transform.position.x,
            noFruit.transform.position.y,
            activePlayer.transform.position.z
        );

        iTween.MoveTo(activePlayer.gameObject, iTween.Hash(
            "position", targetPos, "time", walkTime, "easetype", iTween.EaseType.linear));
        yield return new WaitForSeconds(walkTime);

        yield return new WaitForSeconds(1f);

        List<FruitSpot> matchingFruitSpots = new List<FruitSpot>();

        foreach(FruitSpot fruitSpot in fruitSpots) {
            if(fruitSpot.currentFruit != null && Global.IsFruitInSeason(fruitSpot.currentFruit.fruitType, roundSeason)) {
                matchingFruitSpots.Add(fruitSpot);
            }
        }

        if(matchingFruitSpots.Count > 0) {
            // Wrong answer! some fruits could have been picked

            Global.s.sounds.boo.Play();

            CommentatorText(new string[]{
                string.Format("Mauvaise réponse! {0} se mange en {1}.",
                              Global.FruitName(matchingFruitSpots[0].currentFruit.fruitType),
                              Global.SeasonName(roundSeason))
            });
            yield return new WaitForSeconds(2.5f);


            Player roundWinner = otherPlayer(activePlayer);


            //for (int i = 0; i < roundWinner.roundScore; i++)
            for (int i = 0; i < roundScore; i++)
            {
                roundWinner.gameScore += 1;
                UpdateGameScoresText();

                GameObject go = gameScoreTextForPlayer(roundWinner).gameObject;

                iTween.ScaleFrom(go, iTween.Hash(
                    "scale", 1.2f * go.transform.localScale,
                    "time", 0.1f,
                    "easetype", iTween.EaseType.linear));
                yield return new WaitForSeconds(0.15f);
            }

            //roundWinner.gameScore += roundWinner.roundScore;

            yield return new WaitForSeconds(1f);

            if (roundWinner.gameScore >= MaxScore) // this may even end the game
            {
                StartCoroutine(EndGame(roundWinner));
            }
            else
            {
                StartCoroutine(EndRound(null, keepRoundScores: false));
            }
        } else {

            // Correct answer! Start next round but keep scores!

            Global.s.sounds.cheer.Play();

            CommentatorText(new string[]{
                string.Format("Bonne réponse!")
                //string.Format("Bonne réponse! Aucun des fruits n'est de saison en {0}",Global.SeasonName(roundSeason))
            });
            yield return new WaitForSeconds(1f);

            activePlayer.roundScore += 2; // bonus points for finding "NoFruit"
            roundScore += 1;

            StartCoroutine(EndRound(null, keepRoundScores: true));

        }


    }

	public IEnumerator _playerPickedFruit(FruitGO fruitGO) {
        playerCanPlay = false;
        activePlayer.SetFocus(false);
        /*
		iTween.RotateTo(activePlayer.gameObject, iTween.Hash(
			"z", -15f, "time", 0.25f, "easetype", iTween.EaseType.easeOutBack
		));

		iTween.RotateTo(activePlayer.gameObject, iTween.Hash(
			"z", 15f, "time", 0.5f, "easetype", iTween.EaseType.easeOutBack, "looptype", iTween.LoopType.pingPong
		));*/
        Vector3 targetPos = new Vector3(
            fruitGO.transform.position.x,
            fruitGO.transform.position.y,
            activePlayer.transform.position.z
        );

		iTween.MoveTo(activePlayer.gameObject, iTween.Hash(
            "position", targetPos, "time", walkTime, "easetype", iTween.EaseType.linear));
        yield return new WaitForSeconds (walkTime);

        FruitSpot fruitSpot = fruitGO.transform.parent.GetComponent<FruitSpot>();
        fruitGO.transform.SetParent(activePlayer.transform);
        fruitGO.transform.localPosition = fruitGO.transform.localPosition - Vector3.forward;
        fruitSpot.currentFruit = null;
        fruitNameSpot(fruitSpot).text = "";

		yield return new WaitForSeconds (1f);

        if (Global.IsFruitInSeason(fruitGO.fruitType, roundSeason)) {
            // Correct answer! Simply end this player's turn (round and game continue)

            Global.s.sounds.cheer.Play();

            activePlayer.roundScore += 1;
            roundScore += 1;

            iTween.MoveTo(activePlayer.gameObject, iTween.Hash(
                "position", activePlayer.initialPosition, "time", walkTime, "easetype", iTween.EaseType.linear));
            yield return new WaitForSeconds(walkTime);
            Destroy(fruitGO.gameObject);

            if (fruitSpot.fruitsLeft > 0)
            {
                fruitSpot.fruitsLeft -= 1;
                refillFruitSpot(fruitSpot);

            } else {
                fruitNameSpot(fruitSpot).text = "Epuise";
            }

            UpdateRoundScoresText();

            setActivePlayer(otherPlayer(activePlayer));

            CommentatorText(new string[] {
                string.Format("Bonne réponse!")
                //string.Format("Bonne réponse! Au tour du joueur {0}", activePlayerNumber())
            });
            yield return new WaitForSeconds(1f);

            playerCanPlay = true;
            activePlayer.SetFocus(true);
        }

        else {
            // Wrong answer !

            Global.s.sounds.boo.Play();

            CommentatorText(new string[]{
                string.Format("Mauvaise réponse! {0} se mange en {1}",
                              Global.FruitName(fruitGO.fruitType),
                              Global.SeasonName(Global.SeasonForFruit(fruitGO.fruitType)))
            });
            yield return new WaitForSeconds(2.5f);

            // this ends the round: other player wins this round
            Player roundWinner = otherPlayer(activePlayer);
            //roundWinner.gameScore += roundWinner.roundScore;


            //for (int i = 0; i < roundWinner.roundScore; i++)
            for (int i = 0; i < roundScore; i++)
            {
                roundWinner.gameScore += 1;
                UpdateGameScoresText();

                GameObject go = gameScoreTextForPlayer(roundWinner).gameObject;

                iTween.ScaleFrom(go, iTween.Hash(
                    "scale", 1.2f * go.transform.localScale,
                    "time", 0.1f,
                    "easetype", iTween.EaseType.linear));
                yield return new WaitForSeconds(0.15f);
            }


            if(roundWinner.gameScore >= MaxScore) // this may even end the game
            {
                StartCoroutine(EndGame(roundWinner));
            } else 
            {
                StartCoroutine(EndRound(fruitGO, keepRoundScores: false));
            }
        }
	}

    public void refillFruitSpot(FruitSpot fruitSpot) {
        Global.FruitType fruitType = Global.getRandomFruitBiased(roundSeason);
        GameObject newFruit = Instantiate(prefabs.FruitPrefab(fruitType), fruitSpot.transform, false);
        fruitSpot.currentFruit = newFruit.GetComponent<FruitGO>();
        fruitNameSpot(fruitSpot).text = Global.FruitName(fruitType);
        fruitSpot.displayFruitsLeft();
    }

    public IEnumerator EndRound(FruitGO fruitGO, bool keepRoundScores) {
        iTween.MoveTo(activePlayer.gameObject, iTween.Hash(
                    "position", activePlayer.initialPosition, "time", walkTime,
                    "easetype", iTween.EaseType.linear));
        yield return new WaitForSeconds(walkTime);

        UpdateGameScoresText();
        if (fruitGO != null) {
            Destroy(fruitGO.gameObject);
        }
        round += 1;
        StartRound(round, keepRoundScores);

    }

    public IEnumerator EndGame(Player winner) {

        UpdateGameScoresText();

        yield return new WaitForSeconds(2f);


        CommentatorText(new string[] {
            string.Format("Le vainqueur est le joueur {0}, avec un score de {1} à {2}!",
            winner == player1 ? "1" : "2",
            winner.gameScore, otherPlayer(winner).gameScore),
        }, duration: 5f);
        yield return new WaitForSeconds(6f);




        Debug.LogFormat("{0} won! Scores: player1: {1}, player2: {2}",
                        winner.name, player1.gameScore, player2.gameScore);
        SceneManager.LoadScene(Global.SceneIndex_Menu);

    }

    public CommentatorModal commentatorModal;


    public void CommentatorText(string[] texts, float duration=1.5f)
    {
        StartCoroutine(_CommentatorText(texts, duration));
    }

    public IEnumerator _CommentatorText(string[] texts, float duration) {

        commentatorModal.mainText.text = texts[0];

        iTween.FadeTo(commentatorModal.gameObject, iTween.Hash(
            "alpha", 1f, "time", 0.5f, "easetype", iTween.EaseType.easeInQuad));


        foreach (string text in texts) {
            commentatorModal.mainText.text = text;
            yield return new WaitForSeconds(duration);
        }

        iTween.FadeTo(commentatorModal.gameObject, iTween.Hash(
            "alpha", 0f, "time", 0.5f, "easetype", iTween.EaseType.easeInQuad));
        yield return new WaitForSeconds(0.5f);
    }



}
