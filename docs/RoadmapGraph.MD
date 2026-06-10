```
┌─────────────────────────────────────────────┐
│ Phase 0                                     │
│ Foundation                                  │
├─────────────────────────────────────────────┤
│ • Git Workflow                              │
│ • Feature Branch Workflow                   │
│ • Clean Architecture Setup                  │
│ • .NET Setup                                │
│ • EF Core Setup                             │
│ • SQL Server Integration                    │
│ • Migration Flow                            │
│ • Base Entities                             │
│ • Audit Fields                              │
└──────────────────┬──────────────────────────┘
                   │
                   ▼

┌─────────────────────────────────────────────┐
│ Phase 1                                     │
│ Core Finance Application                    │
├─────────────────────────────────────────────┤
│ Module 1 - Category Management              │
│ • Domain                                    │
│ • Persistence                               │
│ • API                                       │
│ • Client                                    │
│ • UI                                        │
│ • Soft Delete                               │
│                                             │
│ Module 2 - Account Management               │
│ • Domain                                    │
│ • Persistence                               │
│ • API                                       │
│ • Client                                    │
│ • UI                                        │
│ • Soft Delete                               │
│                                             │
│ Module 3 - Transaction Management           │
│ • Domain                                    │
│ • Account Relation                          │
│ • Category Relation                         │
│ • API                                       │
│ • Client                                    │
│ • UI                                        │
│ • Soft Delete                               │
│                                             │
│ Module 4 - Dashboard V1                     │
│ • Summary Cards                             │
│ • Recent Transactions                       │
│ • Expense By Category                       │
│ • Account Summary                           │
│                                             │
│ Release: v1.0.0                             │
└──────────────────┬──────────────────────────┘
                   │
                   ▼

┌─────────────────────────────────────────────┐
│ Phase 2                                     │
│ Enterprise Foundation                       │
├─────────────────────────────────────────────┤
│ Module 5 - Interface                        │
│ • IAccountService                           │
│ • ITransactionService                       │
│ • ICategoryService                          │
│ • ISummaryService                           │
│                                             │
│ Module 6 - Service Layer                    │
│ • Business Logic Separation                 │
│ • Service Pattern                           │
│                                             │
│ Module 7 - Logging                          │
│ • ILogger                                   │
│ • Structured Logging                        │
│                                             │
│ Module 8 - Middleware                       │
│ • Request Pipeline                          │
│ • Global Exception Handling                 │
│                                             │
│ Module 9 - Response Standardization         │
│ • ApiResponse<T>                            │
│ • Standard Error Response                   │
│                                             │
│ Module 10 - Global Query Filter             │
│ • HasQueryFilter()                          │
│ • IgnoreQueryFilters()                      │
│                                             │
│ Release: v1.2.0                             │
└──────────────────┬──────────────────────────┘
                   │
                   ▼

┌─────────────────────────────────────────────┐
│ Phase 3                                     │
│ Data Access & Application Patterns          │
├─────────────────────────────────────────────┤
│ Module 11 - Repository Pattern              │
│ • Repository Abstraction                    │
│                                             │
│ Module 12 - Unit Of Work                    │
│ • Transaction Management                    │
│ • Consistency                               │
│                                             │
│ Module 13 - Validation                      │
│ • FluentValidation                          │
│ • Validation Rules                          │
│                                             │
│ Module 14 - AutoMapper                      │
│ • Entity Mapping                            │
│ • DTO Mapping                               │
│                                             │
│ Module 15 - Testing                         │
│ • Unit Testing                              │
│ • Integration Testing                       │
│ • Validation Testing                        │
│                                             │
│ Release: v1.5.0                             │
└──────────────────┬──────────────────────────┘
                   │
                   ▼

┌─────────────────────────────────────────────┐
│ Phase 4                                     │
│ CQRS Architecture                           │
├─────────────────────────────────────────────┤
│ Module 16 - CQRS                            │
│ • Commands                                  │
│ • Queries                                   │
│ • Handlers                                  │
│                                             │
│ Module 17 - MediatR                         │
│ • Request                                   │
│ • Handler                                   │
│ • Notification                              │
│ • Pipeline Behavior                         │
│                                             │
│ Release: v1.8.0                             │
└──────────────────┬──────────────────────────┘
                   │
                   ▼

┌─────────────────────────────────────────────┐
│ Phase 5                                     │
│ AI Integration                              │
├─────────────────────────────────────────────┤
│ Module 18 - AI Service Abstraction          │
│ • IAIService                                │
│ • OpenAI Provider                           │
│ • Ollama Provider                           │
│                                             │
│ Module 19 - AI Playground                   │
│ • Prompt Playground                         │
│ • Chat Interface                            │
│ • Provider Switching                        │
│                                             │
│ Module 20 - Finance Assistant V1            │
│ • Ask Finance Questions                     │
│ • Conversation History                      │
│ • AI Responses                              │
│                                             │
│ Module 21 - Context Injection               │
│ • Read Finance Data                         │
│ • Build Context Payload                     │
│ • Personalized Responses                    │
│                                             │
│ Module 22 - Finance Insights Engine         │
│ • Spending Analysis                         │
│ • Budget Suggestions                        │
│ • Monthly Reflection                        │
│                                             │
│ Release: v2.0.0                             │
└──────────────────┬──────────────────────────┘
                   │
                   ▼

┌─────────────────────────────────────────────┐
│ Phase 6                                     │
│ Agentic AI                                  │
├─────────────────────────────────────────────┤
│ Module 23 - Tool Calling                    │
│ • QueryTransactionsTool                     │
│ • QueryAccountsTool                         │
│ • QueryCategoriesTool                       │
│ • QueryDashboardTool                        │
│                                             │
│ Module 24 - Agent Workflow                  │
│ • Planning                                  │
│ • Tool Selection                            │
│ • Tool Execution                            │
│ • Response Synthesis                        │
│                                             │
│ Module 25 - Finance Agent                   │
│ • Natural Language Analysis                 │
│ • Financial Reasoning                       │
│                                             │
│ Module 26 - Multi Step Analysis             │
│ • Cross Month Comparison                    │
│ • Trend Analysis                            │
│ • Insight Generation                        │
│                                             │
│ Release: v2.5.0                             │
└──────────────────┬──────────────────────────┘
                   │
                   ▼

┌─────────────────────────────────────────────┐
│ Phase 7                                     │
│ DevOps & Deployment                         │
├─────────────────────────────────────────────┤
│ Module 27 - Docker                          │
│ • Dockerfile                                │
│ • Multi Stage Build                         │
│                                             │
│ Module 28 - Docker Compose                  │
│ • App                                       │
│ • SQL Server                                │
│                                             │
│ Module 29 - CI/CD                           │
│ • Build Pipeline                            │
│ • Test Pipeline                             │
│ • Deployment Pipeline                       │
│                                             │
│ Module 30 - Monitoring                      │
│ • Logging Dashboard                         │
│ • Health Check                              │
│                                             │
│ Module 31 - Secrets Management              │
│ • Environment Variables                     │
│ • Secret Storage                            │
│                                             │
│ Release: v3.0.0                             │
└─────────────────────────────────────────────┘
```