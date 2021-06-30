import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';
import { IonicModule } from '@ionic/angular';

import { NavbarPage } from './navbar.page';
import {Router} from '@angular/router';

let routerMock;

describe('NavbarPage', () => {
  let component: NavbarPage;
  let fixture: ComponentFixture<NavbarPage>;

  routerMock = {
    navigate: jasmine.createSpy('navigate')
  };

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ NavbarPage ],
      imports: [],
      providers: [
        {provide: Router, useValue: routerMock}
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(NavbarPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  }));

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should go to videos', () => {
    const component1 = TestBed.createComponent(NavbarPage).componentInstance;
    component1.navigateTo('navbar/videos', 'videoLink');
    expect(routerMock.navigate).toHaveBeenCalledWith(['navbar/videos']);
  });

  it('should go to analytics', () => {
    const component2 = TestBed.createComponent(NavbarPage).componentInstance;
    component2.navigateTo('navbar/analytics', 'videoLink');
    expect(routerMock.navigate).toHaveBeenCalledWith(['navbar/analytics']);
  });
});
