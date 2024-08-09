using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MyProject
{
	public class AsyncTest : MonoBehaviour
	{
		//async void Start()
  //      {
		//	//await WaitRandom();
		//	WaitRandom();
		//	print("WitRandom ȣ����");
		//	await Wait3Seconds();
		//	print("wait3Seconds ȣ����");
		//	int a = await WaitRandomAndReturn();
		//	print($"{a} �� WaitRandomAndReturn ȣ����");

		//	await WaitAndCallback(() => print("�ϰ� ���� ȣ��ǳ�"));
		//	print("���� ���� ȣ���;��");

		//	//Task.Run();
		//	//new Task().ContinueWith();
  //      }

		 private void Start()
        {
			//async�� �ƴѵ��� Wait3Seconds()�� ���� �Ŀ� ���� �ؾ� �� ���.
			Wait3Seconds().ContinueWith((Task result) => 
			{
				if (result.IsCanceled || result.IsFaulted)
					print("task ����");
				else if (result.IsCompleted)
					print("task ����");

				print("3�� ��"); 
			});
			print("3�� ��");
        }

		//1. void�� ��ȯ�ϴ� async �Լ� :
		//	�Լ� ��ü�� ��� ���������� �ٸ� �Լ����� ��� ������������ ȣ�� �Ұ���.
		async void WaitRandom()
        {
			print($"��� ����{Time.time}");
			await Task.Delay(Random.Range(1000, 2000));
			print($"��� ����{Time.time}");
        }

		//2. Task�� ��ȯ�ϴ� async�Լ� :
		//	�Լ� ��ü�� ��� �����̸�, �ٸ� ��� ��� �Լ����� ��� �������� ȣ�� ����
		async Task Wait3Seconds()
        {
			print($"3�� ��� ����{Time.time}");
			await Task.Delay(3000);
			print($"3�� ��� ����{Time.time}");
        }

		//3. Task<T>�� ��ȯ�ϴ� async �Լ� :
		//	��� ���� �Լ��ΰ� Task�� ��ȯ�ϴ� �Լ��� ������, T return�� �־�߸� ��.
		async Task<int> WaitRandomAndReturn()
        {
			int delay = Random.Range(1000, 2000);
			print($"{(float)delay / 1000} �� ��� ����{Time.time}");
			await Task.Delay(delay);
			print($"{(float)delay / 1000} �� ��� ����{Time.time}");
			return delay;
        }

		async Task WaitAndCallback(Action callback)
        {

        }
	}
}