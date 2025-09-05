using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Text;
using System.Linq;
using System.Globalization;

public class Script : ScriptBase
{
    // Random instance for data generation
    private static readonly Random _random = new Random();
    
    // Shared ID pools for relationships (initialized per request with seed)
    private static Guid[] _sharedAccountIds;
    private static Guid[] _sharedContactIds;
    private static Guid[] _sharedProductIds;
    private static string[] _sharedAccountNames;
    private static string[] _sharedContactNames;
    private static string[] _sharedProductNames;
    
    // ================================================================================================
    // MAIN EXECUTION METHOD
    // ================================================================================================
    
    public override async Task<HttpResponseMessage> ExecuteAsync()
    {
        try
        {
            var operationId = DecodeOperationId(this.Context.OperationId);
            var query = ParseQueryString(this.Context.Request.RequestUri.Query);
            var count = GetIntParameter(query, "count", 10, 1, 1000);
            var seedValue = GetIntParameter(query, "seedValue", 12345, 1, 999999);
            
            // Initialize random with seed for consistent relationships
            var seededRandom = new Random(seedValue);
            
            // Generate shared ID pools for relationships (smart defaults based on count)
            var accountPoolSize = Math.Max(5, Math.Min(count / 3, 50)); // 3-1 ratio, max 50 accounts
            var contactPoolSize = Math.Max(10, Math.Min(count / 2, 100)); // 2-1 ratio, max 100 contacts  
            var productPoolSize = Math.Max(8, Math.Min(count / 4, 75)); // 4-1 ratio, max 75 products
            
            InitializeSharedIdPools(seededRandom, accountPoolSize, contactPoolSize, productPoolSize);

            object data;
            string entityName;

            switch (operationId.ToLower())
            {
                // Dataverse entities
                case "getdataverseaccounts":
                    data = GenerateDataverseAccounts(count, seededRandom);
                    entityName = "accounts";
                    break;
                case "getdataversecontacts":
                    data = GenerateDataverseContacts(count, seededRandom);
                    entityName = "contacts";
                    break;
                case "getdataverseleads":
                    data = GenerateDataverseLeads(count, seededRandom);
                    entityName = "leads";
                    break;
                case "getdataverseopportunities":
                    data = GenerateDataverseOpportunities(count, seededRandom);
                    entityName = "opportunities";
                    break;
                case "getdataversecases":
                    data = GenerateDataverseCases(count, seededRandom);
                    entityName = "cases";
                    break;
                case "getdataverseproducts":
                    data = GenerateDataverseProducts(count, seededRandom);
                    entityName = "products";
                    break;
                case "getdataversequotes":
                    data = GenerateDataverseQuotes(count, seededRandom);
                    entityName = "quotes";
                    break;
                case "getdataverseorders":
                    data = GenerateDataverseOrders(count, seededRandom);
                    entityName = "orders";
                    break;
                case "getdataversecampaigns":
                    data = GenerateDataverseCampaigns(count, seededRandom);
                    entityName = "campaigns";
                    break;
                case "getdataversecompetitors":
                    data = GenerateDataverseCompetitors(count, seededRandom);
                    entityName = "competitors";
                    break;
                case "getdataverseinvoices":
                    data = GenerateDataverseInvoices(count, seededRandom);
                    entityName = "invoices";
                    break;
                case "getdataverseactivities":
                    data = GenerateDataverseActivities(count, seededRandom);
                    entityName = "activities";
                    break;
                case "getdataverseteams":
                    data = GenerateDataverseTeams(count, seededRandom);
                    entityName = "teams";
                    break;
                case "getdataverseterritories":
                    data = GenerateDataverseTerritories(count, seededRandom);
                    entityName = "territories";
                    break;
                case "getdataverseknowledgearticles":
                    data = GenerateDataverseKnowledgeArticles(count, seededRandom);
                    entityName = "knowledgearticles";
                    break;
                case "getdataversecontracts":
                    data = GenerateDataverseContracts(count, seededRandom);
                    entityName = "contracts";
                    break;

                // Business Central entities
                case "getbccustomers":
                    data = GenerateBusinessCentralCustomers(count, seededRandom);
                    entityName = "customers";
                    break;
                case "getbcvendors":
                    data = GenerateBusinessCentralVendors(count, seededRandom);
                    entityName = "vendors";
                    break;
                case "getbcitems":
                    data = GenerateBusinessCentralItems(count, seededRandom);
                    entityName = "items";
                    break;
                case "getbcsalesorders":
                    data = GenerateBusinessCentralSalesOrders(count, seededRandom);
                    entityName = "salesOrders";
                    break;
                case "getbcpurchaseorders":
                    data = GenerateBusinessCentralPurchaseOrders(count, seededRandom);
                    entityName = "purchaseOrders";
                    break;
                    
                // Supply Chain entities
                case "getscwarehouses":
                    data = GenerateSupplyChainWarehouses(count, seededRandom);
                    entityName = "warehouses";
                    break;
                case "getscinventoryitems":
                    data = GenerateSupplyChainInventoryItems(count, seededRandom);
                    entityName = "inventoryItems";
                    break;
                case "getscpurchaserequisitions":
                    data = GenerateSupplyChainPurchaseRequisitions(count, seededRandom);
                    entityName = "purchaseRequisitions";
                    break;
                case "getscproductionorders":
                    data = GenerateSupplyChainProductionOrders(count, seededRandom);
                    entityName = "productionOrders";
                    break;
                case "getscbillofmaterials":
                    data = GenerateSupplyChainBillOfMaterials(count, seededRandom);
                    entityName = "billOfMaterials";
                    break;
                case "getscinventorytransactions":
                    data = GenerateSupplyChainInventoryTransactions(count, seededRandom);
                    entityName = "inventoryTransactions";
                    break;
                    
                // Field Service entities
                case "getfsworkorders":
                    data = GenerateFieldServiceWorkOrders(count, seededRandom);
                    entityName = "workOrders";
                    break;
                case "getfsresources":
                    data = GenerateFieldServiceResources(count, seededRandom);
                    entityName = "resources";
                    break;
                case "getfsequipment":
                    data = GenerateFieldServiceEquipment(count, seededRandom);
                    entityName = "equipment";
                    break;
                case "getfsserviceaccounts":
                    data = GenerateFieldServiceServiceAccounts(count, seededRandom);
                    entityName = "serviceAccounts";
                    break;
                case "getfsproducts":
                    data = GenerateFieldServiceProducts(count, seededRandom);
                    entityName = "products";
                    break;
                    
                default:
                    return CreateErrorResponse("Unknown operation: " + operationId);
            }

            var result = new { value = data };
            return CreateJsonResponse(result);
        }
        catch (Exception ex)
        {
            return CreateErrorResponse($"Error executing operation: {ex.Message}");
        }
    }
    
    // ================================================================================================
    // DATA ARRAYS
    // ================================================================================================
    
    // Names and Demographics
    private static readonly string[] FirstNames = {
        "James", "Mary", "Robert", "Patricia", "John", "Jennifer", "Michael", "Linda", "David", "Elizabeth",
        "William", "Barbara", "Richard", "Susan", "Joseph", "Jessica", "Thomas", "Sarah", "Christopher", "Karen",
        "Charles", "Lisa", "Daniel", "Nancy", "Matthew", "Betty", "Anthony", "Helen", "Mark", "Sandra",
        "Donald", "Donna", "Steven", "Carol", "Paul", "Ruth", "Andrew", "Sharon", "Joshua", "Michelle",
        "Kenneth", "Laura", "Kevin", "Sarah", "Brian", "Kimberly", "George", "Deborah", "Timothy", "Dorothy"
    };
    
    private static readonly string[] LastNames = {
        "Smith", "Johnson", "Williams", "Brown", "Jones", "Garcia", "Miller", "Davis", "Rodriguez", "Martinez",
        "Hernandez", "Lopez", "Gonzalez", "Wilson", "Anderson", "Thomas", "Taylor", "Moore", "Jackson", "Martin",
        "Lee", "Perez", "Thompson", "White", "Harris", "Sanchez", "Clark", "Ramirez", "Lewis", "Robinson",
        "Walker", "Young", "Allen", "King", "Wright", "Scott", "Torres", "Nguyen", "Hill", "Flores",
        "Green", "Adams", "Nelson", "Baker", "Hall", "Rivera", "Campbell", "Mitchell", "Carter", "Roberts"
    };
    
    private static readonly string[] JobTitles = {
        "Software Engineer", "Product Manager", "Sales Representative", "Marketing Specialist", "Data Analyst",
        "Project Manager", "Business Analyst", "Customer Service Representative", "Operations Manager", "HR Manager",
        "Financial Analyst", "Graphic Designer", "Content Writer", "DevOps Engineer", "Quality Assurance", "System Administrator",
        "Account Manager", "Business Development", "Research Analyst", "Technical Writer", "UX Designer", "Database Administrator",
        "Network Engineer", "Security Specialist", "Training Coordinator", "Executive Assistant", "Procurement Specialist",
        "Supply Chain Manager", "Legal Counsel", "Compliance Officer", "Risk Manager", "Business Intelligence Analyst"
    };
    
    private static readonly string[] CompanyNames = {
        "Acme Corporation", "Global Solutions Inc", "TechFlow Systems", "DataSync Technologies", "NextGen Enterprises",
        "Blue Ocean Ventures", "Strategic Innovations", "Digital Dynamics", "Advanced Systems", "Premier Solutions",
        "Integrated Technologies", "Synergy Solutions", "Excellence Corp", "Progressive Systems", "Dynamic Enterprises",
        "Innovative Solutions", "Peak Performance", "Summit Technologies", "Apex Systems", "Pinnacle Corp"
    };
    
    private static readonly string[] StreetNames = {
        "Main St", "First Ave", "Second St", "Third Ave", "Oak St", "Elm St", "Park Ave", "Broadway",
        "Washington St", "Lincoln Ave", "Madison St", "Jefferson Ave", "Franklin St", "Cedar St",
        "Maple Ave", "Pine St", "Sunset Blvd", "Rose Ave", "Spring St", "Valley Rd"
    };
    
    private static readonly string[] Industries = {
        "Technology", "Healthcare", "Finance", "Manufacturing", "Retail", "Education", "Real Estate",
        "Transportation", "Energy", "Telecommunications", "Agriculture", "Construction", "Entertainment",
        "Hospitality", "Insurance", "Consulting", "Media", "Automotive", "Aerospace", "Pharmaceuticals"
    };
    
    // Business Central specific arrays
    private static readonly string[] BCCustomerGroups = {
        "DOMESTIC", "FOREIGN", "INTERCOMPANY", "RETAIL", "WHOLESALE", "GOVERNMENT", "NONPROFIT", "EDUCATION"
    };
    
    private static readonly string[] BCPaymentTerms = {
        "NET30", "NET15", "NET10", "COD", "2%10NET30", "1%15NET30", "IMMEDIATE", "NET45", "NET60"
    };
    
    private static readonly string[] BCCurrencyCodes = {
        "USD", "EUR", "GBP", "CAD", "AUD", "JPY", "CHF", "SEK", "NOK", "DKK"
    };
    
    private static readonly string[] BCItemCategories = {
        "FINISHED", "RAW-MAT", "RESALE", "SERVICE", "MISC", "ASSEMBLY", "KIT", "COMPONENT"
    };
    
    // Supply Chain specific arrays
    private static readonly string[] SupplyChainWarehouseTypes = {
        "Distribution Center", "Manufacturing Plant", "Cross-Dock", "Cold Storage", "Hazmat Storage",
        "Automated Warehouse", "Transit Hub", "Regional Depot", "Consolidation Center", "Fulfillment Center"
    };
    
    private static readonly string[] SupplyChainItemGroups = {
        "Electronics", "Automotive", "Pharmaceutical", "Food & Beverage", "Textiles", "Chemical",
        "Industrial Equipment", "Consumer Goods", "Raw Materials", "Packaging"
    };
    
    private static readonly string[] SupplyChainItemTypes = {
        "Finished Good", "Raw Material", "Work in Progress", "Semi-Finished", "Component", "Assembly",
        "Spare Part", "Consumable", "Tool", "Equipment"
    };
    
