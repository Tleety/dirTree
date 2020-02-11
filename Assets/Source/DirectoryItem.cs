using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class DirectoryItem : MonoBehaviour
{
	private List<DirectoryItem> Children;

	private bool isDirectory = false;

	
	private void Awake()
	{
		if (gameObject.name.EndsWith("/") || gameObject.name.EndsWith("\\"))
		{
			isDirectory = true;
		}
	}
	
	public bool IsDirectory()
	{
		return isDirectory;
	}

	public string GetName()
	{
		return gameObject.name;
	}

	public List<DirectoryItem> GetChildren()
	{
		return Children;
	}

	public int GetChildCount()
	{
		return Children.Count;
	}

	public DirectoryItem GetChild(int n)
	{
		return Children[n];
	}

}
