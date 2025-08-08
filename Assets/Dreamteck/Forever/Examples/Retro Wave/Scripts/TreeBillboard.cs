namespace Dreamteck.Forever
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class TreeBillboard : MonoBehaviour
    {
        Transform trs;
        Transform cameraTrs;

        private void Awake()
        {
            trs = transform;
            cameraTrs = Camera.main.transform;
        }
        void LateUpdate()
        {
            Vector3 flatDir = cameraTrs.position - trs.position;
            trs.InverseTransformDirection(flatDir);
            flatDir.y = 0f;
            trs.TransformDirection(flatDir);
            trs.rotation = Quaternion.LookRotation(flatDir, trs.up);
        }
    }
}
