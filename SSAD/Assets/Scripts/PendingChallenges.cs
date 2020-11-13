using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PendingChallenges : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject GamePanel;
    public GameObject ReqPanel;

    public Button ViewGames;
    public Button ViewRequests;
    
    public void viewGames()
    { 
      TextMeshProUGUI gText = ViewGames.GetComponentInChildren<TextMeshProUGUI>();
      TextMeshProUGUI rText =  ViewRequests.GetComponentInChildren<TextMeshProUGUI>();
      ColorBlock gcolors = ViewGames.colors;
      ColorBlock rcolors = ViewRequests.colors;
      if (GamePanel != null)
      {
        if (ReqPanel.activeSelf)
          {
            ReqPanel.SetActive(false);
          }
        // change view games button to black and view req button to gray 
        gcolors.normalColor = Color.black;
        rText.color = Color.black;
        rcolors.normalColor = Color.gray;
        ViewGames.colors = gcolors;
        ViewRequests.colors = rcolors;
        GamePanel.SetActive(true); 
 
      }
    }

    public void viewRequests()
    { 
      TextMeshProUGUI gText = ViewGames.GetComponentInChildren<TextMeshProUGUI>();
      TextMeshProUGUI rText =  ViewRequests.GetComponentInChildren<TextMeshProUGUI>();
      ColorBlock gcolors = ViewGames.colors;
      ColorBlock rcolors = ViewRequests.colors;
      if (ReqPanel != null)
      {
        if (GamePanel.activeSelf)
            {
              GamePanel.SetActive(false);
            }
        // change view games button to gray and view req button to black 
        gcolors.normalColor = Color.gray;
        gText.color = Color.black;
        rcolors.normalColor = Color.black;
        ViewGames.colors = gcolors;
        ViewRequests.colors = rcolors;
        ReqPanel.SetActive(true);  
      }
    }

}
