using UnityEngine;
using UnityEngine.Events;

public class HitBox : MonoBehaviour
{
    public bool isTriggerEnter;
    [HideInInspector] public Character character;
    [HideInInspector] public UnityEvent<HitBox> OnhitEvent;

    private void OnTriggerEnter(Collider other)
    {
        if (IsLock(other) || !isTriggerEnter) return;
        Debug.Log("B-----" + gameObject.name);
        if (!other.TryGetComponent<HitBox>(out var otherHit)) return;

        if (otherHit.character.teamType == character.teamType) return;

        OnHit(otherHit);
    }

    protected virtual void OnHit(HitBox other)
    {
        OnhitEvent?.Invoke(other);
    }

    protected virtual bool IsLock(Collider other)
    {
        if((gameObject.CompareTag("LeftHand") || gameObject.CompareTag("RightHand")) &&
            (other.CompareTag("LeftHand") || other.CompareTag("RightHand"))) return true;
        else return false;
    }
}