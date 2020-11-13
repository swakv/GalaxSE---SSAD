using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class prevQ : MonoBehaviour
{
    // public TextMeshPro Question;
    public TextMeshProUGUI Question;
    public TextMeshProUGUI TextA;
    public TextMeshProUGUI TextB;
    public TextMeshProUGUI TextC;
    public TextMeshProUGUI TextD;

    public void SetText(){
        Question.text = "QUESTION 1 :  Choose the correct option in terms of Issues related to professional responsibility";
        TextA.text = "A) Confidentiality";
        TextB.text = "B) Intellectual property rights";
        TextC.text = "C) Both Confidentiality & Intellectual property rights";
        TextD.text = "D) Managing Client Relationships";
    }
    
}
