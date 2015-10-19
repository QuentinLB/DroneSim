using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

public class Pathfinder : MonoBehaviour {

    public Transform target;
    Vector3 storeTarget;
    bool savePos;
    bool overrideTarget;

    Vector3 acceleration;
    Vector3 velocity;
    public float maxSpeed = 5f;
    float storeMaxSpeed;
    float targetSpeed;

    Rigidbody rigidBody;

    public List<Vector3> EscapeDirections = new List<Vector3>();

	// Use this for initialization
	void Start () {
        storeMaxSpeed = maxSpeed;
        targetSpeed = storeMaxSpeed;
        rigidBody = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void FixedUpdate()
    {
        Debug.DrawLine(transform.position, target.position);
        Vector3 forces = MoveTowardsTarget(target.position);
        acceleration = forces;
        velocity += 2 * acceleration * Time.deltaTime;
        if (velocity.magnitude > maxSpeed)
        {
            velocity = velocity.normalized * maxSpeed;
        }

        rigidBody.velocity = velocity;

        Quaternion desiredRotation = Quaternion.LookRotation(velocity);
        transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, Time.deltaTime * 2);
    }

    Vector3 MoveTowardsTarget(Vector3 target)
    {
        Vector3 distance = target - transform.position;
        if (distance.magnitude < 25)
        {
            return distance.normalized * -maxSpeed;
        }
        else
        {
            return distance.normalized * maxSpeed;
        }
    }

    RaycastHit[] Rays(Vector3 direction, float offsetX)
    {
        Ray ray = new Ray(transform.position + new Vector3(offsetX, 0, 0), direction);
        Debug.DrawRay(transform.position + new Vector3(offsetX, 0, 0), direction*10*maxSpeed, Color.red);

        float distanceToLookAhead = maxSpeed * 10;
        RaycastHit[] hits = Physics.SphereCastAll(ray, 5, distanceToLookAhead);

        return hits;
    }
}
