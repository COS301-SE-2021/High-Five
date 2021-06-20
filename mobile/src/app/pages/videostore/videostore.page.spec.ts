import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';
import { IonicModule } from '@ionic/angular';

import { VideostorePage } from './videostore.page';
import {VideoMetaData} from '../../models/videoMetaData';

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

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