    private static readonly string[] SupplyChainCategories = {
        "Production", "Maintenance", "Office Supplies", "Safety Equipment", "IT Hardware", "Chemicals",
        "Packaging Materials", "Tools & Equipment", "Spare Parts", "Consumables"
    };
    
    private static readonly string[] SupplyChainUnitsOfMeasure = {
        "EA", "LB", "KG", "GAL", "L", "FT", "M", "SQ FT", "SQ M", "CASE", "BOX", "PALLET", "ROLL", "SET"
    };
    
    private static readonly string[] SupplyChainRequisitionPriorities = {
        "Low", "Normal", "High", "Critical", "Emergency"
    };
    
    private static readonly string[] SupplyChainProductionStatuses = {
        "Planned", "Released", "Started", "In Progress", "Completed", "On Hold", "Cancelled"
    };
    
    private static readonly string[] SupplyChainBOMTypes = {
        "Production", "Assembly", "Planning", "Service", "Phantom", "Engineering"
    };
    
    private static readonly string[] SupplyChainTransactionTypes = {
        "Receipt", "Issue", "Transfer", "Adjustment", "Count", "Sale", "Purchase", "Production",
        "Consumption", "Output", "Scrap", "Rework"
    };
    
    private static readonly string[] SupplyChainDirections = { "In", "Out" };
    
    // Field Service specific arrays
    private static readonly string[] FSWorkOrderTypes = {
        "Preventive Maintenance", "Corrective Maintenance", "Installation", "Inspection", "Repair",
        "Replacement", "Upgrade", "Emergency", "Warranty", "Contract"
    };
    
    private static readonly string[] FSPriorities = {
        "Low", "Normal", "High", "Critical", "Emergency"
    };
    
    private static readonly string[] FSServiceStatuses = {
        "Draft", "Scheduled", "In Progress", "On Hold", "Completed", "Cancelled", "Closed"
    };
    
    private static readonly string[] FSResourceTypes = {
        "Technician", "Engineer", "Specialist", "Supervisor", "Contractor", "Apprentice"
    };
    
    private static readonly string[] FSSkills = {
        "Electrical", "Mechanical", "HVAC", "Plumbing", "Electronics", "Software", "Networking",
        "Welding", "Hydraulics", "Pneumatics", "Instrumentation", "Calibration"
    };
    
    private static readonly string[] FSEquipmentTypes = {
        "Generator", "Motor", "Pump", "Compressor", "Transformer", "Switch", "Valve", "Sensor",
        "Controller", "Drive", "Panel", "Conveyor", "Robot", "Machine"
    };
    
    private static readonly string[] FSReasonCodes = {
        "Normal Wear", "Manufacturing Defect", "Improper Use", "External Damage", "Age Related",
        "Environmental", "Vandalism", "Accident", "Design Flaw", "Installation Error"
    };

    // Dataverse-specific arrays
    private static readonly string[] Products = {
        "Office 365", "Dynamics 365", "Azure Services", "Power Platform", "SharePoint", "Teams", 
        "Exchange Online", "Windows Server", "SQL Server", "Visual Studio", "Surface Pro", 
        "HoloLens", "Azure DevOps", "Power BI", "Power Apps", "Power Automate", "OneDrive",
        "Outlook", "Word", "Excel", "PowerPoint", "Project", "Visio", "Access", "Publisher"
    };

    private static readonly string[] OpportunityStages = {
        "Qualify", "Develop", "Propose", "Close", "Identify", "Research", "Present", "Negotiate"
    };

    private static readonly string[] CaseTypes = {
        "Technical Issue", "Billing Question", "Product Inquiry", "Feature Request", 
        "Bug Report", "Account Access", "Training Request", "Compliance Question",
        "Security Concern", "Performance Issue", "Integration Problem", "Data Migration"
    };

    private static readonly string[] CampaignNames = {
        "Spring Launch", "Summer Promotion", "Fall Campaign", "Winter Sale", "New Product Launch",
        "Customer Retention", "Lead Generation", "Brand Awareness", "Digital Transformation",
        "Innovation Series", "Market Expansion", "Partner Program", "Trade Show Campaign"
    };

    private static readonly string[] ActivityTypes = {
        "Phone Call", "Email", "Meeting", "Task", "Appointment", "Follow-up", "Presentation",
        "Demo", "Training", "Review", "Planning", "Research", "Documentation", "Support"
    };

    private static readonly string[] StockExchanges = {
        "NYSE", "NASDAQ", "LSE", "TSE", "HKSE", "SSE", "Euronext", "TSX", "ASX", "BSE"
    };

    private static readonly string[] KnowledgeTopics = {
        "Setup and Configuration", "Troubleshooting", "Best Practices", "Integration Guide",
        "Security Configuration", "Performance Optimization", "User Training", "API Documentation",
        "Migration Guide", "Backup and Recovery", "Compliance Requirements", "Feature Overview"
    };

    private static readonly string[] Departments = {
        "Sales", "Marketing", "IT", "Finance", "HR", "Operations", "Customer Service",
        "Research and Development", "Quality Assurance", "Legal", "Procurement", "Manufacturing"
    };
    
