using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class BacktoLvl : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void LoadLvlRed()
    {
        SceneManager.LoadScene("RedGalaxy");
    }

    public void LoadLvlBlue()
    {
        SceneManager.LoadScene("BlueGalaxy");
    }

    public void LoadLvlOrange()
    {
        SceneManager.LoadScene("OrangeGalaxy");
    }
    public void LoadLvlGreen()
    {
        SceneManager.LoadScene("GreenGalaxy");
    }

    public void LoadMonsters(){
        string sceneName = PlayerPrefs.GetString("lastLoadedScene");
        SceneManager.LoadScene(sceneName);
    }

    
}
