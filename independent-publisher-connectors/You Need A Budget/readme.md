# You Need A Budget
You Need A Budget (YNAB) allows you to build a personal application to interact with your own budget.

## Publisher: Troy Taylor, Hitachi Solutions

## Prerequisites
You will need to sign up for an account with You Need A Budget. There is a free trial for the first month.

## Obtaining Credentials
From your Account Settings page, find Developer Settings and create a new personal access token.

## Supported Operations
### Get user
Returns authenticated user information.
### Get budgets
Returns budgets list with summary information.
### Get budget by ID
Returns a single budget with all related entities.  This resource is effectively a full budget export.
### Get budget settings by ID
Returns settings for a budget
### Get accounts
Returns all accounts.
### Create account
Creates a new account.
### Get account by ID
Returns a single account.
### Get categories
Returns all categories grouped by category group.  Amounts (budgeted, activity, balance, etc.) are specific to the current budget month (UTC).
### Get Category by ID
Returns a single category.  Amounts (budgeted, activity, balance, etc.) are specific to the current budget month (UTC).
### Get month category by ID
Returns a single category for a specific budget month.  Amounts (budgeted, activity, balance, etc.) are specific to the current budget month (UTC).
### Update month category
Update a category for a specific month.  Only `budgeted` amount can be updated.
### Get payees
Returns all payees.
### Get payee by ID
Returns a single payee.
### Get payee locations
Returns all payee locations.
### Get payee location by ID
Returns a single payee location.
### Get payee locations by payee
Returns all payee locations for a specified payee.
### Get budget months
Returns all budget months.
### Get budget month
Returns a single budget month.
### Get transactions
Returns budget transactions.
### Create transaction
Creates a single transaction or multiple transactions.  If you provide a body containing a `transaction` object, a single transaction will be created and if you provide a body containing a `transactions` array, multiple transactions will be created.  Scheduled transactions cannot be created on this endpoint.
### Update transactions
Updates multiple transactions, by `id` or `import_id`.
### Import transactions
Imports available transactions on all linked accounts for the given budget.  Linked accounts allow transactions to be imported directly from a specified financial institution and this endpoint initiates that import.  Sending a request to this endpoint is the equivalent of clicking "Import" on each account in the web application or tapping the "New Transactions" banner in the mobile applications.  The response for this endpoint contains the transaction ids that have been imported.
### Get transaction by ID
Returns a single transaction.
### Update transaction
Updates a single transaction.
### Get transactions by account
Returns all transactions for a specified account.
### Get transactions by category
Returns all transactions for a specified category.
### Get transactions by payee
Returns all transactions for a specified payee.
### Get scheduled transactions
Returns all scheduled transactions.
### Get scheduled transaction by ID
Returns a single scheduled transaction.
### Bulk create transactions
Creates multiple transactions.

## Known Issues and Limitations
There are no known issues at this time.
