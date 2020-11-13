using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadTitle : MonoBehaviour
{
    public float delay = 4;
    public string NewLevel= "Title";
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LoadLevelAfterDelay(delay));
    }

    IEnumerator LoadLevelAfterDelay(float delay)
     {
         yield return new WaitForSeconds(delay);
         SceneManager.LoadScene(NewLevel);
     }
    // Update is called once per frame
    // void Update()
    // {
        
    // }
}


 
