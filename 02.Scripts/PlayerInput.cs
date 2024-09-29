using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** Player Input ��ũ��Ʈ ������ �÷��̾��� �Է��� �����ϰ� �ٸ� ������Ʈ (PlayerMovement)�� �����մϴ�.
 *  �� �̵�, ȸ��, �߻�, ����, ���� �� ��� �κ��� [������Ƽ]�� ����Ͽ� �������� �аų� ���� ������ 
 *  �����ϰ� ó���ϵ��� �ۼ��Ͽ����ϴ�. **/

/** The Player Input script detects the player's input and passes it to other components.
 * For functional parts such as movement, rotation, firing, loading, jumping, etc., use [Property] to read or write variable values
 * It's written to be flexible. **/

public class PlayerInput : MonoBehaviour
{
    /** �� ��ɿ� ���� �Ҵ�� �Է� ���� �̸� �Դϴ�.
     *  [Edit - Project Setting - Input Manager] ���� Ȯ���� �� �ֽ��ϴ�. **/

    /** Name of the assigned input value for each function.
     * [Edit - Project Setting - Input Manager] **/

    public string moveAxisName = "Vertical";
    public string rotateAxisName = "Horizontal";
    public string fireButtonName = "Fire1";
    public string reloadButtonName = "Reload";
    public string jumpButtonName = "Jump";

    public float move { get; private set; }
    public float rotate { get; private set; }
    public bool fire { get; private set; }
    public bool reload { get; private set; }
    public bool jump { get; private set; }
    public bool movechk {  get; private set; }
    public bool reychk { get; private set; }

    //private bool isFlashlightOn = false; // ������ ����

    //void Start()
    //{
       // fire = false;
    //}

    // Update is called once per frame
    private void Update()
    {
        if(GameManager.Instance != null && GameManager.Instance.isGameover)
        {
            move = 0;
            rotate = 0;
            fire = false;
            reload = false;
            return;
        }

        move = Input.GetAxis(moveAxisName);
        rotate = Input.GetAxis(rotateAxisName);
        fire = Input.GetButtonDown(fireButtonName);
        reload = Input.GetButtonDown(reloadButtonName);
        jump = Input.GetButtonDown(jumpButtonName);

        //if(Input.GetButtonDown(fireButtonName))
        //{
        //    isFlashlightOn = !isFlashlightOn;
        //    fire = isFlashlightOn;
        //}
    }
}
