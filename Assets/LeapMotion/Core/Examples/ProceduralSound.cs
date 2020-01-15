using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralSound : MonoBehaviour {

    public float frequency1;
    public float amplitude1;
    public GameObject leftHand;
    int timeIndex = 0;

    void Update()
    {
        amplitude1 = Mathf.Clamp01(leftHand.transform.position.y * 1.25f + 0.25f);
    }
    void OnAudioFilterRead(float[] data, int channels)
    {
        for (int i = 0; i < data.Length; i += channels)
        {
            float FMfreq2 = CreateSine(timeIndex, frequency1, 44100, amplitude1);
            data[i] = FMfreq2;

            if (channels == 2)
                data[i + 1] = data[i];

            timeIndex++;
        }
    }
    public float CreateSine(int timeIndex, float frequency, float sampleRate, float amplitude)
    {
        return Mathf.Sin(2 * Mathf.PI * timeIndex * frequency / sampleRate) * amplitude;
    }
    public void updateFrequency(float m_N, float m_X)
    {
        frequency1 = Mathf.Clamp(440*Mathf.Exp( ( (m_X-4) + (m_N - 10) / 12 ) * 0.69314718056f ), 15, 22050 );
    }
}
