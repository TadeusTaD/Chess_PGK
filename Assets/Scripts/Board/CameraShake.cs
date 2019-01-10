using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour {

    public static float power = 0.3f;
    public float duration = 1.0f;
    public Transform camera;
    public float slowDownAmount = 1.0f;
    public static bool shakeEnabled = false;

    Vector3 startPos;
    float initDuration;

	// Use this for initialization
	void Start () {
        camera = Camera.main.transform;
        startPos = camera.localPosition;
        initDuration = duration;

	}
	
	// Update is called once per frame
	void Update () {
		if (shakeEnabled)
        {
            if (duration >0.1f)
            {
                camera.localPosition = startPos + Random.insideUnitSphere * power;
                duration -= Time.deltaTime * slowDownAmount + 0.12f;
            }
            else
            {
                shakeEnabled = false;
                duration = initDuration;
                camera.localPosition = startPos;
            }
        }
    }
}