    // Location data - comprehensive US coverage with global cities
    private static readonly (string city, string stateOrProvince, string country)[] Locations = {
        // United States (255 cities - minimum 5 per state)
        // Alabama (5 cities)
        ("Birmingham", "AL", "USA"), ("Montgomery", "AL", "USA"), ("Mobile", "AL", "USA"), ("Huntsville", "AL", "USA"), ("Tuscaloosa", "AL", "USA"),
        
        // Alaska (5 cities)
        ("Anchorage", "AK", "USA"), ("Fairbanks", "AK", "USA"), ("Juneau", "AK", "USA"), ("Sitka", "AK", "USA"), ("Ketchikan", "AK", "USA"),
        
        // Arizona (5 cities)
        ("Phoenix", "AZ", "USA"), ("Tucson", "AZ", "USA"), ("Mesa", "AZ", "USA"), ("Chandler", "AZ", "USA"), ("Scottsdale", "AZ", "USA"),
        
        // Arkansas (5 cities)
        ("Little Rock", "AR", "USA"), ("Fort Smith", "AR", "USA"), ("Fayetteville", "AR", "USA"), ("Springdale", "AR", "USA"), ("Jonesboro", "AR", "USA"),
        
        // California (15 cities)
        ("Los Angeles", "CA", "USA"), ("San Diego", "CA", "USA"), ("San Jose", "CA", "USA"), ("San Francisco", "CA", "USA"),
        ("Fresno", "CA", "USA"), ("Sacramento", "CA", "USA"), ("Long Beach", "CA", "USA"), ("Oakland", "CA", "USA"),
        ("Anaheim", "CA", "USA"), ("Santa Ana", "CA", "USA"), ("Riverside", "CA", "USA"), ("Stockton", "CA", "USA"),
        ("Chula Vista", "CA", "USA"), ("Irvine", "CA", "USA"), ("Fremont", "CA", "USA"),
        
        // Colorado (5 cities)
        ("Denver", "CO", "USA"), ("Colorado Springs", "CO", "USA"), ("Aurora", "CO", "USA"), ("Fort Collins", "CO", "USA"), ("Lakewood", "CO", "USA"),
        
        // Connecticut (5 cities)
        ("Bridgeport", "CT", "USA"), ("New Haven", "CT", "USA"), ("Hartford", "CT", "USA"), ("Stamford", "CT", "USA"), ("Waterbury", "CT", "USA"),
        
        // Delaware (5 cities)
        ("Wilmington", "DE", "USA"), ("Dover", "DE", "USA"), ("Newark", "DE", "USA"), ("Middletown", "DE", "USA"), ("Smyrna", "DE", "USA"),
        
        // Florida (10 cities)
        ("Jacksonville", "FL", "USA"), ("Miami", "FL", "USA"), ("Tampa", "FL", "USA"), ("Orlando", "FL", "USA"),
        ("St. Petersburg", "FL", "USA"), ("Hialeah", "FL", "USA"), ("Tallahassee", "FL", "USA"), ("Fort Lauderdale", "FL", "USA"),
        ("Port St. Lucie", "FL", "USA"), ("Cape Coral", "FL", "USA"),
        
        // Georgia (5 cities)
        ("Atlanta", "GA", "USA"), ("Columbus", "GA", "USA"), ("Augusta", "GA", "USA"), ("Savannah", "GA", "USA"), ("Athens", "GA", "USA"),
        
        // Hawaii (5 cities)
        ("Honolulu", "HI", "USA"), ("Pearl City", "HI", "USA"), ("Hilo", "HI", "USA"), ("Kailua", "HI", "USA"), ("Waipahu", "HI", "USA"),
        
        // Idaho (5 cities)
        ("Boise", "ID", "USA"), ("Meridian", "ID", "USA"), ("Nampa", "ID", "USA"), ("Idaho Falls", "ID", "USA"), ("Pocatello", "ID", "USA"),
        
        // Illinois (8 cities)
        ("Chicago", "IL", "USA"), ("Aurora", "IL", "USA"), ("Rockford", "IL", "USA"), ("Joliet", "IL", "USA"),
        ("Naperville", "IL", "USA"), ("Springfield", "IL", "USA"), ("Peoria", "IL", "USA"), ("Elgin", "IL", "USA"),
        
        // Indiana (5 cities)
        ("Indianapolis", "IN", "USA"), ("Fort Wayne", "IN", "USA"), ("Evansville", "IN", "USA"), ("South Bend", "IN", "USA"), ("Carmel", "IN", "USA"),
        
        // Iowa (5 cities)
        ("Des Moines", "IA", "USA"), ("Cedar Rapids", "IA", "USA"), ("Davenport", "IA", "USA"), ("Sioux City", "IA", "USA"), ("Iowa City", "IA", "USA"),
        
        // Kansas (5 cities)
        ("Wichita", "KS", "USA"), ("Overland Park", "KS", "USA"), ("Kansas City", "KS", "USA"), ("Topeka", "KS", "USA"), ("Olathe", "KS", "USA"),
        
        // Kentucky (5 cities)
        ("Louisville", "KY", "USA"), ("Lexington", "KY", "USA"), ("Bowling Green", "KY", "USA"), ("Owensboro", "KY", "USA"), ("Covington", "KY", "USA"),
        
        // Louisiana (5 cities)
        ("New Orleans", "LA", "USA"), ("Baton Rouge", "LA", "USA"), ("Shreveport", "LA", "USA"), ("Lafayette", "LA", "USA"), ("Lake Charles", "LA", "USA"),
        
        // Maine (5 cities)
        ("Portland", "ME", "USA"), ("Lewiston", "ME", "USA"), ("Bangor", "ME", "USA"), ("South Portland", "ME", "USA"), ("Auburn", "ME", "USA"),
        
        // Maryland (5 cities)
        ("Baltimore", "MD", "USA"), ("Frederick", "MD", "USA"), ("Rockville", "MD", "USA"), ("Gaithersburg", "MD", "USA"), ("Bowie", "MD", "USA"),
        
        // Massachusetts (5 cities)
        ("Boston", "MA", "USA"), ("Worcester", "MA", "USA"), ("Springfield", "MA", "USA"), ("Lowell", "MA", "USA"), ("Cambridge", "MA", "USA"),
        
        // Michigan (5 cities)
        ("Detroit", "MI", "USA"), ("Grand Rapids", "MI", "USA"), ("Warren", "MI", "USA"), ("Sterling Heights", "MI", "USA"), ("Lansing", "MI", "USA"),
        
        // Minnesota (5 cities)
        ("Minneapolis", "MN", "USA"), ("St. Paul", "MN", "USA"), ("Rochester", "MN", "USA"), ("Duluth", "MN", "USA"), ("Bloomington", "MN", "USA"),
        
        // Mississippi (5 cities)
        ("Jackson", "MS", "USA"), ("Gulfport", "MS", "USA"), ("Southaven", "MS", "USA"), ("Hattiesburg", "MS", "USA"), ("Biloxi", "MS", "USA"),
        
        // Missouri (5 cities)
        ("Kansas City", "MO", "USA"), ("St. Louis", "MO", "USA"), ("Springfield", "MO", "USA"), ("Columbia", "MO", "USA"), ("Independence", "MO", "USA"),
        
        // Montana (5 cities)
        ("Billings", "MT", "USA"), ("Missoula", "MT", "USA"), ("Great Falls", "MT", "USA"), ("Bozeman", "MT", "USA"), ("Helena", "MT", "USA"),
        
        // Nebraska (5 cities)
        ("Omaha", "NE", "USA"), ("Lincoln", "NE", "USA"), ("Bellevue", "NE", "USA"), ("Grand Island", "NE", "USA"), ("Kearney", "NE", "USA"),
        
        // Nevada (5 cities)
        ("Las Vegas", "NV", "USA"), ("Henderson", "NV", "USA"), ("Reno", "NV", "USA"), ("North Las Vegas", "NV", "USA"), ("Sparks", "NV", "USA"),
        
        // New Hampshire (5 cities)
        ("Manchester", "NH", "USA"), ("Nashua", "NH", "USA"), ("Concord", "NH", "USA"), ("Derry", "NH", "USA"), ("Rochester", "NH", "USA"),
        
        // New Jersey (5 cities)
        ("Newark", "NJ", "USA"), ("Jersey City", "NJ", "USA"), ("Paterson", "NJ", "USA"), ("Elizabeth", "NJ", "USA"), ("Edison", "NJ", "USA"),
        
        // New Mexico (5 cities)
        ("Albuquerque", "NM", "USA"), ("Las Cruces", "NM", "USA"), ("Rio Rancho", "NM", "USA"), ("Santa Fe", "NM", "USA"), ("Roswell", "NM", "USA"),
        
        // New York (10 cities)
        ("New York", "NY", "USA"), ("Buffalo", "NY", "USA"), ("Rochester", "NY", "USA"), ("Yonkers", "NY", "USA"),
        ("Syracuse", "NY", "USA"), ("Albany", "NY", "USA"), ("New Rochelle", "NY", "USA"), ("Mount Vernon", "NY", "USA"),
        ("Schenectady", "NY", "USA"), ("Utica", "NY", "USA"),
        
        // North Carolina (5 cities)
        ("Charlotte", "NC", "USA"), ("Raleigh", "NC", "USA"), ("Greensboro", "NC", "USA"), ("Durham", "NC", "USA"), ("Winston-Salem", "NC", "USA"),
        
        // North Dakota (5 cities)
        ("Fargo", "ND", "USA"), ("Bismarck", "ND", "USA"), ("Grand Forks", "ND", "USA"), ("Minot", "ND", "USA"), ("West Fargo", "ND", "USA"),
        
        // Ohio (8 cities)
        ("Columbus", "OH", "USA"), ("Cleveland", "OH", "USA"), ("Cincinnati", "OH", "USA"), ("Toledo", "OH", "USA"),
        ("Akron", "OH", "USA"), ("Dayton", "OH", "USA"), ("Parma", "OH", "USA"), ("Canton", "OH", "USA"),
        
        // Oklahoma (5 cities)
        ("Oklahoma City", "OK", "USA"), ("Tulsa", "OK", "USA"), ("Norman", "OK", "USA"), ("Broken Arrow", "OK", "USA"), ("Lawton", "OK", "USA"),
        
        // Oregon (5 cities)
        ("Portland", "OR", "USA"), ("Eugene", "OR", "USA"), ("Salem", "OR", "USA"), ("Gresham", "OR", "USA"), ("Hillsboro", "OR", "USA"),
        
        // Pennsylvania (8 cities)
        ("Philadelphia", "PA", "USA"), ("Pittsburgh", "PA", "USA"), ("Allentown", "PA", "USA"), ("Erie", "PA", "USA"),
        ("Reading", "PA", "USA"), ("Scranton", "PA", "USA"), ("Bethlehem", "PA", "USA"), ("Lancaster", "PA", "USA"),
        
        // Rhode Island (5 cities)
        ("Providence", "RI", "USA"), ("Warwick", "RI", "USA"), ("Cranston", "RI", "USA"), ("Pawtucket", "RI", "USA"), ("East Providence", "RI", "USA"),
        
        // South Carolina (5 cities)
        ("Columbia", "SC", "USA"), ("Charleston", "SC", "USA"), ("North Charleston", "SC", "USA"), ("Mount Pleasant", "SC", "USA"), ("Rock Hill", "SC", "USA"),
        
        // South Dakota (5 cities)
        ("Sioux Falls", "SD", "USA"), ("Rapid City", "SD", "USA"), ("Aberdeen", "SD", "USA"), ("Brookings", "SD", "USA"), ("Watertown", "SD", "USA"),
        
        // Tennessee (5 cities)
        ("Nashville", "TN", "USA"), ("Memphis", "TN", "USA"), ("Knoxville", "TN", "USA"), ("Chattanooga", "TN", "USA"), ("Clarksville", "TN", "USA"),
        
        // Texas (15 cities)
        ("Houston", "TX", "USA"), ("San Antonio", "TX", "USA"), ("Dallas", "TX", "USA"), ("Austin", "TX", "USA"),
        ("Fort Worth", "TX", "USA"), ("El Paso", "TX", "USA"), ("Arlington", "TX", "USA"), ("Corpus Christi", "TX", "USA"),
        ("Plano", "TX", "USA"), ("Laredo", "TX", "USA"), ("Lubbock", "TX", "USA"), ("Garland", "TX", "USA"),
        ("Irving", "TX", "USA"), ("Amarillo", "TX", "USA"), ("Grand Prairie", "TX", "USA"),
        
        // Utah (5 cities)
        ("Salt Lake City", "UT", "USA"), ("West Valley City", "UT", "USA"), ("Provo", "UT", "USA"), ("West Jordan", "UT", "USA"), ("Orem", "UT", "USA"),
        
        // Vermont (5 cities)
        ("Burlington", "VT", "USA"), ("Essex", "VT", "USA"), ("South Burlington", "VT", "USA"), ("Colchester", "VT", "USA"), ("Rutland", "VT", "USA"),
        
        // Virginia (8 cities)
        ("Virginia Beach", "VA", "USA"), ("Norfolk", "VA", "USA"), ("Chesapeake", "VA", "USA"), ("Richmond", "VA", "USA"),
        ("Newport News", "VA", "USA"), ("Alexandria", "VA", "USA"), ("Hampton", "VA", "USA"), ("Portsmouth", "VA", "USA"),
        
        // Washington (5 cities)
        ("Seattle", "WA", "USA"), ("Spokane", "WA", "USA"), ("Tacoma", "WA", "USA"), ("Vancouver", "WA", "USA"), ("Bellevue", "WA", "USA"),
        
        // West Virginia (5 cities)
        ("Charleston", "WV", "USA"), ("Huntington", "WV", "USA"), ("Morgantown", "WV", "USA"), ("Parkersburg", "WV", "USA"), ("Wheeling", "WV", "USA"),
        
        // Wisconsin (5 cities)
        ("Milwaukee", "WI", "USA"), ("Madison", "WI", "USA"), ("Green Bay", "WI", "USA"), ("Kenosha", "WI", "USA"), ("Racine", "WI", "USA"),
        
        // Wyoming (5 cities)
        ("Cheyenne", "WY", "USA"), ("Casper", "WY", "USA"), ("Laramie", "WY", "USA"), ("Gillette", "WY", "USA"), ("Rock Springs", "WY", "USA"),
        
        // District of Columbia (1 city)
        ("Washington", "DC", "USA"),
        
        // International Cities (245 cities)
        // Canada (25 cities)
        ("Toronto", "ON", "Canada"), ("Montreal", "QC", "Canada"), ("Calgary", "AB", "Canada"), ("Ottawa", "ON", "Canada"),
        ("Edmonton", "AB", "Canada"), ("Mississauga", "ON", "Canada"), ("Winnipeg", "MB", "Canada"), ("Vancouver", "BC", "Canada"),
        ("Brampton", "ON", "Canada"), ("Hamilton", "ON", "Canada"), ("Quebec City", "QC", "Canada"), ("Surrey", "BC", "Canada"),
        ("Laval", "QC", "Canada"), ("Halifax", "NS", "Canada"), ("London", "ON", "Canada"), ("Markham", "ON", "Canada"),
        ("Vaughan", "ON", "Canada"), ("Gatineau", "QC", "Canada"), ("Longueuil", "QC", "Canada"), ("Burnaby", "BC", "Canada"),
        ("Saskatoon", "SK", "Canada"), ("Kitchener", "ON", "Canada"), ("Windsor", "ON", "Canada"), ("Regina", "SK", "Canada"),
        ("Richmond", "BC", "Canada"),
        
        // United Kingdom (25 cities)
        ("London", "England", "United Kingdom"), ("Birmingham", "England", "United Kingdom"), ("Manchester", "England", "United Kingdom"), ("Liverpool", "England", "United Kingdom"),
        ("Sheffield", "England", "United Kingdom"), ("Leeds", "Yorkshire", "United Kingdom"), ("Edinburgh", "Scotland", "United Kingdom"), ("Glasgow", "Scotland", "United Kingdom"),
        ("Cardiff", "Wales", "United Kingdom"), ("Belfast", "Northern Ireland", "United Kingdom"), ("Newcastle", "England", "United Kingdom"), ("Nottingham", "England", "United Kingdom"),
        ("Bristol", "England", "United Kingdom"), ("Leicester", "England", "United Kingdom"), ("Coventry", "England", "United Kingdom"), ("Hull", "England", "United Kingdom"),
        ("Bradford", "Yorkshire", "United Kingdom"), ("Stoke-on-Trent", "England", "United Kingdom"), ("Wolverhampton", "England", "United Kingdom"), ("Plymouth", "England", "United Kingdom"),
        ("Derby", "England", "United Kingdom"), ("Southampton", "England", "United Kingdom"), ("Portsmouth", "England", "United Kingdom"), ("Aberdeen", "Scotland", "United Kingdom"),
        ("Swansea", "Wales", "United Kingdom"),
        
        // Germany (25 cities)
        ("Berlin", "Berlin", "Germany"), ("Hamburg", "Hamburg", "Germany"), ("Munich", "Bavaria", "Germany"), ("Cologne", "North Rhine-Westphalia", "Germany"),
        ("Frankfurt", "Hesse", "Germany"), ("Stuttgart", "Baden-Württemberg", "Germany"), ("Düsseldorf", "North Rhine-Westphalia", "Germany"), ("Dortmund", "North Rhine-Westphalia", "Germany"),
        ("Essen", "North Rhine-Westphalia", "Germany"), ("Leipzig", "Saxony", "Germany"), ("Bremen", "Bremen", "Germany"), ("Dresden", "Saxony", "Germany"),
        ("Hanover", "Lower Saxony", "Germany"), ("Nuremberg", "Bavaria", "Germany"), ("Duisburg", "North Rhine-Westphalia", "Germany"), ("Bochum", "North Rhine-Westphalia", "Germany"),
        ("Wuppertal", "North Rhine-Westphalia", "Germany"), ("Bielefeld", "North Rhine-Westphalia", "Germany"), ("Bonn", "North Rhine-Westphalia", "Germany"), ("Münster", "North Rhine-Westphalia", "Germany"),
        ("Karlsruhe", "Baden-Württemberg", "Germany"), ("Mannheim", "Baden-Württemberg", "Germany"), ("Augsburg", "Bavaria", "Germany"), ("Wiesbaden", "Hesse", "Germany"),
        ("Mönchengladbach", "North Rhine-Westphalia", "Germany"),
        
        // France (20 cities)
        ("Paris", "Île-de-France", "France"), ("Marseille", "Provence-Alpes-Côte d'Azur", "France"), ("Lyon", "Auvergne-Rhône-Alpes", "France"), ("Toulouse", "Occitanie", "France"),
        ("Nice", "Provence-Alpes-Côte d'Azur", "France"), ("Nantes", "Pays de la Loire", "France"), ("Montpellier", "Occitanie", "France"), ("Strasbourg", "Grand Est", "France"),
        ("Bordeaux", "Nouvelle-Aquitaine", "France"), ("Lille", "Hauts-de-France", "France"), ("Rennes", "Brittany", "France"), ("Reims", "Grand Est", "France"),
        ("Saint-Étienne", "Auvergne-Rhône-Alpes", "France"), ("Le Havre", "Normandy", "France"), ("Toulon", "Provence-Alpes-Côte d'Azur", "France"), ("Grenoble", "Auvergne-Rhône-Alpes", "France"),
        ("Dijon", "Burgundy-Franche-Comté", "France"), ("Angers", "Pays de la Loire", "France"), ("Nîmes", "Occitanie", "France"), ("Villeurbanne", "Auvergne-Rhône-Alpes", "France"),
        
        // Italy (15 cities)
        ("Rome", "Lazio", "Italy"), ("Milan", "Lombardy", "Italy"), ("Naples", "Campania", "Italy"), ("Turin", "Piedmont", "Italy"),
        ("Palermo", "Sicily", "Italy"), ("Genoa", "Liguria", "Italy"), ("Bologna", "Emilia-Romagna", "Italy"), ("Florence", "Tuscany", "Italy"),
        ("Bari", "Apulia", "Italy"), ("Catania", "Sicily", "Italy"), ("Venice", "Veneto", "Italy"), ("Verona", "Veneto", "Italy"),
        ("Messina", "Sicily", "Italy"), ("Padua", "Veneto", "Italy"), ("Trieste", "Friuli-Venezia Giulia", "Italy"),
        
        // Spain (15 cities)
        ("Madrid", "Community of Madrid", "Spain"), ("Barcelona", "Catalonia", "Spain"), ("Valencia", "Valencian Community", "Spain"), ("Seville", "Andalusia", "Spain"),
        ("Zaragoza", "Aragon", "Spain"), ("Málaga", "Andalusia", "Spain"), ("Murcia", "Region of Murcia", "Spain"), ("Palma", "Balearic Islands", "Spain"),
        ("Las Palmas", "Canary Islands", "Spain"), ("Bilbao", "Basque Country", "Spain"), ("Alicante", "Valencian Community", "Spain"), ("Córdoba", "Andalusia", "Spain"),
        ("Valladolid", "Castile and León", "Spain"), ("Vigo", "Galicia", "Spain"), ("Gijón", "Asturias", "Spain"),
        
        // Netherlands (10 cities)
        ("Amsterdam", "North Holland", "Netherlands"), ("Rotterdam", "South Holland", "Netherlands"), ("The Hague", "South Holland", "Netherlands"), ("Utrecht", "Utrecht", "Netherlands"),
        ("Eindhoven", "North Brabant", "Netherlands"), ("Tilburg", "North Brabant", "Netherlands"), ("Groningen", "Groningen", "Netherlands"), ("Almere", "Flevoland", "Netherlands"),
        ("Breda", "North Brabant", "Netherlands"), ("Nijmegen", "Gelderland", "Netherlands"),
        
        // Australia (10 cities)
        ("Sydney", "NSW", "Australia"), ("Melbourne", "VIC", "Australia"), ("Brisbane", "QLD", "Australia"), ("Perth", "WA", "Australia"),
        ("Adelaide", "SA", "Australia"), ("Gold Coast", "QLD", "Australia"), ("Newcastle", "NSW", "Australia"), ("Canberra", "ACT", "Australia"),
        ("Sunshine Coast", "QLD", "Australia"), ("Wollongong", "NSW", "Australia"),
        
        // Japan (15 cities)
        ("Tokyo", "Tokyo", "Japan"), ("Yokohama", "Kanagawa", "Japan"), ("Osaka", "Osaka", "Japan"), ("Nagoya", "Aichi", "Japan"),
        ("Sapporo", "Hokkaido", "Japan"), ("Fukuoka", "Fukuoka", "Japan"), ("Kobe", "Hyogo", "Japan"), ("Kyoto", "Kyoto", "Japan"),
        ("Kawasaki", "Kanagawa", "Japan"), ("Saitama", "Saitama", "Japan"), ("Hiroshima", "Hiroshima", "Japan"), ("Sendai", "Miyagi", "Japan"),
        ("Kitakyushu", "Fukuoka", "Japan"), ("Chiba", "Chiba", "Japan"), ("Sakai", "Osaka", "Japan"),
        
        // Brazil (15 cities)
        ("São Paulo", "São Paulo", "Brazil"), ("Rio de Janeiro", "Rio de Janeiro", "Brazil"), ("Brasília", "Federal District", "Brazil"), ("Salvador", "Bahia", "Brazil"),
        ("Fortaleza", "Ceará", "Brazil"), ("Belo Horizonte", "Minas Gerais", "Brazil"), ("Manaus", "Amazonas", "Brazil"), ("Curitiba", "Paraná", "Brazil"),
        ("Recife", "Pernambuco", "Brazil"), ("Goiânia", "Goiás", "Brazil"), ("Belém", "Pará", "Brazil"), ("Porto Alegre", "Rio Grande do Sul", "Brazil"),
        ("Guarulhos", "São Paulo", "Brazil"), ("Campinas", "São Paulo", "Brazil"), ("Nova Iguaçu", "Rio de Janeiro", "Brazil"),
        
        // Mexico (15 cities)
        ("Mexico City", "Mexico City", "Mexico"), ("Guadalajara", "Jalisco", "Mexico"), ("Monterrey", "Nuevo León", "Mexico"), ("Puebla", "Puebla", "Mexico"),
        ("Tijuana", "Baja California", "Mexico"), ("León", "Guanajuato", "Mexico"), ("Juárez", "Chihuahua", "Mexico"), ("Torreón", "Coahuila", "Mexico"),
        ("Querétaro", "Querétaro", "Mexico"), ("San Luis Potosí", "San Luis Potosí", "Mexico"), ("Mérida", "Yucatán", "Mexico"), ("Mexicali", "Baja California", "Mexico"),
        ("Aguascalientes", "Aguascalientes", "Mexico"), ("Cuernavaca", "Morelos", "Mexico"), ("Saltillo", "Coahuila", "Mexico"),
        
        // China (15 cities)
        ("Beijing", "Beijing", "China"), ("Shanghai", "Shanghai", "China"), ("Guangzhou", "Guangdong", "China"), ("Shenzhen", "Guangdong", "China"),
        ("Tianjin", "Tianjin", "China"), ("Wuhan", "Hubei", "China"), ("Dongguan", "Guangdong", "China"), ("Chengdu", "Sichuan", "China"),
        ("Nanjing", "Jiangsu", "China"), ("Chongqing", "Chongqing", "China"), ("Xi'an", "Shaanxi", "China"), ("Shenyang", "Liaoning", "China"),
        ("Hangzhou", "Zhejiang", "China"), ("Foshan", "Guangdong", "China"), ("Qingdao", "Shandong", "China"),
        
        // India (15 cities)
        ("Mumbai", "Maharashtra", "India"), ("Delhi", "Delhi", "India"), ("Bangalore", "Karnataka", "India"), ("Hyderabad", "Telangana", "India"),
        ("Ahmedabad", "Gujarat", "India"), ("Chennai", "Tamil Nadu", "India"), ("Kolkata", "West Bengal", "India"), ("Surat", "Gujarat", "India"),
        ("Pune", "Maharashtra", "India"), ("Jaipur", "Rajasthan", "India"), ("Lucknow", "Uttar Pradesh", "India"), ("Kanpur", "Uttar Pradesh", "India"),
        ("Nagpur", "Maharashtra", "India"), ("Indore", "Madhya Pradesh", "India"), ("Thane", "Maharashtra", "India"),
        
        // South Korea (10 cities)
        ("Seoul", "Seoul", "South Korea"), ("Busan", "Busan", "South Korea"), ("Incheon", "Incheon", "South Korea"), ("Daegu", "Daegu", "South Korea"),
        ("Daejeon", "Daejeon", "South Korea"), ("Gwangju", "Gwangju", "South Korea"), ("Suwon", "Gyeonggi", "South Korea"), ("Ulsan", "Ulsan", "South Korea"),
        ("Changwon", "South Gyeongsang", "South Korea"), ("Goyang", "Gyeonggi", "South Korea"),
        
        // South Africa (10 cities)
        ("Johannesburg", "Gauteng", "South Africa"), ("Cape Town", "Western Cape", "South Africa"), ("Durban", "KwaZulu-Natal", "South Africa"), ("Pretoria", "Gauteng", "South Africa"),
        ("Port Elizabeth", "Eastern Cape", "South Africa"), ("Pietermaritzburg", "KwaZulu-Natal", "South Africa"), ("Benoni", "Gauteng", "South Africa"), ("Tembisa", "Gauteng", "South Africa"),
        ("East London", "Eastern Cape", "South Africa"), ("Vereeniging", "Gauteng", "South Africa"),
        
        // Sweden (10 cities)
        ("Stockholm", "Stockholm", "Sweden"), ("Gothenburg", "Västra Götaland", "Sweden"), ("Malmö", "Skåne", "Sweden"), ("Uppsala", "Uppsala", "Sweden"),
        ("Västerås", "Västmanland", "Sweden"), ("Örebro", "Örebro", "Sweden"), ("Linköping", "Östergötland", "Sweden"), ("Helsingborg", "Skåne", "Sweden"),
        ("Jönköping", "Jönköping", "Sweden"), ("Norrköping", "Östergötland", "Sweden"),
        
        // Norway (5 cities)
        ("Oslo", "Oslo", "Norway"), ("Bergen", "Vestland", "Norway"), ("Stavanger", "Rogaland", "Norway"), ("Trondheim", "Trøndelag", "Norway"),
        ("Drammen", "Viken", "Norway"),
        
        // Denmark (5 cities)
        ("Copenhagen", "Capital Region", "Denmark"), ("Aarhus", "Central Denmark", "Denmark"), ("Odense", "Southern Denmark", "Denmark"), ("Aalborg", "North Denmark", "Denmark"),
        ("Esbjerg", "Southern Denmark", "Denmark"),
        
        // Belgium (5 cities)
        ("Brussels", "Brussels-Capital", "Belgium"), ("Antwerp", "Flanders", "Belgium"), ("Ghent", "Flanders", "Belgium"), ("Charleroi", "Wallonia", "Belgium"),
        ("Liège", "Wallonia", "Belgium"),
        
        // Switzerland (5 cities)
        ("Zurich", "Zurich", "Switzerland"), ("Geneva", "Geneva", "Switzerland"), ("Basel", "Basel-Stadt", "Switzerland"), ("Bern", "Bern", "Switzerland"),
        ("Lausanne", "Vaud", "Switzerland"),
        
        // Austria (5 cities)
        ("Vienna", "Vienna", "Austria"), ("Graz", "Styria", "Austria"), ("Linz", "Upper Austria", "Austria"), ("Salzburg", "Salzburg", "Austria"),
        ("Innsbruck", "Tyrol", "Austria"),
        
        // Poland (5 cities)
        ("Warsaw", "Masovian", "Poland"), ("Kraków", "Lesser Poland", "Poland"), ("Łódź", "Łódź", "Poland"), ("Wrocław", "Lower Silesian", "Poland"),
        ("Poznań", "Greater Poland", "Poland")
    };
    
