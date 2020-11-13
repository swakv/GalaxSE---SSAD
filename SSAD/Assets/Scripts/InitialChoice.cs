using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InitialChoice : MonoBehaviour
{

    public Dropdown ddch;

    private void Start(){
        ddch.onValueChanged.AddListener(delegate{
            ddchValueChangedHappened(ddch);
        });
    }

    public void ddchValueChangedHappened( Dropdown  sender){
        // Default is Teacher
        Debug.Log("You have selected : " +  sender.value);
    }
    

}
