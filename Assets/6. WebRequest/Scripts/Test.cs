using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyProject
{
	public class Test : MonoBehaviour
	{
        string[] babbling = {"ayaye", "uuu", "yeye", "yemawoo", "ayaayaa"    };

        private void Start()
        {
            
            print(solution(babbling));
        }

        public int solution(string[] babbling)
        {
            int answer = 0;
            string tempStr = "";


            for (int i = 0; i < babbling.Length; i++)
            {
                int cut = 0;
                while (true)
                {
                    print("1");
                    bool isbabbling = false;
                    if (babbling[i].Length >= cut+2 && babbling[i].Substring(cut, 2) == "ye" || babbling[i].Substring(cut, 2) == "ma")
                    {
                        if (tempStr == babbling[i].Substring(cut, 2))
                            break;
                        
                        tempStr = babbling[i].Substring(cut, 2);
                        cut += 2;
                        isbabbling = true;
                    }
                    else if (babbling[i].Length >= cut + 3 && babbling[i].Substring(cut, 3) == "aya" || babbling[i].Substring(cut, 3) == "woo")
                    {
                        if (tempStr == babbling[i].Substring(cut, 3))
                            break;
                        tempStr = babbling[i].Substring(cut, 3);
                        cut += 3;
                        isbabbling = true;
                    }

                    if (isbabbling && cut == babbling[i].Length-1)
                    {
                        answer++;
                        break;
                    }
                    if (!isbabbling)
                        break;
                }

            }
            return answer;
        }
    }
}
