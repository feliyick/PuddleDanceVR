using UnityEngine;
using UnityEngine.UI;

/// <summary>Manager for SumScore accessible from inspector</summary>
/// <remarks>
/// Attach to game object in scene. 
/// This is a singleton so only one instance can be active at a time.
/// </remarks>
public class SumScoreManager : MonoBehaviour {

    public int initialScore = 0;
    public bool storeHighScore = true, allowNegative = true;
    public Text field; // Text field displaying current score
    public Text highScoreField; // Text field displaying high score


    public int Score;
    public int HighScore;

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
            Add(initialScore);  // Set initial score
        if (storeHighScore) {
            if (PlayerPrefs.HasKey("sumHS")) { 
                // Set high score value and tell manager
                HighScore = PlayerPrefs.GetInt("sumHS");
                UpdatedHS();
            }
            else
                HighScore = 0;
        }

        Updated(); // Set initial score in UI
    }

    /// <summary>Notify this manager of a change in score</summary>
    void Updated () {
        field.text = Score.ToString("0"); // Post new score to text field
    }

    /// <summary>Notify this manager of a change in high score</summary>
    void UpdatedHS () {
        if(storeHighScore)
            highScoreField.text = HighScore.ToString("0"); // Post new high score to text field
    }

    void Add (int pointsToAdd) {
        // Debug.Log(pointsToAdd + " points " + ((pointsToAdd > 0) ? "added" : "removed"));
        Score += pointsToAdd;
        if (Score < 0 && !allowNegative) {
            Score = 0; // Reset score to 0
            Updated(); // Let the manager know we've changed the score
        }
    }

    void Reset () {
        Debug.Log("Reset score");
        Score = 0;
        Updated();

    }
    

}
