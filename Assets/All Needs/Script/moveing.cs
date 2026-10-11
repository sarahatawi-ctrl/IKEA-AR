using System.Collections; 
using System.Collections.Generic; 
using UnityEngine;

public class moveing : MonoBehaviour { [Header("اجزاء الطاولة بالترتيب")] public List<Transform> parts = new List<Transform>();
[Header("اعدادات الحركة")]
public float moveDuration = 0.8f;
public Vector3 offset = new Vector3(0, 1f, 0);

private List<Vector3> targetPositions = new List<Vector3>();
private List<Color> originalColors = new List<Color>(); // لحفظ الألوان الأصلية لكل قطعة
private int currentIndex = 0;
private bool isAnimating = false;

void Start()
{
    // حفظ المواقع الأصلية وألوان القطع في البداية
    foreach (Transform part in parts)
    {
        targetPositions.Add(part.localPosition);
        part.gameObject.SetActive(false);

        // حفظ اللون الأصلي لكل قطعة (أو أبيض افتراضي في حال عدم وجود Renderer)
        Renderer rend = part.GetComponent<Renderer>();
        if (rend != null)
        {
            originalColors.Add(rend.material.color);
        }
        else
        {
            originalColors.Add(Color.white);
        }
    }
}

// 1. تركيب القطعة التالية: Next
public void AssembleNextPart()
{
    if (isAnimating || currentIndex >= parts.Count) return;

    StartCoroutine(AnimatePart(currentIndex, true));
    currentIndex++;
}

// 2. تفكيك/ارجاع الخطوة الأخيرة فقط: Previous
public void AssemblePreviousPart()
{
    if (isAnimating || currentIndex <= 0) return;

    currentIndex--;
    StartCoroutine(AnimatePart(currentIndex, false));
}

// 3. اعادة ضبط كل شيء والبدء من الجديد: Reset
public void ResetAssembly()
{
    if (isAnimating) return;

    StopAllCoroutines();
    for (int i = 0; i < parts.Count; i++)
    {
        if (parts[i] != null)
        {
            parts[i].gameObject.SetActive(false);
            parts[i].localPosition = targetPositions[i];

            // إعادة اللون الأصلي لجميع القطع عند الريست
            Renderer rend = parts[i].GetComponent<Renderer>();
            if (rend != null)
            {
                rend.material.color = originalColors[i];
            }
        }
    }
    currentIndex = 0;
    isAnimating = false;
}

private IEnumerator AnimatePart(int index, bool isNext)
{
    isAnimating = true;

    Transform part = parts[index];
    Vector3 finalPos = targetPositions[index];
    Vector3 startPos = finalPos + offset;

    Renderer currentRend = part.GetComponent<Renderer>();

    if (isNext) // عند التركيب (Next)
    {
        part.localPosition = startPos;
        part.gameObject.SetActive(true);

        // تلوين القطعة الحالية باللون الأحمر عند تركيبها
        if (currentRend != null)
        {
            currentRend.material.color = Color.red;
        }

        // إطفاء اللون الأحمر عن القطعة السابقة وإرجاع لونها الأصلي
        if (index > 0)
        {
            Renderer prevRend = parts[index - 1].GetComponent<Renderer>();
            if (prevRend != null)
            {
                prevRend.material.color = originalColors[index - 1];
            }
        }

        float elapsed = 0f;
        while (elapsed < moveDuration)
        {
            part.localPosition = Vector3.Lerp(startPos, finalPos, elapsed / moveDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        part.localPosition = finalPos;
    }
    else // عند التفكيك والإرجاع (Previous)
    {
        // إرجاع لون القطعة الحالية للونها الأصلي عند التراجع عنها
        if (currentRend != null)
        {
            currentRend.material.color = originalColors[index];
        }

        // إذا تراجعنا للخلف، نجعل القطعة السابقة تعود للون الأحمر
        if (index > 0)
        {
            Renderer prevRend = parts[index - 1].GetComponent<Renderer>();
            if (prevRend != null)
            {
                prevRend.material.color = Color.red;
            }
        }

        float elapsed = 0f;
        while (elapsed < moveDuration)
        {
            part.localPosition = Vector3.Lerp(finalPos, startPos, elapsed / moveDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        part.gameObject.SetActive(false);
        part.localPosition = finalPos;
    }

    isAnimating = false;
}}