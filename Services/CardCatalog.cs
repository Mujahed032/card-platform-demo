using CardPlatformDemo.Models;

namespace CardPlatformDemo.Services;

/// <summary>
/// In-memory catalog + rule engine. No database — all content is seeded here
/// so the app runs anywhere (local + Render) with zero setup.
/// </summary>
public class CardCatalog
{
    public List<CardProduct> Products { get; } = new()
    {
        // ---------------- eMcREY (Card Management System) ----------------
        new CardProduct
        {
            Id = "emcrey-cms",
            Name = "CMS Card (Card Management System)",
            Platform = Platform.eMcREY,
            Category = "Core",
            Accent = "#1f7a8c",
            Icon = "🗂️",
            ShortDescription = "The core engine that runs the full life of a card: issue, activate, authorize, maintain, renew and bill.",
            LongDescription = "eMcREY's CMS is the back-office platform banks integrate instead of building card infrastructure from scratch. It owns the entire card record (PAN, expiry, status) and exposes APIs to issue, activate, block, renew and bill cards.",
            Features = new() { "Card issuance", "Activation", "Authorization", "Maintenance (block/unblock)", "Renewal & reissue", "Billing & statements" }
        },
        new CardProduct
        {
            Id = "emcrey-apex-creditplus",
            Name = "Apex CreditPlus",
            Platform = Platform.eMcREY,
            Category = "Credit",
            Accent = "#7a4ebd",
            Icon = "💠",
            ShortDescription = "A premium credit card product managed through eMcREY with credit line, billing cycles and rewards.",
            LongDescription = "Apex CreditPlus is a credit product running on the eMcREY CMS. It demonstrates credit-line management, monthly billing, and the full issue→activate→authorize→maintain lifecycle with genuine-vs-fraud branching at each step.",
            Features = new() { "Credit line setup", "Monthly billing cycle", "Rewards accrual", "Online & virtual issuance", "Auto-renewal" }
        },
        new CardProduct
        {
            Id = "emcrey-prepaid",
            Name = "eMcREY Prepaid Card",
            Platform = Platform.eMcREY,
            Category = "Prepaid",
            Accent = "#e0892b",
            Icon = "🎁",
            ShortDescription = "A load-first prepaid card — spend only the pre-loaded balance. No bank account required.",
            LongDescription = "The prepaid product shows how eMcREY issues a card tied to a loaded balance rather than an account. Great for gifting, allowances, and spend control. Walk through load, activate, authorize and reload.",
            Features = new() { "Pre-load balance", "Instant virtual issue", "Reload", "Spend limits", "No credit check" }
        },
        new CardProduct
        {
            Id = "emcrey-debit",
            Name = "eMcREY Debit Card",
            Platform = Platform.eMcREY,
            Category = "Debit",
            Accent = "#2e9e5b",
            Icon = "🏦",
            ShortDescription = "A debit card linked directly to the customer's deposit account, managed through eMcREY.",
            LongDescription = "The debit product spends directly from the customer's account. It demonstrates account linkage, real-time balance checks at authorization, and the standard card lifecycle.",
            Features = new() { "Account linkage", "Real-time balance check", "ATM/POS usage", "Instant issuance", "Card controls" }
        },

        // ---------------- FIS Cortex (Rule Engine) ----------------
        new CardProduct
        {
            Id = "cortex-credit",
            Name = "Cortex Credit (Rule-Engine Card)",
            Platform = Platform.Cortex,
            Category = "Credit",
            Accent = "#c0392b",
            Icon = "⚙️",
            ShortDescription = "A credit card whose every transaction is decided in real time by the FIS Cortex rule engine.",
            LongDescription = "FIS Cortex externalises card decisions as editable rules. Every payment is evaluated against active rules — card status, funds, daily limits, risk patterns, fees — and the engine returns approve, decline, challenge or applies a fee. Change a rule and it applies instantly, no code deployment.",
            Features = new() { "Real-time authorization", "Configurable limits", "Fraud/risk rules", "Fee & commission rules", "Contactless ceiling control" }
        },
        new CardProduct
        {
            Id = "cortex-fraud",
            Name = "Cortex Fraud & Risk",
            Platform = Platform.Cortex,
            Category = "Risk",
            Accent = "#8e44ad",
            Icon = "🛡️",
            ShortDescription = "The risk layer: detects suspicious patterns and applies a configured action — decline, challenge or block.",
            LongDescription = "Cortex Fraud & Risk shows how the engine reacts to suspicious activity. A transaction matching a risk pattern triggers a configured action: step-up verification (OTP), decline, or card block. This is the genuine-vs-fraud branch made visible.",
            Features = new() { "Pattern detection", "Step-up (OTP) challenge", "Auto-decline", "Card block action", "Velocity checks" }
        },
        new CardProduct
        {
            Id = "cortex-fees",
            Name = "Cortex Fees & Commission",
            Platform = Platform.Cortex,
            Category = "Fees",
            Accent = "#16a085",
            Icon = "💰",
            ShortDescription = "Calculates product-specific fees and commission on each transaction, driven by rules.",
            LongDescription = "The fees engine shows how Cortex applies the right fee based on the card product and transaction type. Rules decide the percentage or flat fee, commission splits, and when fees are waived.",
            Features = new() { "Product-based fees", "Commission split", "Fee waivers", "FX fees", "Transparent breakdown" }
        },
        new CardProduct
        {
            Id = "cortex-installment",
            Name = "Cortex Installment Plan",
            Platform = Platform.Cortex,
            Category = "Installment",
            Accent = "#2980b9",
            Icon = "📅",
            ShortDescription = "Splits an eligible purchase into scheduled installments and manages collection.",
            LongDescription = "The installment engine converts an eligible transaction into a payment plan. Cortex rules decide eligibility, tenure, and profit/markup, then schedule and collect each installment automatically.",
            Features = new() { "Eligibility rules", "Tenure selection", "Scheduled collection", "Profit/markup calc", "Missed-payment handling" }
        },
    };

