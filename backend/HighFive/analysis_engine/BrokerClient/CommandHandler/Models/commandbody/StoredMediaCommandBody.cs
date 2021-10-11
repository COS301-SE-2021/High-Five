using System.Data;
using System.Text;

namespace analysis_engine.BrokerClient.CommandHandler.Models.commandbody
{
    public class StoredMediaCommandBody : CommandBody
    {
        /// <summary>
        /// Gets or Sets MediaId
        /// </summary>
        public string MediaId { get; set; }
        
        /// <summary>
        /// Gets or Sets PipelineId
        /// </summary>
        public string PipelineId { get; set; }
        
        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class StoredMediaCommandBody {\n");
            sb.Append("  MediaId: ").Append(MediaId).Append("\n");
            sb.Append("  PipelineId: ").Append(PipelineId).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }

        /// <summary>
        /// Returns the JSON string presentation of the object
        /// </summary>
        /// <returns>JSON string presentation of the object</returns>
        public override string ToJson()
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
            return obj.GetType() == GetType() && Equals((StoredMediaCommandBody)obj);
        }

        /// <summary>
        /// Returns true if AnalyzeImageRequest instances are equal
        /// </summary>
        /// <param name="other">Instance of AnalyzeImageRequest to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(StoredMediaCommandBody other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;

            return
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
                    if (PipelineId != null)
                    hashCode = hashCode * 59 + PipelineId.GetHashCode();
                    if (MediaId != null)
                    hashCode = hashCode * 59 + MediaId.GetHashCode();
                    return hashCode;
            }
        }

        #region Operators
        #pragma warning disable 1591

        public static bool operator ==(StoredMediaCommandBody left, StoredMediaCommandBody right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(StoredMediaCommandBody left, StoredMediaCommandBody right)
        {
            return !Equals(left, right);
        }

        #pragma warning restore 1591
        #endregion Operators
    }
}