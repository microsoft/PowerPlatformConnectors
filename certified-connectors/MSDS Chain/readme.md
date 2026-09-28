# MSDS Chain — Chemical Safety Intelligence

MSDS Chain provides AI-powered chemical safety intelligence for laboratory researchers, EHS officers, and compliance teams. It delivers real-time safety data from a database of 4.3M+ chemicals, 72K+ Safety Data Sheets (SDS), and 23 regulatory lists across 11 regions.

## Pre-requisites

- An API key from [msdschain.lagentbot.com](https://msdschain.lagentbot.com) (free tier: 100 calls/month)
- No additional software installation required

## How to get credentials

1. Visit [msdschain.lagentbot.com](https://msdschain.lagentbot.com)
2. Sign up with Google, Microsoft, or Apple account
3. Navigate to your account settings to generate an API key
4. The API key format is `sk-msds-xxxx`

## Supported Operations

### Safety Assessment

| Tool | Description |
|------|-------------|
| **Check Chemical Compatibility** | Pairwise compatibility analysis for a list of chemicals using NFPA classification rules |
| **Get Chemical Risk Warnings** | GHS classification, H-codes, signal words, and flash points |
| **Batch Safety Check** | Comprehensive one-shot report combining compatibility, PPE, and storage for multiple chemicals |
| **Ask Chemical Safety** | Natural language Q&A for any chemical safety question |

### Regulatory Compliance

| Tool | Description |
|------|-------------|
| **Check Regulatory Compliance** | Multi-region regulatory status check (EU REACH/CLP, US OSHA/TSCA, CN, JP, KR, CA, AU, TW, SG) |
| **Check Regulatory Lists** | Scan against 23 regulatory databases (EPA, REACH, SVHC, Prop 65, OSHA PEL, etc.) |
| **Get Exposure Limits** | Occupational exposure limits: OEL, TLV, PEL, MAC across OSHA/ACGIH/EU/JP/CN |

### Handling & Storage

| Tool | Description |
|------|-------------|
| **Get PPE Recommendation** | Personal protective equipment guidance: gloves, eye, respiratory, body protection |
| **Get Storage Guidance** | Storage class, cabinet type, temperature requirements, isolation rules |
| **Check Mixing Order** | Safe addition order for chemical pairs with context-specific guidance |
| **Get Transport Classification** | UN number, hazard class, packing group for IATA/IMDG/ADR |
| **Get Waste Disposal** | Waste classification and disposal procedures |

### Emergency & Health

| Tool | Description |
|------|-------------|
| **Get Emergency Response** | Spill, fire, and exposure response procedures |
| **Get First Aid Guidance** | Route-specific first aid measures (inhalation, skin, eye, ingestion) |

### Data & Research

| Tool | Description |
|------|-------------|
| **Search Chemical Database** | Name, CAS number, or synonym lookup across 4.3M+ chemicals |
| **Get SDS Section** | Retrieve specific SDS sections 1-16 for any chemical |
| **Compare SDS Versions** | 7-dimension structured comparison between SDS versions |
| **Upload MSDS PDF** | Parse and extract structured data from SDS/MSDS PDF files |
| **Get Chemical Alternatives** | Safer substitute recommendations for a given use case |
| **Validate Protocol Chemicals** | Extract and validate chemicals mentioned in lab protocols or code |

### Audit & Reporting

| Tool | Description |
|------|-------------|
| **Create Audit Session** | Start a signed audit session with Ed25519 receipt for traceability |
| **Get Audit Report** | Download signed PDF audit report for a session |

## Authentication

This connector supports API key authentication. Include your API key in the `X-API-Key` header or as a Bearer token in the `Authorization` header.

## Known Issues and Limitations

- **Rate limits:** Free tier is limited to 100 API calls per month. Paid plans available for higher volume.
- **Language support:** Responses available in English, Chinese, Japanese, German, and Indonesian. Default is English.
- **Data coverage:** While the database covers 4.3M+ chemicals, detailed SDS data is available for ~72K chemicals. For chemicals without SDS data, the system uses AI inference from known properties.
- **Regional compliance:** 23 regulatory lists across 11 regions are indexed. Some newer or country-specific regulations may have partial coverage.
- **PDF parsing:** Upload supports PDF format only. Scanned/image-only PDFs may have reduced extraction accuracy.

## Support

- **Website:** [msdschain.lagentbot.com](https://msdschain.lagentbot.com)
- **Email:** contact@lagentbot.com
- **GitHub:** [github.com/littleblakew/msds-chain-mcp](https://github.com/littleblakew/msds-chain-mcp)
- **Privacy Policy:** [msdschain.lagentbot.com/privacy](https://msdschain.lagentbot.com/privacy)

## Publisher

LAgentBot Pte. Ltd. — Singapore
