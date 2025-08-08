namespace Dreamteck.Forever
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using Dreamteck.Splines;

    public class MagicValleyPlayer : MonoBehaviour
    {
        public float initialSpeed = 5f;
        public float endSpeed = 5f;
        public float acceleration = 0.1f;
        public float initialJumpForce = 10f;
        public float continuousJumpForce = 1f;
        public float gravityForce = 20f;
        public float dieHeight = 5f;
        float speed = 0f;
        public LayerMask groundMask;
        public Transform cameraTransform;
        Rigidbody2D rb;
        bool inAir = true;
        Transform trs;
        Bounds colliderBounds = new Bounds();
        bool dead = false;
        bool ready = false;
        ProjectedPlayer projector;
        Vector3 camOffset = Vector3.zero;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            projector = GetComponent<ProjectedPlayer>();
            trs = transform;
            colliderBounds = GetComponent<CapsuleCollider2D>().bounds;
            colliderBounds.center = trs.InverseTransformPoint(colliderBounds.center);
            colliderBounds.size = trs.InverseTransformVector(colliderBounds.size);
            camOffset = cameraTransform.position - trs.position;
            EndScreen.onRestartClicked += OnRestart;
        }

        void OnRestart()
        {
            LevelGenerator.instance.Restart();
            ready = false;
        }

        private void Update()
        {
            if (!ready)
            {
                if (LevelGenerator.instance.ready)
                {
                    Revive();
                    ready = true;
                }
            }
            if (dead) return;
            if (!inAir && Input.GetKeyDown(KeyCode.Space))
            {
                rb.AddForce(Vector2.up * initialJumpForce, ForceMode2D.Impulse);
            }
            if (cameraTransform.position.y - transform.position.y >= dieHeight) Die();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (dead) return;
            if(Vector2.Dot(collision.contacts[0].normal, Vector2.left) >= 0.9f)
            {
                if(Physics2D.OverlapArea(trs.position + Vector3.up * colliderBounds.size.y * 0.4f + Vector3.right * colliderBounds.size.x * 0.55f,
                trs.position + Vector3.down * colliderBounds.size.y * 0.4f + Vector3.right * colliderBounds.size.x * 0.55f, groundMask)) Die(); //Die if we hit something in front
            }
        }

        void Revive()
        {
            dead = false;
            rb.linearVelocity = Vector2.zero;
            trs.rotation = Quaternion.identity;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            GetComponent<CapsuleCollider2D>().enabled = true;
            trs.position = LevelGenerator.instance.segments[0].customEntrance.position + Vector3.up * colliderBounds.size.y;
            rb.AddForce(Vector2.up * initialJumpForce * 0.5f, ForceMode2D.Impulse);
            speed = initialSpeed;
        }

        void Die()
        {
            if (dead) return;
            dead = true;
            rb.linearVelocity = Vector2.zero;
            rb.AddForce((Vector2.up + Vector2.left * 0.2f) * initialJumpForce * 2f, ForceMode2D.Impulse);
            rb.constraints = RigidbodyConstraints2D.None;
            rb.AddTorque(45f);
            GetComponent<CapsuleCollider2D>().enabled = false;
            EndScreen.Open();
        }

        private void FixedUpdate()
        {
            if (dead) return;
            speed = Mathf.MoveTowards(speed, endSpeed, Time.deltaTime * acceleration);
            inAir = !Physics2D.OverlapArea(trs.position + Vector3.down * colliderBounds.size.y * 0.5f + Vector3.left * colliderBounds.size.x * 0.5f,
                trs.position + Vector3.down * colliderBounds.size.y * 0.5f + Vector3.right * colliderBounds.size.x * 0.5f, groundMask);
            Vector2 velocity = rb.linearVelocity;
            velocity.x = speed;
            rb.linearVelocity = velocity;
            rb.AddForce(Vector2.down * gravityForce, ForceMode2D.Force);
            if (inAir && velocity.y > 0f)
            {
                if (Input.GetKey(KeyCode.Space)) rb.AddForce(Vector2.up * continuousJumpForce, ForceMode2D.Force);
            }
        }

        private void LateUpdate()
        {
            cameraTransform.position = projector.result.position + camOffset;
        }
    }
}
