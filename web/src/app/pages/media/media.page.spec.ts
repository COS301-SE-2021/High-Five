import {ComponentFixture, TestBed, waitForAsync} from '@angular/core/testing';
import {IonicModule} from '@ionic/angular';

import {MediaPage} from './media.page';
import {RouterTestingModule} from '@angular/router/testing';
import {PipelinesService} from '../../apis/pipelines.service';
import {HttpClientTestingModule} from '@angular/common/http/testing';
import {SnotifyService, ToastDefaults} from 'ng-snotify';
import {MediaStorageService} from '../../apis/mediaStorage.service';

describe('MediaPage', () => {
  let component: MediaPage;
  let fixture: ComponentFixture<MediaPage>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [MediaPage],
      imports: [IonicModule.forRoot(), RouterTestingModule, HttpClientTestingModule],
      providers: [PipelinesService, SnotifyService, {
        provide: 'SnotifyToastConfig',
        useValue: ToastDefaults
      },MediaStorageService]
    }).compileComponents();

    fixture = TestBed.createComponent(MediaPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  }));

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
