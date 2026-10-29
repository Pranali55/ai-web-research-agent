from pydantic import BaseModel
from typing import List, Optional

class ResearchRequest(BaseModel):
    topic: str
    max_sources: int = 5
    session_id: str = ""

class Source(BaseModel):
    title: str
    url: str
    snippet: str

class StepResult(BaseModel):
    step_number: int
    type: str
    description: str
    result: str
    status: str

class ResearchResponse(BaseModel):
    summary: str
    key_findings: List[str]
    sources: List[Source]
    steps: List[StepResult]
