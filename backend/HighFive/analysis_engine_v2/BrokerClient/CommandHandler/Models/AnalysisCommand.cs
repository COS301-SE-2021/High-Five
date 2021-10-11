using System.Text;
using analysis_engine.BrokerClient.CommandHandler.Models.commandbody;
using analysis_engine.BrokerClient.ResourceUsageCollector.Models;
using Newtonsoft.Json.Linq;

namespace analysis_engine.BrokerClient.CommandHandler.Models
{
    public class AnalysisCommand
    {
        /// <summary>
        /// Gets or Sets CommandId
        /// </summary>
        public string CommandId { get; set; }

        /// <summary>
        /// Gets or Sets MediaType
        /// </summary>
        public string CommandType { get; set; }

        /// <summary>
        /// Gets or Sets UserId
        /// </summary>
        public string UserId { get; set; }
        
        /// <summary>
        /// Gets or Sets Body
        /// </summary>
        public JObject Body { get; set; }
        
        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class AnalysisCommand {\n");
            sb.Append("  CommandId: ").Append(CommandId).Append("\n");
            sb.Append("  CommandType: ").Append(CommandType).Append("\n");
            sb.Append("  Body: ").Append(Body).Append("\n");
            sb.Append("  UserId: ").Append(UserId).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }

        /// <summary>
        /// Returns the JSON string presentation of the object
        /// </summary>
        /// <returns>JSON string presentation of the object</returns>
        public string ToJson()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this, Newtonsoft.Json.Formatting.Indented);
        }

        /// <summary>
        /// Returns true if objects are equal
        /// </summary>
        /// <param name="obj">Object to be compared</param>
        /// <returns>Boolean</returns>
        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((AnalysisCommand)obj);
        }

        /// <summary>
        /// Returns true if AnalyzeImageRequest instances are equal
        /// </summary>
        /// <param name="other">Instance of AnalyzeImageRequest to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(AnalysisCommand other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;

            return
                (
                    CommandId == other.CommandId ||
                    CommandId != null &&
                    CommandId.Equals(other.CommandId)
                ) &&
                (
                    CommandType == other.CommandType ||
                    CommandType != null &&
                    CommandType.Equals(other.CommandType)
                ) &&
                (
                    Body == other.Body ||
                    Body != null &&
                    Body.Equals(other.Body)
                ) &&
                (
                    UserId == other.UserId ||
                    UserId != null &&
                    UserId.Equals(other.UserId)
                );
        }

        /// <summary>
        /// Gets the hash code
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                var hashCode = 41;
                // Suitable nullity checks etc, of course :)
                    if (CommandId != null)
                    hashCode = hashCode * 59 + CommandId.GetHashCode();
                    if (CommandType != null)
                    hashCode = hashCode * 59 + CommandType.GetHashCode();
                    if (Body != null) 
                    hashCode = hashCode * 59 + Body.GetHashCode();
                    if (UserId != null) 
                    hashCode = hashCode * 59 + UserId.GetHashCode();
                return hashCode;
            }
        }

        #region Operators
        #pragma warning disable 1591

        public static bool operator ==(AnalysisCommand left, AnalysisCommand right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(AnalysisCommand left, AnalysisCommand right)
        {
            return !Equals(left, right);
        }

        #pragma warning restore 1591
        #endregion Operators
    }
}