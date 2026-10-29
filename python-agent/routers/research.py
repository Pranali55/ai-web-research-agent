from fastapi import APIRouter
from models.schemas import ResearchRequest, ResearchResponse
from services.agent import ResearchAgentService

router = APIRouter()
agent = ResearchAgentService()

@router.post("/research", response_model=ResearchResponse)
async def run_research(request: ResearchRequest):
    return await agent.research(request)
