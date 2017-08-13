using UnityEngine;
using System.Collections;

[RequireComponent(typeof(FieldOfView))]
[RequireComponent(typeof(ShakeChild))]
[RequireComponent(typeof(ViewArea))]
public class PlayerController : MonoBehaviour {
    public float moveSpeed;

    [Header("Sprite")]
    public float shakeDuration;
    public float shakeMagnitude;

	private Rigidbody rBody;
	private Vector3 velocity;

    private ViewArea view;
    private FieldOfView fieldOfView;
    private ShakeChild shake;

	void Start () {
		rBody = GetComponent<Rigidbody> ();

        view = GetComponent<ViewArea>();
        view.Init();

        fieldOfView = GetComponent<FieldOfView>();

        shake = GetComponent<ShakeChild>();
	}

	void Update () {
        UpdateVelocity();
        CheckInput();
	}

    void FixedUpdate() {
        rBody.MovePosition(rBody.position + velocity * Time.fixedDeltaTime);
    }

    private void UpdateVelocity() {
        //Need to update this to a better controller at some point, feels not quite right
        velocity = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized * moveSpeed;
    }

    private void CheckInput() {
        if (Input.GetButtonDown("Light") && view.IsExpanding == false){
            shake.Shake(shakeDuration, shakeMagnitude);
            view.StartExpand();
        }  
    }
}