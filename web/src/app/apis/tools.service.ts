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
 *//* tslint:disable:no-unused-variable member-ordering */

import {Inject, Injectable, Optional} from '@angular/core';
import {
  HttpClient, HttpHeaders, HttpParams,
  HttpResponse, HttpEvent
} from '@angular/common/http';
import {CustomHttpUrlEncodingCodec} from '../encoder';

import {Observable} from 'rxjs';

import {DeleteToolRequest} from '../models/deleteToolRequest';
import {EmptyObject} from '../models/emptyObject';
import {GetAllToolsResponse} from '../models/getAllToolsResponse';
import {GetToolFilesRequest} from '../models/getToolFilesRequest';
import {GetToolFilesResponse} from '../models/getToolFilesResponse';
import {GetToolMetaDataTypes} from '../models/getToolMetaDataTypes';
import {GetToolTypesResponse} from '../models/getToolTypesResponse';
import {Tool} from '../models/tool';

import {BASE_PATH, COLLECTION_FORMATS} from '../variables';
import {Configuration} from '../configuration';
import {environment} from '../../environments/environment';


@Injectable()
export class ToolsService {

  protected basePath = environment.apiEndpoint;
  public defaultHeaders = new HttpHeaders().set('Authorization', 'Bearer ' + JSON.parse(sessionStorage.getItem(sessionStorage.key(0))).secret);
  public configuration = new Configuration();

  constructor(protected httpClient: HttpClient, @Optional() @Inject(BASE_PATH) basePath: string, @Optional() configuration: Configuration) {
    if (basePath) {
      this.basePath = basePath;
    }
    if (configuration) {
      this.configuration = configuration;
      this.basePath = basePath || configuration.basePath || this.basePath;
    }
  }

  /**
   * @param consumes string[] mime-types
   * @return true: consumes contains 'multipart/form-data', false: otherwise
   */
  private canConsumeForm(consumes: string[]): boolean {
    const form = 'multipart/form-data';
    for (const consume of consumes) {
      if (form === consume) {
        return true;
      }
    }
    return false;
  }


  /**
   *
   * Endpoint for Create Meta Data Type use case
   *
   * @param name
   * @param file
   * @param observe set whether or not to return the data Observable as the body, response or events. defaults to returning the body.
   * @param reportProgress flag to report request and response progress.
   */
  public createMetaDataTypeForm(name: string, file: Blob, observe?: 'body', reportProgress?: boolean): Observable<EmptyObject>;
  public createMetaDataTypeForm(name: string, file: Blob, observe?: 'response', reportProgress?: boolean): Observable<HttpResponse<EmptyObject>>;
  public createMetaDataTypeForm(name: string, file: Blob, observe?: 'events', reportProgress?: boolean): Observable<HttpEvent<EmptyObject>>;
  public createMetaDataTypeForm(name: string, file: Blob, observe: any = 'body', reportProgress: boolean = false): Observable<any> {

    if (name === null || name === undefined) {
      throw new Error('Required parameter name was null or undefined when calling createMetaDataType.');
    }

    if (file === null || file === undefined) {
      throw new Error('Required parameter file was null or undefined when calling createMetaDataType.');
    }

    let headers = this.defaultHeaders;

    // to determine the Accept header
    const httpHeaderAccepts: string[] = [
      'application/json'
    ];
    const httpHeaderAcceptSelected: string | undefined = this.configuration.selectHeaderAccept(httpHeaderAccepts);
    if (httpHeaderAcceptSelected != undefined) {
      headers = headers.set('Accept', httpHeaderAcceptSelected);
    }

    // to determine the Content-Type header
    const consumes: string[] = [
      'multipart/form-data'
    ];

    const canConsumeForm = this.canConsumeForm(consumes);

    let formParams: { append(param: string, value: any): void };
    let useForm = false;
    const convertFormParamsToString = false;
    // use FormData to transmit files using content-type "multipart/form-data"
    // see https://stackoverflow.com/questions/4007969/application-x-www-form-urlencoded-or-multipart-form-data
    useForm = canConsumeForm;
    if (useForm) {
      formParams = new FormData();
    } else {
      formParams = new HttpParams({encoder: new CustomHttpUrlEncodingCodec()});
    }

    if (name !== undefined) {
      formParams = formParams.append('name', <any>name) as any || formParams;
    }
    if (file !== undefined) {
      formParams = formParams.append('file', <any>file) as any || formParams;
    }

    return this.httpClient.request<EmptyObject>('post', `${this.basePath}/tools/createMetaDataType`,
      {
        body: convertFormParamsToString ? formParams.toString() : formParams,
        withCredentials: this.configuration.withCredentials,
        headers,
        observe,
        reportProgress
      }
    );
  }

