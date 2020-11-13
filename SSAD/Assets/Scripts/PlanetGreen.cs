using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class PlanetGreen : MonoBehaviour
{
  public TextMeshProUGUI console;
    public void FixedUpdate()
    {
       if (Input.GetMouseButtonDown(0)) 
       {
                 RaycastHit  hit;
                 Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                  if (Physics.Raycast(ray, out hit, 600)) 
                  {
                      if (hit.transform.name == "one" )
                        {
                          Progress.currentPlanet = 17;
                            Debug.Log(Progress.currentPlanet);
                            if(Progress.currentPlanet <= Progress.unlockedPlanet){
                              SceneManager.LoadScene("GGL1");
                              console.text = "";
                            }
                            else{
                              Debug.Log("Locked");
                              console.text = "Planet locked";
                            }
                        }
                      if (hit.transform.name == "two" )
                        {
                          Progress.currentPlanet = 18;
                            Debug.Log(Progress.currentPlanet);
                            if(Progress.currentPlanet <= Progress.unlockedPlanet){
                              SceneManager.LoadScene("GGL1");
                              console.text = "";
                            }
                            else{
                              Debug.Log("Locked");
                              console.text = "Planet locked";
                            }
                        }
                      if (hit.transform.name == "three" )
                        {
                          Progress.currentPlanet = 19;
                            Debug.Log(Progress.currentPlanet);
                            if(Progress.currentPlanet <= Progress.unlockedPlanet){
                              SceneManager.LoadScene("GGL1");
                              console.text = "";
                            }
                            else{
                              Debug.Log("Locked");
                              console.text = "Planet locked";
                            }
                        }
                      if (hit.transform.name == "four" )
                        {
                          Progress.currentPlanet = 20;
                            Debug.Log(Progress.currentPlanet);
                            if(Progress.currentPlanet <= Progress.unlockedPlanet){
                              SceneManager.LoadScene("GGL1");
                              console.text = "";
                            }
                            else{
                              Debug.Log("Locked");
                              console.text = "Planet locked";
                            }
                        }
                      if (hit.transform.name == "five" )
                        {
                          Progress.currentPlanet = 21;
                            Debug.Log(Progress.currentPlanet);
                            if(Progress.currentPlanet <= Progress.unlockedPlanet){
                              SceneManager.LoadScene("GGL1");
                              console.text = "";
                            }
                            else{
                              Debug.Log("Locked");
                              console.text = "Planet locked";
                            }
                        }
                      if (hit.transform.name == "six" )
                        {
                          Progress.currentPlanet = 22;
                            Debug.Log(Progress.currentPlanet);
                            if(Progress.currentPlanet <= Progress.unlockedPlanet){
                              SceneManager.LoadScene("GGL1");
                              console.text = "";
                            }
                            else{
                              Debug.Log("Locked");
                              console.text = "Planet locked";
                            }
                        }
                      if (hit.transform.name == "seven" )
                        {
                          Progress.currentPlanet = 23;
                            Debug.Log(Progress.currentPlanet);
                            if(Progress.currentPlanet <= Progress.unlockedPlanet){
                              SceneManager.LoadScene("GGL1");
                              console.text = "";
                            }
                            else{
                              Debug.Log("Locked");
                              console.text = "Planet locked";
                            }
                        }
                      if (hit.transform.name == "eight" )
                        {
                          Progress.currentPlanet = 24;
                            Debug.Log(Progress.currentPlanet);
                            if(Progress.currentPlanet <= Progress.unlockedPlanet){
                              SceneManager.LoadScene("GGL1");
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
