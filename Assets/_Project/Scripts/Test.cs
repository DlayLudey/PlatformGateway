using System;
using UnityEngine;

public class Test : MonoBehaviour
{
	public TestStr testStr;
}

[Serializable]
public class TestStr : Weighted<string>
{

}
