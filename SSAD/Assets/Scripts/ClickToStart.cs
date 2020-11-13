using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClickToStart : MonoBehaviour
{
    public static string linkText = "";

    // can call below line from anywhere
    // Debug.Log(ClickToStart.stText);

    // public string NewLevel= "Login";
 
    // IEnumerator LoadLevelAfterDelay(float delay)
    //  {
    //      yield return new WaitForSeconds(delay);
    //      SceneManager.LoadScene(NewLevel);
    //  }
    // Update is called once per frame
    void Start(){
        // StaticClass.CrossSceneInformation = "Hello Scene2!";
        if (System.IO.File.Exists("/Users/Swa/Documents/NTU/Y3S1/CZ3003/PROJECT/GalaxSE/newFile.txt")){
            
            //if user is logged in
                linkText = System.IO.File.ReadAllText("/Users/Swa/Documents/NTU/Y3S1/CZ3003/PROJECT/GalaxSE/newFile.txt");
                Debug.Log("entering file exists"); 
                // System.IO.File.Delete("/Users/Swa/Documents/NTU/Y3S1/CZ3003/PROJECT/GalaxSE/newFile.txt");
                Debug.Log("exiting");
        }
    }
    void Update()
    {
        
        if (Input.GetMouseButtonDown(0)){
            Debug.Log("Pressed primary button.");
            SceneManager.LoadScene("Login");
        }

    }
}


 
