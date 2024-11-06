using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Shooter : MonoBehaviour
{
    public Weapon_Flash gun;
    public Transform gun_Reference_position;
    public Transform left_Handle;
    public Transform right_Handle;

    private PlayerInput playerInput;
    private Animator playerAnimator;

    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        playerAnimator = GetComponent<Animator> ();
    }

    private void OnEnable()
    {
        gun.gameObject.SetActive (true);
    }

    private void OnDisable()
    {
        gun.gameObject.SetActive(false);
    }

    private void Update()
    {
        if(playerInput.fire)
        {
            gun.Fire();
        }
        else if(playerInput.reload)
            {
                if(gun.Reload())
                {
                playerAnimator.SetTrigger("Reload");
                }
            }

            UpdateUI();
    }

    private void UpdateUI()
    {
        if (gun != null && UIManager.Instance != null)
        {
            UIManager.Instance.UpdateAmmoText(gun.magAmmo, gun.ammoRemain);
        }
    }

    /** SciFiGunLightBlack : Transform : Posistion : (X) -0.26 / (Y) -0.107 / (Z) 0.168 
     * 
     *  Left Handle : : Transform : Position : (X) -0.0891 / (Y) 0.0132 / (Z) 0.2654 
     *                              Rotation : (X) 0 / (Y) 51.6 / (Z) 180
     *                              
     *  Right Handle : : Transform : Position : (X) 0.05 / (Y) 0.001872048 / (Z) -0.105
     *                               Rotation : (X) 2.562 / (Y) -0.46 / (Z) -74.247 **/
    private void OnAnimatorIK(int layerIndex)
    {
        gun_Reference_position.position = playerAnimator.GetIKHintPosition(AvatarIKHint.RightElbow);

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
