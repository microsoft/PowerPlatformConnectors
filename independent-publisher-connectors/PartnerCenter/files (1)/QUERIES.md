# ISV Marketplace Analytics - Common Queries Reference

Quick reference for SQL-like queries you can use with the Partner Center Marketplace Insights connector.

## Orders & License Quantities

### All Active Orders with License Counts
```sql
SELECT 
    OfferName, 
    SKU, 
    Quantity, 
    OrderStatus, 
    OrderPurchaseDate,
    CustomerCompanyName, 
    BilledRevenue, 
    Currency
FROM ISVOrder 
WHERE OrderStatus = 'Active'
ORDER BY OrderPurchaseDate DESC
```

### License Count by Offer (Aggregated)
```sql
SELECT 
    OfferName, 
    SKU, 
    SUM(Quantity) as TotalLicenses,
    COUNT(*) as OrderCount
FROM ISVOrder 
WHERE OrderStatus = 'Active'
GROUP BY OfferName, SKU
```

### Trial vs Paid Licenses
```sql
SELECT 
    OfferName,
    IsTrial,
    SUM(Quantity) as Licenses,
    COUNT(*) as Orders
FROM ISVOrder
WHERE OrderStatus = 'Active'
GROUP BY OfferName, IsTrial
```

### Orders Expiring Soon (Next 30 Days)
```sql
SELECT 
    OfferName, 
    SKU, 
    Quantity, 
    CustomerCompanyName, 
    TermEndDate,
    AutoRenew
FROM ISVOrder 
WHERE OrderStatus = 'Active'
ORDER BY TermEndDate ASC
```

---

## Revenue Analytics

### Monthly Revenue Summary
```sql
SELECT 
    MonthStartDate,
    OfferName,
    SUM(BilledRevenue) as TotalRevenue,
    SUM(Quantity) as TotalLicenses,
    Currency
FROM ISVOrder
WHERE OrderStatus = 'Active'
GROUP BY MonthStartDate, OfferName, Currency
ORDER BY MonthStartDate DESC
```

### Revenue by Customer
```sql
SELECT 
    CustomerCompanyName,
    CustomerCountry,
    SUM(BilledRevenue) as TotalRevenue,
    SUM(Quantity) as TotalLicenses,
    COUNT(*) as OrderCount
FROM ISVOrder
WHERE OrderStatus = 'Active'
GROUP BY CustomerCompanyName, CustomerCountry
ORDER BY TotalRevenue DESC
```

### Private Offers Revenue
```sql
SELECT 
    OfferName,
    PrivateOfferName,
    CustomerCompanyName,
    Quantity,
    BilledRevenue
FROM ISVOrder
WHERE IsPrivateOffer = 'True' AND OrderStatus = 'Active'
```

---

## Customer Analytics

### New Customers
```sql
SELECT 
    CustomerCompanyName,
    CustomerCountry,
    OfferName,
    OrderPurchaseDate,
    Quantity
FROM ISVOrder
WHERE IsNewCustomer = 'True'
ORDER BY OrderPurchaseDate DESC
```

### Customer Distribution by Country
```sql
SELECT 
    CustomerCountry,
    COUNT(DISTINCT CustomerCompanyName) as CustomerCount,
    SUM(Quantity) as TotalLicenses
FROM ISVOrder
WHERE OrderStatus = 'Active'
GROUP BY CustomerCountry
ORDER BY CustomerCount DESC
```

### Top Customers by License Count
```sql
SELECT 
    CustomerCompanyName,
    CustomerCountry,
    SUM(Quantity) as TotalLicenses,
    SUM(BilledRevenue) as TotalRevenue,
    COUNT(*) as OrderCount
FROM ISVOrder
WHERE OrderStatus = 'Active'
GROUP BY CustomerCompanyName, CustomerCountry
ORDER BY TotalLicenses DESC
```

---

## Subscription Lifecycle

### Recently Purchased Orders (Last 30 Days)
```sql
SELECT 
    OfferName,
    SKU,
    Quantity,
    CustomerCompanyName,
    OrderPurchaseDate,
    BilledRevenue
FROM ISVOrder
WHERE OrderStatus = 'Active'
ORDER BY OrderPurchaseDate DESC
```

