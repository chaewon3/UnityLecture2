using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//MonoBehaviour�� ������� ���� �Ϲ����� ��ü�� �̱��� ���� ���� ���
public class NonMonoBehaviourSingleton 
{
    // ���� instance �ʵ�� private staitc�ʵ�� ����
    private static NonMonoBehaviourSingleton instance;
    // �ܺ� ��ü�� instance�� �������� ���ؼ��� Getter�޼ҵ� �Ǵ� 
    // C# Get ���� property�� ���� �б��������� ������ �� �ֵ��� ��.
    public static NonMonoBehaviourSingleton Instance
    {
        get
        {
            if (instance == null)
                instance = new NonMonoBehaviourSingleton();
            return instance;
        }
    }

    //�ٸ� ��ü���� �����ڸ� ȣ������ ���ϵ��� �⺻ �������� ���������ڸ� private���� ��ȣ
    private NonMonoBehaviourSingleton() { }

    public static NonMonoBehaviourSingleton Getinstance()
    {
        if (instance == null)
            instance = new NonMonoBehaviourSingleton();
        return instance;
    }
}
