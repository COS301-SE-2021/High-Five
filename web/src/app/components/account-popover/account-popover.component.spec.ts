import {ComponentFixture, TestBed, waitForAsync} from '@angular/core/testing';
import {IonicModule} from '@ionic/angular';

import {AccountPopoverComponent} from './account-popover.component';
import {MsalService} from '@azure/msal-angular';
import {RouterTestingModule} from '@angular/router/testing';

describe('AccountPopoverComponent', () => {
  let component: AccountPopoverComponent;
  let fixture: ComponentFixture<AccountPopoverComponent>;
  const msalServiceMock = {
    logout: jasmine.createSpy('logout')
  };

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [AccountPopoverComponent],
      imports: [IonicModule.forRoot(), RouterTestingModule],
      providers: [{provide: MsalService, useValue: msalServiceMock},]
    }).compileComponents();

    fixture = TestBed.createComponent(AccountPopoverComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  }));

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