  /**
   *
   * Endpoint for Delete Tool use case
   *
   * @param body
   * @param observe set whether or not to return the data Observable as the body, response or events. defaults to returning the body.
   * @param reportProgress flag to report request and response progress.
   */
  public deleteTool(body: DeleteToolRequest, observe?: 'body', reportProgress?: boolean): Observable<EmptyObject>;
  public deleteTool(body: DeleteToolRequest, observe?: 'response', reportProgress?: boolean): Observable<HttpResponse<EmptyObject>>;
  public deleteTool(body: DeleteToolRequest, observe?: 'events', reportProgress?: boolean): Observable<HttpEvent<EmptyObject>>;
  public deleteTool(body: DeleteToolRequest, observe: any = 'body', reportProgress: boolean = false): Observable<any> {

    if (body === null || body === undefined) {
      throw new Error('Required parameter body was null or undefined when calling deleteTool.');
    }

    let headers = this.defaultHeaders;

    // to determine the Accept header
    const httpHeaderAccepts: string[] = [
      'application/json'
    ];
    const httpHeaderAcceptSelected: string | undefined = this.configuration.selectHeaderAccept(httpHeaderAccepts);
    if (httpHeaderAcceptSelected != undefined) {
      headers = headers.set('Accept', httpHeaderAcceptSelected);
    }

    // to determine the Content-Type header
    const consumes: string[] = [
      'application/json'
    ];
    const httpContentTypeSelected: string | undefined = this.configuration.selectHeaderContentType(consumes);
    if (httpContentTypeSelected != undefined) {
      headers = headers.set('Content-Type', httpContentTypeSelected);
    }

    return this.httpClient.request<EmptyObject>('post', `${this.basePath}/tools/deleteTool`,
      {
        body,
        withCredentials: this.configuration.withCredentials,
        headers,
        observe,
        reportProgress
      }
    );
  }

  /**
   *
   * Endpoint for Get Meta Data Types use case
   *
   * @param observe set whether or not to return the data Observable as the body, response or events. defaults to returning the body.
   * @param reportProgress flag to report request and response progress.
   */
  public getMetaDataTypes(observe?: 'body', reportProgress?: boolean): Observable<GetToolMetaDataTypes>;
  public getMetaDataTypes(observe?: 'response', reportProgress?: boolean): Observable<HttpResponse<GetToolMetaDataTypes>>;
  public getMetaDataTypes(observe?: 'events', reportProgress?: boolean): Observable<HttpEvent<GetToolMetaDataTypes>>;
  public getMetaDataTypes(observe: any = 'body', reportProgress: boolean = false): Observable<any> {

    let headers = this.defaultHeaders;

    // to determine the Accept header
    const httpHeaderAccepts: string[] = [
      'application/json'
    ];
    const httpHeaderAcceptSelected: string | undefined = this.configuration.selectHeaderAccept(httpHeaderAccepts);
    if (httpHeaderAcceptSelected != undefined) {
      headers = headers.set('Accept', httpHeaderAcceptSelected);
    }

    // to determine the Content-Type header
    const consumes: string[] = [];

    return this.httpClient.request<GetToolMetaDataTypes>('get', `${this.basePath}/tools/getMetaDataTypes`,
      {
        withCredentials: this.configuration.withCredentials,
        headers,
        observe,
        reportProgress
      }
    );
  }

