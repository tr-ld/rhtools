# Copilot Instructions

## General Guidelines
- First general instruction
- Second general instruction

## Code Style
- Use specific formatting rules
- Follow naming conventions

## Project-Specific Rules
- When styling components, prefer using the project's global theme.css variables (the blush theme in wwwroot/theme.css) rather than adding local :root variables in component CSS files.
- Do not replace the user's empty-array literal '[]' with 'Array.Empty<string>()' when calling GetHoldings; preserve the original 'api.GetHoldings([])' usage.