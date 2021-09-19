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
    public class GetUnreviewedToolsResponse : IEquatable<GetUnreviewedToolsResponse>
    {
        /// <summary>
        /// Gets or Sets UnreviewedTools
        /// </summary>
        [DataMember(Name="unreviewedTools", EmitDefaultValue=false)]
        public List<UnreviewedTool> UnreviewedTools { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class GetUnreviewedToolsResponse {\n");
            sb.Append("  UnreviewedTools: ").Append(UnreviewedTools).Append("\n");
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
            return obj.GetType() == GetType() && Equals((GetUnreviewedToolsResponse)obj);
        }

        /// <summary>
        /// Returns true if GetUnreviewedToolsResponse instances are equal
        /// </summary>
        /// <param name="other">Instance of GetUnreviewedToolsResponse to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(GetUnreviewedToolsResponse other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;

            return 
                (
                    UnreviewedTools == other.UnreviewedTools ||
                    UnreviewedTools != null &&
                    other.UnreviewedTools != null &&
                    UnreviewedTools.SequenceEqual(other.UnreviewedTools)
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
                    if (UnreviewedTools != null)
                    hashCode = hashCode * 59 + UnreviewedTools.GetHashCode();
                return hashCode;
            }
        }

        #region Operators
        #pragma warning disable 1591

        public static bool operator ==(GetUnreviewedToolsResponse left, GetUnreviewedToolsResponse right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(GetUnreviewedToolsResponse left, GetUnreviewedToolsResponse right)
        {
            return !Equals(left, right);
        }

        #pragma warning restore 1591
        #endregion Operators
    }
}
