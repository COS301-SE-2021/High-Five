import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';

import { VideostorePage } from './videostore.page';
import {VideoMetaData} from '../../models/videoMetaData';
import {VideouploadService} from '../../services/videoupload/videoupload.service';
import {RegisterCardComponent} from '../../components/register-card/register-card.component';

const mockVideouploadService = jasmine.createSpyObj('VideouploadService', [ 'getAllVideos']);
mockVideouploadService.getAllVideos.and.callFake(
  ()=>new Array<VideoMetaData>({name: 'testVideoName',dateStored: new Date(2021,6,21),id: 'testID'})
);

describe('VideostorePage', () => {
  let component: VideostorePage;
  let fixture: ComponentFixture<VideostorePage>;



  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ VideostorePage ],
      imports: [IonicModule.forRoot()]
    }).compileComponents();

    fixture = TestBed.createComponent(VideostorePage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  }));
  describe('general',()=>{
    setBeforeEach([IonicModule.forRoot()], [ {provide: VideouploadService, useValue: mockVideouploadService}]);

    it('should create', () => {
      expect(component).toBeTruthy();
    });


  });

  const setBeforeEach=(imports, providers) =>{
    beforeEach(waitForAsync(() => {
      TestBed.configureTestingModule({
        declarations: [ RegisterCardComponent ],
        imports,
        providers
      }).compileComponents();

      fixture = TestBed.createComponent(VideostorePage);
      component = fixture.componentInstance;
      fixture.detectChanges();
    }));
  };
});
