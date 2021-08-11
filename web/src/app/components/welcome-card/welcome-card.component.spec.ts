import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';

import { WelcomeCardComponent } from './welcome-card.component';
import {MsalService} from '@azure/msal-angular';

describe('WelcomeCardComponent', () => {
  let component: WelcomeCardComponent;
  let fixture: ComponentFixture<WelcomeCardComponent>;

  const msalServiceMock = {
    loginRedirect: jasmine.createSpy('loginRedirect')
  };

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ WelcomeCardComponent ],
      imports: [],
      providers: [
        {provide: MsalService, useValue: msalServiceMock}
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(WelcomeCardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  }));

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