    public CardProduct? GetProduct(string id) => Products.FirstOrDefault(p => p.Id == id);

    public IEnumerable<CardProduct> ByPlatform(Platform p) => Products.Where(x => x.Platform == p);

    // ---------------- Lifecycle (eMcREY CMS flow) ----------------
    public List<LifecycleStep> GetLifecycle(string productId)
    {
        // Prepaid has a slightly different flow (load instead of credit line)
        bool prepaid = productId == "emcrey-prepaid";
        return new List<LifecycleStep>
        {
            new LifecycleStep {
                Key = "issue", Title = "1. Issue", Icon = "🆕",
                Description = "The bank requests a new card from the CMS. A card record is created (PAN, expiry, CVV) and linked to the customer. For virtual cards the details are available instantly.",
                GenuineOutcome = "KYC checks pass → CMS creates the card record and returns a masked card reference. A virtual card can be used immediately; plastic is queued for production.",
                FraudOutcome = "Identity/KYC check fails or the applicant is on a watchlist → issuance is rejected and the attempt is logged for review. No card is created."
            },
            new LifecycleStep {
                Key = "activate", Title = "2. Activate", Icon = "✅",
                Description = prepaid
                    ? "The customer loads a balance and confirms receipt. The card moves from issued to active and usable."
                    : "The customer confirms receipt (app / IVR / branch). The card moves from issued to active and becomes usable.",
                GenuineOutcome = "Correct customer confirms via a verified channel → status flips to ACTIVE. The card is now usable for transactions.",
                FraudOutcome = "Activation attempted from an unverified device or with failed OTP → activation is blocked, customer is alerted, and repeated attempts can lock the card."
            },
            new LifecycleStep {
                Key = "authorize", Title = "3. Authorize", Icon = "⚡",
                Description = "Every transaction is authorized in real time. The CMS works with the rule engine to approve or decline based on status, funds/limit and risk.",
                GenuineOutcome = "Card active + funds/limit sufficient + no risk flag → transaction APPROVED and funds are held, then later settled.",
                FraudOutcome = "Suspicious pattern or over-limit / blocked card → transaction DECLINED or a step-up (OTP) challenge is raised before approval."
            },
            new LifecycleStep {
                Key = "maintain", Title = "4. Maintain", Icon = "🔧",
                Description = "Ongoing servicing: block/unblock, change limits, replace lost/stolen cards, PIN reset, and renew before expiry.",
                GenuineOutcome = "Verified customer requests a change (e.g. freeze card) → CMS updates status instantly and every future authorization reflects it.",
                FraudOutcome = "Unverified maintenance request (e.g. attacker tries to raise limits) → change is refused, customer is notified, and the session is flagged."
            },
        };
    }

    // ---------------- Rule engine reference table ----------------
    public List<EngineRule> GetRuleTable() => new()
    {
        new EngineRule { Situation = "Card payment",    RuleText = "Card is active and funds are sufficient",        Result = "Approve, subject to other checks" },
        new EngineRule { Situation = "Blocked card",    RuleText = "Card status is blocked",                         Result = "Decline" },
        new EngineRule { Situation = "Daily spending",  RuleText = "Transaction exceeds the permitted daily limit",  Result = "Decline or apply the configured control" },
        new EngineRule { Situation = "Transaction risk",RuleText = "Transaction matches a suspicious pattern",       Result = "Apply the configured risk action (challenge / block)" },
        new EngineRule { Situation = "Card fee",        RuleText = "Customer's card belongs to a particular product",Result = "Calculate the applicable fee" },
    };

