using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyProject
{
	public class StartPointer : MonoBehaviour
	{
		//internal Vector3 startPoint; 
		//      // ���� vector3�� reference type�̾��� ���, �ʱⰪ �Ҵ� ���θ� null üũ�� ���� ������.
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
		//              Debug.LogError("StartPoint���� �Ҵ���� �ʾҽ��ϴ�. ���� SetInitialValue �Լ��� ���� �ʱⰪ�� �������ּ���.");

		//          // ?. ?? : null���� �� �� �ִ� ������ ������ null���� �ƴ����� üũ�ϴ� ������.
		//          // [����]?.[��]?? ������ null�� ��� ��ȯ�� ��;
		//          // [�븮��]?.[�Լ�](); : �븮�� �Ǵ� Ŭ������  null�� ��� �Լ��� ȣ������ ����.

		//      }

		
		internal Vector3? startPoint;
		//������ boxing�� �����

		public void DisplayPoint()
        {
			//if (startPoint.HasValue)
			//	print(startPoint.Value);
			//else
			//	Debug.LogError("StartPoint���� �Ҵ���� �ʾҽ��ϴ�. ���� startPoint���� �������ּ���.");

			print(startPoint?.ToString() ?? "StartPoint���� �Ҵ���� �ʾҽ��ϴ�. ���� startPoint���� �������ּ���.");
		}
	}
}
