import {CUSTOM_ELEMENTS_SCHEMA} from '@angular/core';
import {TestBed, waitForAsync} from '@angular/core/testing';

import {AppComponent} from './app.component';
import {MsalService} from '@azure/msal-angular';
import {Router} from '@angular/router';

describe('AppComponent', () => {

  const msalServiceMock = {
    instance: jasmine.createSpy('instance')
  };
  const routerMock = {
    navigate: jasmine.createSpy('navigate')
  };

  beforeEach(waitForAsync(() => {

    TestBed.configureTestingModule({
      declarations: [AppComponent],
      schemas: [CUSTOM_ELEMENTS_SCHEMA],
      providers: [
        {provide: MsalService, useValue: msalServiceMock},
        {provide: Router, useValue: routerMock}
      ]
    }).compileComponents();
  }));

  it('should create the app', () => {
    const fixture = TestBed.createComponent(AppComponent);
    const app = fixture.debugElement.componentInstance;
    expect(app).toBeTruthy();
  });
  // TODO: add more tests!

});
