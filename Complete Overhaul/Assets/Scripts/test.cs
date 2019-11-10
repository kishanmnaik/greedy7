using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    // Start is called before the first frame update
    IEnumerator Start()
    {
        string name1 = "qa";
        string name2 = "zz";

        string url = "http://localhost:8888/sqlconnect/getScore.php?name1=" + name1+"&name2="+name2;
        WWW request = new WWW(url);
        yield return request;
        Debug.Log(request.text);
        Debug.Log(request.text);
        Debug.Log(request.text);
        Debug.Log(request.text);
        Debug.Log(request.text);

    }
}
