using System.Net;
using System.Text.Json.Serialization;

namespace BlogPlatform.Models
{
    public class BaseResponseModel
    {
        public bool IsSuccess
        {
            get
            {
                if (Errors.Count() > 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
        public List<string> Errors { get; set; } = new List<string>();

        [JsonIgnore]
        public HttpStatusCode HttpStatusCode { get; set; } = HttpStatusCode.OK;

        public void AddErrorWithStatusCode(string errorText, HttpStatusCode httpStatusCode) 
        { 
            Errors.Add(errorText);
            HttpStatusCode = httpStatusCode;
        }
    }
}
