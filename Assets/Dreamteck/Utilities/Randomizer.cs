namespace Dreamteck.Utilities
{
    using UnityEngine;

    public class Randomizer
    {
        private int _seed;
        private System.Random _random;

        public System.Random random => _random;

        public Randomizer(int seed)
        {
            _seed = seed;
            _random = new System.Random(_seed);
        }

        public float Random01()
        {
            return (float)_random.NextDouble();
        }

        public float Random(float min, float max)
        {
            return (float)DMath.Lerp(min, max, _random.NextDouble());
        }

        public int Random(int min, int max)
        {
            return (int)DMath.Lerp(min, max, _random.NextDouble());
        }

        public Vector2 RandomVector2(float min, float max)
        {
            return new Vector2(Random(min, max), Random(min, max));
        }

        public Vector3 RandomVector3(float min, float max)
        {
            return new Vector3(Random(min, max), Random(min, max), Random(min, max));
        }

        public Vector3 OnUnitSphere()
        {
            return Quaternion.Euler(Random(0f, 360f), Random(0f, 360f), Random(0f, 360f)) * Vector3.forward;
        }

        public Vector3 OnUnitCircle()
        {
            return Quaternion.AngleAxis(Random(0f, 360f), Vector3.forward) * Vector3.up;
        }

        public Vector3 InsideUnitSphere()
        {
            return OnUnitSphere() * Random01();
        }

        public Vector3 InsideUnitCircle()
        {
            return OnUnitCircle() * Random01();
        }

        public void Reset()
        {
            _random = new System.Random(_seed);
        }

        public void Reset(int seed)
        {
            _random = new System.Random(seed);
        }
    }
}