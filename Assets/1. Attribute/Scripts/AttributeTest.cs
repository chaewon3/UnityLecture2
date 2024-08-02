using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttributeTest : MonoBehaviour
{
    //Attribute (�Ӽ�, Ư��)
    //c#������ Attribute�� ��Ȯ�� �ǹ�: �ʵ�, �޼ҵ�� ����� ���� ��Ÿ�����͸� ������ �� �ִ� [Ŭ����]�̴�.
    //Attribute�� ���� �ۼ��ϱ� ���ؼ��� System.Attribute Ŭ������ ����ϰ�, Ŭ������ �ڿ� Attribute�� ���δ�.
    //Attribute Ŭ������ Ȱ���ϱ� ���ؼ��� Ư�� ���(Ŭ����, ����, �Լ�(propertie ����)���� ���� �տ� [Attribute �̸����� Attribute�� �� �̸�]

    private static int myStaticInt;
    private static int myStaticInt2;

    [MyCustom(name = "MyInteger", value = 1)] //[MyCustomAttribute]�� ����
    public int myInt; //��� ����

    [MyCustom] // MycustomAttribute�� �⺻ �����ڸ� ȣ���Ͽ� Attribute(��Ÿ������) ���� 
    public int myInt2;

    public string myString; // TextArea Attribute�� �������� ���� string �������
    [TextArea(minLines: 1,maxLines: 10)]
    public string myTextArea; // TextArea Attribute�� ������ string �������

    [MethodMessage("�̰� private �޼ҵ��Դϴ�.")]
    private void TestMethod()
    {
        print("��н����� Test��");
    }
}

public class MyCustomAttribute : Attribute
{
    public string name;
    public float value;

    public MyCustomAttribute()
    {
        name = "No Name";
        value = -1;
    }
}

//Method�� ���� Attribute
[AttributeUsage(AttributeTargets.Method)] //attribute�� ���������� ������ �� Ŭ���� �տ� ������ Attribute
public class MethodMessageAttribute : Attribute 
{
    public string msg;
    public MethodMessageAttribute(string msg)
    {
        this.msg = msg;
    }
}
