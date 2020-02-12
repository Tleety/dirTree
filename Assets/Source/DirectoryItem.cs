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
	}
	
	public bool IsDirectory()
	{
		return isDirectory;
	}

	public string GetName()
	{
		return gameObject.name;
	}

	public void SetName(string newName)
	{
		name = newName;
		gameObject.GetComponentInChildren<Text>().text = newName;
		if (gameObject.name.EndsWith("/") || gameObject.name.EndsWith("\\"))
		{
			isDirectory = true;
			isOpen      = true;
		}
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

	public GameObject AddChild(GameObject prefab, string itemName)
	{
		if (!isDirectory)
		{
			Debug.LogWarning("Trying to create a child to non directory.");
			return null;
		}

		var dirItem = Instantiate(prefab, transform);
		var dirItemComponent = dirItem.GetComponent<DirectoryItem>();
		dirItemComponent.SetName(itemName);

		Children.Add(dirItemComponent);

		return dirItem;
	}

	public void ChangeOpenState()
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

	public float UpdateChildPositions()
	{
		if (GetChildCount() <= 0 || !isOpen)
			return 0;

		var offsetPosition = 0.0f;
		for(var i = 0; i < GetChildCount(); i++)
		{
			Children[i].gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(20, (-40 * (1+i))+offsetPosition);
			offsetPosition += Children[i].UpdateChildPositions();

		}
		return Children[GetChildCount() - 1].GetComponent<RectTransform>().anchoredPosition.y;
	}
}
