using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class visualizerScaler : MonoBehaviour
{
    [SerializeField] private float startScale;
    [SerializeField] private float multiplier;

    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = Vector3.one * startScale;
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = Vector3.one * (SoundDetector.singleton.freqBand[0] * multiplier + startScale);
    }
}
