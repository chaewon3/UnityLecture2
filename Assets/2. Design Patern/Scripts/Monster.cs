using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyProject
{
    public class Monster : MonoBehaviour
    {
        public Renderer[] bodyrenderers;
        public Renderer[] eyeRenderers;

        public Color bodyDayColor;
        public Color eyeDayColor;

        public Color bodyNightColor;
        public Color eyeNightColor;

        private void Start()
        {
            //GameManager.instance.OnMonsterSpawn(this);
            GameManager.instance.onDayNightChange += OnDatNightChange;
            OnDatNightChange(GameManager.instance.isDay);
        }

        public void OnDestroy()
        {
            //GameManager.instance.OnMonsterSpawn(this);
            GameManager.instance.onDayNightChange -= OnDatNightChange;
        }

        public void OnDatNightChange(bool isDay)
        {
            if(isDay)
            {
                DayColor();
            }
            else
            {
                NightColor();
            }
        }

        //퍼사드 패턴
        public void DayColor()
        {
            foreach(Renderer r in bodyrenderers)
            {
                r.material.color = bodyDayColor;
            }
            foreach (Renderer r in eyeRenderers)
            {
                r.material.color = eyeDayColor;
            }

        }

        public void NightColor()
        {
            foreach (Renderer r in bodyrenderers)
            {
                r.material.color = bodyNightColor;
            }
            foreach (Renderer r in eyeRenderers)
            {
                r.material.color = eyeNightColor;
            }
        }
    }
}
