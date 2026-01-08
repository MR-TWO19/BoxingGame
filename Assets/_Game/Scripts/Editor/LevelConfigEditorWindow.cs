using TwoCore;
using UnityEditor;
using UnityEngine;

public class LevelConfigEditorWindow : BaseEditorWindow
{
    #region static
    public static LevelConfigEditorWindow Ins { get; private set; }

    [MenuItem("TwoCore/Level Config &v", false, 1)]
    private static void OnMenuItemClicked()
    {
        //LevelConfig.CreateAsset(typeof(LevelConfig), "LevelConfig.asset");
        OpenWindow();
    }

    public static void OpenWindow()
    {
        Ins = (LevelConfigEditorWindow)GetWindow(typeof(LevelConfigEditorWindow), false, "Level Setting");
        Ins.autoRepaintOnSceneChange = true;
    }
    #endregion

    #region template
    protected TabContainer tabContainer;
    protected GameModeConfig gameModeConfig;

    protected override Object GetTarget() => GameModeConfig.Ins;

    protected override void OnEnable()
    {
        base.OnEnable();
        Ins = this;
        gameModeConfig = GameModeConfig.Ins;
        tabContainer = new TabContainer();
        _Init();
    }

    protected override void OnDraw()
    {
        Undo.RecordObject(gameModeConfig, "Game Mode Config");

        _OnDraw();

        if (GUI.changed)
            EditorUtility.SetDirty(GameModeConfig.Ins);
    }
    #endregion

    private void _Init()
    {
        // create view here
        tabContainer.AddTab("One Vs One", new OneVsOneTab());
        tabContainer.AddTab("One Vs Many", new OneVsManyTab());
    }

    private void _OnDraw()
    {
        tabContainer.DoDraw();

        // customize draw here
    }
}
