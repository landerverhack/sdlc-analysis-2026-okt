# GameStore

**A teaching demo for the analysis and design phase of the SDLC, with an AI assistant.**

The code is a working e-commerce store for board games — a Clean Architecture .NET 10
API and an Angular client. It exists to be *changed*, not admired. The lesson is
everything that happens **before** the first line of a new feature is written: getting
real requirements out of a stakeholder, agreeing what the words mean, deciding what
genuinely needs a recorded decision, writing a spec, comparing interfaces, and cutting
the work into issues someone else can pick up.

The repository ships with the artifacts that make that possible — a glossary, seven
ADRs, six skills and a set of layer-scoped instruction files — so a session starts with
a codebase that already has opinions, the way a real one does.

---

## For trainers

### The arc

Six steps, each feeding the next. The skills are in
[.github/skills/](.github/skills/) and are invoked with `/` in chat.

| | Skill | What the room sees |
| --- | --- | --- |
| 1 | `interview` | The assistant refuses to start building and interrogates you instead, one question at a time, with options and a recommendation. You play the stakeholder. |
| 2 | `glossary-audit` | It finds words that mean two things, and words that mean the same thing, in code nobody has read carefully. |
| 3 | `adr` | **Usually skipped.** Worth demonstrating *once* on something trivial so the class sees it decline to write a record. |
| 4 | `spec` | Requirements grounded in the glossary, with an Open questions table it refuses to fill by guessing. |
| 5 | `mockups` | Three genuinely different HTML options, opened in the browser. **A human picks the winner** — the assistant will not. |
| 6 | `spec-to-issues` | Real GitHub issues, written as prompts, with acceptance criteria copied verbatim from the spec. |

Then implementation, one issue at a time, against the spec and the accepted mockup.

### One repo per delivery

Never teach twice out of the same repository — the second cohort will see the first
cohort's issues. This repo is a **template**: make a fresh one each time and you get a
clean history and an empty issue tracker.

```bash
gh repo create U2UTraining/gamestore-sdlc-<cohort> \
  --template U2UTraining/gamestore-sdlc-starter --public --clone
```

Afterwards `gh repo archive` it rather than deleting — read-only, free, and you keep
the artifacts from a session that went well. Students make their own copy with the
**Use this template** button.

The seven labels the `spec-to-issues` skill expects (`domain`, `application`,
`infrastructure`, `presentation`, `frontend`, `docs`, `blocked`) come with the template.

### Keep your answer sheet out of this folder

You will want a one-page product-owner brief with your answers to the interview
questions. **Keep it outside the repository, on paper or a second screen.** Anything
inside the workspace can be read by the assistant, and an assistant that has read the
answers will not ask the questions — the whole first station collapses, live, in front
of the room.

### The code is imperfect on purpose

The [glossary](docs/glossary.md) was written from the **domain layer** only, and says
so. The Application, Infrastructure and Presentation layers have never been checked
against it, and they contain genuine inconsistencies — the kind that accumulate in any
codebase where the API and the model drift apart.

**Run `/glossary-audit` yourself before you deliver.** They are not listed here on
purpose, because students read this file, but you should know what is coming.

Other things left honestly rough, any of which can become a discussion: there is no
authentication, no test project, no migrations (`EnsureCreated` only), and the API
returns `202` with no body from checkout, so the client cannot show an order number.

### Prerequisites

- .NET 10 SDK and Node.js
- `gh` authenticated (`gh auth login`) — the issues station needs it
- **Check Copilot licensing on the org** if you plan to show the server-side features,
  such as assigning an issue to the coding agent. They are org-scoped and will silently
  not appear on an unlicensed org. Verify on a throwaway repo, not on the day.
- Run both halves once before the session. The client showing "The GameStore API could
  not be reached" means the API is not running.

---

## Getting started

Both halves have to be running: the client talks to the API over HTTP.

### The API

```bash
dotnet build src/Backend/GameStore.slnx
dotnet run --project src/Backend/src/GameStore.Presentation
```

The API listens on `http://localhost:5038`. A SQLite database is created on first run
and, in Development only, seeded with four Publishers, twelve Games and one Customer
with id 1. Deleting `gamestore.db` resets it and it is seeded again on the next start.
See [src/Backend/README.md](src/Backend/README.md) for how a request flows through the layers.

### The Angular client

In a second terminal:

```bash
cd src/Frontend
npm install
npm start
```

The client runs on `http://localhost:4200` and expects the API on `http://localhost:5038`
— that address is a constant in `src/Frontend/src/app/core/api.config.ts`, together with
the hardcoded `CUSTOMER_ID`, because there is no authentication. `npm run build` produces
a production build in `src/Frontend/dist/`.

The API allows any origin in Development, so no proxy is needed. If the catalogue shows
"The GameStore API could not be reached", the API is not running.

## What is already set up

| | |
| --- | --- |
| [.github/copilot-instructions.md](.github/copilot-instructions.md) | How the coding agent should behave in this repository |
| [docs/glossary.md](docs/glossary.md) | The ubiquitous language — binding for all code and docs |
| [docs/adr/](docs/adr/) | Seven architecture decision records covering the existing design |
| [.github/skills/](.github/skills/) | Seven skills: `interview`, `adr`, `glossary-audit`, `spec`, `mockups`, `spec-to-issues`, and `frontend-design`, the style the client and every mockup are drawn in |
| [.github/instructions/](.github/instructions/) | Layer-scoped rules, applied automatically per file |

## The workflow

Full description in [docs/README.md](docs/README.md).

```
interview ──► glossary-audit ──► spec ──► mockups ──► spec-to-issues ──► code
    │               │             │          │              │
    │          glossary.md    specs/NNN-  design/       GitHub issues
    │                          slug/spec.md  *.html
    │
    └──► adr (rarely) ──► docs/adr/NNNN-*.md
```

The interview happens in the conversation and leaves no file behind; the spec is
written in the same session. From the spec onward everything is on disk, so by the time
an issue exists, everything needed to work on it can be linked to.

## Layout

```
src/Backend/            .NET solution
  src/GameStore.Domain/          entities, value objects, domain events, domain services
  src/GameStore.Application/     use cases, repository interfaces, event handlers
  src/GameStore.Infrastructure/  EF Core, repositories, messaging
  src/GameStore.Presentation/    minimal API endpoints
src/Frontend/           Angular client (four screens, Tailwind + a hand-drawn design system)
  src/app/core/                  API config, models, services holding state in signals
  src/app/shared/                the Money pipe
  src/app/features/              game-catalogue, game-detail, shopping-basket, checkout
docs/                   glossary, ADRs, specs, accepted mockups
```
