/*
 * High Five
 *
 * The OpenAPI specification for High Five's controllers
 *
 * The version of the OpenAPI document: 0.0.1
 * 
 * Generated by: https://openapi-generator.tech
 */

using System;
using System.Text;

namespace broker_analysis_client.Client.Models
{ 
    /// <summary>
    /// 
    /// </summary>
    public class AnalyzedVideoMetaData : IEquatable<AnalyzedVideoMetaData>
    {
        /// <summary>
        /// Gets or Sets Id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The date-time notation as defined by RFC 3339, section 5.6. e.g. 2017-07-21T17:32:28Zring
        /// </summary>
        /// <value>The date-time notation as defined by RFC 3339, section 5.6. e.g. 2017-07-21T17:32:28Zring</value>
        public DateTime DateAnalyzed { get; set; }

        /// <summary>
        /// Gets or Sets Url
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Gets or Sets VideoId
        /// </summary>
        public string VideoId { get; set; }

        /// <summary>
        /// Gets or Sets PipelineId
        /// </summary>
        public string PipelineId { get; set; }

        /// <summary>
        /// Gets or Sets Thumbnail
        /// </summary>
        public string Thumbnail { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class AnalyzedVideoMetaData {\n");
            sb.Append("  Id: ").Append(Id).Append("\n");
            sb.Append("  DateAnalyzed: ").Append(DateAnalyzed).Append("\n");
            sb.Append("  Url: ").Append(Url).Append("\n");
            sb.Append("  VideoId: ").Append(VideoId).Append("\n");
            sb.Append("  PipelineId: ").Append(PipelineId).Append("\n");
            sb.Append("  Thumbnail: ").Append(Thumbnail).Append("\n");
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
            return obj.GetType() == GetType() && Equals((AnalyzedVideoMetaData)obj);
        }

        /// <summary>
        /// Returns true if AnalyzedVideoMetaData instances are equal
        /// </summary>
        /// <param name="other">Instance of AnalyzedVideoMetaData to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(AnalyzedVideoMetaData other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;

            return 
                (
                    Id == other.Id ||
                    Id != null &&
                    Id.Equals(other.Id)
                ) && 
                (
                    DateAnalyzed == other.DateAnalyzed ||
                    DateAnalyzed != null &&
                    DateAnalyzed.Equals(other.DateAnalyzed)
                ) && 
                (
                    Url == other.Url ||
                    Url != null &&
                    Url.Equals(other.Url)
                ) && 
                (
                    VideoId == other.VideoId ||
                    VideoId != null &&
                    VideoId.Equals(other.VideoId)
                ) && 
                (
                    PipelineId == other.PipelineId ||
                    PipelineId != null &&
                    PipelineId.Equals(other.PipelineId)
                ) && 
                (
                    Thumbnail == other.Thumbnail ||
                    Thumbnail != null &&
                    Thumbnail.Equals(other.Thumbnail)
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
                    if (Id != null)
                    hashCode = hashCode * 59 + Id.GetHashCode();
                    if (DateAnalyzed != null)
                    hashCode = hashCode * 59 + DateAnalyzed.GetHashCode();
                    if (Url != null)
                    hashCode = hashCode * 59 + Url.GetHashCode();
                    if (VideoId != null)
                    hashCode = hashCode * 59 + VideoId.GetHashCode();
                    if (PipelineId != null)
                    hashCode = hashCode * 59 + PipelineId.GetHashCode();
                    if (Thumbnail != null)
                    hashCode = hashCode * 59 + Thumbnail.GetHashCode();
                return hashCode;
            }
        }

        #region Operators
        #pragma warning disable 1591

        public static bool operator ==(AnalyzedVideoMetaData left, AnalyzedVideoMetaData right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(AnalyzedVideoMetaData left, AnalyzedVideoMetaData right)
        {
            return !Equals(left, right);
        }

        #pragma warning restore 1591
        #endregion Operators
    }
}
