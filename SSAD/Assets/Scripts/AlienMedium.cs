using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienMedium : MonoBehaviour
{
   
    void Update()
    {
       if (Input.GetMouseButtonDown(0)) 
       {
                 RaycastHit  hit;
                 Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                  if (Physics.Raycast(ray, out hit)) 
                  {
                      if (hit.transform.name == "Medium" )
                        {
                            Debug.Log("MEDIUM");
                        }
                  }
       }
    }
}
