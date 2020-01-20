public enum DialogIndex
{
    MessageDialog,
    PauseDialog
}

public enum DialogMessageType
{
    Start,
    Win,
    Lose
}

public class DialogConfig
{
    public static DialogIndex[] dialogIndices =
    {
        DialogIndex.MessageDialog,DialogIndex.PauseDialog
    };
}

public class DialogParam
{

}

public class MessageDialogParam : DialogParam
{
    public DialogMessageType messageType = DialogMessageType.Start;
    public LevelCounter levelCounter = LevelCounter.Moves;
    public int scoreGoal = 0;
    public int timeGoal = 0;
    public int moveGoal = 0;
}

