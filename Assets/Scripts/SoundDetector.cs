using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundDetector : MonoBehaviour
{
    public static SoundDetector singleton;
    public AudioSource currentAudio;
    private float[] samples = new float[512];
    public float[] freqBand = new float[8];

    // Start is called before the first frame update
    void Start()
    {
        singleton = this;
        currentAudio = SoundHandler.singleton.currentSource;
    }

    // Update is called once per frame
    void Update()
    {
        if(currentAudio != null)
        {
            currentAudio.GetSpectrumData(samples, 0, FFTWindow.Blackman);
        }
        MakeFrequencyBands();
    }

    private void MakeFrequencyBands()
    {
        int count = 0;

        for (int i = 0; i < 8; i++)
        {
            float average = 0;
            int sampleCount = (int)Mathf.Pow(2, i) * 2;

            if(i == 7)
            {
                sampleCount += 2;
            }

            for (int j = 0; j < sampleCount; j++)
            {
                average += samples[count] * (count + 1);
                count++;
            }
            average /= count;

            freqBand[i] = average * 10;
        }

        //if(freqBand[0] >= 1f)
        //{
        //    Debug.Log("bass");
        //}
    }
}
