using UnityEngine;
using UnityEngine.UI;

public class MathCManager : MonoBehaviour
{
    public Image[] collectedItemImages;
    public Sprite letterMSprite;
    public Sprite letterASprite;
    public Sprite letterTSprite;
    public Sprite letterHSprite;

    private int collectedLettersCount = 0;

    void Start()
    {
        for (int i = 0; i < collectedItemImages.Length; i++)
        {
            collectedItemImages[i].gameObject.SetActive(false);
        }
    }

    public void CollectItem(char letter)
    {
        if (letter == 'M')
        {
            ShowCollectedItem(letterMSprite, 0);
        }
        else if (letter == 'A')
        {
            ShowCollectedItem(letterASprite, 1);
        }
        else if (letter == 'T')
        {
            ShowCollectedItem(letterTSprite, 2);
        }
        else if (letter == 'H')
        {
            ShowCollectedItem(letterHSprite, 3);
        }

        collectedLettersCount++;
    }

    private void ShowCollectedItem(Sprite itemSprite, int position)
    {
        collectedItemImages[position].gameObject.SetActive(true);
        collectedItemImages[position].sprite = itemSprite;
    }

    public bool AllItemsCollected()
    {
        return collectedLettersCount >= 4;
    }
}
