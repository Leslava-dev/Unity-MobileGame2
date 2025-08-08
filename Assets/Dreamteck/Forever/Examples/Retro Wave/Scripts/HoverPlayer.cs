namespace Dreamteck.Forever
{
    using UnityEngine;
    using System.Collections;

    public class HoverPlayer : MonoBehaviour
    {
        public float gravityForce = 9.81f;
        public float maxSpeed = 90f;
        public float acceleration = 10f;
        public AnimationCurve accelerationCurve;
        public float brakeForce = 10f;
        public float agility = 5f;
        public float turnSpeed = 5f;
        public float hoverForce = 65f;
        public float maxHoverSpeed = 5f;
        public float hoverHeight = 3.5f;
        public AnimationCurve hoverCurve;
        [Range(0f, 2f)]
        public float stabilizerStrenght = 0.5f;
        private float forwardInput;
        private float sidewaysInput;
        private Rigidbody rb;
        ProjectedPlayer projector;

        public Transform[] engines;
        Transform trs;
        float deathTime = 3f;
        Vector3 startingPosition;


        void Awake()
        {
            rb = GetComponent<Rigidbody>();
            trs = transform;
            startingPosition = trs.position;
            projector = GetComponent<ProjectedPlayer>();
            EndScreen.onRestartClicked += OnRestart;
        }

        void OnRestart()
        {
            deathTime = 3f;
            trs.position = startingPosition;
            trs.rotation = Quaternion.identity;
            LevelGenerator.instance.Restart();
        }

        void Update()
        {
            //Get input
            forwardInput = Input.GetAxis("Vertical");
            sidewaysInput = Input.GetAxis("Horizontal");
        }

        void FixedUpdate()
        {
            for (int i = 0; i < engines.Length; i++)
            {
                Ray ray = new Ray(engines[i].position, -engines[i].up);
                RaycastHit hit;
                float forceRatio = 0f;
                //Raycast from each engine and apply hover force
                if (Physics.Raycast(ray, out hit, hoverHeight))
                {
                    Matrix4x4 hitMatrix = Matrix4x4.TRS(hit.point, Quaternion.LookRotation(projector.result.forward, hit.normal), Vector3.one);
                    Vector3 engineVelocity = rb.GetPointVelocity(engines[i].position);
                    Vector3 localHoverVelocity = hitMatrix.inverse.MultiplyVector(engineVelocity);

                    forceRatio = hoverCurve.Evaluate( (hoverHeight - hit.distance) / hoverHeight);
                    Vector3 appliedHoverForce = engines[i].up * forceRatio * hoverForce;
                    if(localHoverVelocity.y < Mathf.Clamp(rb.linearVelocity.magnitude * 0.15f, 0.5f, 2f)) rb.AddForceAtPosition(appliedHoverForce, engines[i].position, ForceMode.Force);
                    if (localHoverVelocity.y > 0f && forceRatio < 0.6f)
                    {
                        rb.AddForceAtPosition(-engines[i].up * localHoverVelocity.y * Mathf.InverseLerp(0.6f, 0f, forceRatio) * stabilizerStrenght, engines[i].position, ForceMode.Force);
                    }
                }
            }
            //Create a matrix from the current projected result
            Matrix4x4 resultMatrix = Matrix4x4.TRS(projector.result.position, Quaternion.LookRotation(projector.result.forward, Vector3.up), Vector3.one);
            //Set the rotation of the player to match the path direction
            //but also retain the local roll and pitch 
            Vector3 resultLocalPlayerForward = resultMatrix.inverse.MultiplyVector(trs.forward);
            resultLocalPlayerForward.x = 0f;
            resultLocalPlayerForward.Normalize();
            rb.MoveRotation(Quaternion.LookRotation(resultMatrix.MultiplyVector(resultLocalPlayerForward), trs.up));
            
            //Handle player physics forces
            Vector3 localTorque = trs.InverseTransformDirection(rb.angularVelocity);
            localTorque.y = 0f;
            rb.angularVelocity = trs.TransformDirection(localTorque);
            Vector3 localVelocity = trs.InverseTransformDirection(rb.linearVelocity);
            if (forwardInput > 0f && localVelocity.z < maxSpeed)  rb.AddForce(projector.result.forward * acceleration * forwardInput * accelerationCurve.Evaluate(localVelocity.z / maxSpeed), ForceMode.Force);
            else if(forwardInput < 0f && localVelocity.z > 0f) rb.AddForce(-projector.result.forward * brakeForce * -forwardInput, ForceMode.Force);
            if((sidewaysInput > 0f && localVelocity.x < turnSpeed) || (sidewaysInput < 0f && localVelocity.x > -turnSpeed))
            rb.AddRelativeForce(Vector3.right * sidewaysInput * agility, ForceMode.Force);
            rb.AddForce(Vector3.down * gravityForce, ForceMode.Acceleration);
            localVelocity = resultMatrix.inverse.MultiplyVector(rb.linearVelocity);
            localVelocity.z = Mathf.Clamp(localVelocity.z, -maxSpeed, maxSpeed);
            localVelocity.x = Mathf.Clamp(localVelocity.x, -turnSpeed, turnSpeed);
            rb.linearVelocity = resultMatrix.MultiplyVector(localVelocity);
        }

        private void OnCollisionStay(Collision collision)
        {
            if (Vector3.Dot(trs.up, Vector3.up) < 0.5f)
            {
                deathTime -= Time.deltaTime;
                if(deathTime <= 0f)
                {
                    EndScreen.Open();
                }
            }
        }
    }
}
