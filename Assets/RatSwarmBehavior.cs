using System.Collections;
using System.Collections.Generic;
// RatSwarmBehavior.cs
// This script controls an individual rat's behavior in a 2D side-scroller game.
// It uses a simplified Boids algorithm adapted for 2D gameplay and includes gravity.
// The script assumes it's attached to a rat GameObject, which is part of a larger swarm.

using UnityEngine;

public class RatSwarmBehavior : MonoBehaviour
{
    public float speed = 5.0f; // Movement speed
    public float cohesionStrength = 1.0f; // Strength of the cohesion behavior
    public float alignmentStrength = 1.0f; // Strength of the alignment behavior
    public float separationStrength = 1.0f; // Strength of the separation behavior
    public float separationDistance = 1.0f; // Distance to maintain from other rats
    public float gravityStrength = 9.8f; // Simulated gravity strength
    public float followMouseStrength = 2.0f;
    public DistanceBasedBehaviorConfig behaviorConfig; // Reference to the ScriptableObject
    


    private Vector2 velocity;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        velocity = transform.right * speed;
    }

    void Update()
    {
        Flock();
        FollowMouse();
        ApplyGravity();
        AdjustVariableBasedOnDistanceToMouse();

        // Apply calculated velocity
        rb.velocity = velocity;
    }

    void Flock()
    {
        Vector2 cohesionVector = Vector2.zero;
        Vector2 alignmentVector = velocity.normalized; // Start with current direction
        Vector2 separationVector = Vector2.zero;
        int nearbyRats = 0;

        // Detect nearby rats
        foreach (var rat in GameObject.FindGameObjectsWithTag("Rat"))
        {
            if (rat == gameObject) continue; // Skip self

            Vector2 toRat = rat.transform.position - transform.position;
            if (toRat.magnitude < separationDistance)
            {
                cohesionVector += (Vector2)rat.transform.position;
                alignmentVector += (Vector2)rat.GetComponent<Rigidbody2D>().velocity.normalized;
                separationVector += (Vector2)(transform.position - rat.transform.position).normalized / toRat.magnitude;
                nearbyRats++;
            }
        }

        if (nearbyRats > 0)
        {
            cohesionVector = (cohesionVector / nearbyRats - (Vector2)transform.position).normalized;
            alignmentVector = (alignmentVector / nearbyRats).normalized;
            separationVector = (separationVector / nearbyRats).normalized;
        }

        // Calculate final velocity
        velocity += (cohesionVector * cohesionStrength + alignmentVector * alignmentStrength + separationVector * separationStrength) * Time.deltaTime;
        velocity = velocity.normalized * speed;
    }

    void ApplyGravity()
    {
        // Apply a constant downward force to simulate gravity
        velocity += Vector2.down * gravityStrength * Time.deltaTime;
    }

    void FollowMouse()
    {
        // Convert mouse position to world coordinates
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Calculate steering vector towards mouse position
        Vector2 steerDirection = (mousePosition - (Vector2)transform.position).normalized;

        // Apply steering force towards mouse position
        velocity += steerDirection * followMouseStrength * Time.deltaTime;
    }

     void AdjustVariableBasedOnDistanceToMouse()
    {
        if (behaviorConfig == null) return;

        float distance = Vector2.Distance(Camera.main.ScreenToWorldPoint(Input.mousePosition), transform.position);
        float rate = 0;

        // Determine the rate of change based on the rat's distance to the mouse
        if (distance <= behaviorConfig.closeDistanceThreshold)
        {
            rate = behaviorConfig.maxChangeRate;
        }
        else if (distance >= behaviorConfig.farDistanceThreshold)
        {
            rate = behaviorConfig.minChangeRate;
        }
        else
        {
            // Interpolate the rate of change for distances between the thresholds
            float t = (distance - behaviorConfig.closeDistanceThreshold) / (behaviorConfig.farDistanceThreshold - behaviorConfig.closeDistanceThreshold);
            rate = Mathf.Lerp(behaviorConfig.maxChangeRate, behaviorConfig.minChangeRate, t);
        }

        // Adjust the specified variable by the determined rate
        AdjustVariable(behaviorConfig.variableName, rate * Time.deltaTime);
    }

    void AdjustVariable(string variableName, float changeAmount)
    {
        // Use reflection to get and set the variable value
        var field = typeof(RatSwarmBehavior).GetField(variableName);
        if (field != null && behaviorConfig != null)
        {
            float currentValue = (float)field.GetValue(this);
            // Calculate the new value and clamp it between min and max limits
            float newValue = Mathf.Clamp(currentValue + changeAmount, behaviorConfig.minVariableValue, behaviorConfig.maxVariableValue);
            field.SetValue(this, newValue);
        }
    }

    void OnDrawGizmos()
{
    // Existing gizmos for velocity and separation distance
    Gizmos.color = Color.green;
    Gizmos.DrawLine(transform.position, transform.position + (Vector3)velocity);

    Gizmos.color = Color.red;
    Gizmos.DrawWireSphere(transform.position, separationDistance);

    // Check if behaviorConfig is assigned
    if (behaviorConfig != null)
    {
        // Draw spheres for close and far distance thresholds
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, behaviorConfig.closeDistanceThreshold);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, behaviorConfig.farDistanceThreshold);

        // Determine the current rate based on the distance to the mouse
        float distance = Vector2.Distance(Camera.main.ScreenToWorldPoint(Input.mousePosition), transform.position);
        float rate = 0;
        if (distance <= behaviorConfig.closeDistanceThreshold)
        {
            rate = behaviorConfig.maxChangeRate;
        }
        else if (distance >= behaviorConfig.farDistanceThreshold)
        {
            rate = behaviorConfig.minChangeRate;
        }
        else
        {
            float t = (distance - behaviorConfig.closeDistanceThreshold) / (behaviorConfig.farDistanceThreshold - behaviorConfig.closeDistanceThreshold);
            rate = Mathf.Lerp(behaviorConfig.maxChangeRate, behaviorConfig.minChangeRate, t);
        }

        // Use a gradient from green (max rate) to red (min rate) to visualize the rate
        Gizmos.color = Color.Lerp(Color.red, Color.green, Mathf.InverseLerp(behaviorConfig.minChangeRate, behaviorConfig.maxChangeRate, rate));
        Gizmos.DrawWireSphere(transform.position, distance);
    }
}

    
}
