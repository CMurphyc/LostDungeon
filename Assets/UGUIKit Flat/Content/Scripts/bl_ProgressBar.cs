using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class bl_ProgressBar : MonoBehaviour
{

    [Range(1, 100)]
    public float MaxValue = 100;
    [Range(0, 100)]
    public float Value = 100;
    
    [SerializeField]private Text ValueText = null;


    void Update()
    {
        if (m_Image == null)
            return;

        float percent = (Value / MaxValue) * 100;
        percent = percent / 100;
        m_Image.fillAmount = percent;

        if(ValueText != null)
        {
            ValueText.text = (percent * 100).ToString("F0") + "%";
        }
    }

    private Image _image = null;
    public Image m_Image
    {
        get
        {
            if (_image == null)
            {
                _image = GetComponent<Image>();
            }
            return _image;
        }
    }
}