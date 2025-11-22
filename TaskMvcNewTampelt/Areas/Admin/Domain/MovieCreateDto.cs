namespace TaskMvcNewTampelt.Areas.Admin.Domain
{
    public class MovieCreateDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool Status { get; set; } = true;
        public DateTime ReleaseDate { get; set; } = DateTime.Today;
        public int CategoryId { get; set; }
        public IFormFile? MainImage { get; set; }
        public List<IFormFile>? SubImages { get; set; }
        public int[]? ActorIds { get; set; }
    }
}
