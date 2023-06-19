namespace Odin
{
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
