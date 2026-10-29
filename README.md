# 🔍 AI Web Research Agent

An autonomous AI agent that researches any topic by **searching the web, reading pages, analyzing content, and generating a comprehensive report** — built with **.NET 8**, **Python FastAPI**, and **OpenAI GPT-4o-mini**.

## ✨ Features

- 🤖 **Autonomous AI Agent** — works independently step by step
- 🔍 **Web Search** — uses Serper API (Google) or DuckDuckGo
- 📄 **Page Reader** — visits and extracts content from real web pages
- 🧠 **AI Analysis** — cross-references multiple sources with GPT-4o-mini
- 📝 **Research Report** — generates summary + 5 key findings
- 📊 **Step Tracking** — see every step the agent takes (plan → search → read → analyze → summarize)
- 💾 **Session History** — all research saved and retrievable
- ⚡ **Works without API keys** — smart fallback mode built in
- 🐳 **Fully Dockerized**

## 🏗️ Tech Stack

| Layer | Technology |
|-------|-----------|
| Backend API | .NET 8 (ASP.NET Core) |
| AI Agent | Python 3.11 + FastAPI + OpenAI |
| Web Search | Serper API / DuckDuckGo |
| Page Reading | httpx (async HTTP client) |
| Database | SQL Server 2022 |
| Cache | Redis 7 |
| AI Model | GPT-4o-mini |
| Auth | JWT Bearer Tokens |
| Container | Docker + Docker Compose |

## 🚀 Getting Started

1. Clone the repo
2. Create `.env` file:
```env
OPENAI_API_KEY=sk-your-openai-key-here
SERPER_API_KEY=your-serper-key-here  # Optional — get free at serper.dev
```
> ⚠️ Both keys are optional! The agent uses DuckDuckGo + AI fallback automatically.

3. Run:
```bash
docker-compose up --build
```

4. Swagger UI → [http://localhost:5000/swagger](http://localhost:5000/swagger)
5. Agent Docs → [http://localhost:8000/docs](http://localhost:8000/docs)

## 🔬 How the Agent Works

```
User sends topic: "Impact of AI on healthcare"
           ↓
Step 1 → PLAN    : Creates research strategy
Step 2 → SEARCH  : Searches web for top 5 sources
Step 3 → READ    : Visits and reads each webpage
Step 4 → ANALYZE : Cross-references all sources with AI
Step 5 → SUMMARIZE: Generates final report + 5 key findings
           ↓
Returns: Summary + Key Findings + Sources + Step Log
```

## 📡 API Endpoints

### Auth
| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `/api/auth/register` | Register |
| POST | `/api/auth/login` | Login |

### Research
| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `/api/research` | Start new research |
| GET | `/api/research` | My research history |
| GET | `/api/research/{id}` | Get research report |
| DELETE | `/api/research/{id}` | Delete session |

### Example Request
```json
POST /api/research
{
  "topic": "Impact of AI on healthcare in 2024",
  "maxSources": 5
}
```

### Example Response
```json
{
  "topic": "Impact of AI on healthcare in 2024",
  "status": "Completed",
  "summary": "AI is revolutionizing healthcare through...",
  "keyFindings": [
    "AI diagnostics now match expert radiologists in accuracy",
    "Drug discovery timelines reduced by 40% using AI",
    ...
  ],
  "sources": [...],
  "steps": [
    {"stepNumber": 1, "type": "plan", "status": "Completed"},
    {"stepNumber": 2, "type": "search", "status": "Completed"},
    ...
  ]
}
```

## 📁 Project Structure

```
ai-web-research-agent/
├── backend-dotnet/           # .NET 8 API
│   ├── Controllers/          # Auth, Research
│   ├── Services/             # AuthService, ResearchService
│   ├── Models/
│   ├── DTOs/
│   ├── Data/
│   └── Dockerfile
├── python-agent/             # AI Agent (FastAPI + OpenAI)
│   ├── routers/
│   ├── models/
│   ├── services/             # ResearchAgentService (5-step pipeline)
│   └── Dockerfile
└── docker-compose.yml
```

## 👨‍💻 Author

Built as a freelance portfolio project showcasing autonomous AI agent development.
