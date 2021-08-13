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
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Org.OpenAPITools.Converters;

namespace Org.OpenAPITools.Models
{ 
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public class AnalyzedImageMetaData : IEquatable<AnalyzedImageMetaData>
    {
        /// <summary>
        /// Gets or Sets Id
        /// </summary>
        [DataMember(Name="id", EmitDefaultValue=false)]
        public string Id { get; set; }

        /// <summary>
        /// The date-time notation as defined by RFC 3339, section 5.6. e.g. 2017-07-21T17:32:28Zring
        /// </summary>
        /// <value>The date-time notation as defined by RFC 3339, section 5.6. e.g. 2017-07-21T17:32:28Zring</value>
        [DataMember(Name="dateAnalyzed", EmitDefaultValue=false)]
        public DateTime DateAnalyzed { get; set; }

        /// <summary>
        /// Gets or Sets Url
        /// </summary>
        [DataMember(Name="url", EmitDefaultValue=false)]
        public string Url { get; set; }

        /// <summary>
        /// Gets or Sets ImageId
        /// </summary>
        [DataMember(Name="imageId", EmitDefaultValue=false)]
        public string ImageId { get; set; }

        /// <summary>
        /// Gets or Sets PipelineId
        /// </summary>
        [DataMember(Name="pipelineId", EmitDefaultValue=false)]
        public string PipelineId { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class AnalyzedImageMetaData {\n");
            sb.Append("  Id: ").Append(Id).Append("\n");
            sb.Append("  DateAnalyzed: ").Append(DateAnalyzed).Append("\n");
            sb.Append("  Url: ").Append(Url).Append("\n");
            sb.Append("  ImageId: ").Append(ImageId).Append("\n");
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
            return obj.GetType() == GetType() && Equals((AnalyzedImageMetaData)obj);
        }

        /// <summary>
        /// Returns true if AnalyzedImageMetaData instances are equal
        /// </summary>
        /// <param name="other">Instance of AnalyzedImageMetaData to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(AnalyzedImageMetaData other)
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
                    ImageId == other.ImageId ||
                    ImageId != null &&
                    ImageId.Equals(other.ImageId)
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
                    if (Id != null)
                    hashCode = hashCode * 59 + Id.GetHashCode();
                    if (DateAnalyzed != null)
                    hashCode = hashCode * 59 + DateAnalyzed.GetHashCode();
                    if (Url != null)
                    hashCode = hashCode * 59 + Url.GetHashCode();
                    if (ImageId != null)
                    hashCode = hashCode * 59 + ImageId.GetHashCode();
                    if (PipelineId != null)
                    hashCode = hashCode * 59 + PipelineId.GetHashCode();
                return hashCode;
            }
        }

        #region Operators
        #pragma warning disable 1591

        public static bool operator ==(AnalyzedImageMetaData left, AnalyzedImageMetaData right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(AnalyzedImageMetaData left, AnalyzedImageMetaData right)
        {
            return !Equals(left, right);
        }

        #pragma warning restore 1591
        #endregion Operators
    }
}