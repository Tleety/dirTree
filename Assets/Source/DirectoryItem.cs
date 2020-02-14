using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DirectoryItem : MonoBehaviour
{
	private List<DirectoryItem> Children;

	private bool isDirectory = false;
	private bool isOpen = false;
	private bool markedForDeath = false;

	private UnityAction updatePos;

	private void Awake()
	{
		Children = new List<DirectoryItem>();
	}
	
	public bool IsDirectory()
	{
		return isDirectory;
	}

	public bool IsMarkedForDeath()
	{
		return markedForDeath;
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

	private int GetChildCount()
	{
		return Children.Count;
	}

	public GameObject AddChild(GameObject prefab, string itemName, UnityAction updateChildPositionAction)
	{
		if (!isDirectory)
		{
			Debug.LogWarning("Trying to create a child to non directory.");
			return null;
		}

		var dirItem = Instantiate(prefab, transform);
		var dirItemComponent = dirItem.GetComponent<DirectoryItem>();
		dirItemComponent.SetUpdatePos(updateChildPositionAction);

		dirItemComponent.SetName(itemName);

		Children.Add(dirItemComponent);

		return dirItem;
	}

	public void SetUpdatePos(UnityAction updatePosAction)
	{
		updatePos = updatePosAction;
	}

	public void ChangeOpenState()
	{
		if(!IsDirectory() || GetChildCount() <= 0)
			return;
		isOpen = !isOpen;
		UpdateChildVisibility();
		updatePos.Invoke();
	}

	public void DeleteThis()
	{
		gameObject.SetActive(false);
		markedForDeath = true;
		updatePos.Invoke();
		Destroy(gameObject);
	}

	private void UpdateChildVisibility()
	{
		if(GetChildCount() <= 0)
			return;
		foreach (var child in Children)
		{
			child.gameObject.SetActive(isOpen);
		}
	}

	public float UpdateChildPositions()
	{
		if (!gameObject.activeSelf)
			return 0;
		var nextPos = 105f;
		if (!isOpen)
			return nextPos;

		

		for(var i = 0; i < GetChildCount(); i++)
		{
			var child = Children[i];
			if (child.IsMarkedForDeath())
			{
				Children.RemoveAt(i);
				i--;
				continue;
			}
			var rect = child.GetComponent<RectTransform>();
			rect.anchoredPosition = new Vector2(20, -nextPos);
			rect.offsetMax = new Vector2(0, rect.offsetMax.y);
			nextPos += child.UpdateChildPositions();
		}

		return nextPos;
	}
}
