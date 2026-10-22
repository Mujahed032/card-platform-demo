namespace CardPlatformDemo.Models;

/// <summary>
/// Which backend platform powers a card product.
/// Banks rarely build card systems from scratch — they integrate with
/// a Card Management System (eMcREY) and a rule engine (FIS Cortex).
/// </summary>
public enum Platform
{
    eMcREY,
    Cortex
}

/// <summary>A card product shown in the catalog.</summary>
public class CardProduct
{
    public string Id { get; set; } = "";
    public string Name { get; set; } = "";
    public string ShortDescription { get; set; } = "";
    public string LongDescription { get; set; } = "";
    public Platform Platform { get; set; }
    public string Category { get; set; } = "";       // e.g. "Credit", "Debit", "Prepaid"
    public string Accent { get; set; } = "#1f7a8c";   // card color
    public string Icon { get; set; } = "💳";
    public List<string> Features { get; set; } = new();
}

/// <summary>One step in a card lifecycle (issue, activate, authorize, maintain...).</summary>
public class LifecycleStep
{
    public string Key { get; set; } = "";
    public string Title { get; set; } = "";
    public string Description { get; set; } = "";
    public string GenuineOutcome { get; set; } = "";   // what happens for a legitimate user
    public string FraudOutcome { get; set; } = "";      // what happens if fraud is detected
    public string Icon { get; set; } = "✔";
}

/// <summary>A single rule in the Cortex rule engine demo.</summary>
public class EngineRule
{
    public string Situation { get; set; } = "";
    public string RuleText { get; set; } = "";
    public string Result { get; set; } = "";
}

/// <summary>Input the user provides to the rule-engine simulator.</summary>
public class TransactionInput
{
    public string CardProductId { get; set; } = "cortex-credit";
    public decimal Amount { get; set; } = 250;
    public decimal DailyLimit { get; set; } = 1000;
    public decimal AmountSpentToday { get; set; } = 0;
    public bool CardActive { get; set; } = true;
    public bool CardBlocked { get; set; } = false;
    public bool SufficientFunds { get; set; } = true;
    public bool SuspiciousPattern { get; set; } = false;
    public bool Contactless { get; set; } = false;
    public decimal ContactlessCeiling { get; set; } = 300;
}

/// <summary>The decision the engine returns, with the full trace.</summary>
public class EngineDecision
{
    public string Outcome { get; set; } = "";          // APPROVE / DECLINE / CHALLENGE
    public string OutcomeColor { get; set; } = "#2e9e5b";
    public string FiredRule { get; set; } = "";
    public decimal? FeeApplied { get; set; }
    public List<TraceLine> Trace { get; set; } = new();
}

public class TraceLine
{
    public string Rule { get; set; } = "";
    public string Evaluation { get; set; } = "";
    public bool Passed { get; set; }
    public bool Fired { get; set; }
}