  /**
   *
   * Endpoint for Get Tool Files use case
   *
   * @param body
   * @param observe set whether or not to return the data Observable as the body, response or events. defaults to returning the body.
   * @param reportProgress flag to report request and response progress.
   */
  public getToolFiles(body: GetToolFilesRequest, observe?: 'body', reportProgress?: boolean): Observable<GetToolFilesResponse>;
  public getToolFiles(body: GetToolFilesRequest, observe?: 'response', reportProgress?: boolean): Observable<HttpResponse<GetToolFilesResponse>>;
  public getToolFiles(body: GetToolFilesRequest, observe?: 'events', reportProgress?: boolean): Observable<HttpEvent<GetToolFilesResponse>>;
  public getToolFiles(body: GetToolFilesRequest, observe: any = 'body', reportProgress: boolean = false): Observable<any> {

    if (body === null || body === undefined) {
      throw new Error('Required parameter body was null or undefined when calling getToolFiles.');
    }

    let headers = this.defaultHeaders;

    // to determine the Accept header
    const httpHeaderAccepts: string[] = [
      'application/json'
    ];
    const httpHeaderAcceptSelected: string | undefined = this.configuration.selectHeaderAccept(httpHeaderAccepts);
    if (httpHeaderAcceptSelected != undefined) {
      headers = headers.set('Accept', httpHeaderAcceptSelected);
    }

    // to determine the Content-Type header
    const consumes: string[] = [
      'application/json'
    ];
    const httpContentTypeSelected: string | undefined = this.configuration.selectHeaderContentType(consumes);
    if (httpContentTypeSelected != undefined) {
      headers = headers.set('Content-Type', httpContentTypeSelected);
    }

    return this.httpClient.request<GetToolFilesResponse>('post', `${this.basePath}/tools/getToolFiles`,
      {
        body,
        withCredentials: this.configuration.withCredentials,
        headers,
        observe,
        reportProgress
      }
    );
  }

  /**
   *
   * Endpoint for Get Tool Types use case
   *
   * @param observe set whether or not to return the data Observable as the body, response or events. defaults to returning the body.
   * @param reportProgress flag to report request and response progress.
   */
  public getToolTypes(observe?: 'body', reportProgress?: boolean): Observable<GetToolTypesResponse>;
  public getToolTypes(observe?: 'response', reportProgress?: boolean): Observable<HttpResponse<GetToolTypesResponse>>;
  public getToolTypes(observe?: 'events', reportProgress?: boolean): Observable<HttpEvent<GetToolTypesResponse>>;
  public getToolTypes(observe: any = 'body', reportProgress: boolean = false): Observable<any> {

    let headers = this.defaultHeaders;

    // to determine the Accept header
    const httpHeaderAccepts: string[] = [
      'application/json'
    ];
    const httpHeaderAcceptSelected: string | undefined = this.configuration.selectHeaderAccept(httpHeaderAccepts);
    if (httpHeaderAcceptSelected != undefined) {
      headers = headers.set('Accept', httpHeaderAcceptSelected);
    }

    // to determine the Content-Type header
    const consumes: string[] = [];

    return this.httpClient.request<GetToolTypesResponse>('get', `${this.basePath}/tools/getToolTypes`,
      {
        withCredentials: this.configuration.withCredentials,
        headers,
        observe,
        reportProgress
      }
    );
  }

