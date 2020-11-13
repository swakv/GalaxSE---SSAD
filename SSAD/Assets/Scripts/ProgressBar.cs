using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    public Slider slider;
    public GameObject panel_1;
    public GameObject panel_2;
    // Start is called before the first frame update
    void Start()
    {
        // slider = GameObject.Find("Slider").GetComponent<Slider>();
        // panel_1 = GameObject.Find("PanelSlider").GetComponent<GameObject>();
        // panel_2 = GameObject.Find("PanelText").GetComponent<GameObject>();
        panel_1.SetActive(true);
        panel_2.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Progress.currentPlanet == Progress.unlockedPlanet) {
            //slider.show()
            panel_1.SetActive(true);
            panel_2.SetActive(false);
            slider.value = Progress.planetCount;
        }
        else {
            panel_1.SetActive(false);
            panel_2.SetActive(true);
        }
    }
}
