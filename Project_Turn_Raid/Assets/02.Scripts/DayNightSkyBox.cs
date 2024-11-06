using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DayNightSkyBox : MonoBehaviour
{
    [Header("��ī�� �ڽ� �� ������Ʈ �߰�")]
    [SerializeField] Material skybox1;
    [SerializeField] Material skybox2;
    [SerializeField] Material skybox3;
    [SerializeField] GameObject Object_1;

    [SerializeField] int PlusTime = 0; // �ð� ������ ���� ����

    [SerializeField] GameObject directionalLight;   //��ħ�� True
    
    int hours;  //�ð� 
    
    void Start()
    {
        hours = DateTime.Now.Hour;
        StartCoroutine("HourCheck");
    }

    IEnumerator HourCheck()
    {
        //���� �� �� �� �Լ� �����Ű���� ���� 2�� ������ 
        bool first = true;
        int minus_Time = 60 - DateTime.Now.Minute;
        while (true)
        {
            //���� �ð� �޾ƿ�
            hours = DateTime.Now.Hour + PlusTime;
            Check_Environment();
            if (first)
            {
                //���� ���� �ð� 30���̸� 1800�� �� �� �Լ��� �ٽ� ���� 
                yield return new WaitForSeconds(minus_Time * 60);
                first = false;
            }
            yield return new WaitForSeconds(3600f);
        }
    }


    // ��ħ,���� ������ ��, �� 
    void Check_Environment()
    {
        //18~4�ñ���
        if (hours >= 18 || hours <= 4)
        {
         
        }
        else
        {
            
        }

        //��ħ
        if (hours >= 6 && hours <= 18)
        {
            directionalLight.SetActive(true);
            RenderSettings.skybox = skybox1;
        }
        //����, ������ �� 
        //���� 6�� ~ 9�ÿ� ���� 4�� ~ 6�ñ���
        else if ((hours >= 18 && hours <= 21) || (hours >= 4 && hours <= 6))
        {
            directionalLight.SetActive(false);
            RenderSettings.skybox = skybox2;
        }
        //�� 
        else if (hours >= 21 || hours <= 4)
        {
            directionalLight.SetActive(false);
            RenderSettings.skybox = skybox3;
        }
    }
}
