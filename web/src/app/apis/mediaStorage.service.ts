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

import {DeleteImageRequest} from '../models/deleteImageRequest';
import {DeleteVideoRequest} from '../models/deleteVideoRequest';
import {EmptyObject} from '../models/emptyObject';
import {GetAllImagesResponse} from '../models/getAllImagesResponse';
import {GetAllVideosResponse} from '../models/getAllVideosResponse';
import {GetAnalyzedImagesResponse} from '../models/getAnalyzedImagesResponse';
import {GetAnalyzedVideosResponse} from '../models/getAnalyzedVideosResponse';
import {ImageMetaData} from '../models/imageMetaData';
import {VideoMetaData} from '../models/videoMetaData';

import {BASE_PATH, COLLECTION_FORMATS} from '../variables';
import {Configuration} from '../configuration';
import {environment} from '../../environments/environment';


@Injectable()
export class MediaStorageService {

  protected basePath = environment.apiEndpoint;
  public defaultHeaders = new HttpHeaders();
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
   * Endpoint for Delete Analyzed Image use case
   *
   * @param body
   * @param observe set whether or not to return the data Observable as the body, response or events. defaults to returning the body.
   * @param reportProgress flag to report request and response progress.
   */
  public deleteAnalyzedImage(body: DeleteImageRequest, observe?: 'body', reportProgress?: boolean): Observable<EmptyObject>;
  public deleteAnalyzedImage(body: DeleteImageRequest, observe?: 'response', reportProgress?: boolean): Observable<HttpResponse<EmptyObject>>;
  public deleteAnalyzedImage(body: DeleteImageRequest, observe?: 'events', reportProgress?: boolean): Observable<HttpEvent<EmptyObject>>;
  public deleteAnalyzedImage(body: DeleteImageRequest, observe: any = 'body', reportProgress: boolean = false): Observable<any> {

    if (body === null || body === undefined) {
      throw new Error('Required parameter body was null or undefined when calling deleteAnalyzedImage.');
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

    return this.httpClient.request<EmptyObject>('post', `${this.basePath}/media/deleteAnalyzedImage`,
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
   * Endpoint for Delete Analyzed Video use case
   *
   * @param body
   * @param observe set whether or not to return the data Observable as the body, response or events. defaults to returning the body.
   * @param reportProgress flag to report request and response progress.
   */
  public deleteAnalyzedVideo(body: DeleteVideoRequest, observe?: 'body', reportProgress?: boolean): Observable<EmptyObject>;
  public deleteAnalyzedVideo(body: DeleteVideoRequest, observe?: 'response', reportProgress?: boolean): Observable<HttpResponse<EmptyObject>>;
  public deleteAnalyzedVideo(body: DeleteVideoRequest, observe?: 'events', reportProgress?: boolean): Observable<HttpEvent<EmptyObject>>;
  public deleteAnalyzedVideo(body: DeleteVideoRequest, observe: any = 'body', reportProgress: boolean = false): Observable<any> {

    if (body === null || body === undefined) {
      throw new Error('Required parameter body was null or undefined when calling deleteAnalyzedVideo.');
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

    return this.httpClient.request<EmptyObject>('post', `${this.basePath}/media/deleteAnalyzedVideo`,
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
   * Endpoint for Delete Image use case
   *
   * @param body
   * @param observe set whether or not to return the data Observable as the body, response or events. defaults to returning the body.
   * @param reportProgress flag to report request and response progress.
   */
  public deleteImage(body: DeleteImageRequest, observe?: 'body', reportProgress?: boolean): Observable<EmptyObject>;
  public deleteImage(body: DeleteImageRequest, observe?: 'response', reportProgress?: boolean): Observable<HttpResponse<EmptyObject>>;
  public deleteImage(body: DeleteImageRequest, observe?: 'events', reportProgress?: boolean): Observable<HttpEvent<EmptyObject>>;
  public deleteImage(body: DeleteImageRequest, observe: any = 'body', reportProgress: boolean = false): Observable<any> {

    if (body === null || body === undefined) {
      throw new Error('Required parameter body was null or undefined when calling deleteImage.');
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

    return this.httpClient.request<EmptyObject>('post', `${this.basePath}/media/deleteImage`,
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
   * Endpoint for Delete Video use case
   *
   * @param body
   * @param observe set whether or not to return the data Observable as the body, response or events. defaults to returning the body.
   * @param reportProgress flag to report request and response progress.
   */
  public deleteVideo(body: DeleteVideoRequest, observe?: 'body', reportProgress?: boolean): Observable<EmptyObject>;
  public deleteVideo(body: DeleteVideoRequest, observe?: 'response', reportProgress?: boolean): Observable<HttpResponse<EmptyObject>>;
  public deleteVideo(body: DeleteVideoRequest, observe?: 'events', reportProgress?: boolean): Observable<HttpEvent<EmptyObject>>;
  public deleteVideo(body: DeleteVideoRequest, observe: any = 'body', reportProgress: boolean = false): Observable<any> {

    if (body === null || body === undefined) {
      throw new Error('Required parameter body was null or undefined when calling deleteVideo.');
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

    return this.httpClient.request<EmptyObject>('post', `${this.basePath}/media/deleteVideo`,
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
   * Endpoint for Get All Images use case
   *
   * @param observe set whether or not to return the data Observable as the body, response or events. defaults to returning the body.
   * @param reportProgress flag to report request and response progress.
   */
  public getAllImages(observe?: 'body', reportProgress?: boolean): Observable<GetAllImagesResponse>;
  public getAllImages(observe?: 'response', reportProgress?: boolean): Observable<HttpResponse<GetAllImagesResponse>>;
  public getAllImages(observe?: 'events', reportProgress?: boolean): Observable<HttpEvent<GetAllImagesResponse>>;
  public getAllImages(observe: any = 'body', reportProgress: boolean = false): Observable<any> {

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

    return this.httpClient.request<GetAllImagesResponse>('get', `${this.basePath}/media/getAllImages`,
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
   * Endpoint for Get All Videos use case
   *
   * @param observe set whether or not to return the data Observable as the body, response or events. defaults to returning the body.
   * @param reportProgress flag to report request and response progress.
   */
  public getAllVideos(observe?: 'body', reportProgress?: boolean): Observable<GetAllVideosResponse>;
  public getAllVideos(observe?: 'response', reportProgress?: boolean): Observable<HttpResponse<GetAllVideosResponse>>;
  public getAllVideos(observe?: 'events', reportProgress?: boolean): Observable<HttpEvent<GetAllVideosResponse>>;
  public getAllVideos(observe: any = 'body', reportProgress: boolean = false): Observable<any> {

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

    return this.httpClient.request<GetAllVideosResponse>('get', `${this.basePath}/media/getAllVideos`,
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
   * Endpoint for Get Analyzed Images use case
   *
   * @param observe set whether or not to return the data Observable as the body, response or events. defaults to returning the body.
   * @param reportProgress flag to report request and response progress.
   */
  public getAnalyzedImages(observe?: 'body', reportProgress?: boolean): Observable<GetAnalyzedImagesResponse>;
  public getAnalyzedImages(observe?: 'response', reportProgress?: boolean): Observable<HttpResponse<GetAnalyzedImagesResponse>>;
  public getAnalyzedImages(observe?: 'events', reportProgress?: boolean): Observable<HttpEvent<GetAnalyzedImagesResponse>>;
  public getAnalyzedImages(observe: any = 'body', reportProgress: boolean = false): Observable<any> {

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

    return this.httpClient.request<GetAnalyzedImagesResponse>('get', `${this.basePath}/media/getAnalyzedImages`,
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
   * Endpoint for Get Analyzed Videos use case
   *
   * @param observe set whether or not to return the data Observable as the body, response or events. defaults to returning the body.
   * @param reportProgress flag to report request and response progress.
   */
  public getAnalyzedVideos(observe?: 'body', reportProgress?: boolean): Observable<GetAnalyzedVideosResponse>;
  public getAnalyzedVideos(observe?: 'response', reportProgress?: boolean): Observable<HttpResponse<GetAnalyzedVideosResponse>>;
  public getAnalyzedVideos(observe?: 'events', reportProgress?: boolean): Observable<HttpEvent<GetAnalyzedVideosResponse>>;
  public getAnalyzedVideos(observe: any = 'body', reportProgress: boolean = false): Observable<any> {

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

    return this.httpClient.request<GetAnalyzedVideosResponse>('get', `${this.basePath}/media/getAnalyzedVideos`,
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
   * Endpoint for Store Image use case
   *
   * @param file
   * @param observe set whether or not to return the data Observable as the body, response or events. defaults to returning the body.
   * @param reportProgress flag to report request and response progress.
   */
  public storeImageForm(file: Blob, observe?: 'body', reportProgress?: boolean): Observable<ImageMetaData>;
  public storeImageForm(file: Blob, observe?: 'response', reportProgress?: boolean): Observable<HttpResponse<ImageMetaData>>;
  public storeImageForm(file: Blob, observe?: 'events', reportProgress?: boolean): Observable<HttpEvent<ImageMetaData>>;
  public storeImageForm(file: Blob, observe: any = 'body', reportProgress: boolean = false): Observable<any> {

    if (file === null || file === undefined) {
      throw new Error('Required parameter file was null or undefined when calling storeImage.');
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

    if (file !== undefined) {
      formParams = formParams.append('file', <any>file) as any || formParams;
    }

    return this.httpClient.request<ImageMetaData>('post', `${this.basePath}/media/storeImage`,
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
   * Endpoint for Store Video use case
   *
   * @param file
   * @param observe set whether or not to return the data Observable as the body, response or events. defaults to returning the body.
   * @param reportProgress flag to report request and response progress.
   */
  public storeVideoForm(file: Blob, observe?: 'body', reportProgress?: boolean): Observable<VideoMetaData>;
  public storeVideoForm(file: Blob, observe?: 'response', reportProgress?: boolean): Observable<HttpResponse<VideoMetaData>>;
  public storeVideoForm(file: Blob, observe?: 'events', reportProgress?: boolean): Observable<HttpEvent<VideoMetaData>>;
  public storeVideoForm(file: Blob, observe: any = 'body', reportProgress: boolean = false): Observable<any> {

    if (file === null || file === undefined) {
      throw new Error('Required parameter file was null or undefined when calling storeVideo.');
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

    if (file !== undefined) {
      formParams = formParams.append('file', <any>file) as any || formParams;
    }

    return this.httpClient.request<VideoMetaData>('post', `${this.basePath}/media/storeVideo`,
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
