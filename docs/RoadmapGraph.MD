```
Learning Evolution

Build
    ↓
Organize
    ↓
Stabilize
    ↓
Scale
    ↓
Intelligence
    ↓
Autonomy
    ↓
Operate


┌─────────────────────────────────────────────────────────────┐
│ Phase 0                                                     │
│ Foundation                                                   │
├─────────────────────────────────────────────────────────────┤
│ Theme                                                       │
│ Prepare                                                     │
│                                                             │
│ • Git Workflow                                              │
│ • Feature Branch Workflow                                   │
│ • Clean Architecture Setup                                  │
│ • .NET Setup                                                │
│ • EF Core Setup                                             │
│ • SQL Server Integration                                    │
│ • Migration Flow                                            │
│ • Base Entities                                             │
│ • Audit Fields                                              │
└──────────────────────┬──────────────────────────────────────┘
                       │
                       ▼

┌─────────────────────────────────────────────────────────────┐
│ Phase 1                                                     │
│ Core Finance Application                                    │
│ Release: v1.0.0                                             │
├─────────────────────────────────────────────────────────────┤
│ Theme                                                       │
│ Build                                                       │
│                                                             │
│ Module 1 — Category Management                              │
│ • Domain                                                    │
│ • Persistence                                               │
│ • API                                                       │
│ • Client                                                    │
│ • UI                                                        │
│ • Soft Delete                                               │
│                                                             │
│ Module 2 — Account Management                               │
│ • Domain                                                    │
│ • Persistence                                               │
│ • API                                                       │
│ • Client                                                    │
│ • UI                                                        │
│ • Soft Delete                                               │
│                                                             │
│ Module 3 — Transaction Management                           │
│ • Domain                                                    │
│ • Account Relation                                          │
│ • Category Relation                                         │
│ • API                                                       │
│ • Client                                                    │
│ • UI                                                        │
│ • Soft Delete                                               │
│                                                             │
│ Module 4 — Dashboard V1                                     │
│ • Summary Cards                                             │
│ • Recent Transactions                                       │
│ • Expense By Category                                       │
│ • Account Summary                                           │
└──────────────────────┬──────────────────────────────────────┘
                       │
                       ▼

┌─────────────────────────────────────────────────────────────┐
│ Phase 2                                                     │
│ Enterprise Foundation                                       │
│ Release: v1.2.0                                             │
├─────────────────────────────────────────────────────────────┤
│ Theme                                                       │
│ Organize                                                    │
│                                                             │
│ Module 5 — Interface                                        │
│ • IAccountService                                           │
│ • ITransactionService                                       │
│ • ICategoryService                                          │
│ • IDashboardService                                         │
│                                                             │
│ Module 6 — Service Layer                                    │
│ • Business Logic Separation                                 │
│ • Service Pattern                                           │
│                                                             │
│ Module 7 — Logging                                          │
│ • ILogger                                                   │
│ • Structured Logging                                        │
│                                                             │
│ Module 8 — Middleware                                       │
│ • Request Pipeline                                          │
│ • Global Exception Handling                                 │
│                                                             │
│ Module 9 — Response Standardization                         │
│ • ApiResponse<T>                                            │
│ • Standard Error Response                                   │
│                                                             │
│ Module 10 — Global Query Filter                             │
│ • HasQueryFilter()                                          │
│ • IgnoreQueryFilters()                                      │
└──────────────────────┬──────────────────────────────────────┘
                       │
                       ▼

┌─────────────────────────────────────────────────────────────┐
│ Phase 3                                                     │
│ Data Access & Application Patterns                          │
│ Release: v1.3.0                                             │
├─────────────────────────────────────────────────────────────┤
│ Theme                                                       │
│ Stabilize                                                   │
│                                                             │
│ Module 11 — Repository Pattern                              │
│ • Repository Abstraction                                    │
│ • Generic Repository                                        │
│                                                             │
│ Module 12 — Unit Of Work                                    │
│ • Transaction Coordination                                  │
│ • SaveChanges Management                                    │
│                                                             │
│ Module 13 — Validation                                      │
│ • FluentValidation                                          │
│ • Validation Pipeline                                       │
│                                                             │
│ Module 14 — AutoMapper                                      │
│ • DTO Mapping                                               │
│ • Entity Mapping                                            │
│ • Projection                                                │
│                                                             │
│ Module 15 — Testing                                         │
| • Unit Testing                                              |
| • Testing Infrastructure                                    |
| • Service Testing                                           |
| • Validator Testing                                         │
└──────────────────────┬──────────────────────────────────────┘
                       │
                       ▼

┌─────────────────────────────────────────────────────────────┐
│ Phase 4                                                     │
│ CQRS Architecture                                           │
│ Release: v1.4.0                                             │
├─────────────────────────────────────────────────────────────┤
│ Theme                                                       │
│ Scale                                                       │
│                                                             │
│ Module 16 — Pagination                                      │
│ • PagedRequest / PagedResult<T>                              │
│ • Skip/Take Query Pattern                                   │
│                                                             │
│ Module 17 — Authentication                                  │
│ • ASP.NET Core Identity                                     │
│ • JWT Issuance & Validation                                 │
│                                                             │
│ Module 18 — CQRS                                            │
│ • Commands                                                  │
│ • Queries                                                   │
│ • Handlers                                                  │
│                                                             │
│ Module 19 — MediatR + Authorization                         │
│ • Request                                                   │
│ • Handler                                                   │
│ • Pipeline Behavior                                         │
│ • Policy-Based Authorization                                │
│                                                             │
│ Module 20 — Caching                                         │
│ • Cache-Aside Pattern                                       │
│ • Caching Pipeline Behavior                                 │
│ • Cache Invalidation                                        │
└──────────────────────┬──────────────────────────────────────┘
                       │
                       ▼

┌─────────────────────────────────────────────────────────────┐
│ Phase 5                                                     │
│ AI Integration                                              │
│ Release: v1.5.0                                             │
├─────────────────────────────────────────────────────────────┤
│ Theme                                                       │
│ Intelligence                                                │
│                                                             │
│ Module 21 — AI Service Abstraction                          │
│ • IAIService                                                │
│ • OpenAI Provider                                           │
│ • Ollama Provider                                           │
│                                                             │
│ Module 22 — AI Playground                                   │
│ • Prompt Playground                                         │
│ • Chat Interface                                            │
│ • Provider Switching                                        │
│                                                             │
│ Module 23 — Finance Assistant                               │
│ • Ask Finance Questions                                     │
│ • Conversation History                                      │
│ • AI Responses                                              │
│                                                             │
│ Module 24 — Context Engineering                             │
│ • Read Finance Data                                         │
│ • Build Context                                             │
│ • Prompt Construction                                       │
│                                                             │
│ Module 25 — Finance Insights Engine                         │
│ • Spending Analysis                                         │
│ • Budget Suggestions                                        │
│ • Monthly Reflection                                        │
└──────────────────────┬──────────────────────────────────────┘
                       │
                       ▼

┌─────────────────────────────────────────────────────────────┐
│ Phase 6                                                     │
│ Agentic Finance Platform                                    │
│ Release: v2.0.0                                             │
├─────────────────────────────────────────────────────────────┤
│ Theme                                                       │
│ Autonomy                                                    │
│                                                             │
│ Module 26 — Tool Calling                                    │
│ • QueryTransactionsTool                                     │
│ • QueryAccountsTool                                         │
│ • QueryCategoriesTool                                       │
│ • QueryDashboardTool                                        │
│                                                             │
│ Module 27 — Agent Workflow                                  │
│ • Planning                                                  │
│ • Tool Selection                                            │
│ • Tool Execution                                            │
│ • Response Synthesis                                        │
│                                                             │
│ Module 28 — Finance Agent                                   │
│ • Natural Language Analysis                                 │
│ • Financial Reasoning                                       │
│                                                             │
│ Module 29 — Multi-Step Analysis                             │
│ • Cross-Month Comparison                                    │
│ • Trend Analysis                                            │
│ • Insight Generation                                        │
└──────────────────────┬──────────────────────────────────────┘
                       │
                       ▼

┌─────────────────────────────────────────────────────────────┐
│ Phase 7                                                     │
│ Production Platform                                         │
│ Release: v3.0.0                                             │
├─────────────────────────────────────────────────────────────┤
│ Theme                                                       │
│ Operate                                                     │
│                                                             │
│ Module 30 — Docker                                          │
│ • Dockerfile                                                │
│ • Multi-Stage Build                                         │
│                                                             │
│ Module 31 — Docker Compose                                  │
│ • Application                                               │
│ • SQL Server                                                │
│                                                             │
│ Module 32 — CI/CD                                           │
│ • Build Pipeline                                            │
│ • Test Pipeline                                             │
│ • Deployment Pipeline                                       │
│                                                             │
│ Module 33 — Monitoring                                      │
│ • Logging Dashboard                                         │
│ • Health Check                                              │
│                                                             │
│ Module 34 — Secrets Management                              │
│ • Environment Variables                                     │
│ • Secret Storage                                            │
└─────────────────────────────────────────────────────────────┘
```