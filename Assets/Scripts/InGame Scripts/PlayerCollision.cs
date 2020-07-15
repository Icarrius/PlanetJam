using System.Collections;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer flashEffect;

    private readonly Color32 colorWhite = new Color(1, 1, 1, 1), colorTransparent = new Color32(1, 1, 1, 0);

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Planet") || other.CompareTag("Asteroid") || other.CompareTag("Boundary") || other.CompareTag("Deathzone"))
        {
            GetDamage();
        }

        if (other.CompareTag("Finish"))
        {
            Finish(other.GetComponent<FinishLine>());
        }
    }

    private void GetDamage()
    {
        FlashEffect.singleton.FlashScreenWhite();
        Manager.singleton.LoseHandling();
    }

    private void Finish(FinishLine finishLine)
    {
        if (finishLine) finishLine.PlayAnimation(transform.position);
        FlashEffect.singleton.FlashScreen();
        Manager.singleton.WinHandling();
    }

    public IEnumerator ColorLerpTo(Color _color, float _duration)
    {
        float elapsedTime = 0.0f;
        while (elapsedTime < _duration)
        {
            flashEffect.color = Color.Lerp(flashEffect.color, _color, (elapsedTime / _duration));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        yield break;
    }
}
