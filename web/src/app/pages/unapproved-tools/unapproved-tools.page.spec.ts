import {ComponentFixture, TestBed, waitForAsync} from '@angular/core/testing';
import {IonicModule} from '@ionic/angular';

import {UnapprovedToolsPage} from './unapproved-tools.page';
import {UserService} from '../../apis/user.service';
import {HttpClientTestingModule} from '@angular/common/http/testing';
import {SnotifyService, ToastDefaults} from 'ng-snotify';
import {PipelinesService} from '../../apis/pipelines.service';
import {MediaStorageService} from '../../apis/mediaStorage.service';
import {AnalysisService} from '../../apis/analysis.service';
import {ToolsService} from '../../apis/tools.service';

describe('UnapprovedToolsPage', () => {
  let component: UnapprovedToolsPage;
  let fixture: ComponentFixture<UnapprovedToolsPage>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [UnapprovedToolsPage],
      imports: [IonicModule.forRoot(), HttpClientTestingModule],
      providers: [UserService, SnotifyService, {
        provide: 'SnotifyToastConfig',
        useValue: ToastDefaults
      }, PipelinesService, MediaStorageService, AnalysisService, ToolsService]
    }).compileComponents();

    fixture = TestBed.createComponent(UnapprovedToolsPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  }));

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
