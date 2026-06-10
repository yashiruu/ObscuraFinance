# AI Roadmap

## Purpose

This document describes the long-term AI evolution plan for Obscura Finance Tracker.

The goal is not to add AI as a gimmick.

The goal is to understand how AI systems are designed, integrated, grounded, and eventually transformed into agentic systems.

---

# AI Evolution

```text id="c8a8t4"
Finance Tracker
    ↓
AI Playground
    ↓
Finance Assistant
    ↓
Context-Aware Assistant
    ↓
Finance Insights Engine
    ↓
Tool Calling
    ↓
Finance Agent
    ↓
Multi-Step Agent
```

---

# Phase 5 — AI Integration

## Goal

Learn practical LLM integration inside a real application.

---

## Module 18 — AI Service Abstraction

### Purpose

Decouple application logic from AI providers.

Architecture:

```text id="q3a7jk"
Finance Application
        ↓
     IAIService
        ↓
 ┌───────────────┐
 │ OpenAI        │
 │ Ollama        │
 │ Future Model  │
 └───────────────┘
```

Topics:

* Interface Abstraction
* Provider Pattern
* Dependency Injection
* Vendor Independence

---

## Module 19 — AI Playground

### Purpose

Experiment safely before integrating AI into business features.

Features:

* Prompt Playground
* Chat Interface
* Provider Selection
* Prompt History

Topics:

* Prompt Engineering
* Context Windows
* Token Usage
* Temperature
* System Prompts

---

## Module 20 — Finance Assistant V1

### Purpose

Allow users to ask questions about finance data.

Example Questions:

```text id="kwf2o6"
How much did I spend this month?

What are my biggest expenses?

Summarize my spending habits.
```

Architecture:

```text id="jl6a8d"
User Question
      ↓
Prompt
      ↓
LLM
      ↓
Response
```

Topics:

* Structured Prompting
* Output Formatting
* AI UX Design

---

## Module 21 — Context Injection

### Purpose

Provide application data to the model.

Architecture:

```text id="z9uk8g"
User Question
      ↓
Finance Data
      ↓
Context Builder
      ↓
Prompt
      ↓
LLM
      ↓
Response
```

Topics:

* Grounding
* Context Engineering
* Prompt Construction

---

## Module 22 — Finance Insights Engine

### Purpose

Generate meaningful financial insights.

Features:

* Spending Analysis
* Category Trends
* Monthly Reflection
* Budget Suggestions

Architecture:

```text id="0f8tbv"
Transactions
      ↓
Analysis Layer
      ↓
Prompt Template
      ↓
LLM
      ↓
Insight
```

Topics:

* AI Assisted Analytics
* Prompt Templates
* Explainability

---

# Release Milestone

Version:

```text id="b4u6fo"
v2.0.0
```

Deliverable:

```text id="b5r6wp"
AI Powered Finance Tracker
```

---

# Future Exploration

Potential future topics:

* Embeddings
* Semantic Search
* Vector Databases
* Retrieval Augmented Generation (RAG)
* Local Models
* Hybrid AI Architectures

These topics are intentionally postponed until the foundation of the application is mature.
