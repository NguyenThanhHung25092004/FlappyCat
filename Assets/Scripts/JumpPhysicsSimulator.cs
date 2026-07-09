using System.Collections.Generic;
using UnityEngine;

public static class JumpPhysicsSimulator
{
    // Simulates one full "hold to dynamicMaxHoldTime" jump arc using the exact
    // step logic from PlayerMovement.FixedUpdate. Returns time-vs-height samples.
    public static List<(float time, float height)> SimulateMaxJump(
        float jumpForce, float sustainedJumpForce, float holdTime,
        float fallMultiplier, float mass, float gravityScale, float fixedDeltaTime)
    {
        var samples = new List<(float, float)>();
        float gravityY = Physics2D.gravity.y * gravityScale;

        float vy = jumpForce / mass; // impulse: deltaV = impulse / mass
        float y = 0f;
        float t = 0f;
        float holdTimer = 0f;

        samples.Add((t, y));

        while (t < 5f) // safety cutoff
        {
            if (holdTimer < holdTime)
            {
                vy += (sustainedJumpForce / mass) * fixedDeltaTime;
                holdTimer += fixedDeltaTime;
            }

            vy += gravityY * fixedDeltaTime; // built-in gravity, always applied

            if (vy < 0f) // extra fall gravity, matches ApplyGravityMultipliers
            {
                vy += gravityY * (fallMultiplier - 1f) * fixedDeltaTime;
            }

            y += vy * fixedDeltaTime;
            t += fixedDeltaTime;
            samples.Add((t, y));

            if (t > 0.05f && y < 0f) break; // landed back below launch height
        }

        return samples;
    }

    public static float GetAirTimeToHeight(List<(float time, float height)> samples, float targetDeltaY)
    {
        if (targetDeltaY >= 0f)
        {
            for (int i = samples.Count - 1; i > 0; i--)
            {
                var (t0, y0) = samples[i - 1];
                var (t1, y1) = samples[i];

                bool crossedDescending = y0 >= targetDeltaY && y1 < targetDeltaY;
                if (crossedDescending)
                {
                    float ratio = (targetDeltaY - y0) / (y1 - y0);
                    return Mathf.Lerp(t0, t1, ratio);
                }
            }
            return -1f; 
        }
        else
        {
            for (int i = 1; i < samples.Count; i++)
            {
                var (t0, y0) = samples[i - 1];
                var (t1, y1) = samples[i];

                bool crossed = y0 > targetDeltaY && y1 <= targetDeltaY;
                if (crossed)
                {
                    float ratio = (targetDeltaY - y0) / (y1 - y0);
                    return Mathf.Lerp(t0, t1, ratio);
                }
            }
            return -1f;
        }
    }

    public static float GetMaxHeight(List<(float time, float height)> samples)
    {
        float max = 0f;
        foreach (var (_, h) in samples) max = Mathf.Max(max, h);
        return max;
    }
}