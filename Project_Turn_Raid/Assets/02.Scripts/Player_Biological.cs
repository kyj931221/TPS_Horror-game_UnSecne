using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class Player_Biological : Common_Biological_Functions
{
    public Slider HPSlider;
    public Image personIcon;

    public AudioClip deathClip;
    public AudioClip hitClip;
    public AudioClip itemPickupClip;
    public AudioClip stepSound;

    private AudioSource playerAudioPlayer;
    private Animator playerAnimator;

    private PlayerMovement playerMovement;
    //private Player_Shooter playerShooter;
    private Player_Flash playerFlash;

    private void Awake()
    {
        playerAnimator = GetComponent<Animator>();
        playerAudioPlayer = GetComponent<AudioSource>();

        playerMovement = GetComponent<PlayerMovement>();
        //playerShooter = GetComponent<Player_Shooter>();
        playerFlash = GetComponent<Player_Flash>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        HPSlider.gameObject.SetActive(true);
        HPSlider.maxValue = startingHP;
        HPSlider.value = currentHP;
        UpdatePersonIconColor();

        playerMovement.enabled = true;
        //playerShooter.enabled = true;
        playerFlash.enabled = true;
    }

    public override void RecoveryHP(float newRecoveryHP)
    {
        base.RecoveryHP(newRecoveryHP);

        HPSlider.value = currentHP;
    }

    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitDirection)
    {
        if(!dead)
        {
            playerAudioPlayer.PlayOneShot(hitClip);
        }

        base.OnDamage(damage, hitPoint, hitDirection);

        HPSlider.value = currentHP;
        UpdatePersonIconColor();
    }

    private void UpdatePersonIconColor()
    {
        float hpPercentage = (float)currentHP / startingHP;

        Color newColor = Color.Lerp(Color.white, Color.red, 1 - hpPercentage);
        personIcon.color = newColor;
    }

    public override void Die()
    {
        base.Die();

        HPSlider.gameObject.SetActive(false);

        playerAudioPlayer.PlayOneShot(deathClip);
        playerAnimator.SetTrigger("Die");

        playerMovement.enabled = false;
        //playerShooter.enabled = false;
        playerFlash.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        //if(!dead)
        //{
        //    Gain_Item item = other.GetComponent<Gain_Item>();

        //    if(item != null)
        //    {
        //        item.Use(gameObject);
        //        playerAudioPlayer.PlayOneShot(itemPickupClip);
        //    }
        //}
    }

    public void StepSound()
    {
        playerAudioPlayer.PlayOneShot(stepSound);
    }
}