    // ================================================================================================
    // HELPER METHODS
    // ================================================================================================
    
    private void InitializeSharedIdPools(Random seededRandom, int accountPoolSize, int contactPoolSize, int productPoolSize)
    {
        // Generate shared Account ID pool
        _sharedAccountIds = new Guid[accountPoolSize];
        _sharedAccountNames = new string[accountPoolSize];
        for (int i = 0; i < accountPoolSize; i++)
        {
            _sharedAccountIds[i] = Guid.NewGuid();
            _sharedAccountNames[i] = CompanyNames[seededRandom.Next(CompanyNames.Length)];
        }
        
        // Generate shared Contact ID pool  
        _sharedContactIds = new Guid[contactPoolSize];
        _sharedContactNames = new string[contactPoolSize];
        for (int i = 0; i < contactPoolSize; i++)
        {
            _sharedContactIds[i] = Guid.NewGuid();
            var firstName = FirstNames[seededRandom.Next(FirstNames.Length)];
            var lastName = LastNames[seededRandom.Next(LastNames.Length)];
            _sharedContactNames[i] = $"{firstName} {lastName}";
        }
        
        // Generate shared Product ID pool
        _sharedProductIds = new Guid[productPoolSize];
        _sharedProductNames = new string[productPoolSize];
        for (int i = 0; i < productPoolSize; i++)
        {
            _sharedProductIds[i] = Guid.NewGuid();
            _sharedProductNames[i] = Products[seededRandom.Next(Products.Length)];
        }
    }
    
