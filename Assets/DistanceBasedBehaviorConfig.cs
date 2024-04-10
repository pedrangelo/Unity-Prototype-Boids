using System.Collections;
using System.Collections.Generic;
// DistanceBasedBehaviorConfig.cs
// Defines a ScriptableObject that stores configuration for distance-based behavior adjustments.

using UnityEngine;

[CreateAssetMenu(fileName = "DistanceBasedBehaviorConfig", menuName = "ScriptableObjects/DistanceBasedBehaviorConfig", order = 1)]
public class DistanceBasedBehaviorConfig : ScriptableObject
{
    public string variableName; // The name of the variable to adjust
    public float closeDistanceThreshold; // Distance at which the maximum rate of change applies
    public float farDistanceThreshold; // Distance at which the minimum rate of change (or no change) applies
    public float maxChangeRate; // Rate of change when the rat is within the closeDistanceThreshold
    public float minChangeRate; // Rate of change when the rat is beyond the farDistanceThreshold
    public float minVariableValue = 0f; // Minimum limit for the variable
    public float maxVariableValue = 10f; // Maximum limit for the variable

}
