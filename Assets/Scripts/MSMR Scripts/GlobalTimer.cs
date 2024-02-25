using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalTimer : MonoBehaviour
{
    // handles our 5 minute timer
    protected static float remainingTime = 300f;

    public float GetRT() { return remainingTime; }
    public float SetRT(float time) { return remainingTime = time; }
}
