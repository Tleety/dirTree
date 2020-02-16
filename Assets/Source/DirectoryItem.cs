using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// DirectoryItem is the base class of the tree, it holds all the data needed to draw the ui as well as functions to
/// interact with the directory items.
/// The purpose of the class is to be as self contained as possible, having authority of itself and it's children.
/// Most functionality is recursive and will propagate down through children.
/// This is a MonoBehaviour, a base Unity class that can be attached as components to GameObjects and thus edited and
/// viewed from the editor.
/// 
/// A directory item has a few main data:
/// Name
/// 	The name will show in the tree and any name that ends in a "/" will become a directory.
/// Children
/// 	A list of all the children of this specific Directory Item.
/// UpdatePos
/// 	A unity callback function that is set by the TreeManager. It will be called if the Directory Item needs to
/// 	update the view of the tree structure.
/// Prefab
/// 	A unity prefab GameObject, which is basically a template of a GameObject. It is used to create new copies of
/// 	Directory Item when a child is added.
///
/// </summary>
public class DirectoryItem : MonoBehaviour
{
	//A SerializeField is a tag to show private properties in the unity editor.
	[SerializeField]
	private List<DirectoryItem> Children;

	private bool isDirectory    = false;
	private bool isOpen         = false;
	private bool markedForDeath = false;

	private UnityAction updatePos;
	private GameObject  prefab;

	/// <summary>
	/// Awake is a default unity function, it works almost as a constructor with some unity quirks.
	/// Whenever a GameObject is created it Awake will be called.
	/// </summary>
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

	private bool EndsWithSlash(string text)
	{
		return text.EndsWith(value: "/") || text.EndsWith(value: "\\");
	}

	private int GetChildCount()
	{
		return Children.Count;
	}

	private bool HasChildren()
	{
		return GetChildCount() > 0;
	}

	/// <summary>
	/// Sets the name of a Directory Item and sets directory and open status based on name.
	/// </summary>
	/// <param name="newName">New name of the item</param>
	public void SetName(string newName)
	{
		//If the item has children and the new name does not end in a slash, we return since a non items can not have
		//children.
		if (!EndsWithSlash(newName) && HasChildren())
			return;

		name                                           = newName;
		gameObject.GetComponentInChildren<Text>().text = newName;
		if (gameObject.name.EndsWith(value: "/") || gameObject.name.EndsWith(value: "\\"))
		{
			isDirectory = true;
			isOpen      = true;
		}
		else
		{
			isDirectory = false;
			isOpen      = false;
		}
	}

	public void SetUpdatePos(UnityAction updatePosAction)
	{
		updatePos = updatePosAction;
	}

	public void SetPrefab(GameObject prefabDirItem)
	{
		prefab = prefabDirItem;
	}

	/// <summary>
	/// This function is called after a new name has been put into the inputField.
	/// The input field will be shown after pressing the 'e' button.
	/// </summary>
	/// <param name="inputFieldGameObject">The GameObject of the input field.</param>
	public void OnNameInputEnd(GameObject inputFieldGameObject)
	{
		var textComponent = inputFieldGameObject.transform.Find(n: "Text").GetComponent<Text>();
		SetName(textComponent.text);
		inputFieldGameObject.SetActive(value: false);
	}

	/// <summary>
	/// A void return AddChild function since unityEvents cant be connected to any function with a return value.
	/// Called when you press the green '+' button.
	/// </summary>
	/// <param name="name">The name of the new child.</param>
	public void AddChild(string name)
	{
		AddChildWithReturn(name);
	}

	/// <summary>
	/// Function to add a child.
	/// If the current object is not a directory, it should not be able to have children.
	/// Otherwise it will Instantiate a prefab of the directory item GameObject and get the DirectoryItem component
	/// that is attached to it.
	/// It will then set a few properties needed by all Directory Items. They are set here because a prefab does not
	/// have knowledge of them, so they have to be added after Instantiation.
	///
	/// It will add children to the Children list before invoking the TreeManager to update the tree structure.
	/// The structure is updated by callback instead of the UpdateLoop because it will save some performance.
	/// In this program, might be unnecessary but to me it's also makes the code more readable.
	/// </summary>
	/// <param name="name">The new name of the child.</param>
	/// <returns>The created child main GameObject</returns>
	private GameObject AddChildWithReturn(string name)
	{
		if (!isDirectory)
			return null;
		var dirItem          = Instantiate(prefab, transform);
		var dirItemComponent = dirItem.GetComponent<DirectoryItem>();

		dirItemComponent.SetUpdatePos(updatePos);
		dirItemComponent.SetName(name);
		dirItemComponent.SetPrefab(prefab);

		Children.Add(dirItemComponent);
		updatePos.Invoke();
		return dirItemComponent.gameObject;
	}

