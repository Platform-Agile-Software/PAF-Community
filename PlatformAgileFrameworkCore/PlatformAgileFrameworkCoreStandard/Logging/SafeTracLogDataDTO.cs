using System;
// ReSharper disable InconsistentNaming

// ReSharper disable once CheckNamespace
namespace Delta.SafeTrac.Logging
{
    /// <summary>
    /// This is a serializer-independent Data Transfer Object (DTO) for SafeTrac logging data.
    /// Output format is JSON, although this DTO is independent of format. It can be consumed by
    /// something like Newtonsoft. Case is important on property names, in general. Data
    /// validation for properties is done outside this class, as properties are loaded.
    /// </summary>
    public class SafeTracLogDataDTO
    {
        /// <summary>
        /// Log message timestamp in ISO8601 format using GMT time zone if at all possible.
        /// Format is yyyy-mm-ddThh24:mi:ss,fffZ.  The “Z” is included when the timestamp is in GMT.
        /// ToDo: KRM - there should be no reason why this can't always be GMT, right?
        /// </summary>
        /// <remarks>
        /// <para>
        /// Example: 2018-06-08T18:20:24,533Z
        /// </para>
        /// <para>
        /// Required field.
        /// </para>
        /// </remarks>
        public string TimeStamp { get; set; }
        /// <summary>
        /// A unique identifier denoting a logical unit of work. ToDo: KRM is a 64-bit integer OK?
        /// </summary>
        /// <remarks>
        /// <para>
        /// Example: 9181154469895107
        /// </para>
        /// <para>
        /// Required field.
        /// </para>
        /// </remarks>
        public Int64 TranID { get; set; }

