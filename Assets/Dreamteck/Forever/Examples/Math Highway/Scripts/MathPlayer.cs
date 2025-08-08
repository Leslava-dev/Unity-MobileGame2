namespace Dreamteck.Forever
{
    using UnityEngine;

    public class MathPlayer : MonoBehaviour
    {
        LaneRunner runner;
        float boost = 0f;
        public static MathPlayer instance;
        bool canBoost = true;
        float speed = 0f;
        float startSpeed = 0f;
        public Color regularColor;
        public Color boostColor;
        Material material;

        private void Awake()
        {
            runner = GetComponent<LaneRunner>();
            startSpeed = speed = runner.followSpeed;
            instance = this;
            material = GetComponent<MeshRenderer>().sharedMaterial;
            MathGate.onAnswer += OnAnswer;
            EndScreen.onRestartClicked += OnRestart;
        }

        void OnRestart()
        {
            LevelGenerator.instance.Restart();
            runner.followSpeed = speed = startSpeed;
            boost = 0f;
            canBoost = true;
        }

        void OnAnswer()
        {
            canBoost = true;
            boost = 0f;
        }

        private void Update()
        {
            if (boost == 0f)
            {
                //Lane switching logic
                if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) runner.lane--;
                if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) runner.lane++;
                //Capture Boost Input
                if (Input.GetKeyDown(KeyCode.Space) && canBoost)
                {
                    boost = 1f;
                    canBoost = false;
                }
            }
            //Boosting logic
            if (boost > 0f) runner.followSpeed = speed + boost * speed * 2f;
            else runner.followSpeed = speed;
            Color col = Color.Lerp(regularColor, boostColor, boost);
            material.SetColor("_Color", col);
            material.SetColor("_EmissionColor", col);
            boost = Mathf.MoveTowards(boost, 0f, Time.deltaTime * speed * 0.075f);
        }

        public void SetSpeed(float speed)
        {
            this.speed = speed;
            runner.followSpeed = speed;
            if(speed == 0f) EndScreen.Open();
        }

        public float GetSpeed()
        {
            return speed;
        }
    }
}