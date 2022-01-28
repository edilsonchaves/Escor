using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class configurações : MonoBehaviour
{
    [SerializeField] private Slider _fontSlider;
    [SerializeField] private Slider volume = null;
    [SerializeField] private Slider volumeVoz = null;
    [SerializeField] private TextMeshProUGUI _texto;

    public delegate void OnSizeChange();
    public static OnSizeChange SizeChangeDelegate;
    private static readonly string VolumePref = "VolumePref";
    private static readonly string VozPref = "VozPref";
    private float volumeFloat, vozFloat;
    public AudioSource musicaMenu;
    public AudioSource[] vozDoJogo;


    private void Start()
    {

        _fontSlider.value = Manager_Game.Instance.saveGameData.LetterSize;
        volume.value = Manager_Game.Instance.saveGameData.VolumeAmbient;
        volumeVoz.value = Manager_Game.Instance.saveGameData.Volume;

        _fontSlider.value = PlayerPrefs.GetFloat("tamanholetra", 20f);

        ChangeFontSize();

       
    }

    public void ChangeFontSize()
    {
        
        //Muda o tamanho das fontes visíveis
        if (SizeChangeDelegate != null)
        {
            SizeChangeDelegate.Invoke();
        }

        PlayerPrefs.SetFloat("tamanholetra", _fontSlider.value);
                
    }

    private void Awake()
    {
        salvarAsConfig();
    }

    private void salvarAsConfig()
    {
        volumeFloat = PlayerPrefs.GetFloat(VolumePref);
        vozFloat = PlayerPrefs.GetFloat(VozPref);

        musicaMenu.volume = volumeFloat;

        for (int i = 0; i < vozDoJogo.Length; i++)
        {
            vozDoJogo[i].volume = vozFloat;
        }
    }
    
    public void BackButton()
    {
        SaveLoadSystem.SaveFile<GameData>(Manager_Game.Instance.saveGameData);
    }
}