    // ---------------- Rule engine evaluation (the simulator) ----------------
    public EngineDecision Evaluate(TransactionInput t)
    {
        var d = new EngineDecision();
        var product = GetProduct(t.CardProductId);

        // Rule 1: blocked card
        bool blocked = t.CardBlocked;
        d.Trace.Add(new TraceLine { Rule = "Blocked card", Evaluation = blocked ? "Card status = BLOCKED" : "Card not blocked", Passed = !blocked, Fired = blocked });
        if (blocked)
        {
            d.Outcome = "DECLINE"; d.OutcomeColor = "#c0392b"; d.FiredRule = "Blocked card → Decline";
            return d;
        }

        // Rule 2: card active
        d.Trace.Add(new TraceLine { Rule = "Card active", Evaluation = t.CardActive ? "Card is ACTIVE" : "Card is INACTIVE", Passed = t.CardActive, Fired = !t.CardActive });
        if (!t.CardActive)
        {
            d.Outcome = "DECLINE"; d.OutcomeColor = "#c0392b"; d.FiredRule = "Card not active → Decline";
            return d;
        }

        // Rule 3: suspicious pattern (risk)
        d.Trace.Add(new TraceLine { Rule = "Transaction risk", Evaluation = t.SuspiciousPattern ? "Matches a suspicious pattern" : "No risk pattern matched", Passed = !t.SuspiciousPattern, Fired = t.SuspiciousPattern });
        if (t.SuspiciousPattern)
        {
            d.Outcome = "CHALLENGE"; d.OutcomeColor = "#e0892b"; d.FiredRule = "Risk pattern → Step-up (OTP) challenge";
            return d;
        }

        // Rule 4: contactless ceiling
        if (t.Contactless && t.Amount > t.ContactlessCeiling)
        {
            d.Trace.Add(new TraceLine { Rule = "Contactless ceiling", Evaluation = $"Contactless {t.Amount:0} > ceiling {t.ContactlessCeiling:0}", Passed = false, Fired = true });
            d.Outcome = "CHALLENGE"; d.OutcomeColor = "#e0892b"; d.FiredRule = "Over contactless ceiling → PIN required";
            return d;
        }
        d.Trace.Add(new TraceLine { Rule = "Contactless ceiling", Evaluation = t.Contactless ? $"Contactless {t.Amount:0} ≤ ceiling {t.ContactlessCeiling:0}" : "Not a contactless txn", Passed = true, Fired = false });

        // Rule 5: daily limit
        bool overLimit = (t.AmountSpentToday + t.Amount) > t.DailyLimit;
        d.Trace.Add(new TraceLine { Rule = "Daily spending limit", Evaluation = $"Spent {t.AmountSpentToday:0} + {t.Amount:0} vs limit {t.DailyLimit:0}", Passed = !overLimit, Fired = overLimit });
        if (overLimit)
        {
            d.Outcome = "DECLINE"; d.OutcomeColor = "#c0392b"; d.FiredRule = "Over daily limit → Decline";
            return d;
        }

        // Rule 6: sufficient funds
        d.Trace.Add(new TraceLine { Rule = "Sufficient funds", Evaluation = t.SufficientFunds ? "Funds / credit available" : "Insufficient funds", Passed = t.SufficientFunds, Fired = !t.SufficientFunds });
        if (!t.SufficientFunds)
        {
            d.Outcome = "DECLINE"; d.OutcomeColor = "#c0392b"; d.FiredRule = "Insufficient funds → Decline";
            return d;
        }

        // Rule 7: product fee (applies on approve)
        decimal feeRate = product?.Category switch
        {
            "Credit" => 0.015m,
            "Prepaid" => 0.010m,
            _ => 0.005m
        };
        d.FeeApplied = Math.Round(t.Amount * feeRate, 2);
        d.Trace.Add(new TraceLine { Rule = "Card fee", Evaluation = $"Product fee {feeRate:P1} on {t.Amount:0} = {d.FeeApplied:0.00}", Passed = true, Fired = true });

        d.Outcome = "APPROVE"; d.OutcomeColor = "#2e9e5b"; d.FiredRule = "All checks passed → Approve (fee applied)";
        return d;
    }
}
