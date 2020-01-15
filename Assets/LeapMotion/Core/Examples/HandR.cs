using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandR : MonoBehaviour {

    public GameObject[] fingers;
    public GameObject main;
    public GameObject palm;
    public GameObject antenna;
    float handArea = 0;
    float handHorPos = 0;
    Vector3 mainPos;
    Vector3 palmPos;
    float[] dist;
    public Text nText;
    public Text xText;
    public ProceduralSound leapSound;

    // Use this for initialization
    void Start () {
        main = this.transform.GetChild(0).gameObject;
        mainPos = main.transform.position;
        InvokeRepeating("UpdateGUI", 0, 0.333f);
        dist = new float[5];
	}
	
	// Update is called once per frame
	void Update () {
        mainPos = main.transform.position;
        palmPos = palm.transform.position;
        handArea = fakeArea();
        handHorPos = distToAntenna();
	}
    void UpdateGUI()
    {
        nText.text = "N: " + handArea.ToString();
        xText.text = "X: " + handHorPos.ToString();
        leapSound.updateFrequency(handArea, handHorPos);
    }


    public float polygonArea(float []X, float[] Y,int numPoints)
    {
        float area = 0;         // Accumulates area in the loop
        int j = numPoints - 1;  // The last vertex is the 'previous' one to the first

        for (int i = 0; i < numPoints; i++)
        {
            area = area + (X[j] + X[i]) * (Y[j] - Y[i]);
            j = i;  //j is previous vertex to i
        }
        return area / 2;
    }
    public float fakeArea()
    {
        float fakeArea = 0;
        for (int i = 0; i < fingers.Length; i++)
        {
            dist[i] = Vector3.Distance(fingers[i].transform.position, mainPos);
            fakeArea += Vector3.Distance(fingers[i].transform.position, mainPos);
        }
        //CLAMP de 1 a 12 Y LUEGO = Math.floor(x) + math.smoothstep(x-math.floor(x))
        fakeArea = Mathf.Clamp(27.5f * fakeArea - 6.25f, 1.0f, 12.0f);
        float aux = fakeArea-Mathf.Floor(fakeArea);
        //aux*aux*aux*(10+aux*(6*aux-15)
        fakeArea = Mathf.Floor(fakeArea) + SmoothTanH01(aux);
        
        return fakeArea;
    }
    public float distToAntenna()
    {
        float tempDist = Mathf.Clamp(20*(antenna.transform.position.x - palmPos.x), 0, 10);
        float aux = tempDist - Mathf.Floor(tempDist);
        return 10 - (Mathf.Floor(tempDist) + SmoothTanH01(aux));
    }

    float SmoothTanH01(float decimalPart)
    {
        double aux = 0.5004 * System.Math.Tanh( ((double)decimalPart - 0.5) / 0.11);
        float aux2 = Mathf.Clamp(0.5f + (float)aux, 0.0f, 1.0f);
        if (aux2 <= 0.0f)
            aux2 = 0.0f;
        else if (aux2 >= 1.0f)
            aux2 = 1.0f;
        
        return aux2;
    }
}
