using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;


public class Galaxy3 : MonoBehaviour
{
    public TextMeshProUGUI console;
    void Update()
    {
       if (Input.GetMouseButtonDown(0)) 
       {
           
                 RaycastHit  hit;
                 Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                  if (Physics.Raycast(ray, out hit)) 
                  {
                      if (hit.transform.name == "three" )
                        {
                            Galaxy1.currentGalxy = 3;
                            Debug.Log("G3");
                            if(Progress.unlockedPlanet >= 17){
                              SceneManager.LoadScene("GreenGalaxy");
                              console.text = "";
                            }
                            else{
                              Debug.Log("Locked");
                              console.text = "Galaxy locked - unlock all levels in the previous galaxy to unlock!";
                            }
                        }
                  }
       }
    }
}