	/// <summary>
	/// This function will create children from a full path.
	/// It will split the path in 2 at the first '/'. If it is successfully split in 2, we will add back the '/'.
	/// 
	/// The first part of the path is the next child in the string, so we check if it exist as a child to this item.
	/// If it does not exist, we create it as a child and save the return as the next child in line.
	/// 
	/// This function is then called on the child with the rest of the path.
	/// </summary>
	/// <param name="path">A path originating from root where each item is divided by '/'.</param>
	public void AddChildFromPath(string path)
	{
		var paths = path.Split(new[] {'/'}, count: 2);
		if (paths.Length == 2)
			paths[0] += "/";

		var nextDirItem = FindChild(paths[0]);
		if (nextDirItem == null)
			nextDirItem = AddChildWithReturn(paths[0]);

		if (paths.Length == 2)
			nextDirItem.GetComponent<DirectoryItem>()?.AddChildFromPath(paths[1]);
	}

	/// <summary>
	/// Loop through Children and find any with the correct name.
	/// Usually I would use 'transform.Find(name)' instead, which is a native Unity function.
	/// However that function assume that any '/' is the beginning of a new item, and does not work if the name
	/// actually includes that sign. That took me a while to figure out.
	/// </summary>
	/// <param name="name">Name of the child</param>
	/// <returns>The child GameObject if found, or null if not.</returns>
	private GameObject FindChild(string name)
	{
		foreach (var child in Children)
		{
			if (child.name == name)
				return child.gameObject;
		}

		return null;
	}

	/// <summary>
	/// Changes open state for the Directory Item unless its not a directory or if it has not children.
	/// It will change children viability state as well as call the Update function in TreeManager to fix item placements.
	/// </summary>
	public void ChangeOpenState()
	{
		if (!IsDirectory() || !HasChildren())
			return;
		isOpen = !isOpen;
		UpdateChildVisibility();
		updatePos.Invoke();
	}

	/// <summary>
	/// Called when pressing the '-' button. Sets the gameObject to inactive and marks it for death.
	/// Any object marked for death will be removed from Childlists in the UpdatePos loop, thus allowing it to be
	/// safely destroyed afterwards.
	/// </summary>
	public void DeleteThis()
	{
		gameObject.SetActive(value: false);
		markedForDeath = true;
		updatePos.Invoke();
		Destroy(gameObject);
	}

	/// <summary>
	/// If the Directory Item has children, they are set to inactive if the directory is closed, and vise versa.
	/// </summary>
	private void UpdateChildVisibility()
	{
		if (HasChildren())
			return;
		foreach (var child in Children)
		{
			child.gameObject.SetActive(isOpen);
		}
	}

	/// <summary>
	/// Recursively updates the position in the tree of DirectoryItems depth-first style.
	/// </summary>
	/// <returns>The y position that the next Directory item should take.</returns>
	public float UpdateChildPositions()
	{
		// If the Directory Item is inactive, we ignore it and return 0.
		if (!gameObject.activeSelf)
			return 0;

		// Sets the next position to the hardcoded height of a DirectoryItem
		var nextPos = 105f;
		// If it's not open, we are at the end and return the next position.
		if (!isOpen)
			return nextPos;

		// Loop through children, removing any marked for death and set positions on this item before going down 
		// further in the tree, since the parent should be above the children in the tree.
		for (var i = 0; i < GetChildCount(); i++)
		{
			var child = Children[i];
			if (child.IsMarkedForDeath())
			{
				Children.RemoveAt(i);
				i--;
				continue;
			}

			var rect = child.GetComponent<RectTransform>();
			rect.anchoredPosition = new Vector2(x: 20, y: -nextPos);
			rect.offsetMax        = new Vector2(x: 0,  y: rect.offsetMax.y);

			nextPos += child.UpdateChildPositions();
		}

		return nextPos;
	}
}
