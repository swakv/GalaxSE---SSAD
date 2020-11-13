using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
public class ShareOnSocialMedia : MonoBehaviour
{
	string title = "GalaxSE";
	public TextMeshProUGUI score;
	string galaxy = ChallengeAnswerChoice.Galaxy;
    string textToDisplay;
	//Change Once Hosted
	string url = "http://localhost:5000/api?q=";
	void Start() {
		// score = GameObject.Find("score").GetComponent<TMPro>();
		if (ClickToStart.linkText.Length > 0) {
			url = url + ClickToStart.linkText;
			ClickToStart.linkText = null; 
		}
		else {
			url = url + ToChallengeMonster.linkText;
			ToChallengeMonster.linkText = null;
		}
		Debug.Log(score.text);
		textToDisplay = "I scored " + score.text + ". Try and beat my score. " + url;
		Debug.Log(textToDisplay);
	}
	public void ShareOnReddit ()
	{
		
		Application.OpenURL ("http://reddit.com/submit?text="+WWW.EscapeURL(textToDisplay)+"&title="+WWW.EscapeURL(title));
	}

    public void shareOnTwitter () 
	{
		Application.OpenURL ("http://twitter.com/intent/tweet?text=" + WWW.EscapeURL(textToDisplay) + "&amp;lang=en" );
	}
}