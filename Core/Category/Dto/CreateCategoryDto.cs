namespace Core.Category.Dto
{
    public record CreateCategoryDto
    {
        public string Name { get; init; } = default!;
    }
}
