using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SO;
namespace garagekitgames
{
    public class DifficultyController : MonoBehaviour
    {
        public AnimationCurve difficultyCurve;
        public float maxTimeInMinutes;
        private float timeMultiplier = 60;
        public float currentValue;
        // Use this for initialization
        public float difficultyCounter;

        public FloatVariable difficultyMultiplier;
        public FloatVariable gameTime;

        public bool stopDoingShit;
        /*public LevelData currentLevelData;
        public EnemyGroup[] enemyGroups;*/

        void Start()
        {
            difficultyCounter = 0;

            //enemyGroups = currentLevelData.enemyGroups.ToArray();
            //maxTimeInMinutes = difficultyCurve[difficultyCurve.length - 1].time;

        }

        public void SetStopDoingShit(bool value)
        {
            this.stopDoingShit = value;
        }

        public void SetMaxTimeInMinutes(float value)
        {
            maxTimeInMinutes = value;
        }

        public void ResetDifficulty()
        {
            difficultyCounter = 0;
        }

        public void HalfDifficulty()
        {
            difficultyCounter = difficultyCounter * 0.5f;
        }

        public void ThreeFourthDifficulty()
        {
            difficultyCounter = difficultyCounter * 0.75f;
        }

        // Update is called once per frame
        void Update()
        {
            if(stopDoingShit)
            {
                return;
            }

            maxTimeInMinutes = TestManager.Instance.GetCurrentEnemyGroupMaxDifficultyTime();

            if ((difficultyCounter / timeMultiplier) <= maxTimeInMinutes)
            {
                //difficultyCounter += Time.deltaTime;
                //currentValue = difficultyCurve.Evaluate();
                /*for (int i = 0; i < difficultyCurve.length; i++)
                {

                }*/
                //while (difficultyCounter < maxTime)
                //{
                currentValue = difficultyCurve.Evaluate(difficultyCounter / (maxTimeInMinutes * timeMultiplier));
                //float val = curve.Evaluate(timeCounter);
                // Debug.Log("currentValue " + currentValue + "  time " + difficultyCounter);
                // have to find a better solution for treshold sometimes it misses(use Update?)!

                difficultyCounter += Time.deltaTime;
                gameTime.value = difficultyCounter;
                if (currentValue == 0)
                    return;

                else
                    difficultyMultiplier.value = currentValue;
            }
            else
            {
                //max difficulty reached

            }
            



            //}
        }
    }
}

