using Unity.VisualScripting;
using UnityEngine;

public class LayerController : MonoBehaviour
{
    public Direction direction;

    public string layerUpper;
    public string sortingLayerUpper;

    public string layerLower;
    public string sortingLayerLower;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (direction == Direction.Down && collision.transform.position.y < transform.position.y)
        {
            SetLayerAndSortingLayer(collision.gameObject, layerUpper, sortingLayerUpper);
        }
        else if (direction == Direction.Left && collision.transform.position.x < transform.position.x)
        {
            SetLayerAndSortingLayer(collision.gameObject, layerUpper, sortingLayerUpper);
        }
        else if (direction == Direction.Right && collision.transform.position.x > transform.position.x)
        {
            SetLayerAndSortingLayer(collision.gameObject, layerUpper, sortingLayerUpper);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (direction == Direction.Down && collision.transform.position.y < transform.position.y)
        {
            SetLayerAndSortingLayer(collision.gameObject, layerLower, sortingLayerLower);
        }
        else if (direction == Direction.Left && collision.transform.position.x < transform.position.x)
        {
            SetLayerAndSortingLayer(collision.gameObject, layerLower, sortingLayerLower);
        }
        else if (direction == Direction.Right && collision.transform.position.x > transform.position.x)
        {
            SetLayerAndSortingLayer(collision.gameObject, layerLower, sortingLayerLower);
        }
    }

    void SetLayerAndSortingLayer(GameObject player, string layer, string sortingLayer)
    {
        player.layer = LayerMask.NameToLayer(layer);
        
        player.GetComponent<SpriteRenderer>().sortingLayerName = sortingLayer;
        SpriteRenderer[] srs = player.GetComponentsInChildren<SpriteRenderer>();
        foreach(var sr in srs)
        {
            sr.sortingLayerName = sortingLayer;
        }
    }
}

public enum Direction { Left, Right, Down }
