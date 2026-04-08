using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public Button[] buttons;
    public GameObject[] objects;

    public Color normalColor = Color.white;
    public Color selectedColor = new Color(0.9f, 0.9f, 0.9f);

    private int currentIndex = -1;

    void Start()
    {
        Init();
    }

    public void Init()
    {
        OnButtonClick(0);
    }

    public void OnButtonClick(int index)
    {
        currentIndex = index;

        for (int i = 0; i < objects.Length; i++)
        {
            objects[i].SetActive(i == index);
        }

        UpdateButtonColors();
    }

    void UpdateButtonColors()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            ColorBlock cb = buttons[i].colors;

            if (i == currentIndex)
            {
                cb.normalColor = selectedColor;
            }
            else
            {
                cb.normalColor = normalColor;
            }

            buttons[i].colors = cb;
        }
    }
}
