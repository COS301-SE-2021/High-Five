using System.Text;

namespace analysis_engine.BrokerClient.ResourceUsageCollector.Models
{
    public class ServerInformation
    {
        // <summary>
        /// Gets or Sets ServerIp
        /// </summary>
        public string ServerIp { get; set; }

        /// <summary>
        /// Gets or Sets ServerPort
        /// </summary>
        public string ServerPort { get; set; }
        
        /// <summary>
        /// Gets or Sets ServerId
        /// </summary>
        public string ServerId { get; set; }
        
        /// <summary>
        /// Gets or Sets Credentials
        /// </summary>
        public string Credentials { get; set; }
        
        /// <summary>
        /// Gets or Sets Timestamp
        /// </summary>
        public long Timestamp { get; set; }
        
        /// <summary>
        /// Gets or Sets Usage
        /// </summary>
        public PerformanceInfo Usage { get; set; }
        
        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class ServerInformation {\n");
            sb.Append("  ServerIp: ").Append(ServerIp).Append("\n");
            sb.Append("  ServerPort: ").Append(ServerPort).Append("\n");
            sb.Append("  ServerId: ").Append(ServerId).Append("\n");
            sb.Append("  Credentials: ").Append(Credentials).Append("\n");
            sb.Append("  Timestamp: ").Append(Timestamp).Append("\n");
            sb.Append("  Usage: ").Append(Usage).Append("\n");
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
            return obj.GetType() == GetType() && Equals((ServerInformation)obj);
        }

        /// <summary>
        /// Returns true if ServerInformation instances are equal
        /// </summary>
        /// <param name="other">Instance of ServerInformation to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(ServerInformation other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;

            return 
                (
                    ServerId == other.ServerId ||
                    ServerId != null &&
                    ServerId.Equals(other.ServerId)
                ) && 
                (
                    ServerIp == other.ServerIp ||
                    ServerIp != null &&
                    ServerIp.Equals(other.ServerIp)
                ) && 
                (
                    ServerPort == other.ServerPort ||
                    ServerPort != null &&
                    ServerPort.Equals(other.ServerPort)
                ) && 
                (
                    Credentials == other.Credentials ||
                    Credentials != null &&
                    Credentials.Equals(other.Credentials)
                ) && 
                (
                    Usage == other.Usage ||
                    Usage != null &&
                    Usage.Equals(other.Usage)
                ) && 
                (
                    Timestamp == other.Timestamp ||
                    Timestamp.Equals(other.Timestamp)
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
                    if (ServerId != null)
                    hashCode = hashCode * 59 + ServerId.GetHashCode();
                    if (ServerIp != null)
                    hashCode = hashCode * 59 + ServerIp.GetHashCode();
                    if (ServerPort != null)
                    hashCode = hashCode * 59 + ServerPort.GetHashCode();
                    if (Credentials != null)
                    hashCode = hashCode * 59 + Credentials.GetHashCode();
                    if (Usage != null)
                    hashCode = hashCode * 59 + Usage.GetHashCode();
                    if (Timestamp != null)
                    hashCode = hashCode * 59 + Timestamp.GetHashCode();
                return hashCode;
            }
        }

        #region Operators
        #pragma warning disable 1591

        public static bool operator ==(ServerInformation left, ServerInformation right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ServerInformation left, ServerInformation right)
        {
            return !Equals(left, right);
        }

        #pragma warning restore 1591
        #endregion Operators
    }
}