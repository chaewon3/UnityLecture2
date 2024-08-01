using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;
using UnityEngine.UI;

public class AttributeController : MonoBehaviour
{
    private void Start()
    {
        //ColorAttribute�� ���� �ʵ带 ã�´�.
        //BindingFlags : public�̰ų� private ��� ���� static�� �ƴ� ���� �Ҵ� ����� Ž��.
        BindingFlags bind = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
        MonoBehaviour[] monoBehaviours = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None);
        foreach (MonoBehaviour monoBehaviour in monoBehaviours)
        {
            Type type = monoBehaviour.GetType();

            //Ư�� �ݷ���(�迭, ����Ʈ)���� ���ǿ� �����ϴ� ��Ҹ� �������� �� ���,
            //foreach �Ǵ� List.Find�� �ټ� ������ ������ ���ľ� ��.
            //List<FieldInfo> fields = new List<FieldInfo>(type.GetFields(bind));
            //fields.FindAll(null);

            //Linq ������ Ȱ���ϸ� �̸� ����ȭ �� �� ����.
            //1. Linq�� ���ǵ� Ȯ�� �޼��� �̿��ϴ� ���
            IEnumerable<FieldInfo> colorAttachedFields = type.GetFields(bind).Where(field => field.GetCustomAttribute<ColorAttribute>() != null); ;

            //2. Linq�� ���� �������� ����� ������ Ȱ���ϴ� ���
            colorAttachedFields = from field in type.GetFields(bind)
                                  where field.HasAttribute<ColorAttribute>()
                                  select field;

            foreach (FieldInfo field in colorAttachedFields)
            {
                ColorAttribute att = field.GetCustomAttribute<ColorAttribute>();
                object value = field.GetValue(monoBehaviour);

                if (value is Renderer rend)
                {
                    rend.material.color = att.color;
                }
                else if (value is Graphic graph)
                {
                    graph.color = att.color;
                }
                else
                {
                    throw new Exception("����, Color Attribute�� �߸� ���̼̳׿�");
                    //Debug.LogError("����, Color Attribute�� �߸� ���̼̳׿�");
                }

            }
           
            IEnumerable<FieldInfo> sizeAttachedFields = from field in type.GetFields(bind)
                                                        where field.HasAttribute<SizeAttribute>()
                                                        select field;
            print(sizeAttachedFields.Count<FieldInfo>());
            foreach (FieldInfo field in sizeAttachedFields)
            {
                SizeAttribute att = field.GetCustomAttribute<SizeAttribute>();
                object value = field.GetValue(monoBehaviour);

                

                if (value is RectTransform rect)
                {
                    if (att.type == sizeType.size)
                        rect.sizeDelta = att.size;
                    else
                        rect.localScale = att.size;
                }
                else if (value is Transform transform)
                {
                    if (att.type2 == sizeType2.relative)
                        transform.localScale = new Vector3(transform.localScale.x * att.multiful, transform.localScale.y * att.multiful, transform.localScale.z * att.multiful);
                    else
                      transform.localScale = att.size;
                }
                else
                    throw new Exception("����");
            }

        }

    }
}

//Color�� ������ �� �ִ� ������Ʈ �Ǵ� ������Ʈ�� [Color]��� ��Ʈ����Ʈ�� �ٿ��� ���� �����ϰ� ����

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)] // AllowMultiple = true => �ߺ��� ����Ұ��̳�
public class ColorAttribute : Attribute
{
    public Color color;

    public ColorAttribute(float r = 0, float g = 0, float b = 0, float a = 1)
    {
        color = new Color(r, g, b, a);
    }

    public ColorAttribute()
    {
        color = Color.black;
    }

}


 public enum sizeType
{
    scale,
    size
}

public enum sizeType2
{
    local,
    relative
}

[AttributeUsage(AttributeTargets.Field)]
public class SizeAttribute : Attribute
{
    public Vector3 size;
    public float multiful = 1;
    public sizeType type;
    public sizeType2 type2;

    public SizeAttribute(float x, float y, float z, sizeType type = sizeType.scale)
    {
        this.size = new Vector3(x, y, z);
        this.type = type;
    }

    public SizeAttribute(float multiful, sizeType type = sizeType.scale, sizeType2 type2 = sizeType2.local)
    {
        this.multiful = multiful;
        this.type = type;
        this.type2 = type2;
    }

    public SizeAttribute()
    {
        size = new Vector3(2, 2, 2);
        type = sizeType.scale;
    }
}

public static class AttributeHelper
{
    public static bool HasAttribute<T>(this MemberInfo Info)
    {
        return Info.GetCustomAttributes(typeof(T), true).Length > 0;
    }
}


