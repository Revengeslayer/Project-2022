using UnityEngine;
using UnityEngine.UI;

public class FpsUpdater : MonoBehaviour
{
    public Text fpsText;

    private int count;
    private float deltaTime;

    void Update()
    {
        count++;
        deltaTime += Time.deltaTime;

        if (count % 60 == 0)
        {
            count = 1;
            var fps = 60f / deltaTime;
            deltaTime = 0;
            //fpsText.text = $"FPS: {Mathf.Ceil(fps)}";
            Debug.Log($"FPS: {Mathf.Ceil(fps)}");
        }
    }
}
