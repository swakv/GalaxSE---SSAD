using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
 

public class Move : MonoBehaviour
{
    private float speed = 50.0f;
    private float zoomSpeed = 10.0f;
    void Update () {
    
            if (Input.GetKey(KeyCode.D)){
                transform.position += Vector3.right * speed * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.A)){
                transform.position += Vector3.left * speed * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.W)){
                transform.position += Vector3.forward * speed * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.S)){
                transform.position += Vector3.back * speed * Time.deltaTime;
            }
            float scroll = Input.GetAxis("Mouse ScrollWheel");
		    transform.Translate(0, scroll * zoomSpeed, scroll * zoomSpeed, Space.World);
        }
    public Vector3 makeMove(GameObject g, string keyCode) {
        Debug.Log("We're here");
        if (keyCode=="D")
            g.transform.position += Vector3.right * 1 * 1;
        if (keyCode=="A")
            g.transform.position += Vector3.left * 1 * 1;
        if (keyCode=="W") { 
            g.transform.position += Vector3.forward * 1 * 1;
            Debug.Log("kaod");
        }
        if (keyCode=="S")
            g.transform.position += Vector3.back * 1 * 1;
        return g.transform.position; 
    }
}

