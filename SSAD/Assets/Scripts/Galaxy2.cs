using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;


public class Galaxy2 : MonoBehaviour
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
                      
                      if (hit.transform.name == "two" )
                        {
                          Galaxy1.currentGalxy = 2;
                            Debug.Log("G2");
                            if(Progress.unlockedPlanet >= 9){
                              SceneManager.LoadScene("BlueGalaxy");
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
