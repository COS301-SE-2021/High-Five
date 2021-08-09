import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';
import {IonicModule, ModalController} from '@ionic/angular';

import { VideostoreCardComponent } from './videostore-card.component';
import {HttpClient} from '@angular/common/http';
import {HttpClientTestingModule} from '@angular/common/http/testing';

let component: VideostoreCardComponent;
let fixture: ComponentFixture<VideostoreCardComponent>;


/**
 * A mocked version of the VideoPreviewData class. This allows the component to be rendered with mock data,
 * instead of using data from the real object (which gets created outside the VideostoreCardComponent and is passed to the
 * component)
 */
const mockVideoDetail = jasmine.createSpyObj('VideoMetaData', [ ],
  {id: 'test id', name: 'test name', dateStored: '2020-01-01'});



const mockModalController = jasmine.createSpyObj('ModalController', ['create', 'present'], ['style']);

/**
 * Runs all test suites for the VideostoreCardComponent
 */
describe('VideostoreCardComponent', () => {

  const setBeforeEach=(imports, providers) =>{
    beforeEach(waitForAsync(() => {
      TestBed.configureTestingModule({
        declarations: [ VideostoreCardComponent ],
        imports,
        providers
      }).compileComponents();

      fixture = TestBed.createComponent(VideostoreCardComponent);
      component = fixture.componentInstance;
      component.video = mockVideoDetail;
      fixture.detectChanges();
    }));
  };

  /**
   * Runs all tests suits that don't depend on the version of the component.
   */
  describe('general', () => {
    /**
     * This runs pre-flight code before each unit test.
     */
    setBeforeEach([IonicModule.forRoot()], [ {provide: ModalController, useValue: mockModalController},
      {provide: HttpClient, useValue: HttpClientTestingModule}]);

    /**
     * Tests that the component is rendered.
     */
    it('should create', () => {
      expect(component).toBeTruthy();
    });

    /**
     * Tests that the title of the card matches the returned value in the mock object
     */
    it('should show title', () => {
      const title = fixture.debugElement.nativeElement.querySelector('ion-card-title[test="videoTitle"]').innerText;
      expect(title).toBe(mockVideoDetail.name);
    });

    /**
     * Tests that the date of the card matches the date in the mock object.
     */
    it('should show date', () => {
      const date = fixture.debugElement.nativeElement.querySelector('ion-card-content[test="recordedDate"]').innerHTML.trim();
      expect(date).toBe(mockVideoDetail.dateStored);
    });

    /**
     * Tests that the click of the 'play' button calls the 'playVideo' function
     */
    it('should show modal', () => {
      spyOn(component, 'playVideo');
      const btn = fixture.debugElement.nativeElement.querySelector('ion-button[test="playBtn"]');
      btn.click();
      expect(component.playVideo).toHaveBeenCalled();
    });
  });

  /**
   * Runs all test suites for the desktop version of the VideostoreCardComponent.
   * This suit uses a mocked Platform object that returns a width greater than 700.
   */
  describe('desktop', () => {
    const mockPlatform = jasmine.createSpyObj('Platform', ['width']);
    mockPlatform.width.and.callFake(() =>701);

    /**
     * This runs pre-flight code before each unit test.
     */
    setBeforeEach([IonicModule.forRoot()], [ {provide: ModalController, useValue: mockModalController},
      {provide: HttpClient, useValue: HttpClientTestingModule}]);

    /**
     * Tests that the image for the desktop version of the card matches the image in the mock object.
     */
    // DISABLED because right now a hard coded value is used. This will change soon.
    // it('should show desktop image', ()=>{
    //   const img = fixture.debugElement.nativeElement.querySelector('img[test="desktopImage"]').src;
    //   expect(img).toBe(mockVideoDetail.getImageUrl());
    // });
  });


  /**
   * Runs all test suites for the mobile version of the VideostoreCardComponent.
   * This suit uses a mocked Platform object that returns a width less than 700.
   */
  describe('mobile', () => {
    const mockPlatform = jasmine.createSpyObj('Platform', ['width']);
    mockPlatform.width.and.callFake(() =>699);

    setBeforeEach([IonicModule.forRoot()], [ {provide: ModalController, useValue: mockModalController},
      {provide: HttpClient, useValue: HttpClientTestingModule}]);

    /**
     * Tests that the image for the desktop version of the card matches the image in the mock object.
     */
    // DISABLED because right now a hard coded value is used. This will change soon.
    // it('should show mobile image', () => {
    //   const img = fixture.debugElement.nativeElement.querySelector('img[test="mobileImage"]').src;
    //   expect(img).toBe(mockVideoDetail.getImageUrl());
    // });
  });


});
