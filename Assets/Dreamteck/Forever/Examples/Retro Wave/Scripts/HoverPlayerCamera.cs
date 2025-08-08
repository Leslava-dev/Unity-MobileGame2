namespace Dreamteck.Forever
{
    using UnityEngine;

    public class HoverPlayerCamera : MonoBehaviour
    {
        public Transform player;
        public Vector3 positionOffset;
        ProjectedPlayer projector;
        public float rotationSpeed = 10f;
        public float groundRaycastHeight = 100f;
        public float elevateSpeed = 5f;
        public float lowerSpeed = 1f;
        float currentOffset = 0f;
        float targetOffset = 0f;
        Transform trs;
        Transform playerTrs;

        private void Start()
        {
            projector = player.GetComponent<ProjectedPlayer>();
            trs = transform;
            playerTrs = player.transform;
        }

        private void FixedUpdate()
        {
            //Set the rotation of the camera between the player's path projection result and the direction towards the player
            trs.rotation = Quaternion.Slerp(Quaternion.LookRotation(player.position - trs.position), Quaternion.LookRotation(projector.result.forward, Vector3.up), 0.75f);
            //Create an inversed matrix from the projected result
            Matrix4x4 projectionMatrix = Matrix4x4.TRS(projector.result.position, projector.result.rotation, Vector3.one).inverse;
            //Get the player's local position to the projected result
            Vector3 localPos = projectionMatrix.MultiplyPoint3x4(playerTrs.position);
            //Offset the camera based on the positionOffset and the player's local position
            trs.position = projector.result.position - projector.result.forward * positionOffset.z + projector.result.right * (positionOffset.x + localPos.x * 0.85f) + projector.result.up * positionOffset.y;

            //Handle ground check to prevent clipping
            RaycastHit hit;
            float target = 0f;
            Vector3 camPosFlat = trs.position;
            camPosFlat.y = projector.result.position.y;
            Vector3 rayPos = camPosFlat + projector.result.up * groundRaycastHeight;
            if(Physics.Raycast(rayPos, -projector.result.up, out hit, groundRaycastHeight + 1f))
            {
                float dist = groundRaycastHeight - hit.distance;
                if (dist < groundRaycastHeight) target = dist;
            }
            if (targetOffset < target) targetOffset = target;
            else targetOffset = Mathf.MoveTowards(targetOffset, target, Time.deltaTime * lowerSpeed);
            if (currentOffset < targetOffset) currentOffset = Mathf.Lerp(currentOffset, targetOffset, Time.deltaTime * elevateSpeed);
            else currentOffset = Mathf.Lerp(currentOffset, targetOffset, Time.deltaTime * lowerSpeed);
            trs.position += projector.result.up * currentOffset;
        }
    }
}
