using UnityEngine;
using UnityEngine.UI;

public class FontSize : MonoBehaviour
{
    [SerializeField] private Slider _fontSlider;

    public delegate void OnSizeChange();
    public static OnSizeChange SizeChangeDelegate;

    private void Start()
    {
        _fontSlider.value = PlayerPrefs.GetFloat("font_size", 65f);

        ChangeFontSize();
    }

    public void ChangeFontSize()
    {
        //Muda o tamanho das fontes visíveis
        if (SizeChangeDelegate != null)
        {
            SizeChangeDelegate.Invoke();
        }

        //Atualiza o PlayerPrefs com o valor atual
        PlayerPrefs.SetFloat("font_size", _fontSlider.value);
    }
}
