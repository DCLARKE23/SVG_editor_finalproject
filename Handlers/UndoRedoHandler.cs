namespace SVG_editor_finalproject.Handlers
{
    public class UndoRedoHandler
    {
        public DocumentModel Current;
        public Stack<DocumentModel> UndoStack { get; private set; }
        public Stack<DocumentModel> RedoStack { get; private set; }
        public UndoRedoHandler(DocumentModel doc)
        {
            Current = doc;
            UndoStack = new Stack<DocumentModel>();
            RedoStack = new Stack<DocumentModel>();
        }
        public void ResetStack(DocumentModel current)
        {
            Current = current;
            UndoStack.Clear();
            RedoStack.Clear();
        }
        public DocumentModel? Undo()
        {
            if (UndoStack.Count == 0)
            {
                return null;
            }
            RedoStack.Push(Current);
            Current = UndoStack.Pop();
            return Current.Clone();
        }
        public DocumentModel? Redo()
        {
            if (RedoStack.Count == 0)
            {
                return null;
            }
            UndoStack.Push(Current);
            Current = RedoStack.Pop();
            return Current.Clone();
        }
        public void NewItem(DocumentModel item)
        {
            UndoStack.Push(Current);
            Current = item;
            RedoStack.Clear();
        }
    }
}
