using UnityEngine;
using System.Collections;

public class NewBehaviourScript : MonoBehaviour {

    public GameObject pushertomoveball ;

    void Start()
    {
        
    }

    void OnTriggerEnter( Collider col)
    {
      if (col.gameObject.name=="Ball")
            pushertomoveball.SetActive(false);

    }
}
