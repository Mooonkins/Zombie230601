using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooter : MonoBehaviour
{
    public Gun gun;
    public Transform gunPivot;
    public Transform leftHandMount;
    public Transform rightHandMount;

    private PlayerInput playerInput;
    private Animator playerAnim;

    public Vector3 playerVec = new Vector3(0,0,0);
    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        playerAnim = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        gun.gameObject.SetActive(true);
    }
    private void OnDisable()
    {
        gun.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (playerInput.fire)
        {
            gun.Fire();
        }
        else if (playerInput.reload)
        {
            if (gun.Reload())
            {
                playerAnim.SetTrigger("Reload");
            }
        }

        UpdateUI();
    }

    private void UpdateUI()
    {
        if (gun != null && UIManager.instance != null)
        {
            UIManager.instance.UpdateAmmoText(gun.magAmmo, gun.ammoRemain);
        }
    }
    private void OnAnimatorIK(int layerIndex)
    {
        gunPivot.position = playerVec + playerAnim.GetIKHintPosition(AvatarIKHint.RightElbow);

        playerAnim.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1.0f);
        playerAnim.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1.0f);

        playerAnim.SetIKPosition(AvatarIKGoal.LeftHand, leftHandMount.position);
        playerAnim.SetIKRotation(AvatarIKGoal.LeftHand, leftHandMount.rotation);

        playerAnim.SetIKPositionWeight(AvatarIKGoal.RightHand, 1.0f);
        playerAnim.SetIKRotationWeight(AvatarIKGoal.RightHand, 1.0f);

        playerAnim.SetIKPosition(AvatarIKGoal.RightHand, rightHandMount.position);
        playerAnim.SetIKRotation(AvatarIKGoal.RightHand, rightHandMount.rotation);
    }
}
