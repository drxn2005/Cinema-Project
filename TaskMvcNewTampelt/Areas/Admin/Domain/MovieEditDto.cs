namespace TaskMvcNewTampelt.Areas.Admin.Domain
{
    public class MovieEditDto : MovieCreateDto
    {
        public int Id { get; set; }
        public string? ExistingMainImg { get; set; }
    }
}
