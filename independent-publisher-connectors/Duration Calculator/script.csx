using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class Script : ScriptBase
{
    public override async Task<HttpResponseMessage> ExecuteAsync()
    {
        try
        {
            this.Context.Logger?.LogInformation("=== DURATION CALCULATOR SCRIPT STARTED ===");
            this.Context.Logger?.LogInformation($"OperationId: {this.Context.OperationId}");

            JObject json = JObject.Parse(await this.Context.Request.Content.ReadAsStringAsync());
            string operationId = this.Context.OperationId;

            this.Context.Logger?.LogInformation($"Processing operation: {operationId}");
            this.Context.Logger?.LogInformation($"Request JSON: {json.ToString()}");

            var responseContent = operationId switch
            {
                "CalculateBusinessDays" => HandleCalculateBusinessDays(json),
                "CalculateAge" => HandleCalculateAge(json),
                "CalculateWorkingHours" => HandleCalculateWorkingHours(json),
                "ConvertTimeZones" => HandleConvertTimeZones(json),
                "CalculateProjectDuration" => HandleCalculateProjectDuration(json),
                "CalculatePayrollPeriods" => HandleCalculatePayrollPeriods(json),
                "GetTimeZoneInfo" => HandleGetTimeZoneInfo(json),
                "DeadlineCalculation" => HandleDeadlineCalculation(json),
                "CrossTzDuration" => HandleCrossTzDuration(json),
                "CalculateGlobalWorkingHours" => HandleCalculateGlobalWorkingHours(json),
                "CalculateMeetingTimes" => HandleCalculateMeetingTimes(json),
                "ValidateDstCalculations" => HandleValidateDstCalculations(json),
                "CalculateDstTransition" => HandleCalculateDstTransition(json),
                "GetDstHistory" => HandleGetDstHistory(json),
                "HealthCheck" or "Test" => CreateHealthCheckResponse(),
                _ => CreateErrorResponse("Unknown operation ID: " + operationId)
            };

            this.Context.Logger?.LogInformation("Operation completed successfully");

            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = CreateJsonContent(responseContent.ToString());
            return response;
        }
        catch (JsonException ex)
        {
            this.Context.Logger?.LogError(ex, "JSON parsing error");
            return CreateErrorHttpResponse(HttpStatusCode.BadRequest, "Invalid JSON in request body");
        }
        catch (Exception ex)
        {
            this.Context.Logger?.LogError(ex, "Unhandled error in ExecuteAsync");
            return CreateErrorHttpResponse(HttpStatusCode.InternalServerError, $"Internal server error: {ex.Message}");
        }
    }

    private JObject CreateHealthCheckResponse()
    {
        return new JObject
        {
            ["status"] = "OK",
            ["message"] = "Duration Calculator is working",
            ["timestamp"] = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ"),
            ["operationId"] = this.Context.OperationId
        };
    }

    private JObject CreateErrorResponse(string message)
    {
        return new JObject
        {
            ["error"] = message,
            ["timestamp"] = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ")
        };
    }

    private HttpResponseMessage CreateErrorHttpResponse(HttpStatusCode statusCode, string message)
    {
        var errorResponse = new HttpResponseMessage(statusCode);
        errorResponse.Content = CreateJsonContent(CreateErrorResponse(message).ToString());
        return errorResponse;
    }

    private JObject HandleCalculateBusinessDays(JObject json)
    {
        try
        {
            this.Context.Logger?.LogInformation("Processing CalculateBusinessDays");

            // Extract required parameters
            if (!DateTime.TryParse(json.GetValue("startDate")?.ToString(), out DateTime startDate))
            {
                return CreateErrorResponse("Invalid or missing startDate");
            }

            if (!DateTime.TryParse(json.GetValue("endDate")?.ToString(), out DateTime endDate))
            {
                return CreateErrorResponse("Invalid or missing endDate");
            }

            bool includeStartDate = json.GetValue("includeStartDate")?.ToObject<bool>() ?? true;

            // Calculate business days
            int businessDays = CalculateBusinessDaysBetweenDates(startDate, endDate, includeStartDate);

            return new JObject
            {
                ["businessDays"] = businessDays,
                ["startDate"] = startDate.ToString("yyyy-MM-dd"),
                ["endDate"] = endDate.ToString("yyyy-MM-dd"),
                ["includeStartDate"] = includeStartDate,
                ["calculation"] = $"Business days between {startDate:yyyy-MM-dd} and {endDate:yyyy-MM-dd}"
            };
        }
        catch (Exception ex)
        {
            this.Context.Logger?.LogError(ex, "Error in HandleCalculateBusinessDays");
            return CreateErrorResponse($"Error calculating business days: {ex.Message}");
        }
    }

    private JObject HandleCalculateAge(JObject json)
    {
        try
        {
            this.Context.Logger?.LogInformation("Processing CalculateAge");

            if (!DateTime.TryParse(json.GetValue("birthDate")?.ToString(), out DateTime birthDate))
            {
                return CreateErrorResponse("Invalid or missing birthDate");
            }

            DateTime asOfDate = DateTime.Today;
            if (json.ContainsKey("asOfDate") && json.GetValue("asOfDate") != null)
            {
                if (!DateTime.TryParse(json.GetValue("asOfDate").ToString(), out asOfDate))
                {
                    return CreateErrorResponse("Invalid asOfDate format");
                }
            }

            var age = CalculateAge(birthDate, asOfDate);

            return new JObject
            {
                ["years"] = age.Years,
                ["months"] = age.Months,
                ["days"] = age.Days,
                ["totalDays"] = (asOfDate - birthDate).Days,
                ["birthDate"] = birthDate.ToString("yyyy-MM-dd"),
                ["asOfDate"] = asOfDate.ToString("yyyy-MM-dd")
            };
        }
        catch (Exception ex)
        {
            this.Context.Logger?.LogError(ex, "Error in HandleCalculateAge");
            return CreateErrorResponse($"Error calculating age: {ex.Message}");
        }
    }

    private JObject HandleCalculateWorkingHours(JObject json)
    {
        try
        {
            this.Context.Logger?.LogInformation("Processing CalculateWorkingHours");

            if (!DateTime.TryParse(json.GetValue("startDateTime")?.ToString(), out DateTime startDateTime))
            {
                return CreateErrorResponse("Invalid or missing startDateTime");
            }

            if (!DateTime.TryParse(json.GetValue("endDateTime")?.ToString(), out DateTime endDateTime))
            {
                return CreateErrorResponse("Invalid or missing endDateTime");
            }

            // Default working hours (9 AM to 5 PM)
            int workingStartHour = json.GetValue("workingStartHour")?.ToObject<int>() ?? 9;
            int workingEndHour = json.GetValue("workingEndHour")?.ToObject<int>() ?? 17;

            var workingHours = CalculateWorkingHours(startDateTime, endDateTime, workingStartHour, workingEndHour);

            return new JObject
            {
                ["workingHours"] = workingHours,
                ["startDateTime"] = startDateTime.ToString("yyyy-MM-ddTHH:mm:ss"),
                ["endDateTime"] = endDateTime.ToString("yyyy-MM-ddTHH:mm:ss"),
                ["workingStartHour"] = workingStartHour,
                ["workingEndHour"] = workingEndHour
            };
        }
        catch (Exception ex)
        {
            return CreateErrorResponse($"Error calculating working hours: {ex.Message}");
        }
    }

    private JObject HandleConvertTimeZones(JObject json)
    {
        try
        {
            this.Context.Logger?.LogInformation("Processing ConvertTimeZones");

            if (!DateTime.TryParse(json.GetValue("dateTime")?.ToString(), out DateTime dateTime))
            {
                return CreateErrorResponse("Invalid or missing dateTime");
            }

            string sourceTimeZone = json.GetValue("sourceTimeZone")?.ToString() ?? "UTC";
            string targetTimeZone = json.GetValue("targetTimeZone")?.ToString() ?? "UTC";

            var convertedDateTime = ConvertTimeZone(dateTime, sourceTimeZone, targetTimeZone);

            return new JObject
            {
                ["originalDateTime"] = dateTime.ToString("yyyy-MM-ddTHH:mm:ss"),
                ["convertedDateTime"] = convertedDateTime.ToString("yyyy-MM-ddTHH:mm:ss"),
                ["sourceTimeZone"] = sourceTimeZone,
                ["targetTimeZone"] = targetTimeZone,
                ["offsetHours"] = (convertedDateTime - dateTime).TotalHours
            };
        }
        catch (Exception ex)
        {
            return CreateErrorResponse($"Error converting time zones: {ex.Message}");
        }
    }

    private JObject HandleCalculateProjectDuration(JObject json)
    {
        try
        {
            this.Context.Logger?.LogInformation("Processing CalculateProjectDuration");

            if (!DateTime.TryParse(json.GetValue("startDate")?.ToString(), out DateTime startDate))
            {
                return CreateErrorResponse("Invalid or missing startDate");
            }

            if (!DateTime.TryParse(json.GetValue("endDate")?.ToString(), out DateTime endDate))
            {
                return CreateErrorResponse("Invalid or missing endDate");
            }

            bool includeWeekends = json.GetValue("includeWeekends")?.ToObject<bool>() ?? false;

            var duration = CalculateProjectDuration(startDate, endDate, includeWeekends);

            return new JObject
            {
                ["totalDays"] = duration.TotalDays,
                ["workingDays"] = duration.WorkingDays,
                ["weekends"] = duration.Weekends,
                ["startDate"] = startDate.ToString("yyyy-MM-dd"),
                ["endDate"] = endDate.ToString("yyyy-MM-dd"),
                ["includeWeekends"] = includeWeekends
            };
        }
        catch (Exception ex)
        {
            return CreateErrorResponse($"Error calculating project duration: {ex.Message}");
        }
    }

    private JObject HandleCalculatePayrollPeriods(JObject json)
    {
        try
        {
            this.Context.Logger?.LogInformation("Processing CalculatePayrollPeriods");

            if (!DateTime.TryParse(json.GetValue("startDate")?.ToString(), out DateTime startDate))
            {
                return CreateErrorResponse("Invalid or missing startDate");
            }

            if (!DateTime.TryParse(json.GetValue("endDate")?.ToString(), out DateTime endDate))
            {
                return CreateErrorResponse("Invalid or missing endDate");
            }

            string frequency = json.GetValue("frequency")?.ToString() ?? "BiWeekly"; // Weekly, BiWeekly, Monthly

            var payrollPeriods = CalculatePayrollPeriods(startDate, endDate, frequency);

            return new JObject
            {
                ["payrollPeriods"] = JArray.FromObject(payrollPeriods),
                ["totalPeriods"] = payrollPeriods.Count,
                ["frequency"] = frequency,
                ["startDate"] = startDate.ToString("yyyy-MM-dd"),
                ["endDate"] = endDate.ToString("yyyy-MM-dd")
            };
        }
        catch (Exception ex)
        {
            return CreateErrorResponse($"Error calculating payroll periods: {ex.Message}");
        }
    }

    private JObject HandleGetTimeZoneInfo(JObject json)
    {
        try
        {
            this.Context.Logger?.LogInformation("Processing GetTimeZoneInfo");

            string timeZoneId = json.GetValue("timeZoneId")?.ToString() ?? "UTC";

            var timeZoneInfo = GetTimeZoneInfo(timeZoneId);

            return new JObject
            {
                ["timeZoneId"] = timeZoneInfo.Id,
                ["displayName"] = timeZoneInfo.DisplayName,
                ["standardName"] = timeZoneInfo.StandardName,
                ["daylightName"] = timeZoneInfo.DaylightName,
                ["baseUtcOffset"] = timeZoneInfo.BaseUtcOffset.TotalHours,
                ["supportsDaylightSavingTime"] = timeZoneInfo.SupportsDaylightSavingTime,
                ["currentOffset"] = timeZoneInfo.GetUtcOffset(DateTime.Now).TotalHours
            };
        }
        catch (Exception ex)
        {
            return CreateErrorResponse($"Error getting time zone info: {ex.Message}");
        }
    }

    private JObject HandleDeadlineCalculation(JObject json)
    {
        try
        {
            this.Context.Logger?.LogInformation("Processing DeadlineCalculation");

            if (!DateTime.TryParse(json.GetValue("deadlineDate")?.ToString(), out DateTime deadlineDate))
            {
                return CreateErrorResponse("Invalid or missing deadlineDate");
            }

            DateTime currentDate = json.ContainsKey("currentDate") && json.GetValue("currentDate") != null
                ? DateTime.Parse(json.GetValue("currentDate").ToString())
                : DateTime.Now;

            var timeUntilDeadline = CalculateTimeUntilDeadline(currentDate, deadlineDate);

            return new JObject
            {
                ["daysRemaining"] = timeUntilDeadline.Days,
                ["hoursRemaining"] = timeUntilDeadline.Hours,
                ["minutesRemaining"] = timeUntilDeadline.Minutes,
                ["totalDays"] = (int)timeUntilDeadline.TotalDays,
                ["totalHours"] = (int)timeUntilDeadline.TotalHours,
                ["isOverdue"] = timeUntilDeadline.TotalMilliseconds < 0,
                ["deadlineDate"] = deadlineDate.ToString("yyyy-MM-ddTHH:mm:ss"),
                ["currentDate"] = currentDate.ToString("yyyy-MM-ddTHH:mm:ss")
            };
        }
        catch (Exception ex)
        {
            return CreateErrorResponse($"Error calculating deadline: {ex.Message}");
        }
    }

    private JObject HandleCrossTzDuration(JObject json)
    {
        try
        {
            this.Context.Logger?.LogInformation("Processing CrossTzDuration");

            if (!DateTime.TryParse(json.GetValue("startDateTime")?.ToString(), out DateTime startDateTime))
            {
                return CreateErrorResponse("Invalid or missing startDateTime");
            }

            if (!DateTime.TryParse(json.GetValue("endDateTime")?.ToString(), out DateTime endDateTime))
            {
                return CreateErrorResponse("Invalid or missing endDateTime");
            }

            string startTimeZone = json.GetValue("startTimeZone")?.ToString() ?? "UTC";
            string endTimeZone = json.GetValue("endTimeZone")?.ToString() ?? "UTC";

            var duration = CalculateCrossTimeZoneDuration(startDateTime, endDateTime, startTimeZone, endTimeZone);

            return new JObject
            {
                ["duration"] = new JObject
                {
                    ["days"] = duration.Days,
                    ["hours"] = duration.Hours,
                    ["minutes"] = duration.Minutes,
                    ["totalHours"] = duration.TotalHours
                },
                ["startDateTime"] = startDateTime.ToString("yyyy-MM-ddTHH:mm:ss"),
                ["endDateTime"] = endDateTime.ToString("yyyy-MM-ddTHH:mm:ss"),
                ["startTimeZone"] = startTimeZone,
                ["endTimeZone"] = endTimeZone
            };
        }
        catch (Exception ex)
        {
            return CreateErrorResponse($"Error calculating cross-timezone duration: {ex.Message}");
        }
    }

    private JObject HandleCalculateGlobalWorkingHours(JObject json)
    {
        try
        {
            this.Context.Logger?.LogInformation("Processing CalculateGlobalWorkingHours");

            var timeZones = json.GetValue("timeZones")?.ToObject<List<string>>() ?? new List<string> { "UTC" };
            int workingStartHour = json.GetValue("workingStartHour")?.ToObject<int>() ?? 9;
            int workingEndHour = json.GetValue("workingEndHour")?.ToObject<int>() ?? 17;

            var globalWorkingHours = CalculateGlobalWorkingHours(timeZones, workingStartHour, workingEndHour);

            return new JObject
            {
                ["globalWorkingWindows"] = JArray.FromObject(globalWorkingHours),
                ["timeZones"] = JArray.FromObject(timeZones),
                ["workingStartHour"] = workingStartHour,
                ["workingEndHour"] = workingEndHour
            };
        }
        catch (Exception ex)
        {
            return CreateErrorResponse($"Error calculating global working hours: {ex.Message}");
        }
    }

    private JObject HandleCalculateMeetingTimes(JObject json)
    {
        try
        {
            this.Context.Logger?.LogInformation("Processing CalculateMeetingTimes");

            var timeZones = json.GetValue("timeZones")?.ToObject<List<string>>() ?? new List<string> { "UTC" };
            if (!DateTime.TryParse(json.GetValue("preferredTime")?.ToString(), out DateTime preferredTime))
            {
                return CreateErrorResponse("Invalid or missing preferredTime");
            }

            var meetingTimes = CalculateOptimalMeetingTimes(timeZones, preferredTime);

            return new JObject
            {
                ["optimalMeetingTimes"] = JArray.FromObject(meetingTimes),
                ["preferredTime"] = preferredTime.ToString("HH:mm"),
                ["timeZones"] = JArray.FromObject(timeZones)
            };
        }
        catch (Exception ex)
        {
            return CreateErrorResponse($"Error calculating meeting times: {ex.Message}");
        }
    }

    private JObject HandleValidateDstCalculations(JObject json)
    {
        try
        {
            this.Context.Logger?.LogInformation("Processing ValidateDstCalculations");

            if (!DateTime.TryParse(json.GetValue("dateTime")?.ToString(), out DateTime dateTime))
            {
                return CreateErrorResponse("Invalid or missing dateTime");
            }

            string timeZoneId = json.GetValue("timeZoneId")?.ToString() ?? "UTC";

            var dstInfo = ValidateDaylightSavingTime(dateTime, timeZoneId);

            return new JObject
            {
                ["isDaylightSavingTime"] = dstInfo.IsDaylightSavingTime,
                ["utcOffset"] = dstInfo.UtcOffset.TotalHours,
                ["timeZoneId"] = timeZoneId,
                ["dateTime"] = dateTime.ToString("yyyy-MM-ddTHH:mm:ss"),
                ["dstStartDate"] = dstInfo.DstStartDate?.ToString("yyyy-MM-dd"),
                ["dstEndDate"] = dstInfo.DstEndDate?.ToString("yyyy-MM-dd")
            };
        }
        catch (Exception ex)
        {
            return CreateErrorResponse($"Error validating DST calculations: {ex.Message}");
        }
    }

    private JObject HandleCalculateDstTransition(JObject json)
    {
        try
        {
            this.Context.Logger?.LogInformation("Processing CalculateDstTransition");

            if (!int.TryParse(json.GetValue("year")?.ToString(), out int year))
            {
                year = DateTime.Now.Year;
            }

            string timeZoneId = json.GetValue("timeZoneId")?.ToString() ?? "UTC";

            var dstTransitions = CalculateDstTransitions(year, timeZoneId);

            return new JObject
            {
                ["year"] = year,
                ["timeZoneId"] = timeZoneId,
                ["dstStartDate"] = dstTransitions.StartDate?.ToString("yyyy-MM-ddTHH:mm:ss"),
                ["dstEndDate"] = dstTransitions.EndDate?.ToString("yyyy-MM-ddTHH:mm:ss"),
                ["supportsDst"] = dstTransitions.SupportsDst
            };
        }
        catch (Exception ex)
        {
            return CreateErrorResponse($"Error calculating DST transitions: {ex.Message}");
        }
    }

    private JObject HandleGetDstHistory(JObject json)
    {
        try
        {
            this.Context.Logger?.LogInformation("Processing GetDstHistory");

            if (!int.TryParse(json.GetValue("startYear")?.ToString(), out int startYear))
            {
                startYear = DateTime.Now.Year - 5;
            }

            if (!int.TryParse(json.GetValue("endYear")?.ToString(), out int endYear))
            {
                endYear = DateTime.Now.Year;
            }

            string timeZoneId = json.GetValue("timeZoneId")?.ToString() ?? "UTC";

            var dstHistory = GetDstHistory(startYear, endYear, timeZoneId);

            return new JObject
            {
                ["timeZoneId"] = timeZoneId,
                ["startYear"] = startYear,
                ["endYear"] = endYear,
                ["dstHistory"] = JArray.FromObject(dstHistory)
            };
        }
        catch (Exception ex)
        {
            return CreateErrorResponse($"Error getting DST history: {ex.Message}");
        }
    }

    // Helper methods
    private int CalculateBusinessDaysBetweenDates(DateTime startDate, DateTime endDate, bool includeStartDate)
    {
        if (endDate < startDate)
            return 0;

        int businessDays = 0;
        DateTime current = includeStartDate ? startDate : startDate.AddDays(1);

        while (current <= endDate)
        {
            if (current.DayOfWeek != DayOfWeek.Saturday && current.DayOfWeek != DayOfWeek.Sunday)
            {
                businessDays++;
            }
            current = current.AddDays(1);
        }

        return businessDays;
    }

    private (int Years, int Months, int Days) CalculateAge(DateTime birthDate, DateTime asOfDate)
    {
        int years = asOfDate.Year - birthDate.Year;
        int months = asOfDate.Month - birthDate.Month;
        int days = asOfDate.Day - birthDate.Day;

        if (days < 0)
        {
            months--;
            days += DateTime.DaysInMonth(asOfDate.Year, asOfDate.Month == 1 ? 12 : asOfDate.Month - 1);
        }

        if (months < 0)
        {
            years--;
            months += 12;
        }

        return (years, months, days);
    }

    private double CalculateWorkingHours(DateTime startDateTime, DateTime endDateTime, int workingStartHour, int workingEndHour)
    {
        if (endDateTime <= startDateTime) return 0;

        double totalWorkingHours = 0;
        DateTime currentDate = startDateTime.Date;

        while (currentDate <= endDateTime.Date)
        {
            if (currentDate.DayOfWeek != DayOfWeek.Saturday && currentDate.DayOfWeek != DayOfWeek.Sunday)
            {
                DateTime dayStart = currentDate.AddHours(workingStartHour);
                DateTime dayEnd = currentDate.AddHours(workingEndHour);

                DateTime effectiveStart = startDateTime > dayStart ? startDateTime : dayStart;
                DateTime effectiveEnd = endDateTime < dayEnd ? endDateTime : dayEnd;

                if (effectiveStart < effectiveEnd)
                {
                    totalWorkingHours += (effectiveEnd - effectiveStart).TotalHours;
                }
            }
            currentDate = currentDate.AddDays(1);
        }

        return totalWorkingHours;
    }

    private DateTime ConvertTimeZone(DateTime dateTime, string sourceTimeZone, string targetTimeZone)
    {
        try
        {
            TimeZoneInfo sourceZone = TimeZoneInfo.FindSystemTimeZoneById(sourceTimeZone);
            TimeZoneInfo targetZone = TimeZoneInfo.FindSystemTimeZoneById(targetTimeZone);

            DateTime utcTime = TimeZoneInfo.ConvertTimeToUtc(dateTime, sourceZone);
            return TimeZoneInfo.ConvertTimeFromUtc(utcTime, targetZone);
        }
        catch
        {
            // Fallback to UTC if timezone conversion fails
            return dateTime;
        }
    }

    private (int TotalDays, int WorkingDays, int Weekends) CalculateProjectDuration(DateTime startDate, DateTime endDate, bool includeWeekends)
    {
        if (endDate < startDate) return (0, 0, 0);

        int totalDays = (int)(endDate - startDate).TotalDays + 1;
        int workingDays = 0;
        int weekends = 0;

        DateTime current = startDate;
        while (current <= endDate)
        {
            if (current.DayOfWeek == DayOfWeek.Saturday || current.DayOfWeek == DayOfWeek.Sunday)
            {
                weekends++;
            }
            else
            {
                workingDays++;
            }
            current = current.AddDays(1);
        }

        return (totalDays, workingDays, weekends);
    }

    private List<object> CalculatePayrollPeriods(DateTime startDate, DateTime endDate, string frequency)
    {
        var periods = new List<object>();
        DateTime current = startDate;

        while (current <= endDate)
        {
            DateTime periodEnd = frequency switch
            {
                "Weekly" => current.AddDays(6),
                "BiWeekly" => current.AddDays(13),
                "Monthly" => current.AddMonths(1).AddDays(-1),
                _ => current.AddDays(13) // Default to BiWeekly
            };

            if (periodEnd > endDate) periodEnd = endDate;

            periods.Add(new
            {
                periodNumber = periods.Count + 1,
                startDate = current.ToString("yyyy-MM-dd"),
                endDate = periodEnd.ToString("yyyy-MM-dd"),
                days = (int)(periodEnd - current).TotalDays + 1
            });

            current = frequency switch
            {
                "Weekly" => current.AddDays(7),
                "BiWeekly" => current.AddDays(14),
                "Monthly" => current.AddMonths(1),
                _ => current.AddDays(14)
            };
        }

        return periods;
    }

    private TimeZoneInfo GetTimeZoneInfo(string timeZoneId)
    {
        try
        {
            return TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
        }
        catch
        {
            return TimeZoneInfo.Utc; // Fallback to UTC
        }
    }

    private TimeSpan CalculateTimeUntilDeadline(DateTime currentDate, DateTime deadlineDate)
    {
        return deadlineDate - currentDate;
    }

    private TimeSpan CalculateCrossTimeZoneDuration(DateTime startDateTime, DateTime endDateTime, string startTimeZone, string endTimeZone)
    {
        try
        {
            DateTime startUtc = TimeZoneInfo.ConvertTimeToUtc(startDateTime, TimeZoneInfo.FindSystemTimeZoneById(startTimeZone));
            DateTime endUtc = TimeZoneInfo.ConvertTimeToUtc(endDateTime, TimeZoneInfo.FindSystemTimeZoneById(endTimeZone));

            return endUtc - startUtc;
        }
        catch
        {
            return endDateTime - startDateTime; // Fallback to simple calculation
        }
    }

    private List<object> CalculateGlobalWorkingHours(List<string> timeZones, int workingStartHour, int workingEndHour)
    {
        var workingWindows = new List<object>();
        DateTime baseDate = DateTime.Today;

        foreach (string timeZoneId in timeZones)
        {
            try
            {
                TimeZoneInfo tz = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
                DateTime localStart = baseDate.AddHours(workingStartHour);
                DateTime localEnd = baseDate.AddHours(workingEndHour);

                DateTime utcStart = TimeZoneInfo.ConvertTimeToUtc(localStart, tz);
                DateTime utcEnd = TimeZoneInfo.ConvertTimeToUtc(localEnd, tz);

                workingWindows.Add(new
                {
                    timeZone = timeZoneId,
                    localStart = localStart.ToString("HH:mm"),
                    localEnd = localEnd.ToString("HH:mm"),
                    utcStart = utcStart.ToString("HH:mm"),
                    utcEnd = utcEnd.ToString("HH:mm")
                });
            }
            catch
            {
                // Skip invalid time zones
            }
        }

        return workingWindows;
    }

    private List<object> CalculateOptimalMeetingTimes(List<string> timeZones, DateTime preferredTime)
    {
        var meetingTimes = new List<object>();

        foreach (string timeZoneId in timeZones)
        {
            try
            {
                TimeZoneInfo tz = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
                DateTime localTime = TimeZoneInfo.ConvertTime(preferredTime, tz);

                meetingTimes.Add(new
                {
                    timeZone = timeZoneId,
                    localTime = localTime.ToString("HH:mm"),
                    date = localTime.ToString("yyyy-MM-dd"),
                    isBusinessHour = localTime.Hour >= 9 && localTime.Hour <= 17
                });
            }
            catch
            {
                // Skip invalid time zones
            }
        }

        return meetingTimes;
    }

    private (bool IsDaylightSavingTime, TimeSpan UtcOffset, DateTime? DstStartDate, DateTime? DstEndDate) ValidateDaylightSavingTime(DateTime dateTime, string timeZoneId)
    {
        try
        {
            TimeZoneInfo tz = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
            bool isDst = tz.IsDaylightSavingTime(dateTime);
            TimeSpan offset = tz.GetUtcOffset(dateTime);

            // Get DST transition dates for the year
            var adjustmentRules = tz.GetAdjustmentRules();
            DateTime? dstStart = null;
            DateTime? dstEnd = null;

            foreach (var rule in adjustmentRules)
            {
                if (dateTime.Year >= rule.DateStart.Year && dateTime.Year <= rule.DateEnd.Year)
                {
                    dstStart = GetTransitionDate(dateTime.Year, rule.DaylightTransitionStart);
                    dstEnd = GetTransitionDate(dateTime.Year, rule.DaylightTransitionEnd);
                    break;
                }
            }

            return (isDst, offset, dstStart, dstEnd);
        }
        catch
        {
            return (false, TimeSpan.Zero, null, null);
        }
    }

    private (DateTime? StartDate, DateTime? EndDate, bool SupportsDst) CalculateDstTransitions(int year, string timeZoneId)
    {
        try
        {
            TimeZoneInfo tz = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);

            if (!tz.SupportsDaylightSavingTime)
            {
                return (null, null, false);
            }

            var adjustmentRules = tz.GetAdjustmentRules();
            foreach (var rule in adjustmentRules)
            {
                if (year >= rule.DateStart.Year && year <= rule.DateEnd.Year)
                {
                    DateTime? dstStart = GetTransitionDate(year, rule.DaylightTransitionStart);
                    DateTime? dstEnd = GetTransitionDate(year, rule.DaylightTransitionEnd);
                    return (dstStart, dstEnd, true);
                }
            }

            return (null, null, true);
        }
        catch
        {
            return (null, null, false);
        }
    }

    private List<object> GetDstHistory(int startYear, int endYear, string timeZoneId)
    {
        var history = new List<object>();

        for (int year = startYear; year <= endYear; year++)
        {
            var transitions = CalculateDstTransitions(year, timeZoneId);

            history.Add(new
            {
                year = year,
                supportsDst = transitions.SupportsDst,
                dstStartDate = transitions.StartDate?.ToString("yyyy-MM-dd"),
                dstEndDate = transitions.EndDate?.ToString("yyyy-MM-dd")
            });
        }

        return history;
    }

    private DateTime? GetTransitionDate(int year, TimeZoneInfo.TransitionTime transition)
    {
        try
        {
            if (transition.IsFixedDateRule)
            {
                return new DateTime(year, transition.Month, transition.Day, transition.TimeOfDay.Hour, transition.TimeOfDay.Minute, transition.TimeOfDay.Second);
            }
            else
            {
                // Calculate floating date rule (e.g., "second Sunday in March")
                DateTime firstDayOfMonth = new DateTime(year, transition.Month, 1);
                int daysToAdd = ((int)transition.DayOfWeek - (int)firstDayOfMonth.DayOfWeek + 7) % 7;
                DateTime firstOccurrence = firstDayOfMonth.AddDays(daysToAdd);
                DateTime transitionDate = firstOccurrence.AddDays((transition.Week - 1) * 7);

                return new DateTime(transitionDate.Year, transitionDate.Month, transitionDate.Day,
                    transition.TimeOfDay.Hour, transition.TimeOfDay.Minute, transition.TimeOfDay.Second);
            }
        }
        catch
        {
            return null;
        }
    }
}
