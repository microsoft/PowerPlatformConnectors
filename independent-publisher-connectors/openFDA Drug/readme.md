# openFDA
openFDA is an Elasticsearch-based API that serves public FDA data about nouns like drugs, devices, and foods. Each of these nouns has one or more categories, which serve unique data-such as data about recall enforcement reports, or about adverse events. Every query to the API must go through one endpoint for one kind of data.

## Publisher: Woong Choi

## Supported Operations
### Drug Adverse Event
The openFDA drug adverse event API returns data that has been collected from the FDA Adverse Event Reporting System (FAERS), a database that contains information on adverse event and medication error reports submitted to FDA.

### Drug Labeling
Drug manufacturers and distributors submit documentation about their products to FDA in the Structured Product Labeling (SPL) format. The openFDA drug product labeling API returns data from this dataset.
 
### Drug NDC
The openFDA drug NDC Directory endpoint returns data from the NDC Directory, a database that contains information on the National Drug Code (NDC). FDA publishes the listed NDC numbers and the information submitted as part of the listing information in the NDC Directory which is updated daily.

### Drug Enforcement
The openFDA drug enforcement reports API returns data from the FDA Recall Enterprise System (RES), a database that contains information on recall event information submitted to FDA. Currently, this data covers publicly releasable records from 2004-present. The data is updated weekly.

### Drugs@FDA
Drugs@FDA includes most of the drug products approved since 1939. The majority of patient information, labels, approval letters, reviews, and other information are available for drug products approved since 1998.

## Getting Started
Visit [openFDA](https://open.fda.gov/apis/drug/) documentations page for further details.