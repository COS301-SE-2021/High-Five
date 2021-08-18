import {ComponentFixture, TestBed, waitForAsync} from '@angular/core/testing';
import {AngularDelegate, IonicModule, ModalController, PopoverController} from '@ionic/angular';

import {VideostoreCardComponent} from './videostore-card.component';
import {HttpClient} from '@angular/common/http';
import {HttpClientTestingModule} from '@angular/common/http/testing';
import {MediaStorageService} from '../../apis/mediaStorage.service';
import {PipelinesService} from '../../apis/pipelines.service';
import {AnalysisService} from '../../apis/analysis.service';

let component: VideostoreCardComponent;
let fixture: ComponentFixture<VideostoreCardComponent>;


/**
 * A mocked version of the VideoPreviewData class. This allows the component to be rendered with mock data,
 * instead of using data from the real object (which gets created outside the VideostoreCardComponent and is passed to the
 * component)
 */
const mockVideoMetadata = jasmine.createSpyObj('VideoMetaData', [],
  {id: 'test id', name: 'test name', dateStored: new Date(), url: 'test_url', thumbnail: 'test_thumbnail'});


const mockModalController = jasmine.createSpyObj('ModalController', ['create', 'present'], ['style']);

/**
 * Runs all test suites for the VideostoreCardComponent
 */
describe('VideostoreCardComponent', () => {

  const setBeforeEach = (imports, providers) => {
    beforeEach(waitForAsync(() => {
      TestBed.configureTestingModule({
        declarations: [VideostoreCardComponent],
        imports: [HttpClientTestingModule],
        providers: [MediaStorageService, ModalController, AngularDelegate, PopoverController, PipelinesService, AnalysisService]
      }).compileComponents();

      fixture = TestBed.createComponent(VideostoreCardComponent);
      component = fixture.componentInstance;
      component.video = mockVideoMetadata;
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
    setBeforeEach([IonicModule.forRoot()], [{provide: ModalController, useValue: mockModalController},
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
    it('should show name', () => {
      const name = fixture.debugElement.nativeElement.querySelectorAll('ion-card-title')[0].innerText.trim();
      expect(name).toBe(mockVideoMetadata.name);
    });

    /**
     * Tests that the date of the card matches the date in the mock object.
     */
    it('should show date', () => {
      const date = fixture.debugElement.nativeElement.querySelectorAll('ion-text')[0].innerText.trim();
      expect(date).toBe('Date Created : ' + mockVideoMetadata.dateStored);
    });

    /**
     * Tests that the click of the 'play' button calls the 'playVideo' function
     */
    it('should show modal', () => {
      spyOn(component, 'playVideo');
      const btn = fixture.debugElement.nativeElement.querySelectorAll('ion-button')[0];
      btn.click();
      expect(component.playVideo).toHaveBeenCalled();
    });
  });
});
