using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Utils.Editor.Bookmarks
{
    [CreateAssetMenu(menuName = "Bookmark", fileName = "BookmarkData")]
    public class BookmarkScriptableObject : ScriptableObject
    {
        [System.Serializable]
        public class BookmarkEntry
        {
            public bool     ToggleLabel;
            public Object   Reference;
            public string   Description;
        }

        public List<BookmarkEntry> bookmarkLines = new List<BookmarkEntry>();

        public bool showReference = true;
        public Vector2 scrollPosition = Vector2.zero;

        public void AddNewSlot()
        {
            if (Selection.activeObject != null)
            {
                for (int i = 0; i < Selection.objects.Length; i++)
                {
                    bookmarkLines.Add(new BookmarkEntry());

                    int lastIndex = bookmarkLines.Count - 1;

                    //Automatically assign selected object to the reference
                    bookmarkLines[lastIndex].Reference = Selection.objects[i];

                    //Set description to become the selected object's path - Requested by Olivier B
                    bookmarkLines[lastIndex].Description = AssetDatabase.GetAssetPath(Selection.objects[i]);
                }
            }
            else
            {
                bookmarkLines.Add(new BookmarkEntry());
            }
        }

        public void Remove(int index)
        {
            bookmarkLines.RemoveAt(index);
        }
    }
}

