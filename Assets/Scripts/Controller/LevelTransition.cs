using System.Collections;
using UnityEngine;

public class LevelTransition : MonoBehaviour
{
    [SerializeField] private float waitTime = 5f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            StartCoroutine(Transition());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            StopAllCoroutines();
        }
    }

    IEnumerator Transition()
    {
        if (GameManager.Instance.state == GameState.Completed)
        {
            yield return new WaitForSeconds(waitTime);
            AudioManager.Instance.PlaySFX("LevelUp");
            GameManager.Instance.Transition();
        }
        else
        {
            Debug.Log("You need to complete level to move");
        }
    }
}
