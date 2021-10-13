import {ComponentFixture, TestBed, waitForAsync} from '@angular/core/testing';
import {IonicModule} from '@ionic/angular';

import {ToolsPage} from './tools.page';
import {RouterTestingModule} from '@angular/router/testing';
import {UserService} from '../../apis/user.service';
import {SnotifyService, ToastDefaults} from 'ng-snotify';
import {PipelinesService} from '../../apis/pipelines.service';
import {MediaStorageService} from '../../apis/mediaStorage.service';
import {AnalysisService} from '../../apis/analysis.service';
import {ToolsService} from '../../apis/tools.service';
import {HttpClientTestingModule} from '@angular/common/http/testing';

describe('ToolsPage', () => {
  let component: ToolsPage;
  let fixture: ComponentFixture<ToolsPage>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ToolsPage],
      imports: [IonicModule.forRoot(), RouterTestingModule, HttpClientTestingModule],
      providers: [UserService, SnotifyService, {
        provide: 'SnotifyToastConfig',
        useValue: ToastDefaults
      }, PipelinesService, MediaStorageService, AnalysisService, ToolsService]
    }).compileComponents();

    fixture = TestBed.createComponent(ToolsPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  }));

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
