using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class nextQ : MonoBehaviour
{
    // public TextMeshPro Question;
    public TextMeshProUGUI Question;
    public TextMeshProUGUI TextA;
    public TextMeshProUGUI TextB;
    public TextMeshProUGUI TextC;
    public TextMeshProUGUI TextD;

    public void SetText(){
        Question.text = "QUESTION 2 :  “Software engineers should not use their technical skills to misuse other people’s computers.”Here the term misuse refers to:";
        TextA.text = "A) Unauthorized access to computer material";
        TextB.text = "B) Unauthorized modification of computer material";
        TextC.text = "C) Dissemination of viruses or other malware";
        TextD.text = "D) All of the mentioned";
    }
    
}
