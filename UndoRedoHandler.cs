﻿namespace SVG_editor_finalproject
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

        public DocumentModel Undo()
        {
            RedoStack.Push(Current);
            Current = UndoStack.Pop();
            return Current;
        }

        public DocumentModel Redo() 
        {
            UndoStack.Push(Current);
            Current = RedoStack.Pop();
            return Current;
        }

        public void NewItem(DocumentModel item)
        {
            UndoStack.Push(Current);
            Current = item;
            RedoStack.Clear();
        }
    }
}
