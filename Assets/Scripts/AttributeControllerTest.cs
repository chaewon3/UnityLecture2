using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Reflection;
using System;

public class AttributeControllerTest : MonoBehaviour
{
    [Color(0,1,0,1)]
    public new Renderer renderer;

    [SerializeField,Color(r:1,b:0.5f)]
    private Graphic graphic;


    [Size(3, sizeType.scale, sizeType2.relative)]
    public Transform cubeTransform;

    [SerializeField, Size(200,100,0 ,sizeType.size)]
    private RectTransform ImageTransform;

    //[Color]
    //public float notRendererOrGraphic;
}

public class Test : AttributeControllerTest
{
    private void Start()
    {
        renderer.enabled = true;
    }
}
