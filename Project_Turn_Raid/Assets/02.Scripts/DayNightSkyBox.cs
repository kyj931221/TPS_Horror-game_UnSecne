using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DayNightSkyBox : MonoBehaviour
{
    [Header("스카이 박스 및 오브젝트 추가")]
    [SerializeField] Material skybox1;
    [SerializeField] Material skybox2;
    [SerializeField] Material skybox3;
    [SerializeField] GameObject Object_1;

    [SerializeField] int PlusTime = 0; // 시간 조작을 위한 설정

    [SerializeField] GameObject directionalLight;   //아침엔 True
    
    int hours;  //시간 
    
    void Start()
    {
        hours = DateTime.Now.Hour;
        StartCoroutine("HourCheck");
    }

    IEnumerator HourCheck()
    {
        //현재 분 뺀 후 함수 실행시키려고 변수 2개 선언함 
        bool first = true;
        int minus_Time = 60 - DateTime.Now.Minute;
        while (true)
        {
            //현재 시각 받아옴
            hours = DateTime.Now.Hour + PlusTime;
            Check_Environment();
            if (first)
            {
                //만약 현재 시각 30분이면 1800초 후 이 함수를 다시 실행 
                yield return new WaitForSeconds(minus_Time * 60);
                first = false;
            }
            yield return new WaitForSeconds(3600f);
        }
    }


    // 아침,새벽 해지기 전, 밤 
    void Check_Environment()
    {
        //18~4시까지
        if (hours >= 18 || hours <= 4)
        {
         
        }
        else
        {
            
        }

        //아침
        if (hours >= 6 && hours <= 18)
        {
            directionalLight.SetActive(true);
            RenderSettings.skybox = skybox1;
        }
        //새벽, 해지기 전 
        //오후 6시 ~ 9시와 새벽 4시 ~ 6시까지
        else if ((hours >= 18 && hours <= 21) || (hours >= 4 && hours <= 6))
        {
            directionalLight.SetActive(false);
            RenderSettings.skybox = skybox2;
        }
        //밤 
        else if (hours >= 21 || hours <= 4)
        {
            directionalLight.SetActive(false);
            RenderSettings.skybox = skybox3;
        }
    }
}
