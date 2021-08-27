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

import {AddToolsRequest} from '../models/addToolsRequest';
import {CreatePipelineRequest} from '../models/createPipelineRequest';
import {CreatePipelineResponse} from '../models/createPipelineResponse';
import {DeletePipelineRequest} from '../models/deletePipelineRequest';
import {EmptyObject} from '../models/emptyObject';
import {GetPipelineIdsResponse} from '../models/getPipelineIdsResponse';
import {GetPipelineRequest} from '../models/getPipelineRequest';
import {GetPipelinesResponse} from '../models/getPipelinesResponse';
import {Pipeline} from '../models/pipeline';
import {RemoveToolsRequest} from '../models/removeToolsRequest';

import {BASE_PATH, COLLECTION_FORMATS} from '../variables';
import {Configuration} from '../configuration';
import {environment} from '../../environments/environment';


@Injectable()
export class PipelinesService {

  protected basePath = environment.apiEndpoint;
  public defaultHeaders = new HttpHeaders().set('Authorization', 'Bearer ' + localStorage.getItem('jwt'));
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
   * Endpoint for Add Tools use case
   *
   * @param body
   * @param observe set whether or not to return the data Observable as the body, response or events. defaults to returning the body.
   * @param reportProgress flag to report request and response progress.
   */
  public addTools(body: AddToolsRequest, observe?: 'body', reportProgress?: boolean): Observable<EmptyObject>;
  public addTools(body: AddToolsRequest, observe?: 'response', reportProgress?: boolean): Observable<HttpResponse<EmptyObject>>;
  public addTools(body: AddToolsRequest, observe?: 'events', reportProgress?: boolean): Observable<HttpEvent<EmptyObject>>;
  public addTools(body: AddToolsRequest, observe: any = 'body', reportProgress: boolean = false): Observable<any> {

    if (body === null || body === undefined) {
      throw new Error('Required parameter body was null or undefined when calling addTools.');
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

    return this.httpClient.request<EmptyObject>('post', `${this.basePath}/pipelines/addTools`,
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
   * Endpoint for Create Pipeline use case
   *
   * @param body
   * @param observe set whether or not to return the data Observable as the body, response or events. defaults to returning the body.
   * @param reportProgress flag to report request and response progress.
   */
  public createPipeline(body: CreatePipelineRequest, observe?: 'body', reportProgress?: boolean): Observable<CreatePipelineResponse>;
  public createPipeline(body: CreatePipelineRequest, observe?: 'response', reportProgress?: boolean): Observable<HttpResponse<CreatePipelineResponse>>;
  public createPipeline(body: CreatePipelineRequest, observe?: 'events', reportProgress?: boolean): Observable<HttpEvent<CreatePipelineResponse>>;
  public createPipeline(body: CreatePipelineRequest, observe: any = 'body', reportProgress: boolean = false): Observable<any> {

    if (body === null || body === undefined) {
      throw new Error('Required parameter body was null or undefined when calling createPipeline.');
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

    return this.httpClient.request<CreatePipelineResponse>('post', `${this.basePath}/pipelines/createPipeline`,
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
   * Endpoint for Delete Pipeline use case
   *
   * @param body
   * @param observe set whether or not to return the data Observable as the body, response or events. defaults to returning the body.
   * @param reportProgress flag to report request and response progress.
   */
  public deletePipeline(body: DeletePipelineRequest, observe?: 'body', reportProgress?: boolean): Observable<EmptyObject>;
  public deletePipeline(body: DeletePipelineRequest, observe?: 'response', reportProgress?: boolean): Observable<HttpResponse<EmptyObject>>;
  public deletePipeline(body: DeletePipelineRequest, observe?: 'events', reportProgress?: boolean): Observable<HttpEvent<EmptyObject>>;
  public deletePipeline(body: DeletePipelineRequest, observe: any = 'body', reportProgress: boolean = false): Observable<any> {

    if (body === null || body === undefined) {
      throw new Error('Required parameter body was null or undefined when calling deletePipeline.');
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

    return this.httpClient.request<EmptyObject>('post', `${this.basePath}/pipelines/deletePipeline`,
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
   * Endpoint for Get All Tools use case
   *
   * @param observe set whether or not to return the data Observable as the body, response or events. defaults to returning the body.
   * @param reportProgress flag to report request and response progress.
   */
  public getAllTools(observe?: 'body', reportProgress?: boolean): Observable<Array<string>>;
  public getAllTools(observe?: 'response', reportProgress?: boolean): Observable<HttpResponse<Array<string>>>;
  public getAllTools(observe?: 'events', reportProgress?: boolean): Observable<HttpEvent<Array<string>>>;
  public getAllTools(observe: any = 'body', reportProgress: boolean = false): Observable<any> {

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

    return this.httpClient.request<Array<string>>('post', `${this.basePath}/pipelines/getAllTools`,
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
   * Endpoint for Get Pipeline use case
   *
   * @param body
   * @param observe set whether or not to return the data Observable as the body, response or events. defaults to returning the body.
   * @param reportProgress flag to report request and response progress.
   */
  public getPipeline(body: GetPipelineRequest, observe?: 'body', reportProgress?: boolean): Observable<Pipeline>;
  public getPipeline(body: GetPipelineRequest, observe?: 'response', reportProgress?: boolean): Observable<HttpResponse<Pipeline>>;
  public getPipeline(body: GetPipelineRequest, observe?: 'events', reportProgress?: boolean): Observable<HttpEvent<Pipeline>>;
  public getPipeline(body: GetPipelineRequest, observe: any = 'body', reportProgress: boolean = false): Observable<any> {

    if (body === null || body === undefined) {
      throw new Error('Required parameter body was null or undefined when calling getPipeline.');
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

    return this.httpClient.request<Pipeline>('post', `${this.basePath}/pipelines/getPipeline`,
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
   * Endpoint for Get Pipeline Ids use case
   *
   * @param observe set whether or not to return the data Observable as the body, response or events. defaults to returning the body.
   * @param reportProgress flag to report request and response progress.
   */
  public getPipelineIds(observe?: 'body', reportProgress?: boolean): Observable<GetPipelineIdsResponse>;
  public getPipelineIds(observe?: 'response', reportProgress?: boolean): Observable<HttpResponse<GetPipelineIdsResponse>>;
  public getPipelineIds(observe?: 'events', reportProgress?: boolean): Observable<HttpEvent<GetPipelineIdsResponse>>;
  public getPipelineIds(observe: any = 'body', reportProgress: boolean = false): Observable<any> {

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

    return this.httpClient.request<GetPipelineIdsResponse>('post', `${this.basePath}/pipelines/getPipelineIds`,
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
   * Endpoint for Get Pipelines use case
   *
   * @param observe set whether or not to return the data Observable as the body, response or events. defaults to returning the body.
   * @param reportProgress flag to report request and response progress.
   */
  public getPipelines(observe?: 'body', reportProgress?: boolean): Observable<GetPipelinesResponse>;
  public getPipelines(observe?: 'response', reportProgress?: boolean): Observable<HttpResponse<GetPipelinesResponse>>;
  public getPipelines(observe?: 'events', reportProgress?: boolean): Observable<HttpEvent<GetPipelinesResponse>>;
  public getPipelines(observe: any = 'body', reportProgress: boolean = false): Observable<any> {

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

    return this.httpClient.request<GetPipelinesResponse>('post', `${this.basePath}/pipelines/getPipelines`,
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
   * Endpoint for Remove Tools use case
   *
   * @param body
   * @param observe set whether or not to return the data Observable as the body, response or events. defaults to returning the body.
   * @param reportProgress flag to report request and response progress.
   */
  public removeTools(body: RemoveToolsRequest, observe?: 'body', reportProgress?: boolean): Observable<EmptyObject>;
  public removeTools(body: RemoveToolsRequest, observe?: 'response', reportProgress?: boolean): Observable<HttpResponse<EmptyObject>>;
  public removeTools(body: RemoveToolsRequest, observe?: 'events', reportProgress?: boolean): Observable<HttpEvent<EmptyObject>>;
  public removeTools(body: RemoveToolsRequest, observe: any = 'body', reportProgress: boolean = false): Observable<any> {

    if (body === null || body === undefined) {
      throw new Error('Required parameter body was null or undefined when calling removeTools.');
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

    return this.httpClient.request<EmptyObject>('post', `${this.basePath}/pipelines/removeTools`,
      {
        body,
        withCredentials: this.configuration.withCredentials,
        headers,
        observe,
        reportProgress
      }
    );
  }

}
