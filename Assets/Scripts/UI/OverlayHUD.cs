using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverlayHUD : MonoBehaviour
{
    public static OverlayHUD instance;
    public AnimationCurve overlay_fadein, overlay_fadeout;

    public string overlay_test;

    public Overlay[] overlays;

    private Canvas _canvas;

    public bool overlay_showing;

    private bool end_immediate = false;
    void Awake() { instance = this; }
    // Use this for initialization
    void Start()
    {
        _canvas = this.GetComponent<Canvas>();
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space)) LoadOverlay(overlay_test);
    }


    public void LoadOverlay(string name)
    {
        
        for (int i = 0; i < overlays.Length; i++)
        {
            if (name == overlays[i].name)
            {
                StartCoroutine(LoadOverlayRoutine(overlays[i]));
                return;
            }
        }
        Debug.Log("Could not load overlay for " + name);
    }

    IEnumerator LoadOverlayRoutine(Overlay o)
    {
        while(overlay_showing) 
        {
            end_immediate = true;
            yield return null;
        }
        overlay_showing = true;
        end_immediate = false;
        GameObject obj = Instantiate(Resources.Load<GameObject>(o.name));
        obj.transform.SetParent(_canvas.transform, true);
        (obj.transform as RectTransform).sizeDelta = Vector3.zero;

        CanvasGroup group = obj.GetComponent<CanvasGroup>();

        for (float i = 0; i < 0.45F; i += Time.deltaTime)
        {
            group.alpha = overlay_fadein.Evaluate(i / 0.45F);
            yield return null;
        }  
         if (o.lifetime > 0.0F) yield return new WaitForSeconds(o.lifetime);
        
        if (o.WaitForInput != string.Empty)
        {
            if(o.WaitForInput == "any") while(!Input.anyKeyDown && !end_immediate) yield return null;
            else while (!Input.GetButtonDown(o.WaitForInput) && Input.GetAxis(o.WaitForInput) == 0.0F && !end_immediate) yield return null;

            
        }
        
        for (float i = 0; i < 0.45F; i += Time.deltaTime)
        {
            group.alpha = overlay_fadeout.Evaluate(i / 0.45F);
            yield return null;
        }

        Destroy(obj);
        overlay_showing = false;

    }

    public void EndOverlay()
    {
        end_immediate = true;
        //StopAllCoroutines();
    }
}
