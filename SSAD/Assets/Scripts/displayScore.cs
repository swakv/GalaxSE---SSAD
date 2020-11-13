using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class displayScore : MonoBehaviour
{
    // Start is called before the first frame update
    public TMPro.TMP_Text score;
    public Button TwitterButton;
    public Button RedditButton;
    void Start()
    {
        score = GameObject.Find("score").GetComponent<TMPro.TMP_Text>();
        score.text = ChallengeAnswerChoice.score.ToString();
        TwitterButton = GameObject.Find("TwitterButton").GetComponent<Button>();
        RedditButton = GameObject.Find("RedditButton").GetComponent<Button>();

        if (ClickToStart.linkText.StartsWith("A")) {
            ClickToStart.linkText = null;

            TwitterButton.gameObject.SetActive(false);
            RedditButton.gameObject.SetActive(false);
        }
        else {
            TwitterButton.gameObject.SetActive(true);
            RedditButton.gameObject.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
