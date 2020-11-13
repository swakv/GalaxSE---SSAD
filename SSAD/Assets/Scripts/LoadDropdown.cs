using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadDropdown : MonoBehaviour
{
    public float delay = 5;
    public string NewLevel= "DropDown";
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


 
