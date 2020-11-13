using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienHard : MonoBehaviour
{
    
    void Update()
    {
       if (Input.GetMouseButtonDown(0)) 
       {
                 RaycastHit  hit;
                 Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                  if (Physics.Raycast(ray, out hit)) 
                  {
                      if (hit.transform.name == "Hard" )
                        {
                            Debug.Log("HARD");
                        }
                  }
       }
    }
}
