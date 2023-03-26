using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InCheck : MonoBehaviour {

    private Text text;

    void Awake()
    {
        text = GameObject.Find("Text").GetComponent<Text>();
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other)
    {
        text.text = "IN";
        text.color = Color.red;
    }

    void OnTriggerExit(Collider other)
    {
        text.text = "OUT";
        text.color = Color.blue;
    }
}
