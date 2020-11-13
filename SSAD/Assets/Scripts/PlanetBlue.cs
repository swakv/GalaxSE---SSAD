using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class PlanetBlue : MonoBehaviour
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
                      Debug.Log("GoingINBLUE");
                      if (hit.transform.name == "one" )
                        {
                            Progress.currentPlanet = 9;
                            Debug.Log(Progress.currentPlanet);
                            if(Progress.currentPlanet <= Progress.unlockedPlanet){
                              SceneManager.LoadScene("BGL1");
                              console.text = "";
                            }
                            else{
                              Debug.Log("Locked");
                              console.text = "Planet locked";
                            }
                        }
                      if (hit.transform.name == "two" )
                        {
                          Progress.currentPlanet = 10;
                            Debug.Log(Progress.currentPlanet);
                           if(Progress.currentPlanet <= Progress.unlockedPlanet){
                              SceneManager.LoadScene("BGL1");
                              console.text = "";
                            }
                            else{
                              Debug.Log("Locked");
                              console.text = "Planet locked";
                            }
                        }
                      if (hit.transform.name == "three" )
                        {
                          Progress.currentPlanet = 11;
                            Debug.Log(Progress.currentPlanet);
                            if(Progress.currentPlanet <= Progress.unlockedPlanet){
                              SceneManager.LoadScene("BGL1");
                              console.text = "";
                            }
                            else{
                              Debug.Log("Locked");
                              console.text = "Planet locked";
                            }
                        }
                      if (hit.transform.name == "four" )
                        {
                          Progress.currentPlanet = 12;
                            Debug.Log(Progress.currentPlanet);
                            if(Progress.currentPlanet <= Progress.unlockedPlanet){
                              SceneManager.LoadScene("BGL1");
                              console.text = "";
                            }
                            else{
                              Debug.Log("Locked");
                              console.text = "Planet locked";
                            }
                        }
                      if (hit.transform.name == "five" )
                        {
                          Progress.currentPlanet = 13;
                            Debug.Log(Progress.currentPlanet);
                            if(Progress.currentPlanet <= Progress.unlockedPlanet){
                              SceneManager.LoadScene("BGL1");
                              console.text = "";
                            }
                            else{
                              Debug.Log("Locked");
                              console.text = "Planet locked";
                            }
                        }
                      if (hit.transform.name == "six" )
                        {
                          Progress.currentPlanet = 14;
                            Debug.Log(Progress.currentPlanet);
                            if(Progress.currentPlanet <= Progress.unlockedPlanet){
                              SceneManager.LoadScene("BGL1");
                              console.text = "";
                            }
                            else{
                              Debug.Log("Locked");
                              console.text = "Planet locked";
                            }
                        }
                      if (hit.transform.name == "seven" )
                        {
                          Progress.currentPlanet = 15;
                            Debug.Log(Progress.currentPlanet);
                            if(Progress.currentPlanet <= Progress.unlockedPlanet){
                              SceneManager.LoadScene("BGL1");
                              console.text = "";
                            }
                            else{
                              Debug.Log("Locked");
                              console.text = "Planet locked";
                            }
                        }
                      if (hit.transform.name == "eight" )
                        {
                          Progress.currentPlanet = 16;
                            Debug.Log(Progress.currentPlanet);
                            if(Progress.currentPlanet <= Progress.unlockedPlanet){
                              SceneManager.LoadScene("BGL1");
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
