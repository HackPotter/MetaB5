using UnityEditor;
using UnityEngine;

namespace uTest
{
    public class uTestWindow : EditorWindow
    {
        [MenuItem("Metablast/Testing/uTest")]
        public static void ShowTestWindow()
        {
            EditorWindow.GetWindow<uTestWindow>().Show();
        }

        private TestRunner _testRunner;
        private Vector2 _scrollPosition;

        void OnGUI()
        {
            if (_testRunner == null)
            {
                _testRunner = new TestRunner();
            }

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Refresh", EditorStylesExt.ButtonLeft))
            {
                _testRunner = new TestRunner();
            }

            if (GUILayout.Button("Run All", EditorStylesExt.ButtonMiddle))
            {
                foreach (TestFixtureDefinition fixture in _testRunner.TestFixtures)
                {
                    fixture.Run();
                }
            }
            if (GUILayout.Button("Clear Results", EditorStylesExt.ButtonRight))
            {
                foreach (TestFixtureDefinition fixture in _testRunner.TestFixtures)
                {
                    fixture.ClearResults();
                }
            }
            GUILayout.EndHorizontal();

            _scrollPosition = GUILayout.BeginScrollView(_scrollPosition);
            foreach (TestFixtureDefinition fixture in _testRunner.TestFixtures)
            {
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("Run", EditorStylesExt.LockedHeaderButton, GUILayout.ExpandWidth(false)))
                {
                    fixture.Run();
                    Repaint();
                }
                TestStatus fixtureTestStatus = fixture.TestStatus;

                Color? fixtureLabelColor = null;
                if (fixtureTestStatus == TestStatus.Passed)
                {
                    fixtureLabelColor = EditorStylesExt.EditorGreen;
                }
                if (fixtureTestStatus == TestStatus.Failed)
                {
                    fixtureLabelColor = EditorStylesExt.EditorRed;
                }
                EditorGUILayoutExt.BeginStyle(GUI.skin.label, 12, FontStyle.Bold, fixtureLabelColor, null);
                GUILayout.Label(fixture.Name, GUI.skin.label, GUILayout.ExpandWidth(false));
                EditorGUILayoutExt.EndStyle();
                //
                GUILayout.Label("(" + fixture.Tests.Count + " tests)");

                GUILayout.EndHorizontal();

                if (fixtureTestStatus != TestStatus.Passed)
                {
                    GUILayout.BeginVertical(GUI.skin.box);
                    int index = 0;
                    foreach (TestDefinition test in fixture.Tests)
                    {
                        GUILayout.BeginHorizontal();
                        GUILayout.Space(10);
                        Rect statusRect = GUILayoutUtility.GetRect(new GUIContent("__"), GUI.skin.label, GUILayout.ExpandWidth(false));

                        switch (test.Result.TestStatus)
                        {
                            case TestStatus.Failed:
                                EditorGUILayoutExt.BeginStyle(GUI.skin.label, null, FontStyle.Bold, EditorStylesExt.EditorRed, null);
                                GUI.Label(statusRect, "X");
                                EditorGUILayoutExt.EndStyle();
                                break;
                            case TestStatus.Passed:
                                EditorGUILayoutExt.BeginStyle(GUI.skin.label, null, FontStyle.Bold, EditorStylesExt.EditorGreen, null);
                                GUI.Label(statusRect, EditorStylesExt.Checkmark);
                                EditorGUILayoutExt.EndStyle();
                                break;
                            case TestStatus.Unknown:
                                EditorGUILayoutExt.BeginStyle(GUI.skin.label, null, FontStyle.Bold, EditorStylesExt.EditorGray, null);
                                GUI.Label(statusRect, "?");
                                EditorGUILayoutExt.EndStyle();
                                break;
                        }

                        if (GUILayout.Button("Run", EditorStylesExt.ToolbarButton, GUILayout.ExpandWidth(false)))
                        {
                            fixture.RunSingle(index);
                            Repaint();
                        }
                        GUILayout.Label(test.TestName, EditorStylesExt.ToolbarButton, GUILayout.ExpandWidth(false));
                        GUILayout.EndHorizontal();


                        if (test.Result.TestStatus == TestStatus.Failed)
                        {
                            GUILayout.BeginHorizontal();
                            GUILayout.Space(10);
                            GUILayout.TextArea(test.Result.ResultMessage);
                            GUILayout.EndHorizontal();
                        }




                        index++;
                        GUILayout.Space(3);
                    }

                    GUILayout.EndVertical();
                }
                GUILayout.Space(10);
            }

            GUILayout.EndScrollView();
        }
    }
}