using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Monster : MonoBehaviour
{
  ACOrbit aco = new ACOrbit();
  public static string currentMonster;
    public void FixedUpdate()
    {
       if (Input.GetMouseButtonDown(0)) 
       {
                 RaycastHit  hit;
                 Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                  if (Physics.Raycast(ray, out hit, 600)) 
                  {

                      if (hit.transform.name == "Easy" )
                        {
                          var here = aco.UpdateDiffML();
                          Debug.Log("Diff ");
                          Debug.Log(here);
                          currentMonster = here.ToString();
                          PlayerPrefs.SetString ("lastLoadedScene", SceneManager.GetActiveScene ().name);
                            SceneManager.LoadScene("EasyLevel");
                        }
                      else if (hit.transform.name == "Medium" )
                        {
                          var here = aco.UpdateDiffML();
                          Debug.Log("Diff ");
                          Debug.Log(here);
                          currentMonster = here.ToString();
                          PlayerPrefs.SetString ("lastLoadedScene", SceneManager.GetActiveScene ().name);
                            SceneManager.LoadScene("MediumLevel");
                        }
                      else if (hit.transform.name == "Hard" )
                        {
                          var here = aco.UpdateDiffML();
                          Debug.Log("Diff ");
                          Debug.Log(here);
                          currentMonster = here.ToString();
                          PlayerPrefs.SetString ("lastLoadedScene", SceneManager.GetActiveScene ().name);
                            SceneManager.LoadScene("HardLevel");
                        }
                  }
                  
       }
    }
}
