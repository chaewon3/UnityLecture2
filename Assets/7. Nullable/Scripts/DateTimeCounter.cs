using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyProject
{
	public class DateTimeCounter : MonoBehaviour
	{
		//DisplayDateTime() 호출 시 세팅된 시간을 출력을 하는데,
		//dateTime에 값이 세팅되지 않았을 경우 세팅되지 않았다는 문구를 print 하도록 바꿔보세요.
		DateTime? datetime;

		public void DisplayDateTime()
        {
			print(datetime?.ToString() ?? "값이 세팅되지 않았습니다");
        }
	}
}
