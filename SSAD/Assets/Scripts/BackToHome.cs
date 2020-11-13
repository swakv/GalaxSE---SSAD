using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
// using UnityEngine.EventSystem;
public class BackToHome : MonoBehaviour
{
    public void LoadHome()
    {
        ClickToStart.linkText="";
        SceneManager.LoadScene("Home");
    }

    public void UserProfile()
    {
        SceneManager.LoadScene("User Profile");
    }


    
}
