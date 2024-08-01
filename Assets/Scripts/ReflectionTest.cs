using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;

public class ReflectionTest : MonoBehaviour
{
    //reflection
    //system.Reflection���ӽ����̽��� ���Ե� ���
    //������ Ÿ�ӿ��� ������, Ŭ����, �޼ҵ�, ����������� �����͸� ����ϴ� class
    //Attribute�� Ư�� ��ҿ� ���� ��Ÿ�������̹Ƿ�, Reflection�� ���ؼ� ������ ����.

    private AttributeTest attTest;

    private void Awake()
    {
        attTest = GetComponent<AttributeTest>();
    }

    private void Start()
    {
        // ���� Ŭ������ boxing �� �ص� ���� ��ü�� type �� ��ȯ
        MonoBehaviour attTestBoxingForm = attTest;

        //Type attTestType = typeof(AttributeTest);
        Type attTestType = attTestBoxingForm.GetType();
        //print(attTestType);

        //AttributeTest��� Ŭ������ �����͸� Ȯ���غ���

        BindingFlags bind = BindingFlags.NonPublic | BindingFlags.Instance;

        FieldInfo[] fis = attTestType.GetFields(); // �ʵ�(�������)
        print(fis.Length);
        foreach(FieldInfo fi in fis)
        {
            //if (fi.GetCustomAttribute<MyCustomAttribute>() == null)
            //    continue; // fieldInfo�� MyCustomAttribute ��Ʈ����Ʈ�� �����Ǿ� ���� ������ �ǳʶ�
            ////print(fi.Name);

            //MyCustomAttribute customAtt = fi.GetCustomAttribute<MyCustomAttribute>();

            //print($"Name : {fi.Name}, type : {fi.FieldType}, AttNAme : {customAtt.name}," +
            //    $"attValue : {customAtt.value}");

        }

        bind = BindingFlags.NonPublic | BindingFlags.Instance;
        MethodInfo someMi = attTestType.GetMethod("TestMethod"); //sendMessgae�� �����
        MethodInfo[] mis = attTestType.GetMethods(bind);
        foreach(MethodInfo mi in mis)
        {
            if (mi.GetCustomAttribute<MethodMessageAttribute>() == null)
                continue;

            var msgAtt = mi.GetCustomAttribute<MethodMessageAttribute>();

            print($"msg : {msgAtt.msg}");
            mi.Invoke(attTest,null);

        }
        //TestMethod�� MethodInfo �Ǵ� MemberInfo�� Ž���ؼ� MethodMessgaeAttribute.msg�� ����غ�����.

    }
}
