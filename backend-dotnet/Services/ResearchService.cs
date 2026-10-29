using System.Text;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using ResearchAgent.Data;
using ResearchAgent.DTOs;
using ResearchAgent.Models;

namespace ResearchAgent.Services;

public class ResearchService
{
    private readonly AppDbContext _db;
    private readonly HttpClient _http;
    private readonly IConfiguration _config;

    public ResearchService(AppDbContext db, HttpClient http, IConfiguration config)
    {
        _db = db; _http = http; _config = config;
    }

    public async Task<ResearchSessionDto> StartResearchAsync(int userId, StartResearchDto dto)
    {
        // Create session
        var session = new ResearchSession
        {
            UserId = userId,
            Topic = dto.Topic,
            Status = "Running"
        };
        _db.Sessions.Add(session);
        await _db.SaveChangesAsync();

        // Call Python AI agent
        try
        {
            var request = new AgentResearchRequest(dto.Topic, dto.MaxSources ?? 5, session.Id.ToString());
            var baseUrl = _config["AgentService__BaseUrl"];
            var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
            var response = await _http.PostAsync($"{baseUrl}/research", content);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<AgentResearchResponse>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;

            // Save steps
            foreach (var step in result.Steps)
            {
                _db.Steps.Add(new ResearchStep
                {
                    SessionId = session.Id,
                    StepNumber = step.StepNumber,
                    Type = step.Type,
                    Description = step.Description,
                    Result = step.Result,
                    Status = step.Status
                });
            }

            // Update session
            session.Status = "Completed";
            session.Summary = result.Summary;
            session.KeyFindings = JsonSerializer.Serialize(result.KeyFindings);
            session.Sources = JsonSerializer.Serialize(result.Sources);
            session.SourcesCount = result.Sources.Count;
            session.StepsCompleted = result.Steps.Count(s => s.Status == "Completed");
            session.CompletedAt = DateTime.UtcNow;
        }
        catch (Exception ex)
        {
            session.Status = "Failed";
            session.Summary = $"Research failed: {ex.Message}";
        }

        await _db.SaveChangesAsync();
        return await GetSessionAsync(userId, session.Id) ?? throw new Exception("Session not found");
    }

    public async Task<PagedResult<ResearchSessionSummaryDto>> GetAllAsync(int userId, int page, int pageSize)
    {
        var query = _db.Sessions.Where(s => s.UserId == userId).OrderByDescending(s => s.CreatedAt);
        var total = await query.CountAsync();
        var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

        return new PagedResult<ResearchSessionSummaryDto>(
            items.Select(s => new ResearchSessionSummaryDto(
                s.Id, s.Topic, s.Status, s.SourcesCount,
                s.StepsCompleted, s.CreatedAt, s.CompletedAt)).ToList(),
            total, page, pageSize, (int)Math.Ceiling((double)total / pageSize));
    }

    public async Task<ResearchSessionDto?> GetSessionAsync(int userId, int sessionId)
    {
        var session = await _db.Sessions
            .Include(s => s.Steps)
            .FirstOrDefaultAsync(s => s.Id == sessionId && s.UserId == userId);
        if (session == null) return null;
        return MapToDto(session);
    }

    public async Task<bool> DeleteAsync(int userId, int sessionId)
    {
        var session = await _db.Sessions.FirstOrDefaultAsync(s => s.Id == sessionId && s.UserId == userId);
        if (session == null) return false;
        _db.Sessions.Remove(session);
        await _db.SaveChangesAsync();
        return true;
    }

    private static ResearchSessionDto MapToDto(ResearchSession s)
    {
        var findings = s.KeyFindings != null
            ? JsonSerializer.Deserialize<List<string>>(s.KeyFindings) : null;
        var sources = s.Sources != null
            ? JsonSerializer.Deserialize<List<SourceDto>>(s.Sources,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) : null;
        var steps = s.Steps.OrderBy(st => st.StepNumber)
            .Select(st => new ResearchStepDto(st.Id, st.StepNumber, st.Type,
                st.Description, st.Result, st.Status, st.CreatedAt)).ToList();

        return new ResearchSessionDto(s.Id, s.Topic, s.Status, s.Summary,
            findings, sources, s.SourcesCount, s.StepsCompleted,
            s.CreatedAt, s.CompletedAt, steps);
    }
}
