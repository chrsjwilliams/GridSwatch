using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

//UtilScript uses a collection of static functions
//to make programming more convinent in Unity.
//You can think of it as a very basic version of a
//library, like UnityEngine or System.

public class UtilScript : MonoBehaviour
{



	/// <summary>
	/// Make a copy of a Vector3
	/// </summary>
	/// <param name="vec">Vector3 to Clone</param>
	public static Vector3 CloneVector3(Vector3 vec){
		return new Vector3(vec.x, vec.y, vec.z);
	}
		
	/// <summary>
	/// Make a copy of a Vector3 and modify some values
	/// </summary>
	/// <param name="vec">Vector3 to Clone</param>
	/// <param name="xMod">amount to mod x value by</param>
	/// <param name="yMod">amount to mod y value by</param>
	/// <param name="zMod">amount to mod z value by</param>
	public static Vector3 CloneModVector3(
		Vector3 vec, 
		float xMod,
		float yMod,
		float zMod){
		return new Vector3(
			vec.x + xMod,
			vec.y + yMod,
			vec.z + zMod);
	}
}
