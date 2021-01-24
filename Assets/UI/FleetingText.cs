using Assets.Util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FleetingText : MonoBehaviour
{
    private Text TextField { get; set; }
    private FrameTimer DestroyTimer { get; set; }
    private FrameTimer StartFadeTimer { get; set; }
    private FrameTimer FadeTimer { get; set; }
    private bool CurrentyFading { get; set; }

    [SerializeField]
    private const float OpaqueTextTime = 1f;
    [SerializeField]
    private const float FadeTime = 0.5f;

    [SerializeField]
    private const float Speed = 100f;

    void Start()
    {
        TextField = GetComponent<Text>();
        DestroyTimer = new FrameTimer(OpaqueTextTime + FadeTime);
        StartFadeTimer = new FrameTimer(OpaqueTextTime);
        FadeTimer = new FrameTimer(FadeTime);
    }

    // Update is called once per frame
    void Update()
    {
        _Update(Time.deltaTime);
    }

    public void _Update(float deltaTime)
    {
        transform.Translate(0, deltaTime * Speed, 0);

        // Check if timer is destroyed
        if (DestroyTimer.UpdateActivates(deltaTime))
        {
            Destroy(gameObject);
            Destroy(this);
        }

        // Calculate start of fade
        else if (!CurrentyFading)
        {
            CurrentyFading = StartFadeTimer.UpdateActivates(deltaTime);
        }

        // Calculate alpha value of fade
        else
        {
            FadeTimer.Increment(deltaTime);

            float alpha = FadeTimer.RatioRemaining;
            var color = TextField.color;
            color.a = alpha;
            TextField.color = color;
        }
    }

    public void Init(string text, Vector2 position)
    {
        Start();

        TextField.text = text;
        transform.position = position;
    }
}
