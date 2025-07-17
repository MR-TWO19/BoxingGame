using UnityEngine;

public class BotController : MonoBehaviour
{
    [SerializeField] GameObject objTagert;
    [SerializeField] Character character;

    bool isAtk;

    private void UpdateAtkTarget()
    {
        Vector3 targetPos = objTagert.transform.position;
        if (!isAtk)
        {
            character.animator.SetBool("Move", true);
            transform.position = Vector3.MoveTowards(transform.position, targetPos, character.CurCharacterData.Speed * Time.deltaTime);
            transform.LookAt(new Vector3(targetPos.x, transform.position.y, targetPos.z));
        }
        else
        {

        }
    }

    void OnCollisionStay(Collision collision)
    {
        Debug.Log("Đang va chạm với: " + collision.gameObject.name);
    }

}
