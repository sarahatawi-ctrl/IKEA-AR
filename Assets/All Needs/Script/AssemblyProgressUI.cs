using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AssemblyProgressUI : MonoBehaviour
{
    [Header("Progress UI")]
    public Image progressFill;
    public TMP_Text stepText;

    [Header("Assembly Settings")]
    public int totalSteps = 9;

    public void UpdateProgress(int currentStep)
    {
        currentStep = Mathf.Clamp(currentStep, 0, totalSteps);

        if (progressFill != null)
        {
            progressFill.fillAmount =
                (float)currentStep / totalSteps;
        }

        if (stepText != null)
        {
            stepText.text =
                $"Step {currentStep} of {totalSteps}";
        }
    }
}