using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationStart : MonoBehaviour
{

    public static int movespeed = 25;
    public Vector3 userDirection = Vector3.right;

    void Start(){

    }

    // Update is called once per frame
    void Update()
    {
       
        transform.Translate(userDirection * movespeed * Time.deltaTime); 

    }
}
