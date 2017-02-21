using System;

public class CommentAttribute : Attribute
{
    public string Comment
    {
        get;
        private set;
    }

    public CommentAttribute(string comment)
    {
        Comment = comment;
    }
}

