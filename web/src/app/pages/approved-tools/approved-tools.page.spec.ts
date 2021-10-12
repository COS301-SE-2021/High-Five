import {ComponentFixture, TestBed, waitForAsync} from '@angular/core/testing';
import {IonicModule} from '@ionic/angular';

import {ApprovedToolsPage} from './approved-tools.page';
import {UserService} from '../../apis/user.service';
import {SnotifyService, ToastDefaults} from 'ng-snotify';
import {PipelinesService} from '../../apis/pipelines.service';
import {MediaStorageService} from '../../apis/mediaStorage.service';
import {AnalysisService} from '../../apis/analysis.service';
import {ToolsService} from '../../apis/tools.service';
import {HttpClientTestingModule} from '@angular/common/http/testing';

describe('ApprovedToolsPage', () => {
  let component: ApprovedToolsPage;
  let fixture: ComponentFixture<ApprovedToolsPage>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ApprovedToolsPage],
      imports: [IonicModule.forRoot(), HttpClientTestingModule],
      providers: [UserService, SnotifyService, {
        provide: 'SnotifyToastConfig',
        useValue: ToastDefaults
      }, PipelinesService, MediaStorageService, AnalysisService, ToolsService]
    }).compileComponents();

    fixture = TestBed.createComponent(ApprovedToolsPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  }));

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
