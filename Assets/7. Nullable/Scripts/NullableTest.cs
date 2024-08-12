using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyProject
{
	public class NullableTest : MonoBehaviour
	{
		//nullable 자료형 : 원래라면 null값이 올 수 없는 타입(리터럴 타입, enum, struct)에
		//null겂을 할당 할 수 있도록 하는 선언 방식

		int normalInt; // 필드에서 선언한 리터럴타입의 경우, 초기화 할당을 하지 않아도 기본값으로 할당.
		int? nullableInt; // nullable 변수의 경우, 레퍼런스 타입과 같이 기본값이 null;

		private Vector3 vector3; // 리터럴타입이기때문에, 초기값 할당을 하지 않아도 됨.

		private GameObject obj; // 레퍼런스 타입이기때문에 초기값 할당을 하지 않을 경우 null.

		private void Start()
        {
			//print($"normal int : {normalInt}");
			//print($"nullable int : {nullableInt}");

			
			//vector3.x = 1f;
			////지역으로 하면 왜 오류뜨늕
			//print(vector3);

			StartPointer sp = new GameObject().AddComponent<StartPointer>();
			//sp.startPoint = Vector3.zero;
			//sp.DisplayPoint();

			DateTimeCounter counter = new GameObject().AddComponent<DateTimeCounter>();
			counter.DisplayDateTime();

		}
	}
}
