using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PIDController
{
    public float Kp;
    public float Ki;
    public float Kd;

    private float integral, previousError;

    public PIDController(float p, float i, float d)
    {
        Kp = p;
        Ki = i;
        Kd = d;
    }

    public float UpdateError(float currentError, float deltaTime)
    {
        integral += currentError * deltaTime;
        float derivative = (currentError - previousError) / deltaTime;
        float output = Kp * currentError + Ki * integral + Kd * derivative;
        previousError = currentError;
        return output;
    }
}
