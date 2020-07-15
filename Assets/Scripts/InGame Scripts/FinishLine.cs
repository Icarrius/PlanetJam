using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(BezierLineRenderer))]
public sealed class FinishLine : MonoBehaviour
{
    public Animator animator;
    public new BoxCollider2D collider;
    public BezierLineRenderer line;

    // Animator must have a trigger param called 'finishLevel'.
    private readonly int FINISH_LEVEL_TRIGGER_PARAM = Animator.StringToHash("finishLevel");

    // method called automatically by Unity when script is attached
    private void Reset()
    {
        animator = GetComponent<Animator>();
        collider = GetComponent<BoxCollider2D>();
        line = GetComponentInChildren<BezierLineRenderer>();

        collider.isTrigger = true;
    }

    public void PlayAnimation(Vector3 playerPosition)
    {
        Vector3 hitPos = transform.position;
        hitPos.x = (playerPosition.x - transform.position.x);
        if (hitPos.x > line.start.position.x && hitPos.x < line.end.position.x)
        {
            line.control.position = hitPos + Vector3.up;
            animator.SetTrigger(FINISH_LEVEL_TRIGGER_PARAM);
        }
    }
}