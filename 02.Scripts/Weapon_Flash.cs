using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Flash : MonoBehaviour
{
    public enum State
    {
        Ready, // 플래시 ON 준비
        Empty, // 배터리 부족
        Reloading, // 배터리 교체
    }
    public State state { get; private set; } // 현재 손전등 상태

    public Transform fireTransform; // 손전등의 위치 (광선이 시작될 위치)
    private LineRenderer flashLineRenderer;
    private AudioSource flashAudioPlayer;
    public Weapon_Flash_Data flash_Data; // 손전등 데이터
    private Transform flash_Object;

    public int magAmmo;
    public int ammoRemain = 100;
    private float fireDistance = 3f;
    private float lastFireTime; //손전등을 마지막으로 ON/OFF 한 시간
    private bool isFlashlightOn = false; // 손전등 상태 변수
    private Coroutine batteryDrainCoroutine;

    private void Awake()
    {
        flashAudioPlayer = GetComponent<AudioSource>();
        flashLineRenderer = GetComponent<LineRenderer>();
        flash_Object = transform.Find("YellowLight");

        flashLineRenderer.positionCount = 2;
        flashLineRenderer.enabled = false; // 시작 시 비활성화
    }

    private void OnEnable()
    {
        ammoRemain = flash_Data.startAmmoRemain; // 전체 배터리 양 초기화
        magAmmo = flash_Data.magCapacity; // 현재 배터리 가득 채우기

        state = State.Ready;
        lastFireTime = 0;
    }

    // 손전등 켜고 끄기
    public void ToggleFlashlight(bool isOn)
    {
        isFlashlightOn = isOn;
        flashLineRenderer.enabled = isOn; // 손전등이 켜질 때만 LineRenderer 활성화

        // 손전등 켜질 때 배터리 소모 시작
        if (isFlashlightOn)
        {
            if (batteryDrainCoroutine == null)
            {
                batteryDrainCoroutine = StartCoroutine(DrainBattery());
            }
        }
        else
        {
            if (batteryDrainCoroutine != null)
            {
                StopCoroutine(batteryDrainCoroutine);
                batteryDrainCoroutine = null;
            }
        }
    }

    private void Update()
    {
        // 손전등이 켜져 있을 때 광선 업데이트
        if (isFlashlightOn)
        {
            UpdateLineRenderer();
        }
    }
    private IEnumerator DrainBattery()
    {
        while (magAmmo > 0)
        {
            yield return new WaitForSeconds(10f); // 1초마다 배터리 감소
            magAmmo -= 1;

            // 배터리가 0 이하로 내려가지 않도록 조정
            if (magAmmo <= 0)
            {
                magAmmo = 0;
                flash_Object.GetComponent<Light>().enabled = false; // 배터리 소진 시 손전등 끄기
                state = State.Empty; // 상태를 Empty로 변경
                isFlashlightOn = false; // 손전등 상태를 꺼진 상태로
                StopCoroutine(batteryDrainCoroutine);
                batteryDrainCoroutine = null;
            }
        }
    }

    // 발사 함수
    public void Fire()
    {
        if (state == State.Ready && Time.time >= lastFireTime + flash_Data.timeBetFire)
        {
            lastFireTime = Time.time;
            Shot();
        }
    }

    // 발사 시 광선 처리
    private void Shot()
    {
        RaycastHit hit;
        Vector3 hitPosition = Vector3.zero;

        flashAudioPlayer.PlayOneShot(flash_Data.shotClip);

        // 손전등 앞에 물체가 있는지 확인
        if (Physics.Raycast(fireTransform.position, fireTransform.forward, out hit, fireDistance))
        {
            TargetDamage target = hit.collider.GetComponent<TargetDamage>();

            if (target != null)
            {
                target.OnDamage(flash_Data.damage, hit.point, hit.normal); // 타겟에 데미지 적용
                hitPosition = hit.point;
            }
            else
            {
                hitPosition = fireTransform.position + fireTransform.forward * fireDistance;
            }

            flashLineRenderer.SetPosition(1, hitPosition); // 광선 끝점 설정

            //magAmmo--;

            //if (magAmmo <= 0)
            //{
                //flash_Object.GetComponent<Light>().enabled = false;
                //state = State.Empty;
            //}
        }
    }

    // LineRenderer 업데이트 (손전등이 켜져 있는 동안 계속)
    private void UpdateLineRenderer()
    {
        // 손전등 위치를 첫 번째 점으로 설정
        flashLineRenderer.SetPosition(0, fireTransform.position);

        // 손전등이 비추는 방향으로 광선 끝점을 설정
        Vector3 hitPosition = fireTransform.position + fireTransform.forward * fireDistance;
        flashLineRenderer.SetPosition(1, hitPosition); // 광선 끝점 설정
    }

    // 배터리 재장전 함수
    public bool Reload()
    {
        if (state == State.Reloading || ammoRemain <= 0 || magAmmo >= flash_Data.magCapacity)
        {
            return false;
        }

        StartCoroutine(ReloadRoutine());
        return true;
    }

    // 재장전 코루틴
    private IEnumerator ReloadRoutine()
    {
        state = State.Reloading;
        yield return new WaitForSeconds(flash_Data.reloadTime);

        int ammoToFill = flash_Data.magCapacity - magAmmo;
        if (ammoRemain < ammoToFill)
        {
            ammoToFill = ammoRemain;
        }

        magAmmo += ammoToFill;
        ammoRemain -= ammoToFill;

        flash_Object.GetComponent<Light>().enabled = true;
        state = State.Ready;
    }
}