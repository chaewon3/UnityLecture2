using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace MyProject
{
	public class AsyncByTaskFixed : MonoBehaviour
	{
        //async : �Լ� �տ� �ٴ� Ű�����, �ش� �Լ��� ��� ����(�񵿱�) �Լ���� �ǹ̰� ��.
        //await : async(�񵿱�) �Լ� ������ ���Ǹ�, �Լ� ���� ��� ���� ���(Task��)�� �Ϸ� �� ������ ����ϵ��� ��.
        //�� �Ϸ� �� ������ ����ϵ��� ��.
        //��Ƽ �������� ���� �� �� ó�� ���ο� �����带 �����ϴ� ���� �ƴ�, Task�� �����Ͽ� �����ϹǷ� ���� �����忡�� ������ ������.
        async void Start()
        {
            print("�ܹ��� ������ ���۵�");

            Task breadTask = GetFood("��", 2);
            Task pattyTask = GetFood("��Ƽ", 2);
            Task pickleTask = GetFood("��Ŭ", 8);
            Task lettuceTask = GetFood("�����", 4);

            await Task.WhenAll(breadTask, pattyTask, pickleTask, lettuceTask);//.ContinueWith(result => { });

            print("�ܹ��Ű� �޼���");

            //new Thread(GetFood("", 1).Start();
        }

        async Task GetFood(string name, int count)
        {
            for(int i = 1; i<count;i++)
            {
                await Task.Delay(Random.Range(1000, 3000));
                print($"{name} {i}�� �ϼ���");
            }
        } 
    }
}
