This refined scenario—having a small, fixed set of SQL scripts (5-6) like `ADDCase`, `DeleteCase`—actually simplifies the architecture. We can remove **Azure AI Search** because we don't need to index thousands of documents.

We can rely purely on **Azure Logic Apps** and **Azure OpenAI** to select the right script and parameterize it.

### The Architecture: "The SQL Surgeon"

Since you are modifying SQL scripts on the fly, this architecture prioritizes **Context (Intent)** and **Safety (Parameterization)**.



#### Core Workflow

1.  **Trigger:** Azure DevOps (New Ticket).
2.  **Phase 1: Intent Classification (Azure OpenAI):**
    * Determine *which* of the 5-6 scripts is required (e.g., `ADDCase` vs `DeleteCase`).
3.  **Phase 2: Fetch Template (ADO Connector):**
    * Retrieve the raw SQL text from the specific Wiki page identified in Phase 1.
4.  **Phase 3: Parameter Extraction & Validation (Azure OpenAI):**
    * Compare the **Ticket Description** against the **SQL Template**.
    * Extract values and identify missing gaps.
5.  **Phase 4: Execution or Feedback:**
    * **If Valid:** Inject parameters -> Execute in SQL -> Close Ticket.
    * **If Invalid:** Comment on Ticket asking for specific missing info.

---

### Step-by-Step Implementation

#### 1. The Trigger (Azure Logic App)
* **Connector:** **Azure DevOps** -> `When a work item is created`.
* **Filter:** Filter by specific tags (e.g., `tag = 'SQL-Automation'`) to avoid triggering on irrelevant bugs/tasks.

#### 2. Phase 1: Intent Classification (The Router)
Instead of searching a database, we just ask the AI to pick from your known list.

* **Action:** Azure OpenAI -> `Chat Completions`.
* **System Prompt:**
    > "You are a DevOps Router. You have the following available SQL scripts:
    > 1. ADDCase (Creates a new case)
    > 2. DeleteCase (Removes a case)
    > 3. UpdateCase (Modifies existing details)
    >
    > Analyze the user's request. Return ONLY the exact name of the script needed. If the request is unclear, return 'UNKNOWN'."
* **User Input:** (The ADO Ticket Description).

#### 3. Phase 2: Get the Script (The Fetcher)
Now that we know the script name (e.g., "ADDCase"), we fetch it.

* **Action:** **HTTP Request** (Azure DevOps API) or **ADO Connector** (`Get Wiki Page`).
* **Logic:** Construct the URL dynamically based on the output from Phase 1.
    * *Example:* `https://dev.azure.com/{org}/{project}/_apis/wiki/wikis/{wikiId}/pages?path=/SQL_Scripts/ADDCase&includeContent=true`
* **Output:** This gives you the raw SQL template from the Wiki (e.g., `INSERT INTO Cases (ID, Name) VALUES (@id, @name)`).

#### 4. Phase 3: Extraction & "Safe" Update (The Processor)
This is the most critical step. We pass the **Raw SQL** and the **User Ticket** to GPT-4o.

* **Action:** Azure OpenAI -> `Chat Completions`.
* **System Prompt:**
    > "You are a SQL Parameter Extractor.
    > 1. Look at the provided SQL Template. Identify all variables/placeholders (like @CaseID, @CustomerName).
    > 2. Look at the User Ticket. Extract the values for these variables.
    > 3. **Critical:** If a variable is missing in the ticket, add it to a 'missing_fields' list.
    > 4. **Security:** Do not modify the SQL logic. Only map values."
    >
    > **Output JSON:**
    > `{ "status": "ready" | "incomplete", "parameters": { "@CaseID": "123", "@Name": "Acme Corp" }, "missing_fields": [] }`

#### 5. Phase 4: The Branch (Logic App Condition)

**Path A: Status == "incomplete"**
* **Action:** ADO Connector -> `Add a comment`.
* **Message:** "Automation paused. The script `ADDCase` requires the following missing details: @{body('Parse_JSON')?['missing_fields']}. Please update the ticket."

**Path B: Status == "ready"**
* **Action:** **Execute SQL Query (V2)**.
    * *Architectural Note:* Do **NOT** ask the AI to rewrite the full SQL string and execute that (high risk of SQL injection).
    * *The Smart Way:* Use the **Execute SQL Query** action. Pass the raw SQL script from the Wiki as the query, and use the **Output Parameters** from the JSON to fill the Logic App SQL connector's parameter fields dynamically.
* **Action:** ADO Connector -> `Update Work Item` (Set State to Done/Resolved).

---

### Critical Refinement: Handling the "Update SQL" Requirement

You mentioned: *"update the sql scripts accordingly with the correct parameter"*

If your Wiki scripts are written like this:
`SELECT * FROM Table WHERE ID = 'REPLACE_ME'`

The AI can return the full modified string, but this is risky.
**Better Design:** Update your 5-6 Wiki scripts to use standard SQL variables:
`DECLARE @CaseID varchar(50) = '{CaseID}';`

Then, use a simple **Replace String** expression in Logic Apps (not AI) to swap `{CaseID}` with the value the AI extracted. This ensures the AI provides the *data*, but the Logic App controls the *code*.

### Summary of Azure Services Used
1.  **Azure Logic Apps:** The glue holding it all together.
2.  **Azure OpenAI:** To understand the human text and map it to SQL requirements.
3.  **Azure SQL Database:** The destination.
4.  **Azure Key Vault:** (Optional but recommended) To store the SQL connection strings if not using Managed Identity.

### Why this is better for your "5-6 scripts" scenario
* **Cheaper:** No Azure AI Search costs.
* **Faster:** Logic App routing is instant; no indexing lag.
* **Safer:** By separating the "Intent" (Phase 1) from "Execution" (Phase 4), you ensure `DeleteCase` is never run accidentally when the user just wanted to `UpdateCase`.

**Would you like me to write the specific System Prompt for "Phase 3" that handles the parameter extraction from your SQL templates?**
