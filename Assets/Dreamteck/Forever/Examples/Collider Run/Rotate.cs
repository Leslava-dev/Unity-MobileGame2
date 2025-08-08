namespace Dreamteck.Forever
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class Rotate : MonoBehaviour
    {
        public Vector3 speed;
        Transform trs;
        // Start is called before the first frame update
        void Start()
        {
            trs = transform;
        }

        // Update is called once per frame
        void Update()
        {
            trs.Rotate(speed * Time.deltaTime, Space.Self);
        }
    }
}
