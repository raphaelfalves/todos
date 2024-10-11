namespace ToDosProject.Domain.Entities
{
    public class ToDo
    {
        private ToDo() { }
        public ToDo(int id, string title)
        {
            Id = id;
            Title = title;
        }

        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public bool IsConcluded { get; set; } = false;
        public string UserId { get; set; } = null!;
        public User? User { get; set; }

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
