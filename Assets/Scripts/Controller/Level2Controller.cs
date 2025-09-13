using System.Collections;
using UnityEngine;

public class Level2Controller : MonoBehaviour
{
    [SerializeField] private float waitTime = 3f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            StartCoroutine(ActiveLevel2());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            StopAllCoroutines();
        }
    }

    IEnumerator ActiveLevel2()
    {
        yield return new WaitForSeconds(waitTime);
        if(GameManager.Instance.state == GameState.Completed || GameManager.Instance.state == GameState.Failed)
        {
            GameManager.Instance.state = GameState.InProgress;
            GameObject player = GameObject.FindWithTag("Player");
            if(player != null)
            {
                player.layer = 23;
            }
        }
    }
}
