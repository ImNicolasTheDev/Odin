namespace Odin
{
    /// <summary>
    /// Link class used to store links to open by default in-app
    /// </summary>
    public class Link
    {
        public int Id { get; init; }
        public string Text { get; set; }

        public Link(int id, string link)
        {
            Id = id;
            Text = link;
        }
    }
}
