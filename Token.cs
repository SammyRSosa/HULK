public class Token
{
    public string Type { get; private set; }
    public int Position { get; private set; }
    public string Content { get; private set; }
    public Token(string Type, string Content, int Position)
    {
        this.Type = Type;
        this.Content = Content;
        this.Position = Position;
    }
    public override string ToString()
    {
        return $"{this.Type}, {this.Content}, {this.Position}";
    }
}