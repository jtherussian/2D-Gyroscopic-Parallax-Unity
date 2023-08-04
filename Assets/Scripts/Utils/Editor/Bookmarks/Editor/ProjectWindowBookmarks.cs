using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEditorInternal;
using BookmarkEntry = Utils.Editor.Bookmarks.BookmarkScriptableObject.BookmarkEntry;

namespace Utils.Editor.Bookmarks.Editor
{
    public class ProjectWindowBookmarks : EditorWindow
    {
        [SerializeField] private BookmarkScriptableObject _referenceData;

        private SerializedObject serializedObject = null;
        private ReorderableList reorderableList = null;

        private static string path = "Assets/Scripts/Utils/Editor/Bookmarks/Resources";
        private static string asset = "BookmarkData/BookmarkData";

        #region Unity Methods

        private void OnEnable()
        {
            _referenceData = GetBookmarkData();
            CreateReorderableList();
        }

        private void OnGUI()
        {
            // make sure data asset exist
            if (_referenceData == null)
            {
                _referenceData = GetBookmarkData();
                CreateReorderableList();
            }

            ButtonToggleReferenceLabel();

            ButtonUpdateReferencePaths();

            //create a scroll view when it's needed
            _referenceData.scrollPosition = EditorGUILayout.BeginScrollView(_referenceData.scrollPosition);
            {
                //stack the list and button side by side
                EditorGUILayout.BeginHorizontal();
                {
                    DrawerReorderableList();
                    DrawerCloseButton();
                }
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndScrollView();

            //Add new references
            if (GUILayout.Button("Add Bookmarks"))
            {
                AddProjectBookmark();
            }

            //Save data asset if modified
            EditorUtility.SetDirty(_referenceData);
        }

        #endregion

        #region Static Methods

        [MenuItem("Tools/Project Window Bookmarks"), MenuItem("Assets/Bookmark/Show Bookmark")]
        public static void ShowWindow()
        {
            // needed to actually open the bookmark windows.
            ProjectWindowBookmarks aWindow = GetWindow<ProjectWindowBookmarks>();
            aWindow.titleContent.text = MethodBase.GetCurrentMethod().DeclaringType.Name;
            ;
        }

        [MenuItem("Assets/Bookmark/Add")]
        public static void AddProjectBookmark()
        {
            GetBookmarkData().AddNewSlot();

            //Open window, if already open then it will refresh it
            ShowWindow();
        }

        public static BookmarkScriptableObject GetBookmarkData()
        {
            var ScriptableData = Resources.Load<BookmarkScriptableObject>(asset);

            //create a data asset if it does not exist yet.
            if (ScriptableData == null)
            {
                ScriptableData = CreateInstance<BookmarkScriptableObject>();

                if (!AssetDatabase.IsValidFolder(path + "/BookmarkData"))
                {
                    AssetDatabase.CreateFolder(path, "BookmarkData");
                }

                AssetDatabase.CreateAsset(ScriptableData, path + "/" + asset + ".asset");

                ScriptableData = Resources.Load<BookmarkScriptableObject>(asset);
            }

            return ScriptableData;
        }

        #endregion

        #region SetupReorderableList

        private void CreateReorderableList()
        {
            serializedObject = new SerializedObject(_referenceData);

            reorderableList = new ReorderableList(serializedObject,
                serializedObject.FindProperty(nameof(BookmarkScriptableObject.bookmarkLines)),
                true, true, false, false);

            reorderableList.elementHeight = 20f; // set the height of each list element

            reorderableList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
            {
                var element = reorderableList.serializedProperty.GetArrayElementAtIndex(index);
                rect.y += 2;

                //Display Label Toggle
                EditorGUI.PropertyField(new Rect(rect.x, rect.y, 10f, EditorGUIUtility.singleLineHeight),
                    element.FindPropertyRelative(nameof(BookmarkEntry.ToggleLabel)), GUIContent.none);

                //Display Reference
                if (element.FindPropertyRelative(nameof(BookmarkEntry.ToggleLabel)).boolValue)
                {
                    EditorGUI.PropertyField(
                        new Rect(rect.x + 20f, rect.y, rect.width - 20f, EditorGUIUtility.singleLineHeight),
                        element.FindPropertyRelative(nameof(BookmarkEntry.Description)), GUIContent.none);
                }
                else
                {
                    //If true, display both Label and Reference, else Display Reference
                    if (_referenceData.showReference)
                    {
                        EditorGUI.PropertyField(
                            new Rect(rect.x + 20f, rect.y, (rect.width - 20f) * 0.5f,
                                EditorGUIUtility.singleLineHeight),
                            element.FindPropertyRelative(nameof(BookmarkEntry.Description)), GUIContent.none);
                        EditorGUI.PropertyField(
                            new Rect(rect.x + 20f + ((rect.width - 20f) * 0.5f), rect.y, (rect.width - 20f) * 0.5f,
                                EditorGUIUtility.singleLineHeight),
                            element.FindPropertyRelative(nameof(BookmarkEntry.Reference)), GUIContent.none);
                    }
                    else
                    {
                        EditorGUI.PropertyField(
                            new Rect(rect.x + 20f, rect.y, rect.width - 20f, EditorGUIUtility.singleLineHeight),
                            element.FindPropertyRelative(nameof(BookmarkEntry.Reference)), GUIContent.none);
                    }
                }
            };
        }


        private void DrawerReorderableList()
        {
            //Draw Layout for the list
            GUILayout.BeginVertical(GUILayout.ExpandWidth(true));
            {
                //Draw the ReorderableList, update value and save
                serializedObject.Update();
                reorderableList.DoLayoutList();
                serializedObject.ApplyModifiedProperties();

                _referenceData = (BookmarkScriptableObject) serializedObject.targetObject;

                EditorUtility.SetDirty(_referenceData);
            }
            GUILayout.EndVertical();
        }

        //[SerializeField] private float closeButtonSpacing = 25f;
        //[SerializeField] private float closeButtonVerticalSpacing = 25f;

        private void DrawerCloseButton()
        {
            //Draw Layout for the close button
            GUILayout.BeginVertical(GUILayout.MaxWidth(25f));

            if (GUILayout.Button("ALL", GUILayout.Height(20f), GUILayout.ExpandWidth(true)))
            {
                _referenceData.bookmarkLines.Clear();
            }

            GUILayout.Space(3f);
            for (int i = 0; i < _referenceData.bookmarkLines.Count; i++)
            {
                GUILayout.Space(0f);
                if (GUILayout.Button("X", GUILayout.Height(20f), GUILayout.ExpandWidth(true)))
                {
                    _referenceData.bookmarkLines.RemoveAt(i);
                }

                GUILayout.Space(0f);
            }

            GUILayout.EndVertical();
        }


        private void ButtonToggleReferenceLabel()
        {
            if (_referenceData.showReference)
            {
                if (GUILayout.Button("Hide Description"))
                {
                    _referenceData.showReference = false;
                }
            }
            else
            {
                if (GUILayout.Button("Show Description"))
                {
                    _referenceData.showReference = true;
                }
            }
        }

        private void ButtonUpdateReferencePaths()
        {
            if (_referenceData.showReference)
            {
                if (GUILayout.Button("Update Path Reference"))
                {
                    for (int i = 0; i < _referenceData.bookmarkLines.Count; i++)
                    {
                        _referenceData.bookmarkLines[i].Description =
                            AssetDatabase.GetAssetPath(_referenceData.bookmarkLines[i].Reference);
                    }
                }
            }
        }

        #endregion
    }
}