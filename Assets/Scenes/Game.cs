using System.Collections;
using UnityEngine;

public class Game : MonoBehaviour
{
    private ArrayList cubes;
    private GameObject cude;
    void Awake(){

        float distance = 18;
        float halfFOV = (Camera.main.fieldOfView * 0.5f) * Mathf.Deg2Rad;
        float aspect = Camera.main.aspect;

        float height = distance * Mathf.Tan(halfFOV);
        float width = height * aspect;

        vitamin.Logger.info(width,height);
        int total = 100;
        cubes = new ArrayList();
        while (total>0)
        {
            
           GameObject cc = GameObject.CreatePrimitive(PrimitiveType.Cube);
           cc.transform.Rotate(new Vector3(Random.Range(0,360), Random.Range(0, 360), Random.Range(0, 360)));
           cc.transform.localPosition=new Vector3(Random.Range(-width / 2, width / 2), Random.Range(-height/2, height/2), Random.Range(500,1000));
           this.cubes.Add(cc);
           total--;
        }
        cude=GameObject.Find("Cube");
    }
    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        foreach(GameObject cude in cubes)
        {
           cude.transform.Rotate(new Vector3(1, 1, 0));
        }
        cude.transform.Rotate(new Vector3(1, 1, 0));
        //Debug.Log(this.cude.transform.rotation);
    }

}
