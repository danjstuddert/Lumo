using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
    public float moveSpeed;

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

	private Rigidbody rBody;
	private Vector3 velocity;

    private bool isExpanding;
    private FieldOfView fieldOfView;
    private float index;

	void Start () {
		rBody = GetComponent<Rigidbody> ();

        fieldOfView = GetComponent<FieldOfView>();
        fieldOfView.Init(viewRadius);

        viewLight = GetComponentInChildren<Light>();
        UpdateLightRadius(viewRadius);
	}

	void Update () {
        UpdateVelocity();
        CheckInput();
        UpdateFlicker();
	}

    void FixedUpdate() {
        rBody.MovePosition(rBody.position + velocity * Time.fixedDeltaTime);
    }

    private void UpdateVelocity() {
        //Need to update this to a better controller at some point, feels not quite right
        velocity = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized * moveSpeed;
    }

    private void CheckInput() {
        if(Input.GetButtonDown("Light") && isExpanding == false)
            StartCoroutine("ExpandLight");
    }

    private void UpdateFlicker() {
        if (isExpanding)
            return;

        index += Time.deltaTime;
        float flickerAmount = Mathf.Abs(viewRadius + (amplitude * Mathf.Sin(speed * index)));
        fieldOfView.UpdateViewRadius(flickerAmount);
        UpdateLightRadius(flickerAmount);
    }

    private void UpdateLightRadius(float newAmount){
        viewLight.range = newAmount * lightRangeScale;
        viewLight.intensity = newAmount * lightIntensityScale;

        if (viewLight.intensity > maxIntensity)
            viewLight.intensity = maxIntensity;
    }

    private IEnumerator ExpandLight(){
        isExpanding = true;

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

        isExpanding = false;
    }
}