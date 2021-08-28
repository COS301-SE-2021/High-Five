import {ComponentFixture, TestBed, waitForAsync} from '@angular/core/testing';
import {IonicModule} from '@ionic/angular';

import {AccountComponent} from './account.component';
import {MsalService} from '@azure/msal-angular';
import {UsersService} from '../../services/users/users.service';

describe('AccountComponent', () => {
  let component: AccountComponent;
  let fixture: ComponentFixture<AccountComponent>;
  const msalServiceMock = {
    logout: jasmine.createSpy('logout'),
    getActiveAccount: jasmine.createSpy('getActiveAccount'),
    instance: jasmine.createSpyObj('instance', {
      getActiveAccount: jasmine.createSpyObj('getActiveAccount', {
        idTokenClaims: jasmine.createSpyObj('idTokenClaims',{
          ['given_name'] : jasmine.createSpy('[given_name]')
        })
      }),
    }),
  };


  const mockUsersService = {
    purgeMedia: jasmine.createSpy('purgeMedia'),
    upgradeToAdmin: jasmine.createSpy('upgradeToAdmin'),
    purgeOwnMedia: jasmine.createSpy('purgeOwnMedia'),
    getIsAdmin: jasmine.createSpy('getIsAdmin'),
  };

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [AccountComponent],
      imports: [IonicModule.forRoot()],
      providers: [{provide: MsalService, useValue: msalServiceMock}, {
        provide: UsersService,
        useValue: mockUsersService
      }]
    }).compileComponents();

    fixture = TestBed.createComponent(AccountComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  }));

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
