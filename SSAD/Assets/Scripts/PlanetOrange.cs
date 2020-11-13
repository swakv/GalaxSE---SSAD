using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;


public class PlanetOrange : MonoBehaviour
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
                          Progress.currentPlanet = 25;
                            Debug.Log(Progress.currentPlanet);
                            if(Progress.currentPlanet <= Progress.unlockedPlanet){
                              SceneManager.LoadScene("OGL1");
                              console.text = "";
                            }
                            else{
                              Debug.Log("Locked");
                              console.text = "Planet locked";
                            }
                        }
                      if (hit.transform.name == "two" )
                        {
                          Progress.currentPlanet = 26;
                            Debug.Log(Progress.currentPlanet);
                            if(Progress.currentPlanet <= Progress.unlockedPlanet){
                              SceneManager.LoadScene("OGL1");
                              console.text = "";
                            }
                            else{
                              Debug.Log("Locked");
                              console.text = "Planet locked";
                            }
                        }
                      if (hit.transform.name == "three" )
                        {
                          Progress.currentPlanet = 27;
                            Debug.Log(Progress.currentPlanet);
                            if(Progress.currentPlanet <= Progress.unlockedPlanet){
                              SceneManager.LoadScene("OGL1");
                              console.text = "";
                            }
                            else{
                              Debug.Log("Locked");
                              console.text = "Planet locked";
                            }
                        }
                      if (hit.transform.name == "four" )
                        {
                          Progress.currentPlanet = 28;
                            Debug.Log(Progress.currentPlanet);
                            if(Progress.currentPlanet <= Progress.unlockedPlanet){
                              SceneManager.LoadScene("OGL1");
                              console.text = "";
                            }
                            else{
                              Debug.Log("Locked");
                              console.text = "Planet locked";
                            }
                        }
                      if (hit.transform.name == "five" )
                        {
                          Progress.currentPlanet = 29;
                            Debug.Log(Progress.currentPlanet);
                            if(Progress.currentPlanet <= Progress.unlockedPlanet){
                              SceneManager.LoadScene("OGL1");
                              console.text = "";
                            }
                            else{
                              Debug.Log("Locked");
                              console.text = "Planet locked";
                            }
                        }
                      if (hit.transform.name == "six" )
                        {
                          Progress.currentPlanet = 30;
                            Debug.Log(Progress.currentPlanet);
                            if(Progress.currentPlanet <= Progress.unlockedPlanet){
                              SceneManager.LoadScene("OGL1");
                              console.text = "";
                            }
                            else{
                              Debug.Log("Locked");
                              console.text = "Planet locked";
                            }
                        }
                      if (hit.transform.name == "seven" )
                        {
                          Progress.currentPlanet = 31;
                            Debug.Log(Progress.currentPlanet);
                            if(Progress.currentPlanet <= Progress.unlockedPlanet){
                              SceneManager.LoadScene("OGL1");
                              console.text = "";
                            }
                            else{
                              Debug.Log("Locked");
                              console.text = "Planet locked";
                            }
                        }
                      if (hit.transform.name == "eight" )
                        {
                          Progress.currentPlanet = 32;
                            Debug.Log(Progress.currentPlanet);
                            if(Progress.currentPlanet <= Progress.unlockedPlanet){
                              SceneManager.LoadScene("OGL1");
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
