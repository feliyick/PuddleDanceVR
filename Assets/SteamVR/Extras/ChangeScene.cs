using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChangeScene : MonoBehaviour
{

    public Text scoreText;

    // Start is called before the first frame update
    void Start()
    {
        int score = ScoreScript.Score;
        //score = PlayerPrefs.GetInt("score");
        scoreText.text = "Your Final Score is: " + score.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // public void PointerClick(object sender, PointerEventArgs e)
    // {
    //     if (e.target.name == "Button")
    //     {
    //         Debug.Log("Button was clicked");
    //         MoveToScene(1);
    //     }
    // }

    public void MoveToScene(int sceneID){
        SceneManager.LoadScene(sceneID);
    }
}
