using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;


public class Lotto : MonoBehaviour
{
    // 지정된 범위 내의 숫자 중에서 랜덤하게 숫자를 뽑아서 나열하고 싶다.
    public int MaxNumber = 12;
    public int numCount = 12;

    public List<int> luckyNumbers = new List<int>();
    public List<string> teamName = new List<string>();
    public List<Text> uiNames = new List<Text>();
    public GameObject logImage;
    public Button btn_draw;
    public AudioClip[] soundClips = new AudioClip[2];

    Text text_log;
    AudioSource soundSource;

    void Start()
    {
        //luckyNumbers.Capacity = numCount;
        text_log = logImage.GetComponentInChildren<Text>();
        soundSource = GetComponent<AudioSource>();
    }


    public void DrawLottoNumbers()
    {
        btn_draw.interactable = false;

        // 1. 최대 숫자만큼 랜덤한 숫자를 뽑는다.
        for (int i = 0; i < numCount; i++)
        {
            int num = Random.Range(0, MaxNumber);
            luckyNumbers[i] = num;

            // 2. 혹시 중복된 숫자가 있는지 확인한다.
            for (int j = 0; j < i; j++)
            {
                if (num == luckyNumbers[j])
                {
                    // 3. 중복된 숫자가 있었다면 재추첨하기로 하고 중복 검사를 종료한다.
                    i--;
                    break;
                }
            }
        }

        StartCoroutine(ShowResults(3.0f));
    }

    IEnumerator ShowResults(float time)
    {
        logImage.SetActive(true);
        soundSource.clip = soundClips[0];
        soundSource.Play();
        for (int i = (int)time; i > 0; i--)
        {
            text_log.text = $"팀을 추첨하는 중입니다......{i}초";

            yield return new WaitForSeconds(1.0f);
        }
        logImage.SetActive(false);

        soundSource.Stop();
        soundSource.clip = soundClips[1];
        soundSource.Play();

        // UI Text에 출력하기
        for (int i = 0; i < luckyNumbers.Count; i++)
        {
            //result += luckyNumbers[i].ToString() + ", ";
            //print(teamName[luckyNumbers[i]]);
            uiNames[i].text = $"{i + 1}. {teamName[luckyNumbers[i]]}";

            yield return new WaitForSeconds(1.0f);
        }

        string curTime = System.DateTime.Now.ToString();
        curTime = curTime.Replace(':', '-');
        ScreenCapture.CaptureScreenshot(Paths.project + "/" + curTime + ".png");
        print("이미지 저장 완료!");

        btn_draw.interactable = true;

        yield return new WaitForSeconds(1.0f);
        EditorUtility.RevealInFinder(Paths.project + "/");
    }

}
