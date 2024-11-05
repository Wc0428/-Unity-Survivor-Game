using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CaloriesBar : MonoBehaviour
{

    private Slider slider;
    public Text caloriesCounter;
    public GameObject playerState;

    private float currentCalories, maxCalories;
    // Start is called before the first frame update
    void Awake()
    {
        slider = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        currentCalories = playerState.GetComponent<PlayerState>().currentCalories;
        maxCalories = playerState.GetComponent<PlayerState>().maxCalories;

        float fillvalue = currentCalories/ maxCalories;
        slider.value = fillvalue;

        caloriesCounter.text = currentCalories + "/" + maxCalories;

    }
}
