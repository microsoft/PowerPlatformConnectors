# Power Fx Functions
A comprehensive Power Fx function library providing 270+ mathematical, text, logical, date and time, table manipulation, conversion, and utility functions using custom C# script execution.

## Publisher: Troy Taylor

## Prerequisites
You will need to use this connector in Power Platform environments that support custom connectors with code components.

## Obtaining Credentials
This connector uses the hosting environment's authentication. No additional credentials are required.

## Supported Operations

### Evaluate Power Fx Formula
Evaluates a Power Fx formula and returns the result with type information.

### Parse Power Fx Formula
Parses a Power Fx formula and returns syntax tree information.

### Validate Power Fx Formula
Validates a Power Fx formula and returns any syntax or semantic errors.

### Execute Math Function
Executes mathematical functions including: abs, power, sqrt, mod, round, sin, cos, tan, ln, log, exp, pi, int, roundup, rounddown, trunc, asin, acos, atan, atan2, acot, cot, radians, degrees, rand, randbetween, average, max, min, sum, count, counta, gcd, lcm, ceiling, floor, pmt, pv, fv, nper, rate.

### Execute Text Function
Executes text manipulation functions including: left, right, mid, len, upper, lower, trim, find, replace, concatenate, split, trimends, proper, search, substitute, startswith, endswith, char, unichar, encodeurl, encodehtml, plaintext, concat, ismatch, match, matchall, rept, fixed, dollar.

### Execute Logical Function
Executes logical functions including: and, or, not, if, isblank, isempty, isnumeric, iserror, switch, iferror, error, isblankorerror, istoday, isutctoday.

### Execute Date Time Function
Executes date/time functions including: now, today, year, month, day, hour, minute, second, dateadd, datediff, utcnow, utctoday, date, datetime, time, datevalue, datetimevalue, timevalue, weekday, edate, eomonth, weeknum, isoweeknum.

### Execute Conversion Function
Executes type conversion functions including: text, value, boolean, decimal, float, guid, astype.

### Execute Table Function
Executes table manipulation functions including: addcolumns, filter, sort, groupby, summarize, distinct, firstn, lastn, table, sequence, countrows, countif, first, last, index, shuffle.

### Execute Utility Function
Executes utility functions including: rand, randbetween, sequence, shuffle, sort, reverse, coalesce, collect, clear, remove, removeif, update, updateif, patch, parsejson, dec2hex, hex2dec, blank, rgba, colorvalue, colorfade, forall, with.

### Execute Advanced Table Function
Executes advanced table operations including: join, leftjoin, rightjoin, innerjoin, outerjoin, crossjoin, pivot, unpivot, transpose, lookup, xlookup, relate, unrelate, merge, append, union, intersect, except, dropcolumns, renamecolumns, showcolumns.

### Execute JSON Function
Executes JSON parsing and manipulation functions including: parsejson, json, formatjson, jsonextract, jsonpath, isvalidjson, jsonmerge, jsonarray, jsonobject, jsonkeys, jsonvalues.

### Execute Color Function
Executes color manipulation functions including: rgba, rgb, hsl, hsv, colorvalue, colorfade, colorbrightness, colorcontrast, colormix, colorinvert, hex2color, color2hex, colorred, colorgreen, colorblue, coloralpha.

### Execute Encoding Function
Executes data encoding and decoding functions including: base64encode, base64decode, urlencode, urldecode, htmlencode, htmldecode, xmlencode, xmldecode, utf8encode, utf8decode, hash, md5, sha1, sha256.

### Execute Statistics Function
Executes statistical analysis functions including: stdev, stdevp, var, varp, median, mode, percentile, quartile, correlation, covariance, regression, slope, intercept, rsquared, frequency, rank, percentrank, zscore, confidence.

### Check Value Type
Determines the Power Fx type of a given value.

### Coerce Value Type
Attempts to coerce a value to a specific Power Fx type.

### Extract Date Part
Extracts specific parts (year, month, day, hour, minute, second, weekday, dayofyear, quarter, weeknum, isoweeknum) from date/time values.

### Transform Text
Applies text transformations including: upper, lower, proper, trim, trimends, trimstart, trimend, reverse.

### Round Number
Rounds numbers using various methods: round, roundup, rounddown, trunc, int, ceiling, floor.

### Trigonometric Function
Executes trigonometric functions including: sin, cos, tan, asin, acos, atan, atan2, cot, acot, sec, csc, sinh, cosh, tanh.

### Search Text
Searches for text using methods: find, search, startswith, endswith, contains, exact.

### Evaluate Logical Condition
Evaluates logical conditions using: and, or, not, xor, nand, nor.

### Check Value Property
Checks specific properties of values: isblank, isempty, isnumeric, istext, islogical, iserror, isdate, istime, isblankorerror, istoday, isutctoday.

## Known Issues and Limitations
Some advanced Power Fx features may not be fully implemented in this version.