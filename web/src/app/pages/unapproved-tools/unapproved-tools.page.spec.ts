import {ComponentFixture, TestBed, waitForAsync} from '@angular/core/testing';
import {IonicModule} from '@ionic/angular';

import {UnapprovedToolsPage} from './unapproved-tools.page';
import {UserService} from '../../apis/user.service';
import {HttpClientTestingModule} from '@angular/common/http/testing';

describe('UnapprovedToolsPage', () => {
  let component: UnapprovedToolsPage;
  let fixture: ComponentFixture<UnapprovedToolsPage>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [UnapprovedToolsPage],
      imports: [IonicModule.forRoot(), HttpClientTestingModule],
      providers: [UserService]
    }).compileComponents();

    fixture = TestBed.createComponent(UnapprovedToolsPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  }));

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
