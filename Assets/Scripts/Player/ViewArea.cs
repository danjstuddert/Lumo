using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FieldOfView))]
public class ViewArea : MonoBehaviour {
    [Header("View Area")]
    public float viewRadius;
    public float expandedRadius;
    public float expandTime;
    public AnimationCurve expandCurve;
    public float retractTime;
    public AnimationCurve retractCurve;

    [Header("View Flickering")]
    public float amplitude;
    public float speed;

    [Header("Light")]
    public Light viewLight;
    public float lightRangeScale;
    public float lightIntensityScale;
    public float maxIntensity;

    public bool IsExpanding { get; private set; }

    private float index;

    private FieldOfView fieldOfView;

    public void Init(){
        UpdateLightRadius(viewRadius);
        fieldOfView = GetComponent<FieldOfView>();
		fieldOfView.Init(viewRadius);
    }

    void Update() {
        UpdateFlicker();
    }

    public void StartExpand(){
        if (IsExpanding == false)
            StartCoroutine("ExpandLight");
    }

    private void UpdateFlicker() {
        index += Time.deltaTime;
        float flickerAmount = Mathf.Abs(viewRadius + (amplitude * Mathf.Sin(speed * index)));
        fieldOfView.UpdateViewRadius(flickerAmount);
        UpdateLightRadius(flickerAmount);
    }

    private void UpdateLightRadius(float newAmount) {
        viewLight.range = newAmount * lightRangeScale;
        viewLight.intensity = newAmount * lightIntensityScale;

        if (viewLight.intensity > maxIntensity)
            viewLight.intensity = maxIntensity;
    }

    private IEnumerator ExpandLight(){
        IsExpanding = true;

        float currentLerpTime = 0f;
        float originalViewRadius = fieldOfView.CurrentViewRadius;

        while(currentLerpTime <= expandTime){
            currentLerpTime += Time.deltaTime;

            float t = expandCurve.Evaluate(currentLerpTime / expandTime);
            float lerpAmount = Mathf.Lerp(originalViewRadius, expandedRadius, t);

            fieldOfView.UpdateViewRadius(lerpAmount);
            UpdateLightRadius(lerpAmount);
            yield return null;
        }

        fieldOfView.UpdateViewRadius(expandedRadius);

        currentLerpTime = 0f;
        while (currentLerpTime <= retractTime) {
            currentLerpTime += Time.deltaTime;

            float t = retractCurve.Evaluate(currentLerpTime / retractTime);
            float lerpAmount = Mathf.Lerp(expandedRadius, originalViewRadius, t);

            fieldOfView.UpdateViewRadius(lerpAmount);
            UpdateLightRadius(lerpAmount);
            yield return null;
        }

        fieldOfView.UpdateViewRadius(originalViewRadius);

        IsExpanding = false;
    }
}
