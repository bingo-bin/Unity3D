using UnityEngine;
using System.Collections;
using Net;
public class TestSer : MonoBehaviour {

    ClientSocket mSocket;
	void Start () {
        mSocket = new ClientSocket();
        mSocket.ConnectServer("127.0.0.1", 8088);
        mSocket.SendMessage("服务器");
    }

    void Update()
    {
        if (Time.frameCount % 60 == 0)
        {
            mSocket.SendMessage("time:" + Time.frameCount);
        }
    }
}
