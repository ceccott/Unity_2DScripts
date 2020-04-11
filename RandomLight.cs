using Light2D = UnityEngine.Experimental.Rendering.Universal.Light2D;
using UnityEngine;

public class RandomLight : MonoBehaviour
{

    Light2D lt;
    // light properties
    [SerializeField] [Range(0.01f,1f)] private float intensityFlicker = 0.5f;
    [SerializeField] [Range(0.01f,1f)] private float colorFlicker = 0.5f;
    [SerializeField] [Range(0.1f,1f)]  private float saturation = 0.5f;

    // if true triggers TV channel change high freq flicker
    [SerializeField]                   public bool   remote = false;

    // flicker counters
    private float timercc = 0;
    private float dtcc = 0;

    private float timerin = 0;
    private float dtin = 0;

    private float timerrt = 0;
    private float dtrt = 0;
    private bool fast_flick = false;

    // Start is called before the first frame update
    void Start()
    {
        lt = GetComponent<Light2D>();
    }

    void ChangeColor()
    {
        dtcc = Random.Range((1-colorFlicker)-0.1f,(1-colorFlicker)+0.1f);
        dtcc = dtcc < 0.0f ? 0.0f : dtcc;       // flicker interval limit

        // pick random hue and value
        Color color = Color.HSVToRGB(Random.Range(0f,1f),saturation,Random.Range(0.2f,0.8f));

        lt.color = color ;
    }

    void ChangeIntensity()
    {
        dtin = Random.Range((1-intensityFlicker)-0.1f,(1-intensityFlicker)+0.1f);
        dtin = dtin < 0.0f ? 0.0f : dtin;       // flicker interval limit

        float intensity = Random.Range(0.2f,1f);

        lt.intensity = intensity;
    }

    void OnRemote(){
        dtrt = 0.1f;
        fast_flick = !fast_flick;

        lt.intensity = fast_flick ? 0.0f : 0.1f;

        lt.color = Color.white;
    }

    // Update is called once per frame
    void Update()
    {
        //timers update
        timercc += Time.deltaTime;
        timerin += Time.deltaTime;

        if (! remote)
        {
            if (timercc > dtcc)
            {
                ChangeColor();
                timercc = 0;
            }

            if (timerin > dtin)
            {
                ChangeIntensity();
                timerin = 0;
            }

        }else{
            timerrt += Time.deltaTime;

            if (timerrt > dtrt)
            {
                OnRemote();
            }
        }


        // timers reset
        if(timercc > 10.0f)
            timercc = 0;

        if(timerin > 10.0f)
            timerin = 0;

        if(timerrt > 0.5f)
        {
            timerrt = 0;
            remote = false;
        }
    }

    // TV channel change handle
    public void set_remote(bool newValue)
    {
        remote = newValue;
    }
}
