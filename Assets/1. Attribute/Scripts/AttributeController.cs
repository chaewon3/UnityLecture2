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
        //ColorAttribute를 가진 필드를 찾는다.
        //BindingFlags : public이거나 private 상관 없이 static이 아닌 동적 할당 멤버만 탐색.
        BindingFlags bind = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
        MonoBehaviour[] monoBehaviours = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None);
        foreach (MonoBehaviour monoBehaviour in monoBehaviours)
        {
            Type type = monoBehaviour.GetType();

            //특정 콜렉션(배열, 리스트)에서 조건에 부합하는 요소만 가져오려 할 경우,
            //foreach 또는 List.Find등 다소 복잡한 절차를 거쳐야 함.
            //List<FieldInfo> fields = new List<FieldInfo>(type.GetFields(bind));
            //fields.FindAll(null);

            //Linq 문법을 활용하면 이를 간소화 할 수 있음.
            //1. Linq에 정의된 확장 메서드 이용하는 방법
            IEnumerable<FieldInfo> colorAttachedFields = type.GetFields(bind).Where(field => field.GetCustomAttribute<ColorAttribute>() != null); ;

            //2. Linq를 통해 쿼리문과 비슷한 문법을 활용하는 방법
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
                    throw new Exception("저런, Color Attribute를 잘못 붙이셨네요");
                    //Debug.LogError("저런, Color Attribute를 잘못 붙이셨네요");
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
                    throw new Exception("저런");
            }

        }

    }
}

//Color를 조절할 수 있는 컴포넌트 또는 오브젝트에 [Color]라는 어트리뷰트를 붙여서 색을 설정하고 싶음

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)] // AllowMultiple = true => 중복을 허용할것이냐
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


