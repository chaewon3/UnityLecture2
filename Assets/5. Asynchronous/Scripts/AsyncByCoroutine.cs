using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

namespace MyProject
{
	public class AsyncByCoroutine : MonoBehaviour
	{
		//�ܹ��Ÿ� ����� �ʹ�.

		int bread = 0; //�� ����
		int patty = 0;
		int pickle = 0;
		int lettuce = 0;

		public Text text;

		FoodMakerThread breadMaker = new FoodMakerThread();
		FoodMakerThread patMaker = new FoodMakerThread();
		FoodMakerThread pickleMaker = new FoodMakerThread();
		FoodMakerThread lettuceMaker = new FoodMakerThread();


		private void Start()
        {
			breadMaker.StartCook();
			patMaker.StartCook();
			pickleMaker.StartCook();
			lettuceMaker.StartCook();
			StartCoroutine(CheckHamberger());
        }

        private void Update()
        {
			bread = breadMaker.amount;
			patty = patMaker.amount;
			pickle = pickleMaker.amount;
			lettuce = lettuceMaker.amount;

			text.text = $"�� ���� : {bread}, ��Ƽ ����{patty}, ��Ŭ ���� {pickle}, ����� ���� {lettuce}";
        }

		IEnumerator CheckHamberger()
        {
			yield return new WaitUntil(HambergerReady);
			// ������ �����Ǳ� ������ �ƹ��͵� ���ϵ��� null�� ��ȯ
			MakeHamberger();
        }

        bool HambergerReady()
        {
			return bread >= 2 && patty >= 2 && pickle >= 8 && lettuce >= 4;
        }

		void MakeHamberger()
        {
			breadMaker.amount -= 2;
			patMaker.amount -= 2;
			pickleMaker.amount -= 8;
			lettuceMaker.amount -= 4;

			print($"�ܹ��Ű� ����������ϴ�. �ҿ�ð� {Time.time}");
        }
	}
}

public class FoodMakerThread
{
	public int amount =0; //����� ��

	private System.Random rand = new System.Random();

	public void StartCook()
    {
		Thread cookThread = new Thread(Cook);

		cookThread.Start();
    }

	private void Cook()
    {
		while (true)
        {
			int time = rand.Next(1000, 3000);
			Thread.Sleep(time);
			amount++;
        }
    }
}
