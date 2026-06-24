using UnityEngine;
using UnityEngine.UI;

public class HealthHeart : MonoBehaviour
{
    public Sprite fullheart, halfheart, emptyheart;
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Image fillImage;

    private void Awake()
    {
        if (backgroundImage == null)
        {
            backgroundImage = GetComponent<Image>();
        }

        if (fillImage == null)
        {
            Transform fillTransform = transform.Find("Fill");
            if (fillTransform != null)
            {
                fillImage = fillTransform.GetComponent<Image>();
            }
        }

        if (backgroundImage != null)
        {
            backgroundImage.color = Color.white;
            backgroundImage.sprite = emptyheart;
        }

        if (fillImage != null)
        {
            fillImage.color = Color.white;
            fillImage.sprite = fullheart;
            fillImage.type = Image.Type.Filled;
            fillImage.fillMethod = Image.FillMethod.Horizontal;
            fillImage.fillOrigin = (int)Image.OriginHorizontal.Left;
            fillImage.fillAmount = 1f;
        }
    }

    public void SetHeartImage(HeartStatus Status)
    {
        if (backgroundImage != null)
        {
            backgroundImage.sprite = emptyheart;
        }

        if (fillImage == null) return;

        switch (Status)
        {
            case HeartStatus.Empty:
                fillImage.fillAmount = 0f;
                break;
            case HeartStatus.Half:
                fillImage.fillAmount = 0.5f;
                break;
            case HeartStatus.Full:
                fillImage.fillAmount = 1f;
                break;
        }
    }
}
public enum HeartStatus
{
    Empty = 0,
    Half = 1,
    Full = 2
}
