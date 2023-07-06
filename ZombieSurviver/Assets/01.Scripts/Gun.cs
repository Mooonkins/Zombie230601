using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public enum State
    {
        Ready,
        Empty,
        Reloading
    }

    public State state { get; private set; }

    private Transform fireTransform;

    public ParticleSystem muzzleFlashEffect;
    public ParticleSystem shellEjectEffect;

    private LineRenderer bulletLineRenderer;

    private AudioSource gunAudioPlayer;

    public GunData gunData;

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

        /*fireTransform = GetComponentsInChildren<Transform>()[9];*/
        fireTransform = transform.GetChild(3);
    }

    private void OnEnable()
    {
        ammoRemain = gunData.startAmmoRemain;   //전체 예비 탄알 양 초기화

        magAmmo = gunData.magCapacity;  //현재 탄창 가득 채우기

        state = State.Ready;    //총의 현재 상태

        lastFireTime = 0f;      //
    }
    public void Fire()
    {
        if (state == State.Ready && Time.time >= lastFireTime + gunData.timeBetFire)
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
            IDamageable target = hit.collider.GetComponent<IDamageable>();

            if (target != null)
            {
                target.OnDamage(gunData.damage, hit.point, hit.normal);
            }
            hitPosition = hit.point;
        }
        else
            hitPosition = fireTransform.position + fireTransform.forward * fireDistance;

        StartCoroutine(ShotEffect(hitPosition));

        magAmmo--;
        if (magAmmo <= 0)
            state = State.Empty;
    }

    private IEnumerator ShotEffect(Vector3 hitPosition)
    {
        muzzleFlashEffect.Play();

        shellEjectEffect.Play();

        gunAudioPlayer.PlayOneShot(gunData.shotClip);

        bulletLineRenderer.SetPosition(0, fireTransform.position);

        bulletLineRenderer.SetPosition(1, hitPosition);

        bulletLineRenderer.enabled = true;
        bulletLineRenderer.enabled = true;

        yield return new WaitForSeconds(0.03f);

        bulletLineRenderer.enabled = false;
    }

    public bool Reload()
    {
        if(state == State.Reloading || ammoRemain <= 0 || magAmmo >= gunData.magCapacity)
        {
            return false;
        }

        StartCoroutine(ReloadRoutine());
        return false;
    }

    private IEnumerator ReloadRoutine()
    {
        state = State.Reloading;

        yield return new WaitForSeconds(gunData.reloadTime);

        int ammoToFill = gunData.magCapacity - magAmmo;

        if(ammoRemain < ammoToFill)
        {
            ammoToFill = ammoRemain;
        }

        magAmmo += ammoToFill;

        ammoRemain -= ammoToFill;

        state = State.Ready;
    }
}
