import { TestBed } from '@angular/core/testing';

import { VideouploadService } from './videoupload.service';
import {VideostoreCardComponent} from '../../components/videostore-card/videostore-card.component';
import {IonicModule} from '@ionic/angular';
import {HttpClient, HttpClientModule} from '@angular/common/http';
import {HttpClientTestingModule} from '@angular/common/http/testing';

describe('VideouploadService', () => {
  let service: VideouploadService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      //declarations: [ VideouploadService ],
      imports: [IonicModule.forRoot()],
      providers: [{provide: HttpClient, useValue: HttpClientTestingModule}]
    });
    service = TestBed.inject(VideouploadService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
