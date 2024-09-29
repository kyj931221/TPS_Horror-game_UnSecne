using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** Player Movement ������ �÷��̾��� �Է� ���� ���缭 �÷��̾� ĳ���͸� �̵��ϰ�
 *  ������ �ִϸ��̼��� ���۽�ŵ�ϴ�. 
 *  �̵�, ȸ��, ������ ���� ���� ���� ������ �� �ֽ��ϴ�.
 *  Player Input ��ũ��Ʈ�� �Բ� �Է°� �Է¿� ���� ���͸� �и��Ͽ����ϴ�. **/

/** Player Movement moves the player character according to the player's input value
* Operate the appropriate animation.
* You can change the settings for movement, rotation, and jumping.
* You separated the input from the input with the Player Input script. **/

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; //�� �� �������� �ӵ�
    public float rotateSpeed = 180f; // �¿� ȸ�� �ӵ�
    public float jumpForce = 20f; //���� ����
    public float jumpRay = 0.5f;
    private int jumpCount = 0;
    private int maxCount = 1;

    public LayerMask Ground;

    private PlayerInput playerInput; // �÷��̾� �Է��� �˷��ִ� ������Ʈ

    private Rigidbody playerRigidbody; // �÷��̾� ĳ������ ������ٵ�
    private Animator playerAnimator; // �÷��̾� ĳ������ �ִϸ�����

    private bool isGrounded;

    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        playerRigidbody = GetComponent<Rigidbody>();
        playerAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        Jump();
        MoveCheck();
        playerAnimator.SetBool("ReyCheck",ReyCheck());
        playerAnimator.SetBool("Jump", isGrounded);
    }

    private void FixedUpdate()
    {
        Rotate(); // ȸ�� ����
        Move(); // ������ ����

        playerAnimator.SetFloat("Move", playerInput.move);
    }

    private void Move()
    {
        Vector3 moveDistance = playerInput.move * transform.forward * moveSpeed * Time.deltaTime;
        playerRigidbody.MovePosition(playerRigidbody.position + moveDistance);
    }

    private void Rotate()
    {
        float turn = playerInput.rotate * rotateSpeed * Time.deltaTime;
        playerRigidbody.rotation = playerRigidbody.rotation * Quaternion.Euler(0, turn, 0f);
    }

    private void Jump()
    {
        CheckGroundStatus();

        //Debug.Log(isGrounded + " " + playerInput.jump);

        if (playerInput.jump)
        {
            if(jumpCount < maxCount)
            {
                playerRigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                jumpCount++;
            }
        }
    }

    private bool CheckGroundStatus()
    {
        // �÷��̾��� ��ġ���� �Ʒ��� Ray�� ���� ���� ��Ҵ��� üũ �մϴ�.
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 0.1f, Ground);
        if (isGrounded)
        {
            jumpCount = 0;
            return (isGrounded);
        }
        return false;
    }

    /** �ִϸ��̼ǿ��� ��Ȱ�� ǥ���� ���Ͽ� �÷��̾��� �̵��� üũ �մϴ�.
     *  �÷��̾��� ���� ���� ������ �ϸ鼭 ����� �����̸� ���̱� ���Ͽ�
     *  �⺻ Idle �ڼ��� �߰��Ͽ� ��� ��쿡�� �⺻ �ڼ�(Movement)�� �Ѿ�� ���Ͽ�
     *  �׻� True �� �ǵ��� �մϴ�. **/

    /** Check the player's movement for smooth expression in the animation.
     * To reduce the delay caused by landing after the player's jump
     * In order to move to the basic movement in any case by adding the basic Idle posture
     * Always be True. **/

    private bool MoveCheck()
    {
        if(playerInput.move == 0)
        {
            return true;
        }
        return true;
    }

    /** �ִϸ��̼ǿ��� ���鿡 ������ ���� ��(����̳� ������ ����) ���� �ִϸ��̼���
     *  ���� �÷��� �Ǵ� ������ �����ϱ� ���� ���� ĳ��Ʈ�� �����Ͽ� ª�� �Ÿ�������
     *  ���� �ִϸ��̼��� ��Ÿ���� �ʵ��� �մϴ�. **/

    /** When you're close to the ground (step or curved terrain) in an animation, you can see the jump animation
     * To avoid frequent play problems, measure the ray cast for a short distance
     * Do not allow jump animation to appear. **/

    private bool ReyCheck()
    {
        //Debug.Log("Rey Check Stert");
        if (Physics.Raycast(transform.position, Vector3.down, jumpRay, Ground))
        {
            // �÷��̾�� ��������� ���� �������� True �̸� Movement �ִϸ��̼� �״�� ����
            //Debug.Log("Hit");
            return true ;
        }
        // �÷��̾�� ��������� ���� �������� False �̸� Jump �ִϸ��̼� ���� �Ѿ��.
        //Debug.Log("Not Hit");
        return false; // ����
    }
}