### Canceled Orders
```sql
SELECT 
    OfferName,
    SKU,
    Quantity,
    CustomerCompanyName,
    OrderCancelDate,
    BilledRevenue
FROM ISVOrder
WHERE OrderStatus = 'Canceled'
ORDER BY OrderCancelDate DESC
```

### Renewals and Churn Risk
```sql
SELECT 
    OfferName,
    CustomerCompanyName,
    Quantity,
    TermEndDate,
    AutoRenew,
    BilledRevenue
FROM ISVOrder
WHERE OrderStatus = 'Active' AND AutoRenew = 'False'
ORDER BY TermEndDate ASC
```

---

## Usage Data (ISVUsage Dataset)

### Normalized Usage by Offer
```sql
SELECT 
    OfferName,
    SKU,
    UsageDate,
    NormalizedUsage,
    RawUsage,
    MeterName
FROM ISVUsage
ORDER BY UsageDate DESC
```

### Metered Usage Charges
```sql
SELECT 
    OfferName,
    CustomerCompanyName,
    MeterName,
    UsageQuantity,
    EstimatedExtendedCharge
FROM ISVUsage
WHERE OverageUnits > 0
```

---

## Marketplace Engagement (ISVMarketplaceInsights)

### Page Views and CTAs
```sql
SELECT 
    OfferName,
    PageVisits,
    UniqueVisitors,
    CallToAction,
    ReferralDomain
FROM ISVMarketplaceInsights
ORDER BY PageVisits DESC
```

---

## Available Date Ranges

Use `TIMESPAN` clause to filter by date range:

```sql
SELECT * FROM ISVOrder 
TIMESPAN LAST_MONTH

SELECT * FROM ISVOrder 
TIMESPAN LAST_3_MONTHS

SELECT * FROM ISVOrder 
TIMESPAN LAST_6_MONTHS

SELECT * FROM ISVOrder 
TIMESPAN LAST_1_YEAR

SELECT * FROM ISVOrder 
TIMESPAN LIFETIME
```

---

## Full Column Reference - ISVOrder Dataset

| Column | Type | Description |
|--------|------|-------------|
| MarketplaceSubscriptionId | string | Unique subscription identifier |
| OrderId | string | Order identifier |
| MonthStartDate | date | Month of the data |
| OfferType | string | SaaS, VM, Container, etc. |
| OfferName | string | Your offer name |
| OfferId | string | Offer identifier |
| SKU | string | SKU/Plan name |
| PlanId | string | Plan identifier |
| Quantity | int | Number of licenses |
| OrderStatus | string | Active, Canceled |
| OrderAction | string | New, Renew, Cancel |
| OrderPurchaseDate | datetime | Purchase timestamp |
| OrderCancelDate | datetime | Cancellation timestamp |
| TermStartDate | datetime | Term start |
| TermEndDate | datetime | Term end |
| BillingCycle | string | Monthly, Annual |
| BillingTerm | string | Billing term |
| BilledRevenue | decimal | Revenue amount |
| Currency | string | USD, EUR, etc. |
| CustomerCompanyName | string | Customer name |
| CustomerId | string | Customer ID |
| CustomerCountry | string | Customer location |
| IsNewCustomer | boolean | First purchase |
| IsTrial | boolean | Trial subscription |
| HasTrial | boolean | Has trial period |
| TrialEndDate | datetime | Trial expiration |
| AutoRenew | boolean | Auto-renewal enabled |
| IsPrivateOffer | boolean | Private offer |
| PrivateOfferId | string | Private offer ID |
| PrivateOfferName | string | Private offer name |
| IsPrivatePlan | boolean | Private plan |
| ListPriceUSD | decimal | List price in USD |
| DiscountPriceUSD | decimal | Discounted price |

---

## Tips

1. **Test queries first** using the `Test Query` action before creating scheduled reports
2. **Use specific columns** rather than `SELECT *` for better performance
3. **Add filters** with `WHERE` to reduce data volume
4. **Group data** with `GROUP BY` for aggregated insights
5. **Minimum recurrence** for ISVOrder is typically 1 hour
6. **Report expiry** - download links expire, fetch them promptly
