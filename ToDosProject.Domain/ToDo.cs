namespace ToDosProject.Domain
{
    public class ToDo
    {
        private ToDo() {}
        public ToDo(int id, string title)
        {
            Id = id;
            Title = title;
        }

        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public bool IsConcluded { get;private set; } = false;

        public void Conclude()
        {
            IsConcluded = true;
        }

        public void Unconclude()
        {
            IsConcluded = false;
        }
    }
}
