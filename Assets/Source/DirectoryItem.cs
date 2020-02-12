using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class DirectoryItem : MonoBehaviour
{
	private List<DirectoryItem> Children;

	private bool isDirectory = false;
	private bool isOpen = false;

	
	private void Awake()
	{
		Children = new List<DirectoryItem>();
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

	private int GetChildCount()
	{
		return Children.Count;
	}

	public DirectoryItem GetChild(int n)
	{
		return Children[n];
	}

	public bool AddChild(GameObject prefab, string itemName)
	{
		if (!isDirectory)
			return false;

		var dirItem = Instantiate(prefab, transform);
		dirItem.name = itemName;
		dirItem.gameObject.GetComponentInChildren<Text>().text = itemName;

		Children.Add(dirItem.GetComponent<DirectoryItem>());

		UpdateChildPositions();

		return true;
	}

	public void ChangeOpenPosition()
	{
		isOpen = !isOpen;
		UpdateChildVisibility();
	}

	private void UpdateChildVisibility()
	{
		foreach (var child in Children)
		{
			child.gameObject.SetActive(isOpen);
		}
	}

	private void UpdateChildPositions()
	{
		for(var i = 0; i < GetChildCount(); i++)
		{
			Children[i].gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(20, -40 * (1+i));
		}
	}

}
