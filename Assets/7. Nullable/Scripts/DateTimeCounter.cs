using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyProject
{
	public class DateTimeCounter : MonoBehaviour
	{
		//DisplayDateTime() ȣ�� �� ���õ� �ð��� ����� �ϴµ�,
		//dateTime�� ���� ���õ��� �ʾ��� ��� ���õ��� �ʾҴٴ� ������ print �ϵ��� �ٲ㺸����.
		DateTime? datetime;

		public void DisplayDateTime()
        {
			print(datetime?.ToString() ?? "���� ���õ��� �ʾҽ��ϴ�");
        }
	}
}
