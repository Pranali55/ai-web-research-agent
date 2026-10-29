from fastapi import FastAPI
from routers import research

app = FastAPI(title="AI Web Research Agent")
app.include_router(research.router)

@app.get("/health")
def health():
    return {"status": "ok", "service": "ai-web-research-agent"}
