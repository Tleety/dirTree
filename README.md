
# dirTree

This project is created in Unity 3D with C# as a test evaluation.
The main goal is to archive the task while being easy to read for anyone not familiar with Unity.

The project is to create a visual UI of a simple tree directory generated from a string array.

Current TODO:
 - [O] Create a directoryItem
   - [X] Able to have a name
   - [X] Able to have a parent
   - [X] Able to have children if it's a folder
   - [X] Know if it's a folder or not
   - [X] Ability to open folders
   - [X] Button to open fodlers
   - [X] Remove child
   - [X] Add child
   - [X] Create children recursively from a string path
   - [ ] Remove add button when directory is closed
   - [ ] Remove add button when not a directory child
 - [O] Create a TreeManager
   - [X] Holds the root DirectoryItem
   - [X] Create new child with a parent.
   - [X] Divide full string paths into items
   - [X] Create folders and items from the full string paths
   - [ ] Update scrollList content height on tree changes
 - [ ] Documentation
   - [ ] DirectoryItem
   - [ ] TreeManager
   - [ ] General
