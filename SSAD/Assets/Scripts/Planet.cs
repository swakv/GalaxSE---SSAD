using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class Planet : MonoBehaviour
{
  // public static int Progress.currentPlanet;
  // public static int Progress.unlockedPlanet = 1;
  // public static int Progress.planetCount;
    public TextMeshProUGUI console;
    public void FixedUpdate()
    {
       if (Input.GetMouseButtonDown(0)) 
       {
                 RaycastHit  hit;
                 Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                  if (Physics.Raycast(ray, out hit, 600)) 
                  {
                    //   Debug.Log("GoingIN");
                      if (hit.transform.name == "one" )
                        {
                          Progress.currentPlanet = 1;
                            Debug.Log(Progress.currentPlanet);
                            Debug.Log("bla");
                            Debug.Log(Progress.unlockedPlanet);
                            if(Progress.currentPlanet <= Progress.unlockedPlanet){
                              SceneManager.LoadScene("RGL1");
                              console.text = "";
                            }
                            else{
                              Debug.Log("Locked");
                              console.text = "Planet locked";
                            }
                            
                        }
                      if (hit.transform.name == "two" )
                        {
                            Progress.currentPlanet = 2;
                            Debug.Log(Progress.currentPlanet);
                            if(Progress.currentPlanet <= Progress.unlockedPlanet){
                              SceneManager.LoadScene("RGL1");
                              console.text = "";
                            }
                            else{
                              Debug.Log("Locked");
                              console.text = "Planet locked";
                            }
                        }
                      if (hit.transform.name == "three" )
                        {
                            Progress.currentPlanet = 3;
                            Debug.Log(Progress.currentPlanet);
                            if(Progress.currentPlanet <= Progress.unlockedPlanet){
                              SceneManager.LoadScene("RGL1");
                              console.text = "";
                            }
                            else{
                              Debug.Log("Locked");
                              console.text = "Planet locked";
                            }
                        }
                      if (hit.transform.name == "four" )
                        {
                            Progress.currentPlanet = 4;
                            Debug.Log(Progress.currentPlanet);
                            if(Progress.currentPlanet <= Progress.unlockedPlanet){
                              SceneManager.LoadScene("RGL1");
                              console.text = "";
                            }
                            else{
                              Debug.Log("Locked");
                              console.text = "Planet locked";
                            }
                        }
                      if (hit.transform.name == "five" )
                        {
                            Progress.currentPlanet = 5;
                            Debug.Log(Progress.currentPlanet);
                            if(Progress.currentPlanet <= Progress.unlockedPlanet){
                              SceneManager.LoadScene("RGL1");
                              console.text = "";
                            }
                            else{
                              Debug.Log("Locked");
                              console.text = "Planet locked";
                            }
                        }
                      if (hit.transform.name == "six" )
                        {
                            Progress.currentPlanet = 6;
                            Debug.Log(Progress.currentPlanet);
                            if(Progress.currentPlanet <= Progress.unlockedPlanet){
                              SceneManager.LoadScene("RGL1");
                              console.text = "";
                            }
                            else{
                              Debug.Log("Locked");
                              console.text = "Planet locked";
                            }
                        }
                      if (hit.transform.name == "seven" )
                        {
                            Progress.currentPlanet = 7;
                            Debug.Log(Progress.currentPlanet);
                            if(Progress.currentPlanet <= Progress.unlockedPlanet){
                              SceneManager.LoadScene("RGL1");
                              console.text = "";
                            }
                            else{
                              Debug.Log("Locked");
                              console.text = "Planet locked";
                            }
                        }
                      if (hit.transform.name == "eight" )
                        {
                            Progress.currentPlanet = 8;
                            Debug.Log(Progress.currentPlanet);
                            if(Progress.currentPlanet <= Progress.unlockedPlanet){
                              SceneManager.LoadScene("RGL1");
                              console.text = "";
                            }
                            else{
                              Debug.Log("Locked");
                              console.text = "Planet locked";
                            }
                        }
                  }
                  
       }
    }
}
