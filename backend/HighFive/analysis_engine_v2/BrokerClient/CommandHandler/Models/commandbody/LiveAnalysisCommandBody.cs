using System.Text;

namespace analysis_engine.BrokerClient.CommandHandler.Models.commandbody
{
    public class LiveAnalysisCommandBody : CommandBody
    {
        /// <summary>
        /// Gets or Sets StreamId
        /// </summary>
        public string StreamId { get; set; }
        
        /// <summary>
        /// Gets or Sets PublishLink
        /// </summary>
        public string PublishLink { get; set; }
        
        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class StoredMediaCommandBody {\n");
            sb.Append("  StreamId: ").Append(StreamId).Append("\n");
            sb.Append("  PublishLink: ").Append(PublishLink).Append("\n");
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
            return obj.GetType() == GetType() && Equals((LiveAnalysisCommandBody)obj);
        }

        /// <summary>
        /// Returns true if AnalyzeImageRequest instances are equal
        /// </summary>
        /// <param name="other">Instance of AnalyzeImageRequest to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(LiveAnalysisCommandBody other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;

            return
                (
                    StreamId == other.StreamId ||
                    StreamId != null &&
                    StreamId.Equals(other.StreamId)
                ) &&
                (
                    PublishLink == other.PublishLink ||
                    PublishLink != null &&
                    PublishLink.Equals(other.PublishLink)
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
                    if (PublishLink != null)
                    hashCode = hashCode * 59 + PublishLink.GetHashCode();
                    if (StreamId != null)
                    hashCode = hashCode * 59 + StreamId.GetHashCode();
                    return hashCode;
            }
        }

        #region Operators
        #pragma warning disable 1591

        public static bool operator ==(LiveAnalysisCommandBody left, LiveAnalysisCommandBody right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(LiveAnalysisCommandBody left, LiveAnalysisCommandBody right)
        {
            return !Equals(left, right);
        }

        #pragma warning restore 1591
        #endregion Operators
    }
}