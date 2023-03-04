## Power Assist
The Power Assist connector empowers you and your organization's citizen developers to solve business problems faster than ever with the Power Platform, offering actions to manipulate data in familiar ways that are currently difficult or unsupported within Power Automate. Whether you need to quickly sort an Array, perform advanced filtering, make a mathematical calculation, execute a Regular Expression on a string, escape HTML, check the type of a value, or do one of many more common tasks, Power Assist makes it simple. No more long, complex expressions, convoluted conditionals, and inefficient looping: Use Power Assist to make your Power Automate workflows fast and easy to build.

## Prerequisites
1. Go to the [**Power Assist API**](https://rapidapi.com/elevate-digital-elevate-digital-default/api/power-assist) and navigate to the **Pricing** tab. If you are signing up on behalf of your organization, make sure your organization is selected in the dropdown above the pricing options.
2. Subscribe to your desired plan. The **Basic** plan is FREE and offers access to all endpoints, but has a hard limit of 50 requests per month. The **Pro** plan is a paid subscription offering access to all endpoints, with a soft limit of 3,000 requests per month.
3. Sign in or create your account. If you are signing up on behalf of your organization, consider checking the box to create an Organization in RapidAPI so that other users at your organization can share access to the API, without sharing RapidAPI credentials. 
4. Enter your card information, if applicable.
5. Once you are subscribed, go to the **Endpoints** tab and locate the **X-RapidAPI-Key** in the Content Snippet in the right column. Copy this value, as you will need it to authenticate in Power Automate.

## Setting up the connector
1. In the [Power Automate portal](https://make.powerautomate.com/), create a new flow or edit an existing one. 
2. Add a new action to your flow and in the "Choose an operation" menu, search for "Power Assist" under the Premium tab. 
3. Select your preferred action.
4. You will be prompted to supply a Connection name and API Key. Enter a memorable name for the new Connection, and for the API Key, paste the value that you copied from the X-RapidAPI-Key earlier. This Connection will be saved by Power Automate, and available for use in future flows.
5. That's it! You can now use Power Assist actions in all your Power Automate flows. 

## Supported Operations
The connector supports the following operations:
* `Sort Array`: Perform a simple ascending sort of an Array.
* `Reverse Array`: Reverse the order of an array of any data type.
* `Sort Array of Objects by Property`: Accepts an Array of objects and sorts it by the object Property specified.
* `Filter Array`: Filter an array of any data type based on a specified condition.
* `Prepend to Array`: Given an Array and a Value, this action adds the value as the first item in the Array. If an Array is supplied as the Value, a flat array will be returned with each of the items prepended.
* `Check whether Any of the Items in an Array meet a condition`: This action returns True if any of the items in an Array match a specified condition; otherwise, it returns False.
* `Check whether Every item in an Array meet a condition`: This action returns True if all of the items in an Array match a specified condition; otherwise, it returns False.
* `Remove First`: Accepts an Array of any data type. Returns an Array with the first Item that matches the specified condition removed. If no Item matches the condition, the entire Array is returned.
* `Array - Group By`: Group an Array of items. Accepts an Array of any data type. Returns an Object with keys that reflect the values of the provided propertyName. Under each key are the items that reflect the value.
* `Array - Find First`: Get the first item in an Array that meets a specified condition. If no item matches, Null is returned.
* `Round Up Number (aka Math.ceil)`: Rounds a number to the nearest integer. Supports numbers passed in as strings, but does NOT support commas or other formatting in number strings. If an integer is passed in, it will be returned unchanged.
* `Round Down Number (aka Math.floor)`: Rounds a number down to the nearest integer. Supports numbers passed in as strings, but does NOT support commas or other formatting in number strings. If an integer is passed in, it will be returned unchanged.
* `Calculate Average (Arithmetic Mean) from an Array of Numbers`: Calculates the average (mean) from an Array of numbers. Strings that can be converted to numbers are allowed, but formatting such as commas are NOT supported.
* `Calculate Median from an Array of Numbers`: Calculates the median from an Array of numbers. Strings that can be converted to numbers are allowed, but formatting such as commas are NOT supported.
* `Calculate Mode (most frequently occurring number) from an Array of Numbers`: Calculates the mode from an Array of numbers. Strings that can be converted to numbers are allowed, but formatting such as commas are NOT supported. If multiple instances of the same number are passed in separately as a string and a number, they will be counted as instances of the same number.
* `Generate a Random Number`: Generates a pseudo-random number between the minimum of 0 and the specified maximum (maximum must be 1, 10, 100, 1000, 10000).
* `String Replace All - Replace all instances of a substring`: Is case sensitive. Does not accept RegEx. To use RegEx, see the "String RegEx Replace" action.
* `String RegEx Replace - replace values in a string using a Regular Expression`: Find and replace within a String using a RegEx pattern. Include the leading and trailing '/' in your pattern and optionally append flags. If the `/g` flag is used, it will replace all occurrences. Use the `/i` flag to make the search ignore case. Example: `/\w+/gi`
* `Capitalize String - set the first character to uppercase, and all subsequent characters to lower case.`: Sets the first character of the string to upper case, and all subsequent characters to lower case.
* `Trim String - remove whitespace or specified characters from the start and end of a string`: Trims leading and trailing whitespace (the default) or specified characters from a string.
* `Trim Start of String`: Trim whitespace (the default) or specified characters only at the start of a string.
* `Trim End of String`: Trim whitespace (by default) or specified characters from the end of a string.
* `Slugify String - transform text into an ASCII slug which can be used in URLs`: Transform text into an ASCII slug which can be used in safely in URLs. Replaces whitespaces, accentuated, and special characters with a dash. Many non-ascii characters are transformed to similar versions in the ascii character set such as ä to a.
* `Split String into Array of Words`: Split string by delimiter (String or RegEx pattern). The action splits by whitespace by default. If using RegEx, include the leading and trailing '/' in your pattern and optionally append flags.
* `Count Words in String by Specified Delimiter`: Count the words in a String by a delimiter (String or RegEx pattern). The delimiter is whitespace by default. If using RegEx, include the leading and trailing '/' in your pattern and optionally append flags.
* `Strip HTML out of a String`: Remove all HTML and XML tags from a string.
* `Clean a String - trim and replace multiple spaces with a single space`: Trim and replace multiple spaces with a single space. (This includes whitespace characters like \t and \n.)
* `Clean Diacritics from a String`: Replace all diacritic characters (letters with glyphs, such as ä) in a string with the closest ASCII equivalents.
* `Escape HTML in a String`: Convert HTML special characters, like < and >, to their entity equivalents (e.g., `&lt;`). This action supports cent, yen, euro, pound, lt, gt, copy, reg, quote, amp, and apos.
* `Unescape HTML in a String`: Convert entity characters (such as `&lt;`) to HTML equivalents. This action supports cent, yen, euro, pound, lt, gt, copy, reg, quote, amp, apos, and nbsp.
* `Count Instances of a Substring in a String`: Get the number of occurrences of a substring within a string.
* `Chop a String - break a string into an array of strings of specified length`: Chop the string into an Array based on an interval, which defines the size of the pieces.
* `Check that a Value is a String`: Validates whether a supplied value is of type String.
* `Check that a Value is a Number`: Validates that a value is a Number. Numbers inside strings, such as "999" will be evaluated to False unless the "includeNumbersInStrings" parameter is set to True.
* `Check that a Value is Null or Empty`: Check if value is null or empty. Can be used for Strings, Arrays, or Objects.
* `Check that a Value is an Array`: Validate whether a supplied Value is an Array.
* `Check that a Value is an Object`: Validate whether a supplied Value is an Object. Empty Objects will evaluate to True. Arrays and other data types will evaluate to False.
* `Validate Email - check that a String is in the common email format`: Validates that a String matches the common email format. Does NOT send an email. Returns True if it passes; otherwise, False.
* `Validate a String based on RegEx`: Validates a String against a supplied RegEx pattern. Include the leading and trailing '/' in your RegEx pattern and optionally append flags. Returns True if it passes; otherwise, False.