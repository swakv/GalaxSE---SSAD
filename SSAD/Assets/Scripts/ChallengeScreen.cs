using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ChallengeScreen : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadPendingChallenges()
    {
        SceneManager.LoadScene("PendingChallenges");
    }

    public void LoadChallengeFriends()
    {
        Debug.Log("HAVE TO CREATE SCENE");
        //SceneManager.LoadScene("BGL1");
    }
}
