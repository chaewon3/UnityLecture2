using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyProject;
using System;

public class GameManager : MonoBehaviour
{
    // ��� �̱��� �������� ����� ������?
    // �Ѱ����� ����غ��� �ȴ� : ���� å�� ��Ģ�� �����ϴ� �༮�ΰ�?
    // ���� ���� �� �ϳ��� �����ؾ��ϴ� ���
    // �÷��̾� ��ǲ�Ŵ����� �̱�������
    public static GameManager instance { get; private set; }
    public Light light;

    public float dayLength = 5; // �㳷�� ����
    public bool isDay = true;

    // ������ ���� : Ư�� �ӹ��� �����ϴ� ��ü����
    // ���� ��ȭ �Ǵ� Ư�� �̺�Ʈ�� ȣ�� ������ �߻��� �� 
    // �ش� �̺�Ʈ ȣ���� �ʿ��� ��ü���� "���� ���� ���ϸ� �˷��ּ���." ��� ����س��� ������
    // ������ ����

    private List<MonsterColor> monsterColors = new(); //  �����ڵ�
    private List<Monster> monsters = new();

    //C#�� event�� ������ ���� ������ ����ȭ�� ������ ������� �����Ƿ� 
    // event�� Ȱ���ϴ� �͸����ε� ������ ������ �����ߴٰ� �� �� ����.
    public event Action<bool> onDayNightChange;
    public event Action<bool> GameOver;

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

        //�������� �����ڵ鿡�� �޽����� ����
        //foreach(Monster monster in monsters)
        //{
        //    monster.OnDatNightChange(isDay);
        //}

        onDayNightChange?.Invoke(isDay);
    }

    public void PlayerDeadth()
    {
        GameOver?.Invoke(true);
    }

    public void OnMonsterSpawn(Monster monster)
    {
        //monsterColors.Add(monster);
        monsters.Add(monster);
        //monster.OnDatNightChange(isDay);
    }

    public void OnMonsyterDespawn(Monster monster)
    {
        monsters.Remove(monster);
        //monsterColors.Remove(monster);
    }
}