  /**
   *
   * Endpoint for Delete Tool use case
   *
   * @param observe set whether or not to return the data Observable as the body, response or events. defaults to returning the body.
   * @param reportProgress flag to report request and response progress.
   */
  public getTools(observe?: 'body', reportProgress?: boolean): Observable<GetAllToolsResponse>;
  public getTools(observe?: 'response', reportProgress?: boolean): Observable<HttpResponse<GetAllToolsResponse>>;
  public getTools(observe?: 'events', reportProgress?: boolean): Observable<HttpEvent<GetAllToolsResponse>>;
  public getTools(observe: any = 'body', reportProgress: boolean = false): Observable<any> {

    let headers = this.defaultHeaders;

    // to determine the Accept header
    const httpHeaderAccepts: string[] = [
      'application/json'
    ];
    const httpHeaderAcceptSelected: string | undefined = this.configuration.selectHeaderAccept(httpHeaderAccepts);
    if (httpHeaderAcceptSelected != undefined) {
      headers = headers.set('Accept', httpHeaderAcceptSelected);
    }

    // to determine the Content-Type header
    const consumes: string[] = [];

    return this.httpClient.request<GetAllToolsResponse>('get', `${this.basePath}/tools/getAllTools`,
      {
        withCredentials: this.configuration.withCredentials,
        headers,
        observe,
        reportProgress
      }
    );
  }

  /**
   *
   * Endpoint for Upload Analysis Tool use case
   *
   * @param sourceCode
   * @param model
   * @param metadataType
   * @param toolName
   * @param observe set whether or not to return the data Observable as the body, response or events. defaults to returning the body.
   * @param reportProgress flag to report request and response progress.
   */
  public uploadAnalysisToolForm(sourceCode: Blob, model: Blob, metadataType: string, toolName: string, observe?: 'body', reportProgress?: boolean): Observable<Tool>;
  public uploadAnalysisToolForm(sourceCode: Blob, model: Blob, metadataType: string, toolName: string, observe?: 'response', reportProgress?: boolean): Observable<HttpResponse<Tool>>;
  public uploadAnalysisToolForm(sourceCode: Blob, model: Blob, metadataType: string, toolName: string, observe?: 'events', reportProgress?: boolean): Observable<HttpEvent<Tool>>;
  public uploadAnalysisToolForm(sourceCode: Blob, model: Blob, metadataType: string, toolName: string, observe: any = 'body', reportProgress: boolean = false): Observable<any> {

    if (sourceCode === null || sourceCode === undefined) {
      throw new Error('Required parameter sourceCode was null or undefined when calling uploadAnalysisTool.');
    }

    if (model === null || model === undefined) {
      throw new Error('Required parameter model was null or undefined when calling uploadAnalysisTool.');
    }

    if (metadataType === null || metadataType === undefined) {
      throw new Error('Required parameter metadataType was null or undefined when calling uploadAnalysisTool.');
    }

    if (toolName === null || toolName === undefined) {
      throw new Error('Required parameter toolName was null or undefined when calling uploadAnalysisTool.');
    }

    let headers = this.defaultHeaders;

    // to determine the Accept header
    const httpHeaderAccepts: string[] = [
      'application/json'
    ];
    const httpHeaderAcceptSelected: string | undefined = this.configuration.selectHeaderAccept(httpHeaderAccepts);
    if (httpHeaderAcceptSelected != undefined) {
      headers = headers.set('Accept', httpHeaderAcceptSelected);
    }

    // to determine the Content-Type header
    const consumes: string[] = [
      'multipart/form-data'
    ];

    const canConsumeForm = this.canConsumeForm(consumes);

    let formParams: { append(param: string, value: any): void };
    let useForm = false;
    const convertFormParamsToString = false;
    // use FormData to transmit files using content-type "multipart/form-data"
    // see https://stackoverflow.com/questions/4007969/application-x-www-form-urlencoded-or-multipart-form-data
    useForm = canConsumeForm;
    // use FormData to transmit files using content-type "multipart/form-data"
    // see https://stackoverflow.com/questions/4007969/application-x-www-form-urlencoded-or-multipart-form-data
    useForm = canConsumeForm;
    if (useForm) {
      formParams = new FormData();
    } else {
      formParams = new HttpParams({encoder: new CustomHttpUrlEncodingCodec()});
    }

    if (sourceCode !== undefined) {
      formParams = formParams.append('sourceCode', <any>sourceCode) as any || formParams;
    }
    if (model !== undefined) {
      formParams = formParams.append('model', <any>model) as any || formParams;
    }
    if (metadataType !== undefined) {
      formParams = formParams.append('metadataType', <any>metadataType) as any || formParams;
    }
    if (toolName !== undefined) {
      formParams = formParams.append('toolName', <any>toolName) as any || formParams;
    }

    return this.httpClient.request<Tool>('post', `${this.basePath}/tools/uploadAnalysisTool`,
      {
        body: convertFormParamsToString ? formParams.toString() : formParams,
        withCredentials: this.configuration.withCredentials,
        headers,
        observe,
        reportProgress
      }
    );
  }

