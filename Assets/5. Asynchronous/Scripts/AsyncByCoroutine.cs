using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

namespace MyProject
{
	public class AsyncByCoroutine : MonoBehaviour
	{
		//햄버거를 만들고 싶다.

		int bread = 0; //빵 갯수
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

			text.text = $"빵 개수 : {bread}, 패티 개수{patty}, 피클 개수 {pickle}, 양상추 개수 {lettuce}";
        }

		IEnumerator CheckHamberger()
        {
			yield return new WaitUntil(HambergerReady);
			// 조건이 만족되기 전까지 아무것도 안하도록 null을 반환
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

			print($"햄버거가 만들어졌습니다. 소요시간 {Time.time}");
        }
	}
}

public class FoodMakerThread
{
	public int amount =0; //식재료 양

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
