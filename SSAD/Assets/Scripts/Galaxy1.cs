using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;


public class Galaxy1 : MonoBehaviour
{
  public static int currentGalxy;
  public TextMeshProUGUI console;
  public static int unlockedGalaxy = 1;
    void Update()
    {
       if (Input.GetMouseButtonDown(0)) 
       {
                 RaycastHit  hit;
                 Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                  if (Physics.Raycast(ray, out hit)) 
                  {
                      
                      if (hit.transform.name == "one" )
                        {
                          currentGalxy = 1;
                          Debug.Log("G1");
                        
                          SceneManager.LoadScene("RedGalaxy");
                          console.text = "";
                          
                        }
                  }
       }
    }
}
