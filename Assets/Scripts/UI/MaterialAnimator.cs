using System;
using System.Collections;
using UnityEngine;

public class MaterialAnimator : MonoBehaviour
{
    public Material material;
    [SerializeField]
    private float blinkDuration = 0.3f;
    [SerializeField]
    private float gapBetweenBlinks = 0.5f;
    [SerializeField]
    private int blinkCount = 2; // Number of times to blink


    void Start()
    {
        // Ensure the material's opacity is set to zero at the start
        Color color = material.color;
        color.a = 0f;
        material.color = color;

    }

    private void OnEnable()
    {
        CubeController.OnCubeCollided += Animate;
    }
    
    private void OnDisable()
    {
        CubeController.OnCubeCollided -= Animate;
    }

    private void Animate(TargetSide side, EventType eventType)
    {
        if (eventType == EventType.HIT)
        {
            StartCoroutine(AnimateMaterial());
        }
    }

    IEnumerator AnimateMaterial()
    {
        for (int i = 0; i < blinkCount; i++)
        {
            // Increase opacity
            yield return ChangeOpacity(1f);

            // Decrease opacity
            yield return ChangeOpacity(0f);

            // Gap between blinks
            if (i < blinkCount - 1)
                yield return new WaitForSecondsRealtime(gapBetweenBlinks);
        }
    }

    IEnumerator ChangeOpacity(float targetOpacity)
    {
        Color startColor = material.color;
        Color targetColor = material.color;
        targetColor.a = targetOpacity;

        float elapsedTime = 0f;

        while (elapsedTime < blinkDuration)
        {
            material.color = Color.Lerp(startColor, targetColor, elapsedTime / blinkDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        material.color = targetColor;
    }
}
