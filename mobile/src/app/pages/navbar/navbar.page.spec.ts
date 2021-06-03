import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';
import { IonicModule } from '@ionic/angular';

import { NavbarPage } from './navbar.page';
import {Router} from "@angular/router";

let RouterMock;

describe('NavbarPage', () => {
  let component: NavbarPage;
  let fixture: ComponentFixture<NavbarPage>;

  RouterMock = {
    navigate: jasmine.createSpy('navigate')
  }

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ NavbarPage ],
      imports: [],
      providers: [
        {provide: Router, useValue: RouterMock}
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
    let component = TestBed.createComponent(NavbarPage).componentInstance;
    component.navigateTo("navbar/videos", "videoLink");
    expect(RouterMock.navigate).toHaveBeenCalledWith(["navbar/videos"]);
  })

  it('should go to analytics', () => {
    let component = TestBed.createComponent(NavbarPage).componentInstance;
    component.navigateTo("navbar/analytics", "videoLink");
    expect(RouterMock.navigate).toHaveBeenCalledWith(["navbar/analytics"]);
  })
});
