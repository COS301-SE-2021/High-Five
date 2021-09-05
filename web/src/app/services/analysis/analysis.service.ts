import {Injectable} from '@angular/core';
import {environment} from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AnalysisService {

  private readonly url = environment.websocketEndpoint;

  constructor() {

  }
}
