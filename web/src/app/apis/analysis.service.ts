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

import {AnalyzeImageRequest} from '../models/analyzeImageRequest';
import {AnalyzeVideoRequest} from '../models/analyzeVideoRequest';
import {AnalyzedImageMetaData} from '../models/analyzedImageMetaData';
import {AnalyzedVideoMetaData} from '../models/analyzedVideoMetaData';
import {GetLiveAnalysisTokenResponse} from '../models/getLiveAnalysisTokenResponse';

import {BASE_PATH, COLLECTION_FORMATS} from '../variables';
import {Configuration} from '../configuration';
import {environment} from '../../environments/environment';


@Injectable()
export class AnalysisService {

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
   * Endpoint for Analyze Image use case
   *
   * @param body
   * @param observe set whether or not to return the data Observable as the body, response or events. defaults to returning the body.
   * @param reportProgress flag to report request and response progress.
   */
  public analyzeImage(body: AnalyzeImageRequest, observe?: 'body', reportProgress?: boolean): Observable<AnalyzedImageMetaData>;
  public analyzeImage(body: AnalyzeImageRequest, observe?: 'response', reportProgress?: boolean): Observable<HttpResponse<AnalyzedImageMetaData>>;
  public analyzeImage(body: AnalyzeImageRequest, observe?: 'events', reportProgress?: boolean): Observable<HttpEvent<AnalyzedImageMetaData>>;
  public analyzeImage(body: AnalyzeImageRequest, observe: any = 'body', reportProgress: boolean = false): Observable<any> {

    if (body === null || body === undefined) {
      throw new Error('Required parameter body was null or undefined when calling analyzeImage.');
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

    return this.httpClient.request<AnalyzedImageMetaData>('post', `${this.basePath}/analysis/analyzeImage`,
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
   * Endpoint for Analyze Video use case
   *
   * @param body
   * @param observe set whether or not to return the data Observable as the body, response or events. defaults to returning the body.
   * @param reportProgress flag to report request and response progress.
   */
  public analyzeVideo(body: AnalyzeVideoRequest, observe?: 'body', reportProgress?: boolean): Observable<AnalyzedVideoMetaData>;
  public analyzeVideo(body: AnalyzeVideoRequest, observe?: 'response', reportProgress?: boolean): Observable<HttpResponse<AnalyzedVideoMetaData>>;
  public analyzeVideo(body: AnalyzeVideoRequest, observe?: 'events', reportProgress?: boolean): Observable<HttpEvent<AnalyzedVideoMetaData>>;
  public analyzeVideo(body: AnalyzeVideoRequest, observe: any = 'body', reportProgress: boolean = false): Observable<any> {

    if (body === null || body === undefined) {
      throw new Error('Required parameter body was null or undefined when calling analyzeVideo.');
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

    return this.httpClient.request<AnalyzedVideoMetaData>('post', `${this.basePath}/analysis/analyzeVideo`,
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
   * Endpoint for Get Live Analysis Token use case
   *
   * @param observe set whether or not to return the data Observable as the body, response or events. defaults to returning the body.
   * @param reportProgress flag to report request and response progress.
   */
  public getLiveAnalysisToken(observe?: 'body', reportProgress?: boolean): Observable<GetLiveAnalysisTokenResponse>;
  public getLiveAnalysisToken(observe?: 'response', reportProgress?: boolean): Observable<HttpResponse<GetLiveAnalysisTokenResponse>>;
  public getLiveAnalysisToken(observe?: 'events', reportProgress?: boolean): Observable<HttpEvent<GetLiveAnalysisTokenResponse>>;
  public getLiveAnalysisToken(observe: any = 'body', reportProgress: boolean = false): Observable<any> {

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

    return this.httpClient.request<GetLiveAnalysisTokenResponse>('post', `${this.basePath}/analysis/getLiveAnalysisToken`,
      {
        withCredentials: this.configuration.withCredentials,
        headers,
        observe,
        reportProgress
      }
    );
  }

}
