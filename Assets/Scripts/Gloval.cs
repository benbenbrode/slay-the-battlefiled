using UnityEngine;

public class Global : MonoBehaviour
{

    // 방어도와 공격력 변수
    public static int armor = 0;
    public static int attackPower = 0;

    public static int opparmor = 0;
    public static int oppattackPower = 0;

    public static void IncreaseArmor(int amount)
    {
        armor += amount;
    }

    public static void IncreaseAttackPower(int amount)
    {
        attackPower += amount;
    }


}