        /// <summary>
        /// Level of the log message.
        /// </summary>
        /// <remarks>
        /// <para>
        /// One of: "TRACE", "DEBUG", "INFO", "WARN", "ERROR", "FATAL" 
        /// </para>
        /// <para>
        /// Required field.
        /// </para>
        /// </remarks>
        public string Level { get; set; }
        /// <summary>
        /// Name of the program being logged.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Example: "myProg"
        /// </para>
        /// <para>
        /// Required field.
        /// </para>
        /// </remarks>
        public string Prog { get; set; }
        /// <summary>
        /// The class, package, or other logical division of code.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Example: "MyClass"
        /// </para>
        /// <para>
        /// Required field.
        /// </para>
        /// </remarks>
        public string Logger { get; set; }
        /// <summary>
        /// The procedure, function, method of the routine generating the message.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Example: "ProcessBags"
        /// </para>
        /// <para>
        /// Required field.
        /// </para>
        /// </remarks>
        public string Proc { get; set; }
        /// <summary>
        /// The actual log message.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Example: "My Informational text string"
        /// </para>
        /// <para>
        /// Required field.
        /// </para>
        /// </remarks>
        public string Message { get; set; }
        /// <summary>
        /// Airport code of the message.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Example: "MSP"
        /// </para>
        /// <para>
        /// Optional field.
        /// </para>
        /// </remarks>
        public string Station { get; set; }
        /// <summary>
        /// Airline of the message.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Example: "DL", "VS", "AM"
        /// </para>
        /// <para>
        /// Optional field.
        /// </para>
        /// </remarks>
        public string Airline { get; set; }
        /// <summary>
        /// Airline code of message which may not correlate to the actual carrier.
        /// Useful for codeshare data.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Example: "DL", "VS", "AM"
        /// </para>
        /// <para>
        /// Optional field.
        /// </para>
        /// </remarks>
        public string DLAirline { get; set; }
        /// <summary>
        /// Flight number - no leading 0's.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Example: 123
        /// </para>
        /// <para>
        /// Optional field.
        /// </para>
        /// </remarks>
        public int Flight { get; set; }
        /// <summary>
        /// DL codeshare flight number.  No leading zeros..
        /// </summary>
        /// <remarks>
        /// <para>
        /// Example: 9123
        /// </para>
        /// <para>
        /// Optional field.
        /// </para>
        /// </remarks>
        public int DLFlight { get; set; }
        /// <summary>
        /// Schedule departure time of flight in “yyymmdd” or “yyymmdd hh24miss” format.
        /// A date format is “ddmonyyyy” is not acceptable.  IE: 06JUN2018 is NOT OK.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Example: “20180607” or “20180607 145000”
        /// </para>
        /// <para>
        /// Optional field.
        /// </para>
        /// </remarks>
        public string Sched { get; set; }
        /// <summary>
        /// Arrival departure code.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Example: "A" or "D" ONLY.
        /// </para>
        /// <para>
        /// Optional field.
        /// </para>
        /// </remarks>
        public string Ad_code{ get; set; }
        /// <summary>
        /// Flight origin date in yyyymmdd format.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Example: "DL", "VS", "AM"
        /// </para>
        /// <para>
        /// Optional field.
        /// </para>
        /// </remarks>
        public string origin { get; set; }
        /// <summary>
        /// User name/id. ToDo: KRM presumably arbitrary alphanumeric data?
        /// </summary>
        /// <remarks>
        /// <para>
        /// Example: "123456"
        /// </para>
        /// <para>
        /// Optional field.
        /// </para>
        /// </remarks>
        public string User { get; set; }
        /// <summary>
        /// Scanner, belt loader, computer name or any other ID that uniquely
        /// identies the device.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Example: Arbitrary alphanumeric string.
        /// </para>
        /// <para>
        /// Optional field.
        /// </para>
        /// </remarks>
        public string DeviceID { get; set; }
        /// <summary>
        /// Bag tag, awb, ballast id, mail or any other item that is being worked.
        /// ToDo: KRM Is this arbitrary alphanumeric data?
        /// </summary>
        /// <remarks>
        /// <para>
        /// Example: "6006123456"
        /// </para>
        /// <para>
        /// Optional field.
        /// </para>
        /// </remarks>
        public string AcmdtyID { get; set; }
        /// <summary>
        /// Container id of ULD or cart.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Example: "AKE12345DL"
        /// </para>
        /// <para>
        /// Optional field.
        /// </para>
        /// </remarks>
        public string cntnrID { get; set; }
        /// <summary>
        /// Departure airport. Useful for leg based messages.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Example: "MSP"
        /// </para>
        /// <para>
        /// Optional field.
        /// </para>
        /// </remarks>
        public string dprtAirport { get; set; }
        /// <summary>
        /// Arrival airport. Useful for leg based messages.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Example: "ATL"
        /// </para>
        /// <para>
        /// Optional field.
        /// </para>
        /// </remarks>
        public string arrAirport { get; set; }
        /// <summary>
        /// Elapsed time in milliseconds.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Example: "NNNNms"
        /// ToDo: KRM - Is this just an integer or is the "ms" suffix necessary.
        /// </para>
        /// <para>
        /// Optional field.
        /// </para>
        /// </remarks>
        public string Elapsed { get; set; }
        /// <summary>
        /// Nose number of aircraft. 6 characters long. Leading zeros are required.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Example: "003300"
        /// </para>
        /// <para>
        /// Optional field.
        /// </para>
        /// </remarks>
        public string NoseNum { get; set; }
        /// <summary>
        /// Database column or other element being updated.
        /// ToDo: KRM - Will we have this information on the mobile or is this injected somehow along the way?
        /// </summary>
        /// <remarks>
        /// <para>
        /// Example: "Ld_auth_ind"
        /// </para>
        /// <para>
        /// Optional field.
        /// </para>
        /// </remarks>
        public string itemName { get; set; }
        /// <summary>
        /// Previous value if updating data.
        /// ToDo: KRM What is this?
        /// </summary>
        /// <remarks>
        /// <para>
        /// Example: "N"
        /// </para>
        /// <para>
        /// Optional field.
        /// </para>
        /// </remarks>
        public string oldValue { get; set; }
        /// <summary>
        /// Value being stored.
        /// ToDo: KRM What is this?
        /// </summary>
        /// <remarks>
        /// <para>
        /// Example: "Y"
        /// </para>
        /// <para>
        /// Optional field.
        /// ToDo: KRM Presumably this is required if oldValue is specified?
        /// </para>
        /// </remarks>
        public string newValue { get; set; }
        /// <summary>
        /// Event code or string that is causing the work.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Example: "4101","ON","OFF" etc
        /// </para>
        /// <para>
        /// Optional field.
        /// </para>
        /// </remarks>
        public string Event { get; set; }
        /// <summary>
        /// A single key element describing the flight.
        /// Format is:  Airline.flightNumber.OriginDate.DepartureStation.arrivalStation.
        /// Flight number is not zero filled.  Origin date of flight, not scheduled.
        /// ToDo: KRM - Last sentence makes no sense. Please clarify.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Example: "DL.123.20160625.ATL.MSP"
        /// </para>
        /// <para>
        /// Optional field.
        /// </para>
        /// </remarks>
        public string FlightID { get; set; }
   }
}
