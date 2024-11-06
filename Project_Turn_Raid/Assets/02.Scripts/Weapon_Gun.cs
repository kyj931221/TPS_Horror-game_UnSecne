using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Gun : MonoBehaviour
{
    public enum State
    {
        Ready,
        Empty,
        Reloading
    }

    public State state {get; private set;}

    public Transform fireTransform;

    public ParticleSystem gunFireEffect;
    public ParticleSystem shellFireEffect;

    private LineRenderer bulletLineRenderer;

    private AudioSource gunAudioPlayer;

    public Weapon_Gun_Data gun_Data;

    private float fireDistance = 50f;

    public int ammoRemain = 100;
    public int magAmmo;

    private float lastFireTime;

    private void Awake()
    {
        gunAudioPlayer = GetComponent<AudioSource>();
        bulletLineRenderer = GetComponent<LineRenderer>();

        bulletLineRenderer.positionCount = 2;
        bulletLineRenderer.enabled = false;
    }

    private void OnEnable()
    {
        ammoRemain = gun_Data.startAmmoRemain;
        magAmmo = gun_Data.magCapacity;

        state = State.Ready;
        lastFireTime = 0;
    }

    public void Fire()
    {
        if (state == State.Ready && Time.time >= lastFireTime + gun_Data.timeBetFire)
        {
            lastFireTime = Time.time;

            Shot();
        }
    }

    private void Shot()
    {
        RaycastHit hit;

        Vector3 hitPosition = Vector3.zero;

        if (Physics.Raycast(fireTransform.position, fireTransform.forward, out hit, fireDistance))
        {
            TargetDamage target = hit.collider.GetComponent<TargetDamage>();

            if (target != null)
            {
                target.OnDamage(gun_Data.damage, hit.point, hit.normal);

                hitPosition = hit.point;
            }
            else
            {
                hitPosition = fireTransform.position + fireTransform.forward * fireDistance;
            }

            StartCoroutine(ShotEffect(hitPosition));

            magAmmo--;
            if (magAmmo <= 0)
            {
                state = State.Empty;
            }
        }
    }

        private IEnumerator ShotEffect(Vector3 hitPosition)
        {
            gunFireEffect.Play();

            //shellFireEffect.Play();

            gunAudioPlayer.PlayOneShot(gun_Data.shotClip);

            bulletLineRenderer.SetPosition(0, fireTransform.position);

            bulletLineRenderer.SetPosition(1, hitPosition);

            bulletLineRenderer.enabled = true;

            yield return new WaitForSeconds(0.5f);

            bulletLineRenderer.enabled = false;
        }

        public bool Reload()
        {
            if (state == State.Reloading || ammoRemain <= 0 || magAmmo >= gun_Data.magCapacity)
            {
                return false;
            }

            StartCoroutine(ReloadRoutine());
            return true;
        }

        private IEnumerator ReloadRoutine()
        {
            state = State.Reloading;

            yield return new WaitForSeconds(gun_Data.reloadTime);

            int ammoToFill = gun_Data.magCapacity - magAmmo;

            if (ammoRemain < ammoToFill)
            {
                ammoToFill = ammoRemain;
            }

            magAmmo += ammoToFill;

            ammoRemain -= ammoToFill;

            state = State.Ready;
        }
}
