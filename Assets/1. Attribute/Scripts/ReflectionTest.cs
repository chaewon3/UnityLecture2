using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;

public class ReflectionTest : MonoBehaviour
{
    //reflection
    //system.Reflection네임스페이스에 포함된 기능
    //컴파일 타임에서 생성된, 클래스, 메소드, 멤버변수들의 데이터를 취급하는 class
    //Attribute는 특정 요소에 대한 메타데이터이므로, Reflection에 의해서 접근이 가능.

    private AttributeTest attTest;

    private void Awake()
    {
        attTest = GetComponent<AttributeTest>();
    }

    private void Start()
    {
        // 상위 클래스로 boxing 을 해도 원래 객체의 type 을 반환
        MonoBehaviour attTestBoxingForm = attTest;

        //Type attTestType = typeof(AttributeTest);
        Type attTestType = attTestBoxingForm.GetType();
        //print(attTestType);

        //AttributeTest라는 클래스의 데이터를 확인해보자

        BindingFlags bind = BindingFlags.NonPublic | BindingFlags.Instance;

        FieldInfo[] fis = attTestType.GetFields(); // 필드(멤버변수)
        print(fis.Length);
        foreach(FieldInfo fi in fis)
        {
            //if (fi.GetCustomAttribute<MyCustomAttribute>() == null)
            //    continue; // fieldInfo에 MyCustomAttribute 어트리뷰트가 부착되어 있지 않으면 건너뜀
            ////print(fi.Name);

            //MyCustomAttribute customAtt = fi.GetCustomAttribute<MyCustomAttribute>();

            //print($"Name : {fi.Name}, type : {fi.FieldType}, AttNAme : {customAtt.name}," +
            //    $"attValue : {customAtt.value}");

        }

        bind = BindingFlags.NonPublic | BindingFlags.Instance;
        MethodInfo someMi = attTestType.GetMethod("TestMethod"); //sendMessgae와 비슷함
        MethodInfo[] mis = attTestType.GetMethods(bind);
        foreach(MethodInfo mi in mis)
        {
            if (mi.GetCustomAttribute<MethodMessageAttribute>() == null)
                continue;

            var msgAtt = mi.GetCustomAttribute<MethodMessageAttribute>();

            print($"msg : {msgAtt.msg}");
            mi.Invoke(attTest,null);

        }
        //TestMethod의 MethodInfo 또는 MemberInfo를 탐색해서 MethodMessgaeAttribute.msg를 출력해보세요.

    }
}
