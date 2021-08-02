import { TestBed } from '@angular/core/testing';

import { MediaService } from './media.service';
import {VideostoreCardComponent} from '../../components/videostore-card/videostore-card.component';
import {IonicModule} from '@ionic/angular';
import {HttpClient, HttpClientModule} from '@angular/common/http';
import {HttpClientTestingModule} from '@angular/common/http/testing';

describe('MediaService', () => {
  let service: MediaService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      //declarations: [ MediaService ],
      imports: [IonicModule.forRoot()],
      providers: [{provide: HttpClient, useValue: HttpClientTestingModule}]
    });
    service = TestBed.inject(MediaService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
