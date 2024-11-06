using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Flash : MonoBehaviour
{
    public Weapon_Flash flash; // Weapon_Flash 스크립트 참조
    public Transform flash_Reference_position;
    public Transform left_Handle;
    public Transform right_Handle;
    public GameObject flashLight; // 손전등 라이트 오브젝트

    private PlayerInput playerInput;
    private Animator playerAnimator;
    private bool isFlashLightActive = false; // 손전등 상태 저장

    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        playerAnimator = GetComponent<Animator>();
        flashLight.SetActive(false); // 게임 시작 시 손전등 끄기

        if (flash == null)
        {
            flash = GetComponentInChildren<Weapon_Flash>();
        }
    }

    private void OnEnable()
    {
        flash.gameObject.SetActive(true);
    }

    private void OnDisable()
    {
        flash.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (playerInput.fire)
        {
            ToggleFlashlight();            
            flash.Fire();
        }
        if (playerInput.reload)
        {
            if (flash.Reload())
            {
                playerAnimator.SetTrigger("Reload");
            }
        }

        UpdateUI();
    }

    // 
    private void ToggleFlashlight()
    {
        isFlashLightActive = !isFlashLightActive;
        flashLight.SetActive(isFlashLightActive);

        // Weapon_Flash 스크립트에 상태 전달 - LineRenderer 활성화/비활성화
        if(flash != null)
        {
            flash.ToggleFlashlight(isFlashLightActive);
        }
    }

    private void UpdateUI()
    {
        if (flash != null && UIManager.Instance != null)
        {
            UIManager.Instance.UpdateAmmoText(flash.magAmmo, flash.ammoRemain);
        }
    }

    /** FlashlightGold : Transform : Posistion : (X) 0.0036 / (Y) -0.0631 / (Z) 0.0372
     * 
     *  Left Handle : : Transform : Position : (X) 0.0506 / (Y) -0.0251 / (Z) 0.0579
     *                              Rotation : (X) -31.62 / (Y) -164 / (Z) 76.71
     *                              
     *  Right Handle : : Transform : Position : (X) 0 / (Y) -0.1156 / (Z) 0.011
     *                               Rotation : (X) -127.1 / (Y) 37.84 / (Z) 31 **/
    private void OnAnimatorIK(int layerIndex)
    {
        //flash_Reference_position.position = playerAnimator.GetIKHintPosition(AvatarIKHint.RightElbow);

        playerAnimator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1.0f);
        playerAnimator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1.0f);

        playerAnimator.SetIKPosition(AvatarIKGoal.LeftHand, left_Handle.position);
        playerAnimator.SetIKRotation(AvatarIKGoal.LeftHand, left_Handle.rotation);

        playerAnimator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1.0f);
        playerAnimator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1.0f);

        playerAnimator.SetIKPosition(AvatarIKGoal.RightHand, right_Handle.position);
        playerAnimator.SetIKRotation(AvatarIKGoal.RightHand, right_Handle.rotation);
    }
}
