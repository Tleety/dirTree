using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeManager : MonoBehaviour
{

	private DirectoryItem Root;
	private static GameObject DirectoryItemPrefab;

	public string InputArray;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	private bool CreateFolder(DirectoryItem parent, string name)
	{
		if (parent == null)
			return false;
		if (name == null)
			return false;
		return true;
	}
}
