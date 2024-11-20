using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiAndHealth : MonoBehaviour
{
    private static GameObject instance;
    private Canvas canvas;
    // Start is called before the first frame update
    void Awake()
    {
        DontDestroyOnLoad(this);
        if (instance == null)
        {
            instance = this.gameObject;
        }
        else
        {
            Destroy(this.gameObject);
        }
        canvas = GetComponent<Canvas>();
        canvas.worldCamera = GameObject.Find("Moving Main Camera").GetComponent<Camera>();
    }
    void Update()
    {
        if (canvas == null)
        {
            canvas = GetComponent<Canvas>();
            canvas.worldCamera = GameObject.Find("Moving Main Camera").GetComponent<Camera>();            
        }
        
        if (canvas.worldCamera == null)
        {
            canvas.worldCamera = GameObject.Find("Moving Main Camera").GetComponent<Camera>();
        }
    }
}
