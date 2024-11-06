using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** TargetDamage �� ���� �޴� ��� ���鿡�� ����Ǵ� �������̽� �Դϴ�.
 *  ��ӹ޴� Ŭ������ OnDamage �޼��带 �����ؾ� �մϴ�. **/

/** TargetDamage is an interface that applies to all targets under attack.
  * Inherited classes must implement OnDamage methods. **/

public interface TargetDamage
{
    /** damage : ����� ũ�� 
     *  hitPoint : ���ݴ��� ��ġ
     *  hitNormal : ���ݴ��� ǥ���� ���� **/

    void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal);
}
