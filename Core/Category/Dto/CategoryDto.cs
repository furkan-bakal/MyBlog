namespace Core.Category.Dto
{
    public record CategoryDto
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = default!;
        public string CreatedDate { get; init; } = default!;
        public string? UpdatedDate { get; init; }
        public bool IsDeleted { get; init; }
        public string? DeletedDate { get; init; }
    }
}
