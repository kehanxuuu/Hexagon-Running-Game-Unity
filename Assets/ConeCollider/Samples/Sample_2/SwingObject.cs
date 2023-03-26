using UnityEngine;
using System.Collections;

public class SwingObject : MonoBehaviour {

    private float max = 15;
    private float count = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        count += Time.deltaTime * 10f;
        if (count > 360) count = 0;
        var x = Mathf.Sin(count * Mathf.PI / 180) * max;
        this.transform.position = new Vector3(x, this.transform.position.y, this.transform.position.z);

    }
}
