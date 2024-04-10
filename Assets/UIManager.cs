using System.Collections;
using System.Collections.Generic;
// UIManager.cs
// This script updates the public variables of all rat instances based on UI input.
// Attach this script to an empty GameObject in your scene, and link the sliders in the Inspector.

using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Slider speedSlider;
    public Slider cohesionSlider;
    public Slider alignmentSlider;
    public Slider separationSlider;
    public Slider separationDistanceSlider;
    public Slider gravitySlider;

    void Start()
    {
        // Initialize sliders with default values from a RatSwarmBehavior instance if needed
        // For example: speedSlider.value = defaultRatSwarmBehavior.speed;

        // Add listeners for each slider
        speedSlider.onValueChanged.AddListener(delegate { UpdateRatVariable("speed"); });
        cohesionSlider.onValueChanged.AddListener(delegate { UpdateRatVariable("cohesion"); });
        alignmentSlider.onValueChanged.AddListener(delegate { UpdateRatVariable("alignment"); });
        separationSlider.onValueChanged.AddListener(delegate { UpdateRatVariable("separation"); });
        separationDistanceSlider.onValueChanged.AddListener(delegate { UpdateRatVariable("separationDistance"); });
        gravitySlider.onValueChanged.AddListener(delegate { UpdateRatVariable("gravity"); });
    }

    void UpdateRatVariable(string variableName)
    {
        foreach (GameObject rat in GameObject.FindGameObjectsWithTag("Rat"))
        {
            RatSwarmBehavior ratBehavior = rat.GetComponent<RatSwarmBehavior>();

            switch (variableName)
            {
                case "speed":
                    ratBehavior.speed = speedSlider.value;
                    break;
                case "cohesion":
                    ratBehavior.cohesionStrength = cohesionSlider.value;
                    break;
                case "alignment":
                    ratBehavior.alignmentStrength = alignmentSlider.value;
                    break;
                case "separation":
                    ratBehavior.separationStrength = separationSlider.value;
                    break;
                case "separationDistance":
                    ratBehavior.separationDistance = separationDistanceSlider.value;
                    break;
                case "gravity":
                    ratBehavior.gravityStrength = gravitySlider.value;
                    break;
            }
        }
    }
}

