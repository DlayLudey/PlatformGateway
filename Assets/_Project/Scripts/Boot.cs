using System.Collections;
using CarrotHood.PlatformGateway;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Boot : PlatformGateway
{
	private IEnumerator Start()
	{
		yield return Init();
		
		SceneManager.LoadScene(1);
	}
}