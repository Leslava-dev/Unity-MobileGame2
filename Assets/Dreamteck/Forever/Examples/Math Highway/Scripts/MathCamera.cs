namespace Dreamteck.Forever
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using Dreamteck.Splines;
    using System;

    public class MathCamera : MonoBehaviour
    {
        public LaneRunner runner;
        SplineSample evalResult = new SplineSample();
        Transform trs;
        public float cameraDistance = 10f;
        public float cameraHeight = 5f;
        public float cameraAngle = 10f;
        Quaternion rotationSmooth = Quaternion.identity;

        void Awake()
        {
            trs = transform;
        }



        void LateUpdate()
        {
            if (!LevelGenerator.instance.ready)
            {
                return;
            }
            RunLogic();
        }

        private void RunLogic()
        {
            int runnerSegmentIndex = 0;
            for (int i = 0; i < LevelGenerator.instance.segments.Count; i++)
            {
                if (LevelGenerator.instance.segments[i] == runner.segment)
                {
                    runnerSegmentIndex = i;
                    break;
                }
            }
            double globalPercent = LevelGenerator.instance.LocalToGlobalPercent(runner.result.percent, runnerSegmentIndex);
            LevelGenerator.instance.Evaluate(LevelGenerator.instance.Travel(globalPercent, cameraDistance, Spline.Direction.Backward), ref evalResult);
            trs.position = evalResult.position + Vector3.up * cameraHeight;
            rotationSmooth = Quaternion.Slerp(rotationSmooth, Quaternion.Slerp(evalResult.rotation, runner.result.rotation, Time.deltaTime * 0.5f), Time.deltaTime * 2f);
            trs.rotation = rotationSmooth * Quaternion.AngleAxis(cameraAngle, Vector3.right);
        }
    }
}
