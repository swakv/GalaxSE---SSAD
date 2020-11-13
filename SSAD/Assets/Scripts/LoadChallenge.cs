using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
public class LoadChallenge : MonoBehaviour
{
    Dropdown m_Dropdown1;
    string m_Message;
    int m_Dropdown1Value;
    // Start is called before the first frame update
    void Start()
    {
      //Fetch the DropDown component from the GameObject
      m_Dropdown1 = GetComponent<Dropdown>();
      //Output the first Dropdown index value
      Debug.Log("Starting Dropdown Value : " + m_Dropdown1.value);
    }

    // Update is called once per frame
    void Update()
    {
      //Keep the current index of the Dropdown in a variable
      m_Dropdown1Value = m_Dropdown1.value;
      //Change the message to say the name of the current Dropdown selection using the value
      m_Message = m_Dropdown1.options[m_Dropdown1Value].text;
    }
    public void LoadChalLevel()
    {
        Debug.Log("entering");
        SceneManager.LoadScene("ChallengeMonster");
    }


    
}
