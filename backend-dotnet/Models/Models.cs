namespace ResearchAgent.Models;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public ICollection<ResearchSession> Sessions { get; set; } = new List<ResearchSession>();
}

public class ResearchSession
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Topic { get; set; } = string.Empty;
    public string Status { get; set; } = "Pending"; // Pending, Running, Completed, Failed
    public string? Summary { get; set; }
    public string? KeyFindings { get; set; } // JSON array
    public string? Sources { get; set; }     // JSON array
    public int SourcesCount { get; set; } = 0;
    public int StepsCompleted { get; set; } = 0;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? CompletedAt { get; set; }
    public User User { get; set; } = null!;
    public ICollection<ResearchStep> Steps { get; set; } = new List<ResearchStep>();
}

public class ResearchStep
{
    public int Id { get; set; }
    public int SessionId { get; set; }
    public int StepNumber { get; set; }
    public string Type { get; set; } = string.Empty; // search, read, analyze, summarize
    public string Description { get; set; } = string.Empty;
    public string? Result { get; set; }
    public string Status { get; set; } = "Pending"; // Pending, Running, Completed, Failed
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public ResearchSession Session { get; set; } = null!;
}
