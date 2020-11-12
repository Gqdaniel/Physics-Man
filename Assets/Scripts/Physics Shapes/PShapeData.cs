using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PShapeData : MonoBehaviour
{
    public static PShapeData instance;

    public float minForce = 5f;
    public float maxForce = 30f;
    public Gradient vectorForceColorGradient;
    public float minArrowScale = 0.1f;
    public float maxArrowScale = 2f;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
            return;
        }
    }

    public float getForceScaleRatio(float force)
    {
        float perc = getForceRatio(force);
        return Mathf.Lerp(minArrowScale, maxArrowScale, perc);
    }

    public Color getForceColor(float force)
    {
        return vectorForceColorGradient.Evaluate(getForceRatio(force));
    }

    private float getForceRatio(float force)
    {
        return (force - minForce) / (maxForce - minForce);
    }

    public bool isValidForce(float force)
    {
        return force >= minForce && force <= maxForce;
    }

    public string forceRangeToString()
    {
        return "(" + minForce + ", " + maxForce + ")";
    }
}
