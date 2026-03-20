class PipeResponse:
    def __init__(self, status_code: str, content: str):
        self.status_code = status_code
        self.content = content
    def __str__(self):
        return f"PipeResponse(status_code={self.status_code}, content='{self.content}')"