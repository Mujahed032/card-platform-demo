# CardPlatform Demo

An educational **ASP.NET Core MVC (.NET 8)** web app that visually explains how banks
power their card products by integrating with:

- **eMcREY** — a Card Management System (issue, activate, authorize, maintain, renew, bill)
- **FIS Cortex** — a rule engine (real-time authorization, fraud, fees/commission, installments)

No database — all content is seeded in-memory, so it runs anywhere with zero setup.

## Features

- **Home** — hero banner explaining why banks integrate rather than build from scratch, plus a catalog of eMcREY and Cortex card products.
- **Card details** — a lifecycle walkthrough (Issue → Activate → Authorize → Maintain) with a **Genuine customer vs Fraud attempt** toggle showing the outcome at each step.
- **Rule Engine simulator** — set a transaction scenario (amount, limits, card status, suspicious flag, contactless) and see the decision (APPROVE / DECLINE / CHALLENGE), applied fee, and a full rule-by-rule trace.

## Run locally

Requires the .NET 8 SDK.

```bash
dotnet run
```

Then open the URL shown in the console (defaults to http://localhost:8080).

In **Visual Studio**: open `CardPlatformDemo.csproj` and press **F5**.

## Deploy on Render (free)

This repo includes a `Dockerfile`. On Render:

1. **New → Web Service** (not Static Site).
2. Connect this GitHub repo.
3. Render auto-detects the Dockerfile and builds the image.
4. The app binds to the `PORT` env var Render provides.

> Note: a **Web Service** is required (not a Static Site) because this is a running .NET application, not pre-built static files.

## Tech

ASP.NET Core MVC, Razor views, Bootstrap 5, C# 12 / .NET 8. Content is in
`Services/CardCatalog.cs` and is easy to edit.
