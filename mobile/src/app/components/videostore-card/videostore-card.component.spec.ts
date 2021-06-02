import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';
import {IonicModule, Platform} from '@ionic/angular';

import { VideostoreCardComponent } from './videostore-card.component';
import {Router} from "@angular/router";


/**
 * A mocked version of the VideoPreviewData class. This allows the component to be rendered with mock data,
 * instead of using data from the real object (which gets created outside the VideostoreCardComponent and is passed to the
 * component)
 */
let mockVideoDetail = jasmine.createSpyObj('VideoPreviewData', [ 'getTitle', 'getRecordedDate', 'getImageUrl' ]);

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

let mockModalController = jasmine.createSpyObj('ModalController')

/**
 * Runs all test suites for the VideostoreCardComponent
 */
describe('VideostoreCardComponent', () => {
  let component: VideostoreCardComponent;
  let fixture: ComponentFixture<VideostoreCardComponent>;

  /**
   * Runs all test suites for the desktop version of the VideostoreCardComponent.
   * This suit uses a mocked Platform object that returns a width greater than 700.
   */
  describe('desktop', () => {
    let mockPlatform = jasmine.createSpyObj('Platform', ['width']);
    mockPlatform.width.and.callFake(function () {
      return 701;
    });

    /**
     * This runs pre-flight code before each unit test.
     */
    beforeEach(waitForAsync(() => {

      //Sets up the component configuration for each module
      TestBed.configureTestingModule({
        declarations: [ VideostoreCardComponent ],
        imports: [IonicModule.forRoot()],
        providers: [
          {provide: Platform, useValue: mockPlatform} // provide our own mock object for ionic's Platform object
        ]
      }).compileComponents();

      fixture = TestBed.createComponent(VideostoreCardComponent);
      component = fixture.componentInstance;
      component.data = mockVideoDetail; // pass in our mock VideoPreviewData to the component
      fixture.detectChanges();
    }));

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
      const title = fixture.debugElement.nativeElement.querySelector('ion-card-title[name="videoTitle"]').innerText
      expect(title).toBe(mockVideoDetail.getTitle());
    });

    /**
     * Tests that the date of the card matches the date in the mock object.
     */
    it('should show date', () => {
      const date = fixture.debugElement.nativeElement.querySelector('ion-card-content[name="recordedDate"]').innerHTML.trim()
      expect(date).toBe(mockVideoDetail.getRecordedDate())
    });

    /**
     * Tests that the image for the desktop version of the card matches the image in the mock object.
     */
    it('should show desktop image', ()=>{
      let img = fixture.debugElement.nativeElement.querySelector('img[name="desktopImage"]').src;
      expect(img).toBe(mockVideoDetail.getImageUrl());
    })
  });


  /**
   * Runs all test suites for the mobile version of the VideostoreCardComponent.
   * This suit uses a mocked Platform object that returns a width less than 700.
   */
  describe('mobile', () => {
    let mockPlatform = jasmine.createSpyObj('Platform', ['width']);
    mockPlatform.width.and.callFake(function () {
      return 699;
    });
    beforeEach(waitForAsync(() => {
      TestBed.configureTestingModule({
        declarations: [ VideostoreCardComponent ],
        imports: [IonicModule.forRoot()],
        providers: [
          {provide: Platform, useValue: mockPlatform}
        ]
      }).compileComponents();

      fixture = TestBed.createComponent(VideostoreCardComponent);
      component = fixture.componentInstance;
      component.data = mockVideoDetail;
      fixture.detectChanges();
    }));

    /**
     * Tests that the image for the desktop version of the card matches the image in the mock object.
     */
    it('should show mobile image', () => {
      let img = fixture.debugElement.nativeElement.querySelector('img[name="mobileImage"]').src;
      expect(img).toBe(mockVideoDetail.getImageUrl());
    })
  })
});
