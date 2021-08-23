/**
 * High Five
 * The OpenAPI specification for High Five's controllers
 *
 * OpenAPI spec version: 0.0.1
 *
 *
 * NOTE: This class is auto generated by the swagger code generator program.
 * https://github.com/swagger-api/swagger-codegen.git
 * Do not edit the class manually.
 */

export interface AnalyzedImageMetaData {
  id?: string;
  /**
   * The date-time notation as defined by RFC 3339, section 5.6. e.g. 2017-07-21T17:32:28Zring
   */
  dateAnalyzed?: Date;
  url?: string;
  imageId?: string;
  pipelineId?: string;
}
