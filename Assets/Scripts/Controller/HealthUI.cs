using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class HealthUI : MonoBehaviour
{
    public static HealthUI Instance { get; private set; }

    [SerializeField] private Image heartPrefab;
    [SerializeField] private Sprite fullHeart;
    [SerializeField] private Sprite emptyHeart;

    List<Image> hearts = new();

    private void Awake()
    {
        if(Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void SetMaxHeart(int maxHeart)
    {
        foreach(var heart in hearts)
        {
            Destroy(heart.gameObject);
        }

        hearts.Clear();

        for(int i = 0; i < maxHeart; i++)
        {
            var newHeart = Instantiate(heartPrefab, transform);
            newHeart.sprite = fullHeart;
            hearts.Add(newHeart);
        }
    }

    public void UpdateHeart(int currentHeath)
    {
        for(int i = 0; i < hearts.Count; i++)
        {
            if(i < currentHeath)
            {
                hearts[i].sprite = fullHeart;
            }
            else
            {
                hearts[i].sprite = emptyHeart;
            }
        }
    }
}
