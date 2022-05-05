using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScoreScript : MonoBehaviour
{
   
    public int initialScore = 0;
    public bool storeHighScore = true, allowNegative = true;
    public Text field; // Text field displaying current score
    public Text highScoreField; // Text field displaying high score
    public Text streakField;
    public Text streakField2;

    public static int Score = 0;
    public int HighScore;
    public int correctStreak = 0;
    public int scoreMultiplier = 1;

    void Awake() {
        // Ensure only one instance is running
        // if (instance == null)
        //     instance = this; // Set instance to this object
        // else
        //     Destroy(gameObject); // Kill yo self
        // Make sure the linked references didn't go missing
        if (field == null)
            Debug.LogError("Missing reference to 'field' on <b>SumScoreManager</b> component");
        if (storeHighScore && highScoreField == null)
            Debug.LogError("Missing reference to 'highScoreField' on <b>SumScoreManager</b> component");
    }

    void Start() {
        Reset(); // Ensure score is 0 when object loads
        if (initialScore != 0)
            AddPoints(initialScore);  // Set initial score
        if (storeHighScore) {
            if (PlayerPrefs.HasKey("sumHS")) { 
                // Set high score value and tell manager
                HighScore = PlayerPrefs.GetInt("sumHS");
                UpdatedHS();
            }
            else
                HighScore = 0;
        }
        PlayerPrefs.SetInt("score", Score);

        Updated(); // Set initial score in UI
    }

    /// <summary>Notify this manager of a change in score</summary>
    void Updated () {
        field.text = Score.ToString(); // Post new score to text field
    }

    /// <summary>Notify this manager of a change in high score</summary>
    void UpdatedHS () {
        if(storeHighScore)
            highScoreField.text = HighScore.ToString(); // Post new high score to text field
    }

    public void AddPoints (int pointsToAdd) {
        // Debug.Log(pointsToAdd + " points " + ((pointsToAdd > 0) ? "added" : "removed"));
        Score += pointsToAdd * scoreMultiplier;
        if (correctStreak >= 0 && pointsToAdd > 0) {
            correctStreak += 1;
        } else if (pointsToAdd < 0) {
            correctStreak = 0;
        }
        UpdateStreak();

        if (Score < 0 && !allowNegative) {
            Score = 0; // Reset score to 0
            
        }

        if (Score < -5) {
            GameOver();
        }
        Updated(); // Let the manager know we've changed the score
    }

    void UpdateStreak() {
        if (correctStreak >= 40) {
            scoreMultiplier = 8;
        } else if (correctStreak >= 20) {
            scoreMultiplier = 4;
        } else if (correctStreak >= 10) {
            scoreMultiplier = 2;
        } else {
            scoreMultiplier = 1;
        }
        streakField.text = "x" + scoreMultiplier.ToString();
        streakField2.text = "Streak!" +  streakField.text;

        if (correctStreak < 10) {
             streakField2.text = "";
        }
        PlayerPrefs.SetInt("score", Score);
    }

    // void Showtext(){
    //     yield return new WaitForSeconds(5);
    // }

    public void Reset () {
        Debug.Log("Reset score");
        Score = 0;
        Updated();
    }

    void GameOver() {
        Destroy(GameObject.Find("Spawner"));
        
        SceneManager.LoadScene(4);
    }
}


// using UnityEngine;
// using UnityEngine.UI;

// /// <summary>Manager for SumScore accessible from inspector</summary>
// /// <remarks>
// /// Attach to game object in scene. 
// /// This is a singleton so only one instance can be active at a time.
// /// </remarks>
// public class SumScoreManager : MonoBehaviour {

    

// }
