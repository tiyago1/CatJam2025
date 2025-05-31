using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClassicProgressBar : MonoBehaviour
{
    [Header("Colors")] [SerializeField] private Color m_MainColor = Color.white;
    [SerializeField] private Color m_FillColor = Color.green;
    [SerializeField] private Color aColor = Color.green;
    [SerializeField] private Color bColor = Color.green;

    [Header("General")] [SerializeField] private int m_NumberOfSegments = 5;
    [SerializeField] private float m_SizeOfNotch = 5;
    [Range(0, 1f)] [SerializeField] public float FillAmount = 0.0f;

    private RectTransform m_RectTransform;
    private Image m_Image;
    private List<Image> m_ProgressToFill = new List<Image>();
    private float m_SizeOfSegment;

    public void Awake()
    {
        // get rect transform
        m_RectTransform = GetComponent<RectTransform>();

        // get image
        m_Image = GetComponentInChildren<Image>();
        m_Image.color = m_MainColor;
        m_Image.gameObject.SetActive(false);

        // count size of segments
        m_SizeOfSegment = m_RectTransform.sizeDelta.x / m_NumberOfSegments;
        for (int i = 0; i < m_NumberOfSegments; i++)
        {
            GameObject currentSegment =
                Instantiate(m_Image.gameObject, transform.position, Quaternion.identity, transform);
            currentSegment.SetActive(true);

            Image segmentImage = currentSegment.GetComponent<Image>();
            segmentImage.fillAmount = m_SizeOfSegment;

            RectTransform segmentRectTransform = segmentImage.GetComponent<RectTransform>();
            segmentRectTransform.sizeDelta = new Vector2(m_SizeOfSegment, segmentRectTransform.sizeDelta.y);
            segmentRectTransform.position += (Vector3.right * i * m_SizeOfSegment) -
                (Vector3.right * m_SizeOfSegment * (m_NumberOfSegments / 2)) + (Vector3.right * i * m_SizeOfNotch);
            segmentRectTransform.rotation = Quaternion.Euler(0,0,90);
            Image segmentFillImage = segmentImage.transform.GetChild(0).GetComponent<Image>();
            segmentFillImage.color = m_FillColor;
            m_ProgressToFill.Add(segmentFillImage);
            segmentFillImage.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(m_SizeOfSegment,
                segmentFillImage.GetComponent<RectTransform>().sizeDelta.y);
        }
    }

    public void Update()
    {
        for (int i = 0; i < m_NumberOfSegments; i++)
        {
            m_ProgressToFill[i].fillAmount = m_NumberOfSegments * FillAmount - i;
        }

        SetColor(Color.Lerp(aColor, bColor, FillAmount));
    }

    public void SetColor(Color color)
    {
        for (int i = 0; i < m_ProgressToFill.Count; i++)
        {
            m_ProgressToFill[i].color = color;
        }
    }

    private float ConvertFragmentToWidth(float fragment)
    {
        return m_RectTransform.sizeDelta.x * fragment;
    }
}