using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MusiscScript : MonoBehaviour {

    public Slider slider;
    public AudioSource musisc;

    private void Start()
    {
        slider.value = 1;
    }
    // Update is called once per frame
    void Update () {
		musisc.volume=slider.value;
	}
}
