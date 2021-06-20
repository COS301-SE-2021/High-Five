import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';
import {IonicModule, ModalController, Platform} from '@ionic/angular';

import { VideostoreCardComponent } from './videostore-card.component';
import createSpyObj = jasmine.createSpyObj;

let component: VideostoreCardComponent;
let fixture: ComponentFixture<VideostoreCardComponent>;


/**
 * A mocked version of the VideoPreviewData class. This allows the component to be rendered with mock data,
 * instead of using data from the real object (which gets created outside the VideostoreCardComponent and is passed to the
 * component)
 */
const mockVideoDetail = jasmine.createSpyObj('VideoPreviewData', [ 'getTitle', 'getRecordedDate', 'getImageUrl' ]);

//mocks the getTitle() function of VideoPreviewData
mockVideoDetail.getTitle.and.callFake(function() {
  return 'Test title';
});

//mocks the getRecordedDate() function of VideoPreviewData
mockVideoDetail.getRecordedDate.and.callFake(function() {
  return '2021-01-01';
});

//mocks the getImageUrl() function of VideoPreviewData
mockVideoDetail.getImageUrl.and.callFake(function() {
  return 'https://example.com/img.png';
});


const mockModalController = createSpyObj('ModalController', ['create', 'present'], ['style']);

/**
 * Runs all test suites for the VideostoreCardComponent
 */
describe('VideostoreCardComponent', () => {

  /**
   * Runs all tests suits that don't depend on the version of the component.
   */
  describe('general', () => {
    /**
     * This runs pre-flight code before each unit test.
     */
    setBeforeEach([IonicModule.forRoot()], [ {provide: ModalController, useValue: mockModalController}]);

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
      expect(title).toBe(mockVideoDetail.getTitle());
    });

    /**
     * Tests that the date of the card matches the date in the mock object.
     */
    it('should show date', () => {
      const date = fixture.debugElement.nativeElement.querySelector('ion-card-content[test="recordedDate"]').innerHTML.trim();
      expect(date).toBe(mockVideoDetail.getRecordedDate());
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
    mockPlatform.width.and.callFake(function() {
      return 701;
    });

    /**
     * This runs pre-flight code before each unit test.
     */
    setBeforeEach([IonicModule.forRoot()], [{provide: Platform, useValue: mockPlatform}]);

    /**
     * Tests that the image for the desktop version of the card matches the image in the mock object.
     */
    it('should show desktop image', ()=>{
      const img = fixture.debugElement.nativeElement.querySelector('img[test="desktopImage"]').src;
      expect(img).toBe(mockVideoDetail.getImageUrl());
    });
  });


  /**
   * Runs all test suites for the mobile version of the VideostoreCardComponent.
   * This suit uses a mocked Platform object that returns a width less than 700.
   */
  describe('mobile', () => {
    const mockPlatform = jasmine.createSpyObj('Platform', ['width']);
    mockPlatform.width.and.callFake(function() {
      return 699;
    });

    setBeforeEach([IonicModule.forRoot()], [{provide: Platform, useValue: mockPlatform}]);

    /**
     * Tests that the image for the desktop version of the card matches the image in the mock object.
     */
    it('should show mobile image', () => {
      const img = fixture.debugElement.nativeElement.querySelector('img[test="mobileImage"]').src;
      expect(img).toBe(mockVideoDetail.getImageUrl());
    });
  });

  function setBeforeEach(imports, providers) {
    beforeEach(waitForAsync(() => {
      TestBed.configureTestingModule({
        declarations: [ VideostoreCardComponent ],
        imports,
        providers
      }).compileComponents();

      fixture = TestBed.createComponent(VideostoreCardComponent);
      component = fixture.componentInstance;
      component.data = mockVideoDetail;
      fixture.detectChanges();
    }));
  }
});