    private Guid GetRandomSharedAccountId(Random random)
    {
        return _sharedAccountIds[random.Next(_sharedAccountIds.Length)];
    }
    
    private Guid GetRandomSharedContactId(Random random)
    {
        return _sharedContactIds[random.Next(_sharedContactIds.Length)];
    }
    
    private Guid GetRandomSharedProductId(Random random)
    {
        return _sharedProductIds[random.Next(_sharedProductIds.Length)];
    }
    
    private string GetAccountNameById(Guid accountId)
    {
        var index = Array.IndexOf(_sharedAccountIds, accountId);
        return index >= 0 ? _sharedAccountNames[index] : "Unknown Account";
    }
    
    private string GetContactNameById(Guid contactId)
    {
        var index = Array.IndexOf(_sharedContactIds, contactId);
        return index >= 0 ? _sharedContactNames[index] : "Unknown Contact";
    }
    
    private string GetProductNameById(Guid productId)
    {
        var index = Array.IndexOf(_sharedProductIds, productId);
        return index >= 0 ? _sharedProductNames[index] : "Unknown Product";
    }
    
    private Dictionary<string, string> ParseQueryString(string query)
    {
        var result = new Dictionary<string, string>();
        if (string.IsNullOrEmpty(query)) return result;
        
        query = query.TrimStart('?');
        var pairs = query.Split('&');
        
        foreach (var pair in pairs)
        {
            var parts = pair.Split('=');
            if (parts.Length == 2)
            {
                result[parts[0]] = Uri.UnescapeDataString(parts[1]);
            }
        }
        
        return result;
    }
    
    private int GetIntParameter(Dictionary<string, string> query, string key, int defaultValue, int min, int max)
    {
        if (query.TryGetValue(key, out string value) && int.TryParse(value, out int result))
        {
            return Math.Max(min, Math.Min(max, result));
        }
        return defaultValue;
    }
    
    private string DecodeOperationId(string operationId)
    {
        // Power Platform connectors typically pass operation IDs directly
        // If it looks like base64, try to decode it, otherwise return as-is
        if (string.IsNullOrEmpty(operationId))
            return operationId;
            
        // Check if it's a valid operation name pattern
        if (operationId.All(c => char.IsLetterOrDigit(c) || c == '_'))
            return operationId;
            
        try
        {
            byte[] data = Convert.FromBase64String(operationId);
            var decoded = Encoding.UTF8.GetString(data);
            // Return decoded only if it looks like a valid operation name
            if (decoded.All(c => char.IsLetterOrDigit(c) || c == '_'))
                return decoded;
        }
        catch (FormatException)
        {
            // Not base64, ignore
        }
        
        return operationId;
    }
    
