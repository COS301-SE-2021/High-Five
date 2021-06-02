import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';
import { IonicModule } from '@ionic/angular';

import { NavbarPage } from './navbar.page';
import {Router} from "@angular/router";

let RouterMock;

/**
 * Tests the entire test suite for the NavbarPage
 */
describe('NavbarPage', () => {
  let component: NavbarPage;
  let fixture: ComponentFixture<NavbarPage>;

  /**
   * Creates a mock implementation for Ionic's Router object
   */
  RouterMock = {
    navigate: jasmine.createSpy('navigate')
  }

  /**
   * Pre-flight code that runs before each test
   */
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

  /**
   * Tests that the creation of the component is successful
   */
  it('should create', () => {
    expect(component).toBeTruthy();
  });

  /**
   * Tests that the component calls the navigate function and that it passed in the
   * correct parameter.
   */
  it('should go to videos', () => {
    let component = TestBed.createComponent(NavbarPage).componentInstance;
    component.navigateTo("navbar/videos", "videoLink");
    expect(RouterMock.navigate).toHaveBeenCalledWith(["navbar/videos"]);
  })

  /**
   * Tests that the component calls the navigate function and that it passed in the
   * correct parameter.
   */
  it('should go to analytics', () => {
    let component = TestBed.createComponent(NavbarPage).componentInstance;
    component.navigateTo("navbar/analytics", "videoLink");
    expect(RouterMock.navigate).toHaveBeenCalledWith(["navbar/analytics"]);
  })
});
