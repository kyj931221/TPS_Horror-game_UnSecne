using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Flash : MonoBehaviour
{
    public enum State
    {
        Ready, // �÷��� ON �غ�
        Empty, // ���͸� ����
        Reloading, // ���͸� ��ü
    }
    public State state { get; private set; } // ���� ������ ����

    public Transform fireTransform; // �������� ��ġ (������ ���۵� ��ġ)
    private LineRenderer flashLineRenderer;
    private AudioSource flashAudioPlayer;
    public Weapon_Flash_Data flash_Data; // ������ ������
    private Transform flash_Object;

    public int magAmmo;
    public int ammoRemain = 100;
    private float fireDistance = 3f;
    private float lastFireTime; //�������� ���������� ON/OFF �� �ð�
    private bool isFlashlightOn = false; // ������ ���� ����
    private Coroutine batteryDrainCoroutine;

    private void Awake()
    {
        flashAudioPlayer = GetComponent<AudioSource>();
        flashLineRenderer = GetComponent<LineRenderer>();
        flash_Object = transform.Find("YellowLight");

        flashLineRenderer.positionCount = 2;
        flashLineRenderer.enabled = false; // ���� �� ��Ȱ��ȭ
    }

    private void OnEnable()
    {
        ammoRemain = flash_Data.startAmmoRemain; // ��ü ���͸� �� �ʱ�ȭ
        magAmmo = flash_Data.magCapacity; // ���� ���͸� ���� ä���

        state = State.Ready;
        lastFireTime = 0;
    }

    // ������ �Ѱ� ����
    public void ToggleFlashlight(bool isOn)
    {
        isFlashlightOn = isOn;
        flashLineRenderer.enabled = isOn; // �������� ���� ���� LineRenderer Ȱ��ȭ

        // ������ ���� �� ���͸� �Ҹ� ����
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
        // �������� ���� ���� �� ���� ������Ʈ
        if (isFlashlightOn)
        {
            UpdateLineRenderer();
        }
    }
    private IEnumerator DrainBattery()
    {
        while (magAmmo > 0)
        {
            yield return new WaitForSeconds(10f); // 1�ʸ��� ���͸� ����
            magAmmo -= 1;

            // ���͸��� 0 ���Ϸ� �������� �ʵ��� ����
            if (magAmmo <= 0)
            {
                magAmmo = 0;
                flash_Object.GetComponent<Light>().enabled = false; // ���͸� ���� �� ������ ����
                state = State.Empty; // ���¸� Empty�� ����
                isFlashlightOn = false; // ������ ���¸� ���� ���·�
                StopCoroutine(batteryDrainCoroutine);
                batteryDrainCoroutine = null;
            }
        }
    }

    // �߻� �Լ�
    public void Fire()
    {
        if (state == State.Ready && Time.time >= lastFireTime + flash_Data.timeBetFire)
        {
            lastFireTime = Time.time;
            Shot();
        }
    }

    // �߻� �� ���� ó��
    private void Shot()
    {
        RaycastHit hit;
        Vector3 hitPosition = Vector3.zero;

        flashAudioPlayer.PlayOneShot(flash_Data.shotClip);

        // ������ �տ� ��ü�� �ִ��� Ȯ��
        if (Physics.Raycast(fireTransform.position, fireTransform.forward, out hit, fireDistance))
        {
            TargetDamage target = hit.collider.GetComponent<TargetDamage>();

            if (target != null)
            {
                target.OnDamage(flash_Data.damage, hit.point, hit.normal); // Ÿ�ٿ� ������ ����
                hitPosition = hit.point;
            }
            else
            {
                hitPosition = fireTransform.position + fireTransform.forward * fireDistance;
            }

            flashLineRenderer.SetPosition(1, hitPosition); // ���� ���� ����

            //magAmmo--;

            //if (magAmmo <= 0)
            //{
                //flash_Object.GetComponent<Light>().enabled = false;
                //state = State.Empty;
            //}
        }
    }

    // LineRenderer ������Ʈ (�������� ���� �ִ� ���� ���)
    private void UpdateLineRenderer()
    {
        // ������ ��ġ�� ù ��° ������ ����
        flashLineRenderer.SetPosition(0, fireTransform.position);

        // �������� ���ߴ� �������� ���� ������ ����
        Vector3 hitPosition = fireTransform.position + fireTransform.forward * fireDistance;
        flashLineRenderer.SetPosition(1, hitPosition); // ���� ���� ����
    }

    // ���͸� ������ �Լ�
    public bool Reload()
    {
        if (state == State.Reloading || ammoRemain <= 0 || magAmmo >= flash_Data.magCapacity)
        {
            return false;
        }

        StartCoroutine(ReloadRoutine());
        return true;
    }

    // ������ �ڷ�ƾ
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