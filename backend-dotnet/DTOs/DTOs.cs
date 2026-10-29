namespace ResearchAgent.DTOs;

// Auth
public record RegisterDto(string Name, string Email, string Password);
public record LoginDto(string Email, string Password);
public record AuthResponseDto(string Token, string Name, string Email);

// Research
public record StartResearchDto(string Topic, int? MaxSources);

public record ResearchStepDto(
    int Id, int StepNumber, string Type,
    string Description, string? Result, string Status, DateTime CreatedAt);

public record ResearchSessionDto(
    int Id, string Topic, string Status,
    string? Summary, List<string>? KeyFindings,
    List<SourceDto>? Sources, int SourcesCount,
    int StepsCompleted, DateTime CreatedAt,
    DateTime? CompletedAt, List<ResearchStepDto> Steps);

public record ResearchSessionSummaryDto(
    int Id, string Topic, string Status,
    int SourcesCount, int StepsCompleted,
    DateTime CreatedAt, DateTime? CompletedAt);

public record SourceDto(string Title, string Url, string Snippet);

// Agent Request/Response
public record AgentResearchRequest(string Topic, int MaxSources, string SessionId);
public record AgentResearchResponse(
    string Summary, List<string> KeyFindings,
    List<SourceDto> Sources, List<AgentStepResult> Steps);
public record AgentStepResult(int StepNumber, string Type, string Description, string Result, string Status);

// Pagination
public record PagedResult<T>(List<T> Data, int TotalCount, int Page, int PageSize, int TotalPages);
