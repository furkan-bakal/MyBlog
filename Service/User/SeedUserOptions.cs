namespace Service.User
{
    // Seed admin bilgileri kaynak koda gömülmez; ortam değişkeni (SeedUser__UserName gibi)
    // veya user-secrets üzerinden gelir.
    public record SeedUserOptions
    {
        public const string SectionName = "SeedUser";

        public string UserName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Password { get; set; } = default!;
    }
}