    private HttpResponseMessage CreateJsonResponse(object data)
    {
        var json = JsonConvert.SerializeObject(data, Newtonsoft.Json.Formatting.Indented);
        var response = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        };
        return response;
    }
    
    private HttpResponseMessage CreateErrorResponse(string message)
    {
        var error = new { error = new { message = message } };
        var json = JsonConvert.SerializeObject(error);
        var response = new HttpResponseMessage(HttpStatusCode.BadRequest)
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        };
        return response;
    }
    
    // ================================================================================================
    // UTILITY METHODS
    // ================================================================================================
    
    private static string GetRandomItem<T>(T[] array, Random random = null)
    {
        var rnd = random ?? _random;
        return array[rnd.Next(array.Length)].ToString();
    }
    
    private static DateTime GenerateRandomDate(DateTime start, DateTime end, Random random = null)
    {
        var rnd = random ?? _random;
        var range = end - start;
        var randomDays = rnd.Next((int)range.TotalDays);
        return start.AddDays(randomDays);
    }
    
    private static (string city, string stateOrProvince, string country) GetRandomLocation(Random random = null)
    {
        var rnd = random ?? _random;
        return Locations[rnd.Next(Locations.Length)];
    }
    
    private static string GeneratePostalCode(string country, Random random = null)
    {
        var rnd = random ?? _random;
        char[] letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
        switch (country.ToUpper())
        {
            case "USA":
                return $"{rnd.Next(10000, 99999)}";
            case "CANADA":
                return $"{letters[rnd.Next(letters.Length)]}{rnd.Next(0, 9)}{letters[rnd.Next(letters.Length)]} {rnd.Next(0, 9)}{letters[rnd.Next(letters.Length)]}{rnd.Next(0, 9)}";
            case "UK":
                return $"{letters[rnd.Next(letters.Length)]}{letters[rnd.Next(letters.Length)]}{rnd.Next(1, 99)} {rnd.Next(0, 9)}{letters[rnd.Next(letters.Length)]}{letters[rnd.Next(letters.Length)]}";
            case "AUSTRALIA":
                return $"{rnd.Next(1000, 9999)}";
            default:
                return $"{rnd.Next(10000, 99999)}";
        }
    }
    
    private static string GeneratePhoneNumber(Random random = null)
    {
        var rnd = random ?? _random;
        return $"({rnd.Next(200, 999)}) {rnd.Next(200, 999)}-{rnd.Next(1000, 9999)}";
    }
    
    private static string GenerateEmail(string firstName, string lastName, string companyName = null)
    {
        var domain = companyName != null 
            ? companyName.ToLower().Replace(" ", "").Replace("inc", "").Replace("corp", "").Replace(".", "") + ".com"
            : GetRandomItem(new[] { "gmail.com", "outlook.com", "yahoo.com", "company.com", "email.com" });
        
        return $"{firstName.ToLower()}.{lastName.ToLower()}@{domain}";
    }
    
    // ================================================================================================
    // BUSINESS CENTRAL GENERATORS
    // ================================================================================================
    
    private object[] GenerateBusinessCentralCustomers(int count, Random random)
    {
        return Enumerable.Range(1, count).Select(i => new
        {
            id = $"C{i:D6}",
            displayName = $"{GetRandomItem(FirstNames)} {GetRandomItem(LastNames)}",
            type = "Customer",
            addressLine1 = $"{random.Next(100, 9999)} {GetRandomItem(StreetNames)}",
            city = GetRandomLocation().city,
            state = GetRandomLocation().stateOrProvince,
            country = GetRandomLocation().country,
            postalCode = GeneratePostalCode("USA"),
            phoneNumber = GeneratePhoneNumber(),
            email = GenerateEmail(GetRandomItem(FirstNames), GetRandomItem(LastNames)),
            customerGroup = GetRandomItem(BCCustomerGroups),
            paymentTerms = GetRandomItem(BCPaymentTerms),
            currencyCode = GetRandomItem(BCCurrencyCodes),
            creditLimit = random.Next(1000, 100000),
            blocked = random.NextDouble() < 0.05,
            lastModifiedDateTime = DateTime.Now.AddDays(-random.Next(1, 365))
        }).ToArray();
    }
    
    private object[] GenerateBusinessCentralVendors(int count, Random random)
    {
        return Enumerable.Range(1, count).Select(i => new
        {
            id = $"V{i:D6}",
            displayName = GetRandomItem(CompanyNames),
            addressLine1 = $"{random.Next(100, 9999)} {GetRandomItem(StreetNames)}",
            city = GetRandomLocation().city,
            state = GetRandomLocation().stateOrProvince,
            country = GetRandomLocation().country,
            postalCode = GeneratePostalCode("USA"),
            phoneNumber = GeneratePhoneNumber(),
            email = GenerateEmail("info", "contact", GetRandomItem(CompanyNames)),
            paymentTerms = GetRandomItem(BCPaymentTerms),
            currencyCode = GetRandomItem(BCCurrencyCodes),
            blocked = random.NextDouble() < 0.03,
            lastModifiedDateTime = DateTime.Now.AddDays(-random.Next(1, 365))
        }).ToArray();
    }
    
    private object[] GenerateBusinessCentralItems(int count, Random random)
    {
        return Enumerable.Range(1, count).Select(i => new
        {
            id = $"ITEM{i:D6}",
            displayName = $"Product {i}",
            type = "Inventory",
            itemCategory = GetRandomItem(BCItemCategories),
            blocked = random.NextDouble() < 0.02,
            unitPrice = Math.Round(random.NextDouble() * 1000 + 10, 2),
            unitCost = Math.Round(random.NextDouble() * 500 + 5, 2),
            inventory = random.Next(0, 1000),
            lastModifiedDateTime = DateTime.Now.AddDays(-random.Next(1, 365))
        }).ToArray();
    }
    
    private object[] GenerateBusinessCentralSalesOrders(int count, Random random)
    {
        return Enumerable.Range(1, count).Select(i => new
        {
            id = $"SO{i:D6}",
            number = $"SO{i:D6}",
            customerId = $"C{random.Next(1, 1000):D6}",
            customerName = $"{GetRandomItem(FirstNames)} {GetRandomItem(LastNames)}",
            orderDate = GenerateRandomDate(DateTime.Now.AddDays(-30), DateTime.Now),
            requestedDeliveryDate = GenerateRandomDate(DateTime.Now, DateTime.Now.AddDays(30)),
            status = GetRandomItem(new[] { "Draft", "Open", "Released", "Pending Approval", "Pending Prepayment" }),
            currencyCode = GetRandomItem(BCCurrencyCodes),
            totalAmountExcludingTax = Math.Round(random.NextDouble() * 10000 + 100, 2),
            totalAmountIncludingTax = Math.Round(random.NextDouble() * 12000 + 120, 2),
            lastModifiedDateTime = DateTime.Now.AddDays(-random.Next(1, 30))
        }).ToArray();
    }
    
    private object[] GenerateBusinessCentralPurchaseOrders(int count, Random random)
    {
        return Enumerable.Range(1, count).Select(i => new
        {
            id = $"PO{i:D6}",
            number = $"PO{i:D6}",
            vendorId = $"V{random.Next(1, 500):D6}",
            vendorName = GetRandomItem(CompanyNames),
            orderDate = GenerateRandomDate(DateTime.Now.AddDays(-30), DateTime.Now),
            expectedReceiptDate = GenerateRandomDate(DateTime.Now, DateTime.Now.AddDays(30)),
            status = GetRandomItem(new[] { "Draft", "Open", "Released", "Pending Approval" }),
            currencyCode = GetRandomItem(BCCurrencyCodes),
            totalAmountExcludingTax = Math.Round(random.NextDouble() * 15000 + 200, 2),
            totalAmountIncludingTax = Math.Round(random.NextDouble() * 18000 + 240, 2),
            lastModifiedDateTime = DateTime.Now.AddDays(-random.Next(1, 30))
        }).ToArray();
    }
    
    // ================================================================================================
    // SUPPLY CHAIN GENERATORS
    // ================================================================================================
    
    private object[] GenerateSupplyChainWarehouses(int count, Random random)
    {
        return Enumerable.Range(1, count).Select(i => {
            var location = GetRandomLocation();
            return new
            {
                id = $"WH{i:D3}",
                name = $"{GetRandomItem(SupplyChainWarehouseTypes)} - {location.city}",
                type = GetRandomItem(SupplyChainWarehouseTypes),
                address = $"{random.Next(100, 9999)} {GetRandomItem(StreetNames)}",
                city = location.city,
                state = location.stateOrProvince,
                country = location.country,
                postalCode = GeneratePostalCode(location.country),
                manager = $"{GetRandomItem(FirstNames)} {GetRandomItem(LastNames)}",
                capacity = random.Next(5000, 100000),
                status = GetRandomItem(new[] { "Active", "Inactive", "Maintenance" }),
                lastModifiedDateTime = DateTime.Now.AddDays(-random.Next(1, 365))
            };
        }).ToArray();
    }
    
    private object[] GenerateSupplyChainInventoryItems(int count, Random random)
    {
        return Enumerable.Range(1, count).Select(i => new
        {
            itemNumber = $"ITM{i:D6}",
            itemName = $"{GetRandomItem(SupplyChainCategories)} Item {i}",
            description = $"High quality item for {GetRandomItem(SupplyChainItemGroups).ToLower()} use",
            itemGroup = GetRandomItem(SupplyChainItemGroups),
            itemType = GetRandomItem(SupplyChainItemTypes),
            unitOfMeasure = GetRandomItem(SupplyChainUnitsOfMeasure),
            standardCost = Math.Round(random.NextDouble() * 500 + 10, 2),
            listPrice = Math.Round(random.NextDouble() * 800 + 15, 2),
            category = GetRandomItem(SupplyChainCategories),
            status = GetRandomItem(new[] { "Active", "Inactive", "Discontinued" }),
            lastModifiedDateTime = DateTime.Now.AddDays(-random.Next(1, 365))
        }).ToArray();
    }
    
    private object[] GenerateSupplyChainPurchaseRequisitions(int count, Random random)
    {
        return Enumerable.Range(1, count).Select(i => new
        {
            requisitionNumber = $"REQ{i:D6}",
            requestor = $"{GetRandomItem(FirstNames)} {GetRandomItem(LastNames)}",
            department = GetRandomItem(new[] { "Production", "Maintenance", "IT", "Office", "Quality" }),
            requisitionDate = GenerateRandomDate(DateTime.Now.AddDays(-30), DateTime.Now),
            requiredDate = GenerateRandomDate(DateTime.Now, DateTime.Now.AddDays(21)),
            priority = GetRandomItem(SupplyChainRequisitionPriorities),
            totalAmount = Math.Round(random.NextDouble() * 50000 + 1000, 2),
            currency = GetRandomItem(BCCurrencyCodes),
            status = GetRandomItem(new[] { "Draft", "Submitted", "Approved", "Rejected", "Ordered" }),
            approver = $"{GetRandomItem(FirstNames)} {GetRandomItem(LastNames)}",
            lastModifiedDateTime = DateTime.Now.AddDays(-random.Next(1, 30))
        }).ToArray();
    }
    
    private object[] GenerateSupplyChainProductionOrders(int count, Random random)
    {
        return Enumerable.Range(1, count).Select(i => new
        {
            productionOrderNumber = $"PO{i:D6}",
            item = $"ITM{random.Next(100000, 999999)}",
            quantity = random.Next(50, 1000),
            unitOfMeasure = GetRandomItem(SupplyChainUnitsOfMeasure),
            startDate = GenerateRandomDate(DateTime.Now.AddDays(-14), DateTime.Now.AddDays(30)),
            endDate = GenerateRandomDate(DateTime.Now, DateTime.Now.AddDays(44)),
            priority = GetRandomItem(SupplyChainRequisitionPriorities),
            status = GetRandomItem(SupplyChainProductionStatuses),
            warehouse = $"WH{random.Next(1, 10):D2}",
            productionManager = $"{GetRandomItem(FirstNames)} {GetRandomItem(LastNames)}",
            estimatedCost = Math.Round(random.NextDouble() * 25000 + 1000, 2),
            lastModifiedDateTime = DateTime.Now.AddDays(-random.Next(1, 30))
        }).ToArray();
    }
    
    private object[] GenerateSupplyChainBillOfMaterials(int count, Random random)
    {
        return Enumerable.Range(1, count).Select(i => new
        {
            bomNumber = $"BOM{i:D6}",
            parentItem = $"ITM{random.Next(100000, 999999)}",
            componentItem = $"ITM{random.Next(100000, 999999)}",
            quantity = Math.Round(random.NextDouble() * 10 + 0.1, 3),
            unitOfMeasure = GetRandomItem(SupplyChainUnitsOfMeasure),
            bomType = GetRandomItem(SupplyChainBOMTypes),
            version = $"V{random.Next(1, 10)}.{random.Next(0, 9)}",
            status = GetRandomItem(new[] { "Active", "Inactive", "Under Review", "Approved" }),
            lastModifiedDateTime = DateTime.Now.AddDays(-random.Next(1, 365))
        }).ToArray();
    }
    
    private object[] GenerateSupplyChainInventoryTransactions(int count, Random random)
    {
        return Enumerable.Range(1, count).Select(i => new
        {
            transactionNumber = $"TXN{i:D8}",
            transactionType = GetRandomItem(SupplyChainTransactionTypes),
            item = $"ITM{random.Next(100000, 999999)}",
            quantity = random.Next(1, 500),
            unitOfMeasure = GetRandomItem(SupplyChainUnitsOfMeasure),
            unitCost = Math.Round(random.NextDouble() * 100 + 5, 2),
            warehouse = $"WH{random.Next(1, 10):D2}",
            direction = GetRandomItem(SupplyChainDirections),
            transactionDate = GenerateRandomDate(DateTime.Now.AddDays(-30), DateTime.Now),
            lastModifiedDateTime = DateTime.Now.AddDays(-random.Next(1, 30))
        }).ToArray();
    }
    
    // ================================================================================================
    // FIELD SERVICE GENERATORS
    // ================================================================================================
    
    private object[] GenerateFieldServiceWorkOrders(int count, Random random)
    {
        return Enumerable.Range(1, count).Select(i => new
        {
            workOrderNumber = $"WO{i:D6}",
            workOrderType = GetRandomItem(FSWorkOrderTypes),
            priority = GetRandomItem(FSPriorities),
            status = GetRandomItem(FSServiceStatuses),
            serviceAccount = GetRandomItem(CompanyNames),
            primaryResource = $"{GetRandomItem(FirstNames)} {GetRandomItem(LastNames)}",
            scheduledStart = GenerateRandomDate(DateTime.Now.AddDays(-7), DateTime.Now.AddDays(14)),
            scheduledEnd = GenerateRandomDate(DateTime.Now.AddDays(-7), DateTime.Now.AddDays(14)),
            address = $"{random.Next(100, 9999)} {GetRandomItem(StreetNames)}",
            city = GetRandomLocation().city,
            estimatedDuration = random.Next(60, 480), // minutes
            actualDuration = random.Next(45, 520), // minutes
            lastModifiedDateTime = DateTime.Now.AddDays(-random.Next(1, 30))
        }).ToArray();
    }
    
    private object[] GenerateFieldServiceResources(int count, Random random)
    {
        return Enumerable.Range(1, count).Select(i => new
        {
            resourceId = $"RES{i:D4}",
            name = $"{GetRandomItem(FirstNames)} {GetRandomItem(LastNames)}",
            resourceType = GetRandomItem(FSResourceTypes),
            primarySkill = GetRandomItem(FSSkills),
            territory = GetRandomLocation().city,
            phoneNumber = GeneratePhoneNumber(),
            email = GenerateEmail(GetRandomItem(FirstNames), GetRandomItem(LastNames)),
            hourlyRate = Math.Round(random.NextDouble() * 100 + 25, 2),
            isActive = random.NextDouble() < 0.9,
            lastModifiedDateTime = DateTime.Now.AddDays(-random.Next(1, 365))
        }).ToArray();
    }
    
    private object[] GenerateFieldServiceEquipment(int count, Random random)
    {
        return Enumerable.Range(1, count).Select(i => new
        {
            equipmentId = $"EQ{i:D6}",
            name = $"{GetRandomItem(FSEquipmentTypes)} {i}",
            equipmentType = GetRandomItem(FSEquipmentTypes),
            serialNumber = $"SN{random.Next(100000, 999999)}",
            manufacturer = GetRandomItem(CompanyNames),
            model = $"Model-{random.Next(100, 999)}",
            installDate = GenerateRandomDate(DateTime.Now.AddDays(-1825), DateTime.Now.AddDays(-30)),
            warrantyExpiration = GenerateRandomDate(DateTime.Now.AddDays(-365), DateTime.Now.AddDays(365)),
            serviceAccount = GetRandomItem(CompanyNames),
            location = GetRandomLocation().city,
            status = GetRandomItem(new[] { "Active", "Inactive", "Under Maintenance", "Retired" }),
            lastModifiedDateTime = DateTime.Now.AddDays(-random.Next(1, 365))
        }).ToArray();
    }
    
    private object[] GenerateFieldServiceServiceAccounts(int count, Random random)
    {
        return Enumerable.Range(1, count).Select(i => {
            var location = GetRandomLocation();
            return new
            {
                accountId = $"SA{i:D6}",
                accountName = GetRandomItem(CompanyNames),
                primaryContact = $"{GetRandomItem(FirstNames)} {GetRandomItem(LastNames)}",
                address = $"{random.Next(100, 9999)} {GetRandomItem(StreetNames)}",
                city = location.city,
                state = location.stateOrProvince,
                country = location.country,
                postalCode = GeneratePostalCode(location.country),
                phoneNumber = GeneratePhoneNumber(),
                email = GenerateEmail("contact", "info", GetRandomItem(CompanyNames)),
                industry = GetRandomItem(Industries),
                serviceTerritory = location.city,
                isActive = random.NextDouble() < 0.95,
                lastModifiedDateTime = DateTime.Now.AddDays(-random.Next(1, 365))
            };
        }).ToArray();
    }
    
    private object[] GenerateFieldServiceProducts(int count, Random random)
    {
        return Enumerable.Range(1, count).Select(i => new
        {
            productId = $"PROD{i:D6}",
            productName = $"Service Product {i}",
            productType = GetRandomItem(new[] { "Inventory", "Non-Inventory", "Service" }),
            category = GetRandomItem(SupplyChainCategories),
            unitOfMeasure = GetRandomItem(SupplyChainUnitsOfMeasure),
            listPrice = Math.Round(random.NextDouble() * 500 + 10, 2),
            standardCost = Math.Round(random.NextDouble() * 300 + 5, 2),
            vendor = GetRandomItem(CompanyNames),
            isActive = random.NextDouble() < 0.9,
            lastModifiedDateTime = DateTime.Now.AddDays(-random.Next(1, 365))
        }).ToArray();
    }
    
    // ================================================================================================
    // DATAVERSE GENERATORS
    // ================================================================================================
    
    private object[] GenerateDataverseAccounts(int count, Random random)
    {
        return Enumerable.Range(1, count).Select(i => {
            var location = GetRandomLocation(random);
            // Use shared Account ID from pool if available, otherwise generate new
            var accountId = i <= _sharedAccountIds.Length ? _sharedAccountIds[i - 1] : Guid.NewGuid();
            var companyName = i <= _sharedAccountNames.Length ? _sharedAccountNames[i - 1] : GetRandomItem(CompanyNames, random);
            
            return new
            {
                accountid = accountId.ToString(),
                name = companyName,
                accountnumber = $"ACC{i:D6}",
                telephone1 = GeneratePhoneNumber(random),
                emailaddress1 = GenerateEmail("info", "contact", companyName),
                websiteurl = $"https://www.{companyName.ToLower().Replace(" ", "").Replace("inc", "").Replace("corp", "")}.com",
                address1_line1 = $"{random.Next(100, 9999)} {GetRandomItem(StreetNames, random)}",
                address1_city = location.city,
                address1_stateorprovince = location.stateOrProvince,
                address1_country = location.country,
                address1_postalcode = GeneratePostalCode(location.country, random),
                industrycode = random.Next(1, 20), // Industry picklist value
                revenue = Math.Round(random.NextDouble() * 10000000 + 100000, 2),
                numberofemployees = random.Next(10, 5000),
                description = $"Leading {GetRandomItem(Industries, random).ToLower()} company providing innovative solutions",
                statecode = 0, // Active
                statuscode = 1, // Active status
                createdon = GenerateRandomDate(DateTime.Now.AddDays(-365), DateTime.Now, random),
                modifiedon = GenerateRandomDate(DateTime.Now.AddDays(-30), DateTime.Now, random)
            };
        }).ToArray();
    }

    private object[] GenerateDataverseContacts(int count, Random random)
    {
        return Enumerable.Range(1, count).Select(i => {
            var location = GetRandomLocation(random);
            var firstName = GetRandomItem(FirstNames, random);
            var lastName = GetRandomItem(LastNames, random);
            var parentAccountId = GetRandomSharedAccountId(random);
            
            return new
            {
                contactid = Guid.NewGuid().ToString(),
                firstname = firstName,
                lastname = lastName,
                fullname = $"{firstName} {lastName}",
                emailaddress1 = GenerateEmail(firstName, lastName),
                telephone1 = GeneratePhoneNumber(random),
                mobilephone = GeneratePhoneNumber(random),
                jobtitle = GetRandomItem(JobTitles, random),
                department = GetRandomItem(Departments, random),
                parentcustomerid = parentAccountId.ToString(),
                parentcustomername = GetAccountNameById(parentAccountId),
                address1_line1 = $"{random.Next(100, 9999)} {GetRandomItem(StreetNames, random)}",
                address1_city = location.city,
                address1_stateorprovince = location.stateOrProvince,
                address1_country = location.country,
                address1_postalcode = GeneratePostalCode(location.country, random),
                birthdate = GenerateRandomDate(DateTime.Now.AddYears(-65), DateTime.Now.AddYears(-25), random),
                gendercode = random.Next(1, 3),
                statecode = 0, // Active
                statuscode = 1, // Active status
                createdon = GenerateRandomDate(DateTime.Now.AddDays(-365), DateTime.Now, random),
                modifiedon = GenerateRandomDate(DateTime.Now.AddDays(-30), DateTime.Now, random)
            };
        }).ToArray();
    }

    private object[] GenerateDataverseLeads(int count, Random random)
    {
        return Enumerable.Range(1, count).Select(i => {
            var location = GetRandomLocation();
            var firstName = GetRandomItem(FirstNames);
            var lastName = GetRandomItem(LastNames);
            return new
            {
                leadid = Guid.NewGuid().ToString(),
                firstname = firstName,
                lastname = lastName,
                fullname = $"{firstName} {lastName}",
                emailaddress1 = GenerateEmail(firstName, lastName),
                telephone1 = GeneratePhoneNumber(),
                mobilephone = GeneratePhoneNumber(),
                jobtitle = GetRandomItem(JobTitles),
                companyname = GetRandomItem(CompanyNames),
                websiteurl = $"https://www.{GetRandomItem(CompanyNames).ToLower().Replace(" ", "")}.com",
                address1_line1 = $"{random.Next(100, 9999)} {GetRandomItem(StreetNames)}",
                address1_city = location.city,
                address1_stateorprovince = location.stateOrProvince,
                address1_country = location.country,
                address1_postalcode = GeneratePostalCode(location.country),
                revenue = Math.Round(random.NextDouble() * 5000000 + 50000, 2),
                numberofemployees = random.Next(1, 1000),
                industrycode = random.Next(1, 20),
                leadqualitycode = random.Next(1, 4),
                leadsourcecode = random.Next(1, 8),
                statecode = 0, // Open
                statuscode = 1, // New
                createdon = GenerateRandomDate(DateTime.Now.AddDays(-180), DateTime.Now),
                modifiedon = GenerateRandomDate(DateTime.Now.AddDays(-30), DateTime.Now)
            };
        }).ToArray();
    }

    private object[] GenerateDataverseOpportunities(int count, Random random)
    {
        return Enumerable.Range(1, count).Select(i => {
            return new
            {
                opportunityid = Guid.NewGuid().ToString(),
                name = $"{GetRandomItem(Products)} Deal - {GetRandomItem(CompanyNames)}",
                customerid = Guid.NewGuid().ToString(),
                accountid = Guid.NewGuid().ToString(),
                contactid = Guid.NewGuid().ToString(),
                estimatedvalue = Math.Round(random.NextDouble() * 1000000 + 10000, 2),
                actualvalue = Math.Round(random.NextDouble() * 500000 + 5000, 2),
                budgetamount = Math.Round(random.NextDouble() * 1200000 + 15000, 2),
                estimatedclosedate = GenerateRandomDate(DateTime.Now.AddDays(30), DateTime.Now.AddDays(180)),
                actualclosedate = GenerateRandomDate(DateTime.Now.AddDays(-30), DateTime.Now.AddDays(60)),
                closeprobability = random.Next(10, 95),
                salesstage = random.Next(1, 7),
                stepname = GetRandomItem(OpportunityStages),
                statecode = 0, // Open
                statuscode = 1, // In Progress
                createdon = GenerateRandomDate(DateTime.Now.AddDays(-365), DateTime.Now),
                modifiedon = GenerateRandomDate(DateTime.Now.AddDays(-30), DateTime.Now)
            };
        }).ToArray();
    }

    private object[] GenerateDataverseCases(int count, Random random)
    {
        return Enumerable.Range(1, count).Select(i => {
            return new
            {
                incidentid = Guid.NewGuid().ToString(),
                ticketnumber = $"CAS-{random.Next(100000, 999999)}",
                title = GetRandomItem(CaseTypes),
                description = $"Customer reported issue with {GetRandomItem(Products)}. Investigation required.",
                customerid = Guid.NewGuid().ToString(),
                contactid = Guid.NewGuid().ToString(),
                accountid = Guid.NewGuid().ToString(),
                caseorigincode = random.Next(1, 6),
                casetypecode = random.Next(1, 4),
                prioritycode = random.Next(1, 4),
                severitycode = random.Next(1, 4),
                statecode = 0, // Active
                statuscode = 1, // In Progress
                escalated = random.Next(0, 2) == 1,
                followupby = GenerateRandomDate(DateTime.Now.AddDays(1), DateTime.Now.AddDays(30)),
                createdon = GenerateRandomDate(DateTime.Now.AddDays(-90), DateTime.Now),
                modifiedon = GenerateRandomDate(DateTime.Now.AddDays(-7), DateTime.Now)
            };
        }).ToArray();
    }

    private object[] GenerateDataverseProducts(int count, Random random)
    {
        return Enumerable.Range(1, count).Select(i => {
            return new
            {
                productid = Guid.NewGuid().ToString(),
                name = GetRandomItem(Products),
                productnumber = $"PRD-{i:D4}",
                description = $"High-quality {GetRandomItem(Products)} for enterprise use",
                price = Math.Round(random.NextDouble() * 10000 + 100, 2),
                standardcost = Math.Round(random.NextDouble() * 5000 + 50, 2),
                currentcost = Math.Round(random.NextDouble() * 5500 + 60, 2),
                listprice = Math.Round(random.NextDouble() * 12000 + 120, 2),
                quantityonhand = random.Next(0, 1000),
                quantityallocated = random.Next(0, 100),
                productstructure = random.Next(1, 4),
                producttypecode = random.Next(1, 5),
                vendorname = GetRandomItem(CompanyNames),
                vendorpartnumber = $"VPN-{random.Next(10000, 99999)}",
                statecode = 0, // Active
                statuscode = 1, // Active
                createdon = GenerateRandomDate(DateTime.Now.AddDays(-730), DateTime.Now),
                modifiedon = GenerateRandomDate(DateTime.Now.AddDays(-30), DateTime.Now)
            };
        }).ToArray();
    }

    private object[] GenerateDataverseQuotes(int count, Random random)
    {
        return Enumerable.Range(1, count).Select(i => {
            return new
            {
                quoteid = Guid.NewGuid().ToString(),
                quotenumber = $"QUO-{random.Next(100000, 999999)}",
                name = $"Quote for {GetRandomItem(CompanyNames)} - {GetRandomItem(Products)}",
                customerid = Guid.NewGuid().ToString(),
                accountid = Guid.NewGuid().ToString(),
                contactid = Guid.NewGuid().ToString(),
                opportunityid = Guid.NewGuid().ToString(),
                totalamount = Math.Round(random.NextDouble() * 500000 + 10000, 2),
                totallineitemamount = Math.Round(random.NextDouble() * 450000 + 9000, 2),
                totaltax = Math.Round(random.NextDouble() * 50000 + 1000, 2),
                totaldiscountamount = Math.Round(random.NextDouble() * 25000, 2),
                freightamount = Math.Round(random.NextDouble() * 5000, 2),
                effectivefrom = GenerateRandomDate(DateTime.Now.AddDays(-30), DateTime.Now),
                effectiveto = GenerateRandomDate(DateTime.Now.AddDays(30), DateTime.Now.AddDays(90)),
                requestdeliveryby = GenerateRandomDate(DateTime.Now.AddDays(45), DateTime.Now.AddDays(120)),
                statecode = 0, // Draft
                statuscode = 1, // Draft
                revisionnumber = random.Next(1, 5),
                createdon = GenerateRandomDate(DateTime.Now.AddDays(-60), DateTime.Now),
                modifiedon = GenerateRandomDate(DateTime.Now.AddDays(-7), DateTime.Now)
            };
        }).ToArray();
    }

    private object[] GenerateDataverseOrders(int count, Random random)
    {
        return Enumerable.Range(1, count).Select(i => {
            return new
            {
                salesorderid = Guid.NewGuid().ToString(),
                ordernumber = $"ORD-{random.Next(100000, 999999)}",
                name = $"Order for {GetRandomItem(CompanyNames)} - {GetRandomItem(Products)}",
                customerid = Guid.NewGuid().ToString(),
                accountid = Guid.NewGuid().ToString(),
                contactid = Guid.NewGuid().ToString(),
                opportunityid = Guid.NewGuid().ToString(),
                quoteid = Guid.NewGuid().ToString(),
                totalamount = Math.Round(random.NextDouble() * 750000 + 15000, 2),
                totallineitemamount = Math.Round(random.NextDouble() * 700000 + 14000, 2),
                totaltax = Math.Round(random.NextDouble() * 75000 + 1500, 2),
                totaldiscountamount = Math.Round(random.NextDouble() * 37500, 2),
                freightamount = Math.Round(random.NextDouble() * 7500, 2),
                datedelivered = GenerateRandomDate(DateTime.Now.AddDays(-30), DateTime.Now.AddDays(30)),
                requestdeliveryby = GenerateRandomDate(DateTime.Now.AddDays(15), DateTime.Now.AddDays(60)),
                submitdate = GenerateRandomDate(DateTime.Now.AddDays(-14), DateTime.Now),
                statecode = 0, // Active
                statuscode = 1, // New
                ispricelocked = random.Next(0, 2) == 1,
                createdon = GenerateRandomDate(DateTime.Now.AddDays(-90), DateTime.Now),
                modifiedon = GenerateRandomDate(DateTime.Now.AddDays(-7), DateTime.Now)
            };
        }).ToArray();
    }

    private object[] GenerateDataverseCampaigns(int count, Random random)
    {
        return Enumerable.Range(1, count).Select(i => {
            return new
            {
                campaignid = Guid.NewGuid().ToString(),
                name = GetRandomItem(CampaignNames),
                codename = $"CAMP-{i:D4}",
                description = $"Marketing campaign targeting {GetRandomItem(Departments)} segment",
                budgetallocatedamount = Math.Round(random.NextDouble() * 500000 + 10000, 2),
                actualcost = Math.Round(random.NextDouble() * 400000 + 8000, 2),
                expectedrevenue = Math.Round(random.NextDouble() * 1000000 + 50000, 2),
                actualrevenue = Math.Round(random.NextDouble() * 800000 + 40000, 2),
                proposedstart = GenerateRandomDate(DateTime.Now.AddDays(-60), DateTime.Now.AddDays(30)),
                proposedend = GenerateRandomDate(DateTime.Now.AddDays(30), DateTime.Now.AddDays(180)),
                actualstart = GenerateRandomDate(DateTime.Now.AddDays(-45), DateTime.Now.AddDays(15)),
                actualend = GenerateRandomDate(DateTime.Now.AddDays(45), DateTime.Now.AddDays(210)),
                typecode = random.Next(1, 6),
                statecode = 0, // Active
                statuscode = 1, // Proposed
                expectedresponse = random.Next(5, 25),
                otherexpenses = Math.Round(random.NextDouble() * 25000, 2),
                promotioncodename = $"PROMO{random.Next(100, 999)}",
                createdon = GenerateRandomDate(DateTime.Now.AddDays(-180), DateTime.Now),
                modifiedon = GenerateRandomDate(DateTime.Now.AddDays(-14), DateTime.Now)
            };
        }).ToArray();
    }

    private object[] GenerateDataverseCompetitors(int count, Random random)
    {
        return Enumerable.Range(1, count).Select(i => {
            var location = GetRandomLocation();
            return new
            {
                competitorid = Guid.NewGuid().ToString(),
                name = GetRandomItem(CompanyNames),
                overview = $"Major competitor in the {GetRandomItem(Departments)} market segment",
                strengths = "Strong market presence, competitive pricing, good customer service",
                weaknesses = "Limited product range, slower innovation cycle",
                opportunities = "Expanding into new markets, digital transformation",
                threats = "Economic downturn, increased competition, regulatory changes",
                keyproduct = GetRandomItem(Products),
                websiteurl = $"https://www.{GetRandomItem(CompanyNames).ToLower().Replace(" ", "")}.com",
                address1_line1 = $"{random.Next(100, 9999)} {GetRandomItem(StreetNames)}",
                address1_city = location.city,
                address1_stateorprovince = location.stateOrProvince,
                address1_country = location.country,
                address1_postalcode = GeneratePostalCode(location.country),
                stockexchange = GetRandomItem(StockExchanges),
                tickersymbol = $"{GetRandomItem(CompanyNames).Substring(0, 3).ToUpper()}{random.Next(10, 99)}",
                createdon = GenerateRandomDate(DateTime.Now.AddDays(-730), DateTime.Now),
                modifiedon = GenerateRandomDate(DateTime.Now.AddDays(-30), DateTime.Now)
            };
        }).ToArray();
    }

    private object[] GenerateDataverseInvoices(int count, Random random)
    {
        return Enumerable.Range(1, count).Select(i => {
            var location = GetRandomLocation();
            return new
            {
                invoiceid = Guid.NewGuid().ToString(),
                invoicenumber = $"INV-{random.Next(100000, 999999)}",
                name = $"Invoice for {GetRandomItem(CompanyNames)} - {GetRandomItem(Products)}",
                customerid = Guid.NewGuid().ToString(),
                accountid = Guid.NewGuid().ToString(),
                contactid = Guid.NewGuid().ToString(),
                salesorderid = Guid.NewGuid().ToString(),
                totalamount = Math.Round(random.NextDouble() * 1000000 + 20000, 2),
                totallineitemamount = Math.Round(random.NextDouble() * 950000 + 19000, 2),
                totaltax = Math.Round(random.NextDouble() * 100000 + 2000, 2),
                totaldiscountamount = Math.Round(random.NextDouble() * 50000, 2),
                freightamount = Math.Round(random.NextDouble() * 10000, 2),
                datedelivered = GenerateRandomDate(DateTime.Now.AddDays(-60), DateTime.Now),
                duedate = GenerateRandomDate(DateTime.Now.AddDays(15), DateTime.Now.AddDays(45)),
                invoicedate = GenerateRandomDate(DateTime.Now.AddDays(-30), DateTime.Now),
                statecode = 0, // Active
                statuscode = 1, // New
                ispricelocked = random.Next(0, 2) == 1,
                billto_name = GetRandomItem(CompanyNames),
                billto_line1 = $"{random.Next(100, 9999)} {GetRandomItem(StreetNames)}",
                billto_city = location.city,
                billto_stateorprovince = location.stateOrProvince,
                billto_postalcode = GeneratePostalCode(location.country),
                billto_country = location.country,
                createdon = GenerateRandomDate(DateTime.Now.AddDays(-90), DateTime.Now),
                modifiedon = GenerateRandomDate(DateTime.Now.AddDays(-7), DateTime.Now)
            };
        }).ToArray();
    }

    private object[] GenerateDataverseActivities(int count, Random random)
    {
        return Enumerable.Range(1, count).Select(i => {
            return new
            {
                activityid = Guid.NewGuid().ToString(),
                subject = GetRandomItem(ActivityTypes),
                description = $"Follow up activity regarding {GetRandomItem(Products)} discussion",
                regardingobjectid = Guid.NewGuid().ToString(),
                ownerid = Guid.NewGuid().ToString(),
                actualstart = GenerateRandomDate(DateTime.Now.AddDays(-30), DateTime.Now),
                actualend = GenerateRandomDate(DateTime.Now.AddDays(-29), DateTime.Now.AddDays(1)),
                scheduledstart = GenerateRandomDate(DateTime.Now.AddDays(-35), DateTime.Now.AddDays(-5)),
                scheduledend = GenerateRandomDate(DateTime.Now.AddDays(-34), DateTime.Now.AddDays(-4)),
                actualdurationminutes = random.Next(15, 240),
                scheduleddurationminutes = random.Next(30, 180),
                prioritycode = random.Next(0, 3),
                statecode = 0, // Open
                statuscode = 1, // Open
                activitytypecode = GetRandomItem(ActivityTypes),
                isbilled = random.Next(0, 2) == 1,
                isworkflowcreated = random.Next(0, 2) == 1,
                createdon = GenerateRandomDate(DateTime.Now.AddDays(-180), DateTime.Now),
                modifiedon = GenerateRandomDate(DateTime.Now.AddDays(-7), DateTime.Now)
            };
        }).ToArray();
    }

    private object[] GenerateDataverseTeams(int count, Random random)
    {
        return Enumerable.Range(1, count).Select(i => {
            var location = GetRandomLocation();
            return new
            {
                teamid = Guid.NewGuid().ToString(),
                name = $"{GetRandomItem(Departments)} Team",
                description = $"Responsible for {GetRandomItem(Departments)} operations and strategy",
                teamtype = random.Next(0, 4),
                businessunitid = Guid.NewGuid().ToString(),
                administratorid = Guid.NewGuid().ToString(),
                emailaddress = GenerateEmail("team", GetRandomItem(Departments)),
                regardingobjectid = Guid.NewGuid().ToString(),
                isdefault = random.Next(0, 2) == 1,
                systemmanaged = random.Next(0, 2) == 1,
                queueid = Guid.NewGuid().ToString(),
                address1_line1 = $"{random.Next(100, 9999)} {GetRandomItem(StreetNames)}",
                address1_city = location.city,
                address1_stateorprovince = location.stateOrProvince,
                address1_country = location.country,
                address1_postalcode = GeneratePostalCode(location.country),
                createdon = GenerateRandomDate(DateTime.Now.AddDays(-1095), DateTime.Now),
                modifiedon = GenerateRandomDate(DateTime.Now.AddDays(-30), DateTime.Now)
            };
        }).ToArray();
    }

    private object[] GenerateDataverseTerritories(int count, Random random)
    {
        return Enumerable.Range(1, count).Select(i => {
            return new
            {
                territoryid = Guid.NewGuid().ToString(),
                name = $"{GetRandomItem(new[] { "North", "South", "East", "West", "Central" })} {GetRandomItem(new[] { "Region", "Territory", "District", "Zone" })}",
                description = $"Sales territory covering multiple states and regions",
                managerid = Guid.NewGuid().ToString(),
                organizationid = Guid.NewGuid().ToString(),
                createdon = GenerateRandomDate(DateTime.Now.AddDays(-1095), DateTime.Now),
                modifiedon = GenerateRandomDate(DateTime.Now.AddDays(-90), DateTime.Now),
                createdby = Guid.NewGuid().ToString(),
                modifiedby = Guid.NewGuid().ToString(),
                versionnumber = random.Next(1000000, 9999999),
                importsequencenumber = random.Next(1, 999999),
                overriddencreatedon = GenerateRandomDate(DateTime.Now.AddDays(-1100), DateTime.Now.AddDays(-1095)),
                timezoneruleversionnumber = random.Next(1, 100),
                utcconversiontimezonecode = random.Next(-12, 14)
            };
        }).ToArray();
    }

    private object[] GenerateDataverseKnowledgeArticles(int count, Random random)
    {
        return Enumerable.Range(1, count).Select(i => {
            return new
            {
                knowledgearticleid = Guid.NewGuid().ToString(),
                title = $"How to {GetRandomItem(KnowledgeTopics)}",
                articlepublicnumber = $"KB{random.Next(100000, 999999)}",
                description = $"Comprehensive guide on {GetRandomItem(KnowledgeTopics)} best practices",
                content = $"This article provides detailed information about {GetRandomItem(KnowledgeTopics)}. Follow these steps...",
                keywords = $"{GetRandomItem(KnowledgeTopics)}, guide, tutorial, help",
                majorversionnumber = random.Next(1, 5),
                minorversionnumber = random.Next(0, 10),
                languagelocaleid = 1033, // English (United States)
                publishedon = GenerateRandomDate(DateTime.Now.AddDays(-365), DateTime.Now),
                expirationdate = GenerateRandomDate(DateTime.Now.AddDays(365), DateTime.Now.AddDays(1095)),
                statecode = 3, // Published
                statuscode = 7, // Published
                rating = Math.Round(random.NextDouble() * 4 + 1, 1),
                ratingcount = random.Next(0, 100),
                ratingsum = random.Next(0, 500),
                isinternal = random.Next(0, 2) == 1,
                islatestversion = random.Next(0, 2) == 1,
                isprimary = random.Next(0, 2) == 1,
                isrootarticle = random.Next(0, 2) == 1,
                readyforreview = random.Next(0, 2) == 1,
                review = random.Next(0, 3),
                createdon = GenerateRandomDate(DateTime.Now.AddDays(-730), DateTime.Now),
                modifiedon = GenerateRandomDate(DateTime.Now.AddDays(-30), DateTime.Now)
            };
        }).ToArray();
    }

    private object[] GenerateDataverseContracts(int count, Random random)
    {
        return Enumerable.Range(1, count).Select(i => {
            var location = GetRandomLocation();
            return new
            {
                contractid = Guid.NewGuid().ToString(),
                contractnumber = $"CON-{random.Next(100000, 999999)}",
                title = $"Service Contract - {GetRandomItem(CompanyNames)}",
                customerid = Guid.NewGuid().ToString(),
                accountid = Guid.NewGuid().ToString(),
                contactid = Guid.NewGuid().ToString(),
                contracttemplateid = Guid.NewGuid().ToString(),
                totalvalue = Math.Round(random.NextDouble() * 2000000 + 50000, 2),
                totalrevenue = Math.Round(random.NextDouble() * 1800000 + 45000, 2),
                netprice = Math.Round(random.NextDouble() * 1500000 + 40000, 2),
                activeon = GenerateRandomDate(DateTime.Now.AddDays(-365), DateTime.Now),
                expireson = GenerateRandomDate(DateTime.Now.AddDays(365), DateTime.Now.AddDays(1095)),
                cancelon = GenerateRandomDate(DateTime.Now.AddDays(400), DateTime.Now.AddDays(1200)),
                contractservicelevelcode = random.Next(1, 4),
                contracttypecode = random.Next(1, 5),
                billingcustomerid = Guid.NewGuid().ToString(),
                billingaccountid = Guid.NewGuid().ToString(),
                billingcontactid = Guid.NewGuid().ToString(),
                billtoaddress_line1 = $"{random.Next(100, 9999)} {GetRandomItem(StreetNames)}",
                billtoaddress_city = location.city,
                billtoaddress_stateorprovince = location.stateOrProvince,
                billtoaddress_country = location.country,
                billtoaddress_postalcode = GeneratePostalCode(location.country),
                statecode = 0, // Draft
                statuscode = 1, // Draft
                createdon = GenerateRandomDate(DateTime.Now.AddDays(-400), DateTime.Now),
                modifiedon = GenerateRandomDate(DateTime.Now.AddDays(-30), DateTime.Now)
            };
        }).ToArray();
    }
}