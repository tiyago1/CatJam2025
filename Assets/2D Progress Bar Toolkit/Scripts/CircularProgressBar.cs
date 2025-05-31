using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class CircularProgressBar : MonoBehaviour
{
    [Header("Colors")] [SerializeField] private Color mainColor = Color.white;

    [SerializeField] private Color fillColor = Color.green;
    [SerializeField] private Color backIncreaseColor = Color.green;
    [SerializeField] private Color backDecreaseColor = Color.green;

    [SerializeField] private Color fullColor;
    [SerializeField] private Color midColor;
    [SerializeField] private Color endColor;
    
    [Header("General")] [SerializeField] private int numberOfSegments = 5;
    [Range(0, 360)] [SerializeField] private float startAngle = 40;
    [Range(0, 360)] [SerializeField] private float endAngle = 320;
    [SerializeField] private float sizeOfNotch = 5;

    [OnValueChanged("SetFillAmount")] [Range(0, 1f)] [SerializeField]
    private float fillAmount = 0.0f;

    [OnValueChanged("SetBackAmount")] [Range(0, 1f)] [SerializeField]
    private float backAmount = 0.0f;
    [SerializeField] private float min;
    [SerializeField] private float max;
    
    [SerializeField] private float backAnimationDuration = 1.0f;
    [SerializeField] private float backAnimationDelay = 0.2f;
    [SerializeField] private bool isPlayOnAwake;

    private Image _image;
    private List<Image> _progressToFill;
    private List<Image> _progressToBackFill;
    private float _sizeOfSegment;

    private void Awake()
    {
        _progressToFill = new List<Image>();
        _progressToBackFill = new List<Image>();
        _image = GetComponentInChildren<Image>();
        _image.color = mainColor;
        _image.gameObject.SetActive(false);

        if (isPlayOnAwake)
            Initialize();
    }

    public void Initialize()
    {
        float startNormalAngle = NormalizeAngle(startAngle);
        float endNormalAngle = NormalizeAngle(360 - endAngle);
        float notchesNormalAngle = (numberOfSegments - 1) * NormalizeAngle(sizeOfNotch);
        float allSegmentsAngleArea = 1 - startNormalAngle - endNormalAngle - notchesNormalAngle;

        _sizeOfSegment = allSegmentsAngleArea / numberOfSegments;
        for (int i = 0; i < numberOfSegments; i++)
        {
            GameObject currentSegment =
                Instantiate(_image.gameObject, transform.position, Quaternion.identity, transform);
            currentSegment.SetActive(true);

            Image segmentImage = currentSegment.GetComponent<Image>();
            segmentImage.fillAmount = _sizeOfSegment;

            Image segmentFillImage = segmentImage.transform.GetChild(1).GetComponent<Image>();
            segmentFillImage.color = fillColor;
            _progressToFill.Add(segmentFillImage);

            Image segmentBackFillImage = segmentImage.transform.GetChild(0).GetComponent<Image>();
            segmentBackFillImage.color = backIncreaseColor;
            _progressToBackFill.Add(segmentBackFillImage);

            float zRot = startAngle + i * ConvertCircleFragmentToAngle(_sizeOfSegment) + i * sizeOfNotch;
            segmentImage.transform.rotation = Quaternion.Euler(0, 0, -zRot);
        }
    }

    public void SetRange(float newMin, float newMax)
    {
        min = newMin;
        max = newMax;
    }

    public void SetBackColor(Color color)
    {
        for (int i = 0; i < _progressToBackFill.Count; i++)
        {
            _progressToBackFill[i].color = color;
        }
    }
    
    public void SetFillColor(Color color)
    {
        for (int i = 0; i < _progressToFill.Count; i++)
        {
            _progressToFill[i].color = color;
        }
    }

    [Button]
    public Color Test(float amount)
    {
        return Color.Lerp(fullColor, endColor, fillAmount);
    }

    [Button]
    public void SetFillAmount(float amount)
    {
        fillAmount = amount;
        for (int i = 0; i < numberOfSegments; i++)
        {
            _progressToFill[i].fillAmount = (fillAmount * ((endAngle - startAngle) / 360)) - _sizeOfSegment * i;
        }
        
        SetFillColor(Color.Lerp(fullColor, endColor, fillAmount));
    }

    [Button]
    public void SetBackAmount(float amount)
    {
        backAmount = amount;
        for (int i = 0; i < numberOfSegments; i++)
        {
            _progressToBackFill[i].fillAmount = (backAmount * ((endAngle - startAngle) / 360)) - _sizeOfSegment * i;
        }
    }

    [Button]
    public void SetProgressValue(float amount)
    {
        amount = NormalizeValue(amount, min, max);
        float oldValue = fillAmount;
        float newValue = amount;

        if (newValue < oldValue)
        {
            SetBackColor(backDecreaseColor);
            SetFillAmount(newValue);
            SetBackAmount(oldValue);
            DOVirtual.Float(oldValue, newValue, backAnimationDuration, SetBackAmount)
                .SetEase(Ease.Linear)
                .SetDelay(backAnimationDelay);
        }
        else if (newValue > oldValue)
        {
            SetBackColor(backIncreaseColor);
            SetFillAmount(oldValue);
            SetBackAmount(newValue);
            DOVirtual.Float(oldValue, newValue, backAnimationDuration, SetFillAmount)
                .SetEase(Ease.Linear)
                .SetDelay(backAnimationDelay);
        }
    }

    private float NormalizeAngle(float angle)
    {
        return Mathf.Clamp01(angle / 360f);
    }

    private float ConvertCircleFragmentToAngle(float fragment)
    {
        return 360 * fragment;
    }

    private float ReRangeValue(float value, float oldMin, float oldMax, float newMin, float newMax)
    {
        return newMin + (value - oldMin) * (newMax - newMin) / (oldMax - oldMin);
    }

    private float NormalizeValue(float value, float newMin, float newMax)
    {
        return ReRangeValue(value, newMin, newMax, 0, 0.86f);
    }
}