using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class HomeScreen : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void LoadStartGame()
    {
        SceneManager.LoadScene("Galaxies");
    }

    public void LoadChallenges()
    {
        SceneManager.LoadScene("CreateLevel");
    }

}
