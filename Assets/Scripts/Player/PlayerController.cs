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

	Rigidbody myRigidbody;
	Camera viewCamera;
	Vector3 velocity;

    private bool isExpanding;
    private FieldOfView fieldOfView;
    private float index;

	void Start () {
		myRigidbody = GetComponent<Rigidbody> ();
		viewCamera = Camera.main;

        fieldOfView = GetComponent<FieldOfView>();
        fieldOfView.Init(viewRadius);
	}

	void Update () {
        UpdateVelocity();
        CheckInput();
        UpdateFlicker();
	}

    void FixedUpdate() {
        myRigidbody.MovePosition(myRigidbody.position + velocity * Time.fixedDeltaTime);
    }

    private void UpdateVelocity() {
        //Need to update this to a better controller at some point, feels not quite right
        velocity = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized * moveSpeed;
    }

    private void CheckInput() {
        if(Input.GetButtonDown("Light") && isExpanding == false){
            StartCoroutine("ExpandLight");
        }
    }

    private void UpdateFlicker() {
        if (isExpanding)
            return;

        index += Time.deltaTime;
        fieldOfView.UpdateViewRadius(Mathf.Abs(viewRadius + (amplitude * Mathf.Sin(speed * index))));
    }

    private IEnumerator ExpandLight(){
        isExpanding = true;

        float currentLerpTime = 0f;
        float originalViewRadius = fieldOfView.CurrentViewRadius;

        while(currentLerpTime <= expandTime){
            currentLerpTime += Time.deltaTime;

            float t = expandCurve.Evaluate(currentLerpTime / expandTime);

            fieldOfView.UpdateViewRadius(Mathf.Lerp(originalViewRadius, expandedRadius, t));
            yield return null;
        }

        fieldOfView.UpdateViewRadius(expandedRadius);

        currentLerpTime = 0f;
        while (currentLerpTime <= retractTime) {
            currentLerpTime += Time.deltaTime;

            float t = retractCurve.Evaluate(currentLerpTime / retractTime);

            fieldOfView.UpdateViewRadius(Mathf.Lerp(expandedRadius, originalViewRadius, t));
            yield return null;
        }

        fieldOfView.UpdateViewRadius(originalViewRadius);

        isExpanding = false;
    }
}