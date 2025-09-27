# OpenFEC (Independent Publisher)

## Publisher: Dan Romano

## Prerequisites

- A valid **FEC API Key** (you can use `DEMO_KEY` for testing, but expect rate limits).  

## Obtaining Credentials

- Obtain an API key(https://api.open.fec.gov/developers/) from FEC.gov


## Supported Operations

Some of the important endpoint categories in this connector include:

|operationType | operationName | operationDescription |
|---|---|
|GET | `/calendar-dates/` | Get FEC calendar events |
|GET | `/calendar-dates/export/` | Download FEC Calendar Dates (CSV or ICS) |
|GET | `/candidate/{candidate_id}/committees/history/` | Committee History by Candidate |
|GET | `/candidate/{candidate_id}/committees/history/{cycle}/` | Committee History for a Specific Cycle |
|GET | `/candidate/{candidate_id}/committees/` | Get Committees for a Candidate |
|GET | `/candidate/{candidate_id}/history/` | Get Candidate Details (History) |
|GET | `/candidate/{candidate_id}/history/{cycle}/` | Get Candidate Details per Election Cycle |
|GET | `/candidate/{candidate_id}/` | Get Candidate Details (Current) |
|GET | `/candidate/{candidate_id}/filings/` | Get Candidate Filings |
|GET | `/candidate/{candidate_id}/totals/` | Get Candidate Totals by Cycle |
|GET | `/candidates/totals/by_office Get Agg. Receipts/Disbursements by Office and Cycle |
|GET | `/candidates/totals/by_office/by_party/` | Get Agg. Receipts/Disbursements by Office and Party per Election Cycle |
|GET | `/candidates/totals/` | Get Agg. Receipts and Disbursements by Cycle |
|GET | `/candidates/` | Get Candidates Basic Info |
|GET | `/candidates/search/` | Search Candidates |
|GET | `/committee/{committee_id}/candidates/history/` | Get Candidate History by Committee |
|GET | `/committee/{committee_id}/candidates/history/{cycle}/` | Get Candidate History by Election Cycle |
|GET | `/committee/{committee_id}/candidates/` | Get Candidate by Committee (Current) |
|GET | `/committee/{committee_id}/history/` | Get Candidate by Committee (History) |
|GET | `/committee/{committee_id}/history/{cycle}/` | Get Candidate Committee History by Cycle |
|GET | `/committee/{committee_id}/` | /committee/:committee_id/ |
|GET | `/committee/{committee_id}/communication_costs/by_candidate/` | Get Communication Costs by Committee by Candidate |
|GET | `/committee/{committee_id}/electioneering/by_candidate/` | Get Electioneering by Committee by Candidate |
|GET | `/committee/{committee_id}/filings/` | Get Filings for a Specific Committee |
|GET | `/committee/{committee_id}/reports/` | Get Financial Reports for a Committee |
|GET | `/committee/{committee_id}/totals/` | Get Committee Financial Totals |
|GET | `/communication_costs/aggregates/` | Get Communication Cost Aggregates |
|GET | `/communication_costs/by_candidate/` | Get Communication Costs by Candidate |
|GET | `/communication_costs/totals/by_candidate/` | Get Communication Cost Totals by Candidate |
|GET | `/efile/reports/house-senate/` | Get Electronic F3/F3X/F3P Summary Filings for House and Senate |
|GET | `/efile/reports/pac-party/` | Get Electronic F3X Summary Filings for PACs and Party Committees |
|GET | `/efile/reports/presidential/` | Get Presidential Committee Electronic Filings (F3P Reports) |
|GET | `/efile/filings/` | Retrieve eFiled Filings |
|GET | `/electioneering Get Electioneering Communications |
|GET | `/electioneering/aggregates/` | Get Electioneering Communications Cost Aggregates |
|GET | `/electioneering/by_candidate/` | Get Electioneering Costs Agg. by Candidate |
|GET | `/electioneering/totals/by_candidate/` | Get Total Electioneering Communications by Candidate and Cycle |
|GET | `/elections/` | Get Election-level Financial Summary for All Candidates in a Race |
|GET | `/elections/search/` | Search Elections by Location and Cycle |
|GET | `/elections/summary/` | Summary of Financials for a Specific Election |
|GET | `/names/audit_candidates/` | Search Candidates and Committees by Name |
|GET | `/names/audit_committees/` | Search Audited Committees by Name |
|GET | `/names/candidates/` | Search Candidates by Name |
|GET | `/names/committees/` | Search Committees by Name |
|GET | `/presidential/contributions/by_candidate/` | Get Presidential Contributions by Candidate |
|GET | `/presidential/contributions/by_size/` | Get Presidential Contributions by Size |
|GET | `/presidential/contributions/by_state/` | Get Presidential Contributions by State |
|GET | `/presidential/coverage_end_date/` | Get Presidential Coverage End Date |
|GET | `/presidential/financial_summary/` | Get Presidential Financial Summary by Candidate |
|GET | `/schedules/schedule_a/by_size/` | Get Schedule A Contributions by Size |
|GET | `/schedules/schedule_a/by_size/by_candidate/` | Get Schedule A Contributions by Size by Candidate |
|GET | `/schedules/schedule_a/by_state/by_candidate/` | Get Schedule A Contributions by State by Candidate |
|GET | `/schedules/schedule_a/by_state/by_candidate/totals/` | Get Schedule A Contributions Totals by Size by Candidate |
|GET | `/schedules/schedule_a/by_state/Get Schedule A Contributions by State |
|GET | `/schedules/schedule_a/by_state/totals/` | Get Schedule A Contribution Totals by State |
|GET | `/schedules/schedule_a/` | Get Schedule A Itemized Receipts |
|GET | `/schedules/schedule_a/by_employer/` | Get Aggregated Contributions by Employer |
|GET | `/schedules/schedule_a/by_occupation/` | Get Aggregated Contributions by Occupation |
|GET | `/schedules/schedule_a/by_zip/` | Get Aggregated Contributions by ZIP Code |
|GET | `/schedules/schedule_a/efile/` | Get eFiled Schedule A Contributions |
|GET | `/schedules/schedule_a/{sub_id}/` | Get a Single Schedule A Transaction by Sub ID |
|GET | `/schedules/schedule_b/` | Get Schedule B Disbursements |
|GET | `/schedules/schedule_b/by_purpose/Get Schedule B Disbursements By Purpose |
|GET | `/schedules/schedule_b/by_recipient/` |  Get Schedule B Disbursements By Recipient |
|GET | `/schedules/schedule_b/by_recipient_id/` | Get Schedule B Disbursements By Recipient Id |
|GET | `/schedules/schedule_b/efile/` | Get Schedule eFiled Disbursements |
|GET | `/schedules/schedule_b/{sub_id}/` | Get Schedule B By SubId |
|GET | `/schedules/schedule_c/` | Get Schedule C Loans |
|GET | `/schedules/schedule_c/{sub_id}/` | Get Schedule C By Sub Id |
|GET | `/schedules/schedule_d/` | Get Schedule D |
|GET | `/schedules/schedule_d/{sub_id}/` | Get Schedule D Records By Sub Id |
|GET | `/schedules/schedule_e/` | Get Schedule E |
|GET | `/schedules/schedule_e/by_candidate/` | Get Schedule E Totals By Candidate |
|GET | `/schedules/schedule_e/efile/` | Get Schedule E eFile |
|GET | `/schedules/schedule_e/totals/by_candidate/` | Get Schedule E Totals by Candidate |
|GET | `/schedules/schedule_f/` | Get Schedule F Party Expenditures |
|GET | `/schedules/schedule_f/{sub_id}/` | Get Schedule F By Id |
|GET | `/totals/by_entity/` | Get Cumulative Receipt Totals by Entity Type over a two-year cycle |
|GET | `/totals/{committee_type}/` | Get Totals by Committee Type |
|GET | `/audit-case/` | Get Audit Case Records |
|GET | `/audit-category/` | Get Audit Categories and Sub-Categories |
|GET | `/committees/` | Search for Committees |
|GET | `/communication_costs/` | Retrieve Communication Costs |
|GET | `/election-dates/` | Retrieve Election Dates |
|GET | `/filings/` | Get All FEC Filings |
|GET | `/operations-log/` | Get FEC Data Log |
|GET | `/rad-analyst/` | Retrieve RAD Analysts Assigned to Committees |
|GET | `/reporting-dates/` | Retrieve Audit Reporting Dates |
|GET | `/state-election-office/` | Retrieve State and Local Election Office Details |


## Known Limitations

- The “efile” endpoints are raw and may omit categories or standardized fields present in processed data.  
- Rate limiting could apply when using `DEMO_KEY` or during peak usage.  
- Some fields may be `null` or missing if not reported (e.g., occupation, employer, ZIP).

## Frequently Asked Questions

- **Default Values**: Many operations have defaults set for parameters like `cycle`, `page`, `per_page`, `sort`, etc., to mimic real FEC data flows.  
- **Pagination**:  
  - For large data sets, use *keyset pagination* rather than naive page numbers wherever indicated (e.g. `last_index`, `last_contribution_receipt_date`) to avoid missing or duplicating records.  
  - For “efile” endpoints: only the most recent ~4 months of data are available and data is raw/unprocessed.  
- **Nulls and Sorting**: There are parameters like `sort_hide_null`, `sort_nulls_last`, `sort_null_only` to manage how null values are treated in sorted results. Use them thoughtfully.  
- **Limits**: Be mindful of data volume—filters like `committee_id`, `cycle`, `state`, etc., can dramatically reduce the response size. Per‑request size is limited (e.g., via `per_page`).