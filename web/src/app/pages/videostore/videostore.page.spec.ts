import {ComponentFixture, TestBed, waitForAsync} from '@angular/core/testing';

import {VideostorePage} from './videostore.page';
import {IonicModule} from '@ionic/angular';
import {MediaStorageService} from "../../apis/mediaStorage.service";
import {BehaviorSubject} from "rxjs";
import {distinctUntilChanged} from "rxjs/operators";

const mockMediaStorageService = jasmine.createSpyObj('MediaStorageService', ['deleteImage', 'deleteVideo', 'getAllImages', 'getAllVideos', 'storeImageForm', 'storeVideoForm']);
mockMediaStorageService.getAllVideos.and.callFake(
  (func) => func([{
    name: 'testVideoName',
    dateStored: new Date(2021, 6, 21),
    id: 'testID',
    thumbnail: 'test_thumbnail'
  }])
);

mockMediaStorageService.deleteImage.and.callFake(
  (imageId: string = 'id') => ((new BehaviorSubject(false).asObservable().pipe(distinctUntilChanged())))
);
mockMediaStorageService.deleteVideo.and.callFake(
  (videoId: string = 'id') => ((new BehaviorSubject(false).asObservable().pipe(distinctUntilChanged())))
);

mockMediaStorageService.getAllVideos.and.callFake(
  () => ((new BehaviorSubject([]).asObservable().pipe(distinctUntilChanged())))
);

mockMediaStorageService.getAllImages.and.callFake(
  () => ((new BehaviorSubject([]).asObservable().pipe(distinctUntilChanged())))
);


describe('VideostorePage', () => {
  let component: VideostorePage;
  let fixture: ComponentFixture<VideostorePage>;

  const setBeforeEach = (imports, providers) => {
    beforeEach(waitForAsync(() => {
      TestBed.configureTestingModule({
        declarations: [VideostorePage],
        imports: [IonicModule.forRoot()],
        providers: [{provide: MediaStorageService, useValue: mockMediaStorageService}]
      }).compileComponents();

      fixture = TestBed.createComponent(VideostorePage);
      component = fixture.componentInstance;
      component.videos = [];
      component.images = [];
      fixture.detectChanges();
    }));
  };

  describe('general', () => {
    setBeforeEach([IonicModule.forRoot()], []);

    it('should create', () => {
      expect(component).toBeTruthy();
    });
  });

});
