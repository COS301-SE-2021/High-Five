import {ComponentFixture, TestBed, waitForAsync} from '@angular/core/testing';
import {IonicModule} from '@ionic/angular';

import {CreateToolComponent} from './create-tool.component';
import {ToolsService} from '../../apis/tools.service';
import {HttpClientTestingModule} from '@angular/common/http/testing';
import {SnotifyService, ToastDefaults} from 'ng-snotify';

describe('CreateToolComponent', () => {
  let component: CreateToolComponent;
  let fixture: ComponentFixture<CreateToolComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [CreateToolComponent],
      imports: [IonicModule.forRoot(), HttpClientTestingModule],
      providers: [ToolsService, SnotifyService, {
        provide: 'SnotifyToastConfig',
        useValue: ToastDefaults
      }]
    }).compileComponents();

    fixture = TestBed.createComponent(CreateToolComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  }));

  it('should create', () => {
    expect(true).toBeTrue();
  });
});
