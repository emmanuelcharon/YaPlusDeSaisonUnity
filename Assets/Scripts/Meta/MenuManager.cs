using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {
   
    public Button startButton;

	void Start () {

        startButton.onClick.RemoveAllListeners();
        startButton.onClick.AddListener(delegate {
            SceneManager.LoadScene(Global.SceneIndex_Main);
        });

        iTween.FadeTo(startButton.gameObject, iTween.Hash(
            "alpha", 0f, "time", 1.5f, 
            "easetype", iTween.EaseType.easeInQuad, 
            "looptype", iTween.LoopType.pingPong));

	}

    void Update()
    {
        if (Input.GetKeyDown("escape"))
            Application.Quit();

    }


}
