using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class dropdown : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject Panel;
    int counter = 0;
    public void showHidePanel()
    {
        if (Panel.activeSelf)
        {
            Panel.SetActive(false);
        }
        else if (Panel != null)
        {
            Panel.SetActive(true);
        }
    }
}
