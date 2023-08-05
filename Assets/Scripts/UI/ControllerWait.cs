using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerWait : MonoBehaviour
{

    [SerializeField]
    private GameObject C1, C2,C3;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void showControllersRight()
    {
        C1.SetActive(true);
    }

    public void showControllersleft()
    {
        C2.SetActive(true);

        C3.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
