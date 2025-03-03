# Address Labs

Address Labs is a free service designed to parse addresses within the United States into their individual components. It efficiently breaks down a single address string into structured data, making it easier to work with address information in various applications.

## Publisher: Richard Wilson

## Prerequisites

To use this connector, no special prerequisites or authentication is required. The Address Labs API is freely available and does not require an API key.

## Supported Operations

### Parse Address

Parses a single address string into its component parts, such as street number, street name, city, state, ZIP code, and more.

- **Inputs**:

  | Name        | Description                                                                                          | Example                                 |
  |-------------|------------------------------------------------------------------------------------------------------|-----------------------------------------|
  | `Address`   | The address you want to parse, entered as a single string.                                            | `5404 Wisconsin Ave #700 Chevy Chase MD 20815` |

- **Outputs**:

  Returns the parsed address information, including components such as street number, street name, unit, city, state, ZIP code, and more.

  | Name                    | Description                                                                                          | Example                                 |
  |-------------------------|------------------------------------------------------------------------------------------------------|-----------------------------------------|
  | `number`                | Street number                                                                                       | `5404`                                  |
  | `street`                | Street name                                                                                         | `Wisconsin`                             |
  | `street_suffix`         | Street suffix (e.g., 'St', 'Ave')                                                                   | `Ave`                                   |
  | `unit`                  | Unit number                                                                                         | `700`                                   |
  | `city`                  | City                                                                                                | `Chevy Chase`                           |
  | `state`                 | State abbreviation                                                                                  | `MD`                                    |
  | `zip`                   | 5-digit ZIP code                                                                                    | `20815`                                 |
  | `intersection`          | Indicates if the address is an intersection (true/false)                                            | `false`                                 |
  | `delivery.address_line` | 1st line of the address formatted following USPS guidelines                                         | `5404 Wisconsin Ave # 700`              |
  | `delivery.last_line`    | 2nd line of the address formatted following USPS guidelines                                         | `Chevy Chase MD  20815`                 |
  | `input`                 | Original input address string                                                                       | `5404 Wisconsin Ave #700 Chevy Chase MD 20815` |

## Obtaining Credentials

No credentials are necessary to use the Address Labs API. It is publicly accessible for anyone to use.

## Known Issues and Limitations

Currently, the Address Labs API is only capable of parsing addresses within the United States. No international support is provided at this time. Additionally, the API may return incomplete results if the input address is formatted in an unexpected way. Always refer to this section for updated information.
