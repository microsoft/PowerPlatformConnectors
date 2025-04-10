# Meekou

Meekou connector provides a powerful base functions. Using this connector, you can focus on implementing business logic without write duplicate operation to do basic operations.

## Publisher: Meekou

## Prerequisites

You will need the following to proceed:

* A Microsoft Power Apps or Power Automate plan with custom connector feature
* The Power platform CLI tools

## Supported Operations

### RoundUp

Round up number value

```yaml
input:
    1.25
```

![image](https://github.com/user-attachments/assets/84f98a39-13f6-4be3-8bd5-50199834cdec)

### Evaluate
  
Calculate Text formula value

```yaml
formula:
    1+2
```

![image](https://github.com/user-attachments/assets/bb44afd9-a99b-4bee-991d-ba8115ca8f13)

### Sum

Sum node value by path

```yaml
data:
    {
        "Value": [
            {
                "Id": 1,
                "Name": "Jack",
                "Score": 90
            },
            {
                "Id": 2,
                "Name": "Cindy",
                "Score": 90
            }
        ]
    }
path:    
    Value[*].Score
```

![Sum](https://github.com/user-attachments/assets/460e2228-ba01-4ee1-beaf-4c6a2be7a228)

### HtmlToPdf

Convert html string to pdf

```yaml
htmlContent:
    <div>米可</div>
```

![HtmlToPdf](https://github.com/user-attachments/assets/5444443a-41ac-4bc8-9160-dd416bffc52f)

## Obtaining Credentials

No Authenticate required.

## Known Issues and Limitations

There is no known issues.