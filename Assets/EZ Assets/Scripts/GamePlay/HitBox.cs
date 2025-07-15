using UnityEngine;
using UnityEngine.Events;

public class HitBox : MonoBehaviour
{
    public BodyPart bodyPart;
    public TeamType teamType;
    public bool isTriggerEnter;
    [HideInInspector] public Character character;
    [HideInInspector] public UnityEvent<HitBox, Character> OnhitEvent;

    private void OnTriggerEnter(Collider other)
    {
        if (IsLock(other) || !isTriggerEnter) return;
        Debug.Log("B-----" + gameObject.name);
        if (!other.TryGetComponent<HitBox>(out var otherHit)) return;

        if (otherHit.teamType == teamType) return;

        OnHit(otherHit);
    }

    protected virtual void OnHit(HitBox other)
    {
        OnhitEvent?.Invoke(this, character);
    }

    protected virtual bool IsLock(Collider other)
    {
        if((gameObject.CompareTag("LeftHand") || gameObject.CompareTag("RightHand")) &&
            (other.CompareTag("LeftHand") || other.CompareTag("RightHand"))) return true;
        else return false;
    }
}