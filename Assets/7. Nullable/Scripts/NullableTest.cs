using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyProject
{
	public class NullableTest : MonoBehaviour
	{
		//nullable �ڷ��� : ������� null���� �� �� ���� Ÿ��(���ͷ� Ÿ��, enum, struct)��
		//null���� �Ҵ� �� �� �ֵ��� �ϴ� ���� ���

		int normalInt; // �ʵ忡�� ������ ���ͷ�Ÿ���� ���, �ʱ�ȭ �Ҵ��� ���� �ʾƵ� �⺻������ �Ҵ�.
		int? nullableInt; // nullable ������ ���, ���۷��� Ÿ�԰� ���� �⺻���� null;

		private Vector3 vector3; // ���ͷ�Ÿ���̱⶧����, �ʱⰪ �Ҵ��� ���� �ʾƵ� ��.

		private GameObject obj; // ���۷��� Ÿ���̱⶧���� �ʱⰪ �Ҵ��� ���� ���� ��� null.

		private void Start()
        {
			//print($"normal int : {normalInt}");
			//print($"nullable int : {nullableInt}");

			
			//vector3.x = 1f;
			////�������� �ϸ� �� �����߈d
			//print(vector3);

			StartPointer sp = new GameObject().AddComponent<StartPointer>();
			//sp.startPoint = Vector3.zero;
			//sp.DisplayPoint();

			DateTimeCounter counter = new GameObject().AddComponent<DateTimeCounter>();
			counter.DisplayDateTime();

		}
	}
}
