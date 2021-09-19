import {ComponentFixture, TestBed, waitForAsync} from '@angular/core/testing';
import {AngularDelegate, IonicModule, PopoverController} from '@ionic/angular';

import {NavbarPage} from './navbar.page';
import {Router} from '@angular/router';
import {MsalService} from '@azure/msal-angular';


describe('NavbarPage', () => {
  let component: NavbarPage;
  let fixture: ComponentFixture<NavbarPage>;


  const routerMock = {
    navigate: jasmine.createSpy('navigate')
  };

  const msalServiceMock = {
    logout: jasmine.createSpy('logout')
  };

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [NavbarPage],
      imports: [],
      providers: [
        {provide: Router, useValue: routerMock},
        {provide: MsalService, useValue: msalServiceMock},
        PopoverController, AngularDelegate
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(NavbarPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  }));

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
