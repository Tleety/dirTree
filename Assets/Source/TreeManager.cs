using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeManager : MonoBehaviour
{

	[SerializeField]
	private DirectoryItem Root;
	
	[SerializeField]
	private GameObject DirectoryItemPrefab;

	public string InputArray;
    // Start is called before the first frame update
    void Start()
    {
	    CreateItem(Root, "SomeItem");
	    CreateItem(Root, "newfolder/");
	    CreateItem(Root, "folder2/");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	private bool CreateItem(DirectoryItem parent, string dirName)
	{
		if (dirName == null)
			return false;
		if (parent == null)
		{
			parent = Root;
		}

		return parent.AddChild(DirectoryItemPrefab, dirName);
	}
}
