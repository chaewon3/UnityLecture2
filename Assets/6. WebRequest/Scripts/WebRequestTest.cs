using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace MyProject
{
	public class WebRequestTest : MonoBehaviour
	{
		public string imageURL;
		public RawImage rawimage;
		public Image image;
        private void Start()
        {
			// 사용하지 않는 리턴이 올때는 메모리 낭비이기떄문에 _ = 로 리턴을 무시해줄 수 있따.
			_ = StartCoroutine(GetWebTexture(imageURL));
        }

        IEnumerator GetWebTexture(string url)
        {
			//http로 웹 요청(Request)를 보낼 객체 생성
			UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
			//비동기식(을 모방한 코루틴)으로 Response를 받을 때 까지 대기
			var operation = www.SendWebRequest();
			yield return operation;

			if(www.result != UnityWebRequest.Result.Success)
            {
				Debug.LogError($"HTTP 통신 실패 : {www.error}");
            }
			else
            {
				Debug.Log("텍스쳐 다운로드 성공!");
				//rawimage.texture = ((DownloadHandlerTexture)www.downloadHandler).texture;

				Texture texture = ((DownloadHandlerTexture)www.downloadHandler).texture;

				Sprite sprite = Sprite.Create((Texture2D)texture, new Rect(0, 0, texture.width, texture.height), new Vector2(.5f, .5f));

				image.sprite = sprite;
				//image.SetNativeSize();
			}


		}
	}
}
