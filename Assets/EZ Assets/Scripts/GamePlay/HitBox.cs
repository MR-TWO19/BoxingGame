using UnityEngine;
using UnityEngine.Events;

public class HitBox : MonoBehaviour
{
    public BodyPart bodyPart;
    public TeamType teamType;
    [HideInInspector] public Character character;
    [HideInInspector] public UnityEvent<HitBox, Character> OnhitEvent;

    private void OnTriggerEnter(Collider other)
    {
        if ((bodyPart == BodyPart.LeftHand || bodyPart == BodyPart.RightHand) && (other.CompareTag("LeftHand") || other.CompareTag("RightHand"))) return;
        Debug.Log("B-----" + gameObject.name);
        if (!other.TryGetComponent<HitBox>(out var otherHit)) return;

        if (otherHit.teamType == teamType) return;

        OnHit(otherHit);
    }

    protected virtual void OnHit(HitBox other)
    {
        OnhitEvent?.Invoke(this, character);
    }
}