  /**
   *
   * Endpoint for Upload Drawing Tool use case
   *
   * @param sourceCode
   * @param metadataType
   * @param toolName
   * @param observe set whether or not to return the data Observable as the body, response or events. defaults to returning the body.
   * @param reportProgress flag to report request and response progress.
   */
  public uploadDrawingToolForm(sourceCode: Blob, metadataType: string, toolName: string, observe?: 'body', reportProgress?: boolean): Observable<Tool>;
  public uploadDrawingToolForm(sourceCode: Blob, metadataType: string, toolName: string, observe?: 'response', reportProgress?: boolean): Observable<HttpResponse<Tool>>;
  public uploadDrawingToolForm(sourceCode: Blob, metadataType: string, toolName: string, observe?: 'events', reportProgress?: boolean): Observable<HttpEvent<Tool>>;
  public uploadDrawingToolForm(sourceCode: Blob, metadataType: string, toolName: string, observe: any = 'body', reportProgress: boolean = false): Observable<any> {

    if (sourceCode === null || sourceCode === undefined) {
      throw new Error('Required parameter sourceCode was null or undefined when calling uploadDrawingTool.');
    }

    if (metadataType === null || metadataType === undefined) {
      throw new Error('Required parameter metadataType was null or undefined when calling uploadDrawingTool.');
    }

    if (toolName === null || toolName === undefined) {
      throw new Error('Required parameter toolName was null or undefined when calling uploadDrawingTool.');
    }

    let headers = this.defaultHeaders;

    // to determine the Accept header
    const httpHeaderAccepts: string[] = [
      'application/json'
    ];
    const httpHeaderAcceptSelected: string | undefined = this.configuration.selectHeaderAccept(httpHeaderAccepts);
    if (httpHeaderAcceptSelected != undefined) {
      headers = headers.set('Accept', httpHeaderAcceptSelected);
    }

    // to determine the Content-Type header
    const consumes: string[] = [
      'multipart/form-data'
    ];

    const canConsumeForm = this.canConsumeForm(consumes);

    let formParams: { append(param: string, value: any): void };
    let useForm = false;
    const convertFormParamsToString = false;
    // use FormData to transmit files using content-type "multipart/form-data"
    // see https://stackoverflow.com/questions/4007969/application-x-www-form-urlencoded-or-multipart-form-data
    useForm = canConsumeForm;
    if (useForm) {
      formParams = new FormData();
    } else {
      formParams = new HttpParams({encoder: new CustomHttpUrlEncodingCodec()});
    }

    if (sourceCode !== undefined) {
      formParams = formParams.append('sourceCode', <any>sourceCode) as any || formParams;
    }
    if (metadataType !== undefined) {
      formParams = formParams.append('metadataType', <any>metadataType) as any || formParams;
    }
    if (toolName !== undefined) {
      formParams = formParams.append('toolName', <any>toolName) as any || formParams;
    }

    return this.httpClient.request<Tool>('post', `${this.basePath}/tools/uploadDrawingTool`,
      {
        body: convertFormParamsToString ? formParams.toString() : formParams,
        withCredentials: this.configuration.withCredentials,
        headers,
        observe,
        reportProgress
      }
    );
  }

}
