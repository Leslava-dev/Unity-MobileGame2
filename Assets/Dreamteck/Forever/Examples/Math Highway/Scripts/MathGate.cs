namespace Dreamteck.Forever
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class MathGate : Builder
    {
        public delegate void EmptyHandler();
        public static event EmptyHandler onAnswer;
        public Material regularMaterial;
        public Material correctMaterial;
        public Material wrongMaterial;
        public float displayDistance = 20f;
        public int minOperandA = 1;
        public int maxOperandA = 30;
        public int minOperandB = 0;
        public int maxOperandB = 20;
        public float speedIncreasePerRightAnswer = 2f;
        public TextMesh questionText;
        MathTab[] tabs = new MathTab[0];
        Transform cameraTrs;
        float lastAlpha = 0f;
        float alpha = 0f;
        [System.NonSerialized]
        public bool answered = false;

        protected override void Build()
        {
            base.Build();
            //Come up with the math question and random answers
            cameraTrs = Camera.main.transform;
            tabs = GetComponentsInChildren<MathTab>();
            int a = Random(minOperandA, maxOperandA + 1);
            int b = Random(minOperandB, maxOperandB + 1);
            int operation = Random(0, 3);
            string[] operations = new string[] { "+", "-", "*"};
            questionText.text = a + " " + operations[operation] + " " + b;
            float answer = 0f;
            switch (operation)
            {
                case 0: answer = a + b; break;
                case 1: answer = a - b; break;
                case 2: answer = a * b; break;
            }
            answer = Mathf.Round(answer * 10f) / 10f;
            int correctTab = Random(0, tabs.Length);
            for (int i = 0; i < tabs.Length; i++)
            {
                tabs[i].gate = this;
                if(i == correctTab) tabs[i].Initialize(answer.ToString(), true);
                else
                {
                    int randomAnswer = Random(-20, 99);
                    if (randomAnswer == answer) randomAnswer += Random(1, 11) * (Random(0, 2) == 0 ? -1 : 1);
                    tabs[i].Initialize(randomAnswer.ToString(), false);
                }
                tabs[i].SetMaterial(regularMaterial);
                tabs[i].SetAlpha(0f);
            }
            SetTextAlpha(questionText, 0f);
        }

        private void Update()
        {
            if (!isDone) return;
            if (!answered)
            {
                //Make the texts appear if the question is not answered
                if (Vector3.Distance(trs.position, cameraTrs.position) <= displayDistance) alpha = Mathf.MoveTowards(alpha, 1f, Time.deltaTime * MathPlayer.instance.GetSpeed() * 0.1f);
                else alpha = Mathf.MoveTowards(alpha, 0f, Time.deltaTime);
            } else
            {
                alpha = Mathf.MoveTowards(alpha, 0f, Time.deltaTime * 0.1f);
            }
            if(alpha != lastAlpha)
            {
                SetTextAlpha(questionText, alpha);
                for (int i = 0; i < tabs.Length; i++) tabs[i].SetAlpha(alpha);
                lastAlpha = alpha;
            }
        }

        void SetTextAlpha(TextMesh tm, float alpha) {
            Color col = tm.color;
            col.a = alpha;
            tm.color = col;
        }

        public void OnWrongAnswer()
        {
            for (int i = 0; i < tabs.Length; i++)
            {
                if (tabs[i].isCorrect) tabs[i].SetMaterial(correctMaterial);
                else tabs[i].SetMaterial(wrongMaterial);
            }
            answered = true;
            //Divide the player's speed by two
            MathPlayer.instance.SetSpeed(MathPlayer.instance.GetSpeed()/ 2f);
            //If the speed is less or equal to 5, stop the player
            if (MathPlayer.instance.GetSpeed() <= 5f) MathPlayer.instance.SetSpeed(0f);
            if (onAnswer != null) onAnswer();
        }

        public void OnCorrectAnswer()
        {
            for (int i = 0; i < tabs.Length; i++)
            {
                if (tabs[i].isCorrect) tabs[i].SetMaterial(correctMaterial);
            }
            answered = true;
            //Increase the speed of the player
            MathPlayer.instance.SetSpeed(MathPlayer.instance.GetSpeed() + speedIncreasePerRightAnswer);
            if (onAnswer != null) onAnswer();
        }
    }
}
