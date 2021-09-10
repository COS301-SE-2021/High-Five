using System.Text;
using analysis_engine.BrokerClient.ResourceUsageCollector.Models;

namespace analysis_engine.BrokerClient.CommandHandler.Models
{
    public class AnalysisCommand
    {
        /// <summary>
        /// Gets or Sets CpuUsage
        /// </summary>
        public string CommandId { get; set; }

        /// <summary>
        /// Gets or Sets CpuUsage
        /// </summary>
        public string MediaType { get; set; }
        
        /// <summary>
        /// Gets or Sets CpuUsage
        /// </summary>
        public string MediaId { get; set; }
        
        /// <summary>
        /// Gets or Sets CpuUsage
        /// </summary>
        public string PipelineId { get; set; }
        
        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class AnalysisCommand {\n");
            sb.Append("  CommandId: ").Append(CommandId).Append("\n");
            sb.Append("  MediaType: ").Append(MediaType).Append("\n");
            sb.Append("  MediaId: ").Append(MediaId).Append("\n");
            sb.Append("  PipelineId: ").Append(PipelineId).Append("\n");
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
            return obj.GetType() == GetType() && Equals((PerformanceInfo)obj);
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
                    MediaType == other.MediaType ||
                    MediaType != null &&
                    MediaType.Equals(other.MediaType)
                ) &&
                (
                    MediaId == other.MediaId ||
                    MediaId != null &&
                    MediaId.Equals(other.MediaId)
                ) &&
                (
                    PipelineId == other.PipelineId ||
                    PipelineId != null &&
                    PipelineId.Equals(other.PipelineId)
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
                    if (MediaType != null)
                    hashCode = hashCode * 59 + MediaType.GetHashCode();
                    if (MediaId != null)
                    hashCode = hashCode * 59 + MediaId.GetHashCode();
                    if (PipelineId != null)
                    hashCode = hashCode * 59 + PipelineId.GetHashCode();
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