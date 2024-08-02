using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyProject;
using System;

public class GameManager : MonoBehaviour
{
    // 어떤걸 싱글톤 패턴으로 만들면 좋은가?
    // 한가지만 고민해보면 된다 : 단일 책임 원칙에 부합하는 녀석인가?
    // 게임 씬에 단 하나만 존재해야하는 경우
    // 플레이어 인풋매니저는 싱글톤으로
    public static GameManager instance { get; private set; }
    public Light light;

    public float dayLength = 5; // 밤낮의 길이
    public bool isDay = true;

    // 옵저버 패턴 : 특정 임무를 수행하는 객체에게
    // 상태 변화 또는 특정 이벤트의 호출 조건이 발생할 시 
    // 해당 이벤트 호출이 필요한 객체들이 "나도 상태 변하면 알려주세요." 라고 등록해놓는 형태의
    // 디자인 패턴

    private List<Monster> monsters = new(); //  구독자들

    //C#의 event는 옵저버 패턴 구현에 최적화된 구조로 만들어져 있으므로 
    // event를 활용하는 것만으로도 옵저버 패턴을 적용했다고 볼 수 있음.
    public event Action<bool> onDayNightChange;

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private float dayTemp;

    private void Update()
    {
        if(Time.time - dayTemp>dayLength)
        {
            dayTemp = Time.time;
            isDay = !isDay;
            light.gameObject.SetActive(isDay);
        }

        //옵저버가 구독자들에게 메시지를 보냄
        //foreach(Monster monster in monsters)
        //{
        //    monster.OnDatNightChange(isDay);
        //}

        onDayNightChange?.Invoke(isDay);
    }

    public void OnMonsterSpawn(Monster monster)
    {
        monsters.Add(monster);
        monster.OnDatNightChange(isDay);
    }

    public void OnMonsyterDespawn(Monster monster)
    {
        monsters.Remove(monster);
    }
}
