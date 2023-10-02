using TransactionManager.Service.APILoggingService;
using TransactionManager.Models.DBModels;
using Newtonsoft.Json;
namespace TransactionManager.Response
{
	public class GenericResponseDTO<T1, T2>
	{
        public string api = "UNKNOWN";
        public int HttpStatusCode { get; set; } = StatusCodes.Status500InternalServerError; // Default to 200 OK
        private string user = "UNKNOWN";
        private T1 requestParameters = default(T1);
        public string Message { get; set; } = "Internal Server Error";
        public T2 Data { get; set; } = default(T2);
        public string timestamp { get; set; } = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time")).ToString("dd/MM/yyyy HH:mm:ss");
        public GenericResponseDTO(DatabaseContext dbContext, string apiName, string userOrState, T1 parameters, int statusCodes, string message, DateTime timestamp, T2 data = default(T2))
        {
            this.api = apiName;
            this.user = userOrState;
            this.HttpStatusCode = statusCodes;
            this.Message = message;
            this.requestParameters = parameters;
            this.timestamp = TimeZoneInfo.ConvertTimeFromUtc(timestamp, TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time")).ToString("dd/MM/yyyy HH:mm:ss");
            this.Data = data;
            string requestBody = "";
            if (this.requestParameters != null)
            {
                if (typeof(T1) == typeof(string) && this.requestParameters is string reqParams && !string.IsNullOrEmpty(reqParams))
                {
                    requestBody = reqParams;
                }
                else
                {
                    requestBody = JsonConvert.SerializeObject(this.requestParameters);
                    if (requestBody.Length > 65535)
                    {
                        requestBody = "Please check console.log instead. Request body too long to save to DB!";
                    }
                }
            }
            string logData = "";
            if (this.Data != null)
            {
                if (typeof(T2) == typeof(string) && this.Data is string strData && !string.IsNullOrEmpty(strData))
                {
                    logData = strData;
                }
                else if (typeof(T2) == typeof(int))
                {
                    logData = ((int)(object)this.Data).ToString();
                }
                else if (this.Data.GetType().ToString().StartsWith("<>__AnonType0`"))
                {
                    logData = this.Data.GetType().ToString();
                }
                else if (typeof(T2) == typeof(Exception))
                {
                    throw new InvalidDataException("DO NOT USE EXCEPTION FOR T2!");
                }
                else
                {
                    logData = JsonConvert.SerializeObject((typeof(T2) == typeof(Exception)) ? ((Exception)(object)this.Data) : this.Data);
                    if (logData.Length > 65535)
                    {
                        logData = "Please check logger/debugger instead. LogData too long to save into DB!";
                    }
                }
            }
            APILogsService.saveToDb(dbContext, apiName, userOrState, requestBody, statusCodes, message, TimeZoneInfo.ConvertTimeFromUtc(timestamp, TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time")), logData);
        }
	}
}

