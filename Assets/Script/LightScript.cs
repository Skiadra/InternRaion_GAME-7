using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightScript : MonoBehaviour
{
    public Light myLight;

    public bool changeIntensity = false;
    public float intensityChangeSpeed = 1.0f;
    public float maxIntensity = 4.0f;



    // Start is called before the first frame update
    void Start()
    {
        myLight = GetComponent<Light>();
        myLight.intensity = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (changeIntensity)
        {
            myLight.intensity += Time.deltaTime*intensityChangeSpeed;
            if (myLight.intensity >= maxIntensity)
            {
                changeIntensity = false;
            }
        } else
        {
            myLight.intensity -= Time.deltaTime*intensityChangeSpeed;
            if (myLight.intensity <= 2)
            {
                changeIntensity = true;
            }
        }
    }
}
