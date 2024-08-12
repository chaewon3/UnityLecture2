using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyProject
{
	public class StartPointer : MonoBehaviour
	{
		//internal Vector3 startPoint; 
		//      // 만일 vector3가 reference type이었을 경우, 초기값 할당 여부를 null 체크를 통해 가능함.
		//      public bool isInitialized;

		//      public void SetInitialValue(Vector3 StartPoint)
		//      {
		//          this.startPoint = startPoint;
		//          isInitialized = true;
		//      }

		//      public void DisplayPoint()
		//      {
		//          if (isInitialized)
		//              print(startPoint);
		//          else
		//              Debug.LogError("StartPoint값이 할당되지 않았습니다. 먼저 SetInitialValue 함수를 통해 초기값을 세팅해주세요.");

		//          // ?. ?? : null값이 올 수 있는 변수의 내용이 null인지 아닌지를 체크하는 연산자.
		//          // [변수]?.[값]?? 변수가 null일 경우 반환할 값;
		//          // [대리자]?.[함수](); : 대리자 또는 클래스가  null일 경우 함수를 호출하지 않음.

		//      }

		
		internal Vector3? startPoint;
		//일종의 boxing이 적용됨

		public void DisplayPoint()
        {
			//if (startPoint.HasValue)
			//	print(startPoint.Value);
			//else
			//	Debug.LogError("StartPoint값이 할당되지 않았습니다. 먼저 startPoint값을 세팅해주세요.");

			print(startPoint?.ToString() ?? "StartPoint값이 할당되지 않았습니다. 먼저 startPoint값을 세팅해주세요.");
		}
	}
}